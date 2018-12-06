// Created by Christoph Vogel (christoph.vogel@uzh.ch) from the University of Zurich
// Created: 2018-10-01
// 
// Licensed under the MIT License.

using AudioTracker.Models;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using Shared;
using System.Collections.Generic;

namespace AudioTracker.Helpers
{
    public class AudioDeviceHelper
    {
        internal static int? GetDeviceNumberFromFriendlyName(string FriendlyName)
        {
            int NumberOfWaveInDevices = WaveIn.DeviceCount;
            for (int i = 0; i < NumberOfWaveInDevices; i++)
            {
                WaveInCapabilities deviceInfo = WaveIn.GetCapabilities(i);
                string deviceName31characters;
                if (FriendlyName.Length > 31)
                {
                    deviceName31characters = FriendlyName.Substring(0, 31);
                }
                else
                {
                    deviceName31characters = FriendlyName;
                }
                if (deviceInfo.ProductName == deviceName31characters)
                {
                    return i;
                }
            }
            return null;
        }

        internal static MMDevice GetDeviceFromDeviceName(string DeviceName)
        {
            MMDeviceCollection AudioDevices = GetActiveInputDevicesCollection();
            foreach (MMDevice CurrentDevice in AudioDevices)
            {
                if (CurrentDevice.FriendlyName == DeviceName)
                {
                    return CurrentDevice;
                }
            }
            return null;
        }

        internal static bool SetPropertiesFromDeviceNumber(AudioDevice TargetDevice, int DeviceNumber)
        {
            if (TargetDevice.DeviceNumber != DeviceNumber)
            {
                return false;
            }
            else
            {
                WaveInCapabilities deviceInfo = WaveIn.GetCapabilities(DeviceNumber);
                TargetDevice.ProductName = deviceInfo.ProductName;
                TargetDevice.ProductGuid = deviceInfo.ProductGuid;
                TargetDevice.NameGuid = deviceInfo.NameGuid;
                TargetDevice.ManufacturerGuid = deviceInfo.ManufacturerGuid;
                TargetDevice.Channels = deviceInfo.Channels;
                return true;
            }
        }

        public static MMDeviceCollection GetActiveInputDevicesCollection()
        {
            MMDeviceEnumerator deviceEnumerator = new MMDeviceEnumerator();
            MMDeviceCollection AudioDevices = deviceEnumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);
            return AudioDevices;
        }

        public static List<string> GetActiveInputDevicesList()
        {
            List<string> DevicesList = new List<string>();
            MMDeviceCollection AudioDevices = GetActiveInputDevicesCollection();
            foreach (MMDevice CurrentDevice in AudioDevices)
            {
                DevicesList.Add(CurrentDevice.FriendlyName);
            }
            /*
            var deviceList = new List<string>();
            int waveInDevices = WaveIn.DeviceCount;
            for (int waveInDevice = 0; waveInDevice < waveInDevices; waveInDevice++)
            {
                string deviceProductName = WaveIn.GetCapabilities(waveInDevice).ProductName;
                deviceList.Add(deviceProductName);
            }
            */
            return DevicesList;
        }

        /*
        internal static MMDevice GetAudioDeviceFromDeviceNumber(int AudioDeviceNumber)
        {
            int numberOfWaveInDevices = WaveIn.DeviceCount;
            for (int waveInDevice = 0; waveInDevice < numberOfWaveInDevices; waveInDevice++)
            {
                if (waveInDevice == AudioDeviceNumber)
                {
                    WaveInCapabilities deviceInfo = WaveIn.GetCapabilities(waveInDevice);
                    MMDeviceEnumerator deviceEnumerator = new MMDeviceEnumerator();
                    MMDeviceCollection AudioDevices = deviceEnumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);
                    foreach (MMDevice CurrentDevice in AudioDevices)
                    {
                        string deviceName31characters;
                        if (CurrentDevice.FriendlyName.Length > 31)
                        {
                            deviceName31characters = CurrentDevice.FriendlyName.Substring(0, 31);
                        }
                        else
                        {
                            deviceName31characters = CurrentDevice.FriendlyName;
                        }
                        if (waveInDevice == Settings.inputAudioDeviceNumber && deviceInfo.ProductName == deviceName31characters)
                        {
                            return CurrentDevice;
                        }
                    }
                }
            }
            return null;
        }
        */

    }
}
