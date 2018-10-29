// Created by Christoph Vogel (christoph.vogel@uzh.ch) from the University of Zurich
// Created: 2018-10-01
// 
// Licensed under the MIT License.

using System;

namespace AudioTracker
{
    public static class Settings
    {
        //Data Collection Settings
        public const bool IsEnabled = true; // for initial release deactivating is not enabled
        public const bool IsMuted = false;
        public const bool IS_RAW_RECORDING_ENABLED = true;
        internal const string Name = "Audio Tracker";
        internal const string TRACKER_ENEABLED_SETTING = "AudioTrackerEnabled";
        internal const int SAVE_TO_DATABASE_INTERVAL = 30 * 1000;

        internal static string AudioDevice;
        internal static int Channels = 2;
        internal static int SampleRate = 44100; // in kHz
        //TODO: recording setting

        //Deamon
        internal const string TRACKER_NAME = "Audio Tracker";
        internal const int SYNCHRONIZE_INTERVALL_FIRST = 2 * 60 * 1000; //2 minutes
        internal const int SYNCHRONIZE_INTERVALL_SECOND = 20 * 60 * 1000; //20 minutes

        private static int liumRunInterval = 30; // in seconds
        private const int MouseSnapshotIntervalInSeconds = 1;
        public static TimeSpan MouseSnapshotInterval = TimeSpan.FromSeconds(MouseSnapshotIntervalInSeconds);

        internal const int UserInputAggregationIntervalInSeconds = 60; // save one entry every 60 seconds into the database => if changed, make sure to change tsStart and tsEnd in SaveToDatabaseTick
        internal static TimeSpan UserInputAggregationInterval = TimeSpan.FromSeconds(UserInputAggregationIntervalInSeconds);

        //Database table names
        internal static readonly string AUDIO_TABLE_NAME = "audio";
        internal static readonly string AUDIO_RECORDINGS_TABLE_NAME = "audio_recordings";

        //Database field names
        internal static readonly string DOWNLOAD_START_DATE = "FitbitDownloadStartDate";
        internal static readonly string LAST_SYNCED_DATE = "FitbitLastSynced";

    }
}
