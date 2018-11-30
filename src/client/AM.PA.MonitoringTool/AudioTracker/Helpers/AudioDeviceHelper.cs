using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioTracker.Helpers
{
    class AudioDeviceHelper
    {
        internal static int? GetDeviceNumberFromDeviceName(string DeviceName)
        {
            int NumberOfWaveInDevices = WaveIn.DeviceCount;
            for (int i = 0; i < NumberOfWaveInDevices; i++)
            {
                WaveInCapabilities deviceInfo = WaveIn.GetCapabilities(i);
                if (deviceInfo.ProductName == DeviceName)
                {
                    return i;
                }
            }
            return null;

            /*
            int NumberOfWaveInDevices = WaveIn.DeviceCount;
            for (int i = 0; i < NumberOfWaveInDevices; i++)
            {
                WaveInCapabilities deviceInfo = WaveIn.GetCapabilities(i);
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
                    if (deviceInfo.ProductName == deviceName31characters)
                    {
                        return i;
                    }
                }
            }
            */
        }

        internal static MMDevice GetDeviceFromDeviceName(string DeviceName)
        {
            MMDeviceEnumerator deviceEnumerator = new MMDeviceEnumerator();
            MMDeviceCollection AudioDevices = deviceEnumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);
            foreach (MMDevice CurrentDevice in AudioDevices)
            {
                if (CurrentDevice.DeviceFriendlyName == DeviceName)
                {
                    return CurrentDevice;
                }
            }
            return null;
        }

        internal static MMDevice GetAudioDeviceFromDeviceNumber(int AudioDeviceNumber)
        {
            int waveInDevices = WaveIn.DeviceCount;
            for (int waveInDevice = 0; waveInDevice < waveInDevices; waveInDevice++)
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
    }
}
