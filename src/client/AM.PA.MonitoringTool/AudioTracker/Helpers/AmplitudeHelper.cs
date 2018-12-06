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
    class AmplitudeHelper
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

    }
}
