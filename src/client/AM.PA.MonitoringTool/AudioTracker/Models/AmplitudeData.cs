// Created by Christoph Vogel (christoph.vogel@uzh.ch) from the University of Zurich
// Created: 2018-10-01
// 
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioTracker.Models
{
    class AmplitudeData
    {
        //public NumberOfSamples // Duration // Bit-Tiefe // Channels??
        public DateTime StartTime { get; set; }
        public double MaxValue { get; set; }
        public double MinValue { get; set; }
        public double AvgValue { get; set; }
        public double ModeValue { get; set; }
        public int ModeOccurrences { get; set; }
        public double RelativeAmplitudeRMSValue { get; set; }
        public double AmplitudeDBFS { get; set; }
    }
}
