// Created by Christoph Vogel (christoph.vogel@uzh.ch) from the University of Zurich
// Created: 2018-10-01
// 
// Licensed under the MIT License.

using AudioTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AudioTracker.Helpers
{
    static class AmplitudeHelper
    {
        internal static double[] GetRelativeAmplitudes(byte[] InputBuffer, int BytesRecorded)
        {
            double[] relativeAmplitudes = new double[BytesRecorded / 2];
            int j = 0;
            for (int i = 0; i < BytesRecorded; i += 2)
            {
                short sample = (short)((InputBuffer[i + 1] << 8) | InputBuffer[i + 0]);
                float sample32 = sample / 32768.0f; // shouldn't this be 32767?
                relativeAmplitudes[j] = sample32;
                j++;
            }
            return relativeAmplitudes;
        }

        // TODO: this is partially redundant to GetAmplitudeData; refactor
        internal static List<AmplitudeData> GetRelativeAmplitudeSegments(double[] RelativeAmplitudeData, int NumberOfSegments, DateTime StartTime, double MillisecondsPerSample)
        {
            int SizeOfSmallerArrays = (int)Math.Round((double)RelativeAmplitudeData.Length / (double)NumberOfSegments);
            var AmplitudeChunks = Split(RelativeAmplitudeData, SizeOfSmallerArrays);
            List<AmplitudeData> NewAmplitudeDataList = new List<AmplitudeData>();
            int counter = 0;
            foreach (var CurrentData in AmplitudeChunks)
            {
                AmplitudeData CurrentAmplitudeData = new AmplitudeData();
                CurrentAmplitudeData.RelativeAmplitudeRMSValue = GetAmplitudeRootMeanSquare(CurrentData.ToArray<double>());
                Tuple<double, int> ModeAndCount = GetModeOfRelativeAmplitudes(CurrentData.ToArray<double>());
                CurrentAmplitudeData.MaxValue = CurrentData.Max();
                CurrentAmplitudeData.MinValue = CurrentData.Min();
                CurrentAmplitudeData.AvgValue = CurrentData.Sum() / CurrentData.ToArray<double>().Length;
                CurrentAmplitudeData.AmplitudeDBFS = 20 * Math.Log(CurrentAmplitudeData.RelativeAmplitudeRMSValue, 10);
                CurrentAmplitudeData.ModeValue = ModeAndCount.Item1;
                CurrentAmplitudeData.ModeOccurrences = ModeAndCount.Item2;
                CurrentAmplitudeData.StartTime = StartTime.AddMilliseconds(counter * CurrentData.ToArray<double>().Length * MillisecondsPerSample);
                NewAmplitudeDataList.Add(CurrentAmplitudeData);
                counter++;
            }
            return NewAmplitudeDataList;
        }

        internal static Tuple<double, int> GetModeOfRelativeAmplitudes(double[] relativeAmplitudes)
        {
            Dictionary<double, int> bucket = new Dictionary<double, int>();
            double modeAmplitudeValue = double.MinValue;
            int maxCount = 0;
            int count;
            foreach (double sample in relativeAmplitudes)
            {
                if (bucket.TryGetValue(sample, out count))
                {
                    count++;
                    bucket[sample] = count;
                }
                else
                {
                    count = 1;
                    bucket.Add(sample, count);
                }

                if (count >= maxCount)
                {
                    modeAmplitudeValue = sample;
                    maxCount = count;
                }
            }
            Tuple<double, int> result = new Tuple<double, int>(modeAmplitudeValue, maxCount);
            return result;
        }

        internal static double GetAmplitudeRootMeanSquare(double[] relativeAmplitudes)
        {
            // square samples, take average of these squares, take root
            double SumOfSquares = 0.0;
            for (int i = 0; i < relativeAmplitudes.Length; i++)
            {
                SumOfSquares += relativeAmplitudes[i] * relativeAmplitudes[i];
            }
            return Math.Sqrt(SumOfSquares / relativeAmplitudes.Length);
        }

        internal static bool IsMicrophoneProbablyMuted(AmplitudeData AmplitudeDataToCheck)
        {
            const double MAX_VALUE_THRESHOLD = 0.0015;
            const double MIN_VALUE_THRESHOLD = -0.0015;
            const double MODE_OCCURRENCE_THRESHOLD = 25000;
            bool IsMicrophoneProbablyMuted = false;
            if (AmplitudeDataToCheck.MaxValue < MAX_VALUE_THRESHOLD && AmplitudeDataToCheck.MinValue > MIN_VALUE_THRESHOLD && 
                AmplitudeDataToCheck.ModeOccurrences > MODE_OCCURRENCE_THRESHOLD)
            {
                Console.WriteLine("Microphone is probably muted...");
                IsMicrophoneProbablyMuted = true;
            }
            return IsMicrophoneProbablyMuted;
        }

        internal static List<AmplitudeData> GetAmplitudeData(byte[] InputBuffer, int BytesRecorded, int TimeResolution)
        {
            //TODO: calculate median
            //TODO: average should be calculated as average of *absolute values*
            //TODO: display number of samples
            //TODO: sum of samples non-relative, max, min, avg non-relative
            //TODO: chech endianess of system (BitConverter.IsLittleEndian)
            // relative loudness: last second: average |amplitude|
            //TODO: calculate SNR
            //TODO: calculate dynamic range

            double[] relativeAmplitudes = GetRelativeAmplitudes(InputBuffer, BytesRecorded);
            List<AmplitudeData> newAmplitudeDataList = new List<AmplitudeData>();
            AmplitudeData newAmplitudeData = new AmplitudeData();
            newAmplitudeData.MaxValue = relativeAmplitudes.Max();
            newAmplitudeData.MinValue = relativeAmplitudes.Min();
            newAmplitudeData.AvgValue = relativeAmplitudes.Sum() / relativeAmplitudes.Length;
            Tuple<double, int> modeAndOccurrence = GetModeOfRelativeAmplitudes(relativeAmplitudes);
            newAmplitudeData.ModeValue = modeAndOccurrence.Item1;
            newAmplitudeData.ModeOccurrences = modeAndOccurrence.Item2;
            newAmplitudeData.RelativeAmplitudeRMSValue = GetAmplitudeRootMeanSquare(relativeAmplitudes);
            newAmplitudeData.AmplitudeDBFS = 20 * Math.Log(newAmplitudeData.RelativeAmplitudeRMSValue, 10);
            newAmplitudeDataList.Add(newAmplitudeData);
            return newAmplitudeDataList;
        }

        /// <summary>
        /// Helper method to split an array into several smaller arrays.
        /// </summary>
        /// <typeparam name="T">The type of the array.</typeparam>
        /// <param name="ArrayToSplit">The array which shall be splitted</param>
        /// <param name="SizeOfSmallerArrays">The size of the smaller arrays which will be returned</param>
        /// <returns>An array containing smaller arrays.</returns>
        public static IEnumerable<IEnumerable<T>> Split<T>(this T[] ArrayToSplit, int SizeOfSmallerArrays)
        {
            for (var i = 0; i < (float)ArrayToSplit.Length / SizeOfSmallerArrays; i++)
            {
                yield return ArrayToSplit.Skip(i * SizeOfSmallerArrays).Take(SizeOfSmallerArrays);
            }
        }

    }
}
