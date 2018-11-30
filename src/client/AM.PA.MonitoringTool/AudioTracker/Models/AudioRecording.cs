using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioTracker.Models
{
    internal class AudioRecording
    {
        public DateTime RecordingStartTime { get; set; }
        public DateTime RecordingEndTime { get; set; }
        public string FilenameMp3 { get; set; }
        public string FilenameLium { get; set; }
        public string LiumConsoleOutput { get; set; }
        public int NumSamples { get; set; }
        public int LengthMilliseconds { get; set; }
        public double MinValueRelative { get; set; }
        public double MaxValueRelative { get; set; }
        public double AvgValueRelative { get; set; }
        public double ModeValueRelative { get; set; }
        public double ModeValueOccurrences;
        public double MedianValueRelative { get; set; }
        public double PeakToPeakLevel { get; set; }
        public bool IsMute { get; set; }

        public AudioRecording(DateTime RecordingStartTime, DateTime RecordingEndTime, string FilenameMp3, string FilenameLium, string LiumConsoleOutput, int NumSamples, 
            int LengthMilliseconds, double MinValueRelative, double MaxValueRelative, double AvgValueRelative, double ModeValueRelative, double ModeValueOccurrences, 
            double MedianValueRelative, double PeakToPeakLevel, bool IsMute)
        {
            this.RecordingStartTime = RecordingStartTime;
            this.RecordingEndTime = RecordingEndTime;
            this.FilenameMp3 = FilenameMp3;
            this.FilenameLium = FilenameLium;
            this.LiumConsoleOutput = LiumConsoleOutput;
            this.NumSamples = NumSamples;
            this.LengthMilliseconds = LengthMilliseconds;
            this.MinValueRelative = MinValueRelative;
            this.MaxValueRelative = MaxValueRelative;
            this.AvgValueRelative = AvgValueRelative;
            this.ModeValueRelative = ModeValueRelative;
            this.ModeValueOccurrences = ModeValueOccurrences;
            this.MedianValueRelative = MedianValueRelative;
            this.PeakToPeakLevel = PeakToPeakLevel;
            this.IsMute = IsMute;
        }

    }
}
