// Created by Christoph Vogel (christoph.vogel@uzh.ch) from the University of Zurich
// Created: 2018-10-01
// 
// Licensed under the MIT License.

using System.ComponentModel;

namespace AudioTracker.Models
{
    class LiumSegment
    {
        internal int InputFileName { get; set; } // instead of show name
        internal int ChannelNumber { get; set; } // the channel number
        internal int StartIndexInFeatures { get; set; } // the start of the segment in features
        internal int LenghtInFeatures { get; set; } // the length of the segment in features
        internal string SpeakerLabel { get; set; } // the speaker label
        internal LiumGender SpeakerGender { get; set; } // the speaker gender
        internal LiumBandType BandType { get; set; } // the type of band
        internal LiumEnvironment Environment { get; set; } // the type of environment

        internal LiumSegment(string[] SegmentProperties)
        {
            StartIndexInFeatures = int.Parse(SegmentProperties[2]);
            LenghtInFeatures = int.Parse(SegmentProperties[3]);
            SpeakerLabel = SegmentProperties[7];
            SpeakerGender = GetGenderFromFlag(SegmentProperties[4]);
            BandType = GetBandTypeFromFlag(SegmentProperties[5]);
            Environment = GetEnvironmentFromFlag(SegmentProperties[6]);
        }

        internal LiumSegment(int InputFileName, int ChannelNumber, int StartIndexInFeatures, int LenghtInFeatures, string SpeakerLabel, 
            LiumGender SpeakerGender, LiumBandType BandType, LiumEnvironment Environment)
        {
            this.InputFileName = InputFileName;
            this.ChannelNumber = ChannelNumber;
            this.StartIndexInFeatures = StartIndexInFeatures;
            this.LenghtInFeatures = LenghtInFeatures;
            this.SpeakerLabel = SpeakerLabel;
            this.SpeakerGender = SpeakerGender;
            this.BandType = BandType;
            this.Environment = Environment;
        }

        internal static LiumGender GetGenderFromFlag(string Flag)
        {
            switch (Flag)
            {
                case "M":
                    return LiumGender.MALE;
                case "F":
                    return LiumGender.FEMALE;
                case "U":
                    return LiumGender.UNKNOWN;
                default:
                    return LiumGender.UNKNOWN;
            }
        }

        internal static LiumBandType GetBandTypeFromFlag(string Flag)
        {
            switch (Flag)
            {
                case "S":
                    return LiumBandType.STUDIO;
                case "T":
                    return LiumBandType.TELEPHONE;
                case "U":
                    return LiumBandType.UNKNOWN;
                default:
                    return LiumBandType.UNKNOWN;
            }
        }

        internal static LiumEnvironment GetEnvironmentFromFlag(string Flag)
        {
            switch (Flag)
            {
                case "M":
                    return LiumEnvironment.MUSIC;
                case "S":
                    return LiumEnvironment.SPEECH_ONLY;
                case "U":
                    return LiumEnvironment.UNKNOWN;
                default:
                    return LiumEnvironment.UNKNOWN;
            }
        }

        // getStartAsTimestamp
        // getDurationInSeconds
        // getEndAsTimestamp
    }

    internal enum LiumGender
    {
        [Description("male")]
        MALE,
        [Description("female")]
        FEMALE,
        [Description("unknown")]
        UNKNOWN
    }

    internal enum LiumBandType
    {
        [Description("studio")]
        STUDIO,
        [Description("telephone")]
        TELEPHONE,
        [Description("unknown")]
        UNKNOWN
    }

    internal enum LiumEnvironment // music, speech only
    {
        [Description("music")]
        MUSIC,
        [Description("speech only")]
        SPEECH_ONLY,
        [Description("unknown")]
        UNKNOWN
    }
}
