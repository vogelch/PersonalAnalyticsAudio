// Created by Christoph Vogel (christoph.vogel@uzh.ch) from the University of Zurich
// Created: 2018-10-01
// 
// Licensed under the MIT License.

using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioTracker.Models
{
    class AudioDevice
    {
        // MMDevice properties
        public string FriendlyName { get; set; } // friendly name as returned by MMDevice.FriendlyName
        public string DeviceFriendlyName { get; set; } // device friendly name as returned by MMDevice.DeviceFriendlyName
        public string ID { get; set; } // device ID as returned by MMDevice.ID; e.g. {0.0.1.00000000}.{af645e92-996d-46ab-93ef-f6a857ebe2c9}
        public MMDevice DeviceInstance { get; set; }

        // WaveIn properties
        public int? DeviceNumber { get; set; } // device number as assigned by WaveIn; this is needed to initialize recording
        public string ProductName { get; set; } // product name as returned by WaveIn.GetCapabilities(i).ProductName; this is identical to the MMDevice.FriendlyName but cropped to 32 characters
        public Guid ProductGuid { get; set; } // product GUID as returned by WaveIn.GetCapabilities(i).ProductGuid; e.g. abcc5b9a-c263-463b-a72f-a5bf64c86eba; this is *not* identical to the MM Device ID
        public Guid NameGuid { get; set; } // name GUID as returned by WaveIn.GetCapabilities(i).NameGuid; e.g. fc575dd4-2f44-463b-a72f-a5bf64c86eba
        public Guid ManufacturerGuid { get; set; } // manufacturer GUID as returned by WaveIn.GetCapabilities(i).ManufacturerGuid; e.g. 4e1cfa5e-1679-463b-a72f-a5bf64c86eba
        public int? Channels { get; set; } // channels of device as returned by WaveIn.GetCapabilities(i).Channels
    }
}
