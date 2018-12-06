// Created by Christoph Vogel (christoph.vogel@uzh.ch) from the University of Zurich
// Created: 2018-10-01
// 
// Licensed under the MIT License.

using AudioTracker.Models;
using NAudio.CoreAudioApi;
using System;

namespace AudioTracker
{
    public static class Settings
    {
        //Data Collection Settings
        internal const string Name = "Audio Tracker";
        internal const string TRACKER_ENEABLED_SETTING = "AudioTrackerEnabled";
        public const bool IsEnabled = true; // for initial release deactivating is not enabled
        public const bool IsMuted = false;
        public const bool IS_RAW_RECORDING_ENABLED = true;
        public const bool IS_DEVICE_EVENT_NOTIFICATION_ENABLED = true;
        public const bool IS_DEVICE_EVENT_RECORDING_ENABLED = true;

        //Audio recording settings
        internal static int RecordingSampleRate = 16000; // in kHz
        internal static int RecordingNumberOfChannels = 1;
        internal static string RecordingFileNamePrefix = "audio";
        internal static int AudioRecordingChunkLength = 30000; // in milliseconds

        internal static AudioDevice InputAudioDevice;
        //public static string InputAudioDeviceName;
        //internal static int? InputAudioDeviceNumber = 0;
        //internal static MMDevice InputAudioDevice;
        //internal static string InputAudioDeviceID;

        //Database table names
        internal static readonly string AUDIO_TABLE_NAME = "audio";
        internal static readonly string AUDIO_RECORDINGS_TABLE_NAME = "audio_recording";
        internal static readonly string AUDIO_VOLUME_TABLE_NAME = "audio_volume";

        //Daemon settings
        internal const string TRACKER_NAME = "Audio Tracker";
        //private const int liumRunInterval = 30; // in seconds

        public static string GetAudioDeviceFriendlyName()
        {
            return InputAudioDevice.FriendlyName;
        }
    }
}
