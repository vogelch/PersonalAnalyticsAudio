// Created by Christoph Vogel (christoph.vogel@uzh.ch) from the University of Zurich
// Created: 2018-10-01
// 
// Licensed under the MIT License.

using NAudio.CoreAudioApi;
using System;

namespace AudioTracker
{
    public static class Settings
    {
        //Data Collection Settings
        public const bool IsEnabled = true; // for initial release deactivating is not enabled
        public const bool IsMuted = false;
        public const bool IS_RAW_RECORDING_ENABLED = true;
        public const bool IS_DEVICE_EVENT_NOTIFICATION_ENABLED = true;
        public const bool IS_DEVICE_EVENT_RECORDING_ENABLED = true;
        internal const string Name = "Audio Tracker";
        internal const string TRACKER_ENEABLED_SETTING = "AudioTrackerEnabled";

        internal static int? inputAudioDeviceNumber = 0;
        public static string inputAudioDeviceName; //TODO: look into access modifier
        internal static MMDevice inputAudioDevice;
        internal static int Channels = 2;
        internal static int SampleRate = 44100; // in kHz
        internal static int AudioRecordingChunkLength = 30000; // in milliseconds
        //TODO: recording setting

        //Deamon
        internal const string TRACKER_NAME = "Audio Tracker";
        private const int liumRunInterval = 30; // in seconds

        internal const int UserInputAggregationIntervalInSeconds = 60; // save one entry every 60 seconds into the database => if changed, make sure to change tsStart and tsEnd in SaveToDatabaseTick
        internal static TimeSpan UserInputAggregationInterval = TimeSpan.FromSeconds(UserInputAggregationIntervalInSeconds);

        //Database table names
        internal static readonly string AUDIO_TABLE_NAME = "audio";
        internal static readonly string AUDIO_RECORDINGS_TABLE_NAME = "audio_recording";
        internal static readonly string AUDIO_VOLUME_TABLE_NAME = "audio_volume";
    }
}
