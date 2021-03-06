﻿// Created by Christoph Vogel (christoph.vogel@uzh.ch) from the University of Zurich
// Created: 2018-10-01
// 
// Licensed under the MIT License.

using Shared;
using Shared.Data;
using Shared.Data.Extractors;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Globalization;
using AudioTracker.Models;

namespace AudioTracker.Data
{
    public class Queries
    {
        internal static readonly KeyValuePair<string, string>[] AUDIO_RECORDINGS_COLUMN_NAMES = new KeyValuePair<string, string>[]
        {
            new KeyValuePair<string, string>("id", "INTEGER PRIMARY KEY"),
            new KeyValuePair<string, string>("recording_start_time", "DATETIME"),
            new KeyValuePair<string, string>("recording_end_time", "DATETIME"),
            // audio device name in use
            new KeyValuePair<string, string>("filename_mp3", "TEXT"),
            new KeyValuePair<string, string>("filename_lium", "TEXT"),
            new KeyValuePair<string, string>("lium_console_output", "TEXT"),
            new KeyValuePair<string, string>("num_samples", "INTEGER"),
            new KeyValuePair<string, string>("length_milliseconds", "INTEGER"),
            new KeyValuePair<string, string>("min_value_relative", "REAL"),
            new KeyValuePair<string, string>("max_value_relative", "REAL"),
            new KeyValuePair<string, string>("avg_value_relative", "REAL"),
            new KeyValuePair<string, string>("mode_value_relative", "REAL"),
            new KeyValuePair<string, string>("mode_value_occurrences", "REAL"),
            new KeyValuePair<string, string>("median_value_relative", "REAL"),
            new KeyValuePair<string, string>("peak_to_peak_level", "REAL"),
            new KeyValuePair<string, string>("is_mute", "BOOLEAN"),
            new KeyValuePair<string, string>("storing_timestamp", "DATETIME DEFAULT CURRENT_TIMESTAMP")
        };

        internal static readonly KeyValuePair<string, string>[] AUDIO_VOLUME_COLUMN_NAMES = new KeyValuePair<string, string>[]
        {
            new KeyValuePair<string, string>("id", "INTEGER PRIMARY KEY"),
            new KeyValuePair<string, string>("audio_timestamp", "DATETIME"),
            new KeyValuePair<string, string>("rms_relative_value", "REAL"),
            new KeyValuePair<string, string>("rms_dbfs", "REAL"),
            new KeyValuePair<string, string>("storing_timestamp", "DATETIME DEFAULT CURRENT_TIMESTAMP")
        };

        internal static readonly KeyValuePair<string, string>[] DEVICE_EVENT_COLUMN_NAMES = new KeyValuePair<string, string>[]
        {
            //new KeyValuePair<string, string>("id", "INTEGER PRIMARY KEY"),
            //new KeyValuePair<string, string>("audio_timestamp", "DATETIME"),
            //new KeyValuePair<string, string>("rms_relative_value", "REAL"),
            //new KeyValuePair<string, string>("storing_timestamp", "DATETIME DEFAULT CURRENT_TIMESTAMP")
        };

        internal static readonly KeyValuePair<string, string>[] LIUM_ANALYSIS_SEGMENT_COLUMN_NAMES = new KeyValuePair<string, string>[]
        {
            new KeyValuePair<string, string>("id", "INTEGER PRIMARY KEY"),
            new KeyValuePair<string, string>("start_absolute_time", "DATETIME"),
            new KeyValuePair<string, string>("end_absolute_time", "DATETIME"),
            new KeyValuePair<string, string>("filename_lium", "TEXT"),
            new KeyValuePair<string, string>("cluster_id", "INTEGER"),
            new KeyValuePair<string, string>("show_name", "TEXT"),
            new KeyValuePair<string, string>("channel_number", "INTEGER"),
            new KeyValuePair<string, string>("start_index", "TEXT"),
            new KeyValuePair<string, string>("length", "TEXT"),
            new KeyValuePair<string, string>("speaker_label", "TEXT"),
            new KeyValuePair<string, string>("speaker_gender", "TEXT"),
            new KeyValuePair<string, string>("speaker_band_type", "TEXT"),
            new KeyValuePair<string, string>("speaker_environment", "TEXT"),
            new KeyValuePair<string, string>("storing_timestamp", "DATETIME DEFAULT CURRENT_TIMESTAMP")
        };

        internal static readonly KeyValuePair<string, string>[] LIUM_ANALYSIS_CLUSTER_COLUMN_NAMES = new KeyValuePair<string, string>[]
        {
            new KeyValuePair<string, string>("id", "INTEGER PRIMARY KEY"),
            new KeyValuePair<string, string>("cluster_label", "TEXT"),
            new KeyValuePair<string, string>("filename_lium", "TEXT"),
            new KeyValuePair<string, string>("score_fs", "REAL"),
            new KeyValuePair<string, string>("score_ft", "REAL"),
            new KeyValuePair<string, string>("score_ms", "REAL"),
            new KeyValuePair<string, string>("score_mt", "REAL"),
            new KeyValuePair<string, string>("merge_text", "TEXT"),
            new KeyValuePair<string, string>("merge_with_value", "REAL"),
            new KeyValuePair<string, string>("storing_timestamp", "DATETIME DEFAULT CURRENT_TIMESTAMP")
        };

        private static readonly string QUERY_CREATE = "CREATE TABLE IF NOT EXISTS " + Settings.AUDIO_TABLE_NAME + " ("
                                                                            + "id INTEGER PRIMARY KEY, "
                                                                            + "time TEXT, "
                                                                            + "DATE_OF_SLEEP" + " TEXT UNIQUE, "
                                                                            + "VALUE" + " INTEGER)";

        //private static readonly string QUERY_INDEX = "CREATE INDEX IF NOT EXISTS windows_activity_ts_start_idx ON " + Settings.DbTable + " (tsStart);";

        #region Daemon Queries
        
        internal static void CreateAudioTable()
        {
            try
            {
                var res = Database.GetInstance().ExecuteDefaultQuery(QUERY_CREATE);
                //if (res == 1) Database.GetInstance().ExecuteDefaultQuery(QUERY_INDEX); // add index when table was newly created
            }
            catch (Exception e)
            {
                Logger.WriteToLogFile(e);
            }
        }

        private static string GetAudioRecordingInsertQuery(AudioRecording newAudioRecording)
        {
            string query = "INSERT INTO " + Settings.AUDIO_RECORDINGS_TABLE_NAME +
                " (recording_start_time, recording_end_time, filename_mp3, filename_lium, lium_console_output, num_samples, " +
                "length_milliseconds, min_value_relative, max_value_relative, avg_value_relative, mode_value_relative, mode_value_occurrences, " +
                "median_value_relative, peak_to_peak_level, is_mute) VALUES (" +
                "'" + newAudioRecording.RecordingStartTime.ToString("yyyy-MM-dd HH:mm:ss.ffff") + "'" + ", " +
                "'" + newAudioRecording.RecordingEndTime.ToString("yyyy-mm-dd HH:MM:ss.ffff") + "'" + ", " +
                "'" + newAudioRecording.FilenameMp3 + "'" + ", " +
                "'" + newAudioRecording.FilenameLium + "'" + ", " +
                "'" + newAudioRecording.LiumConsoleOutput + "'" + ", " +
                newAudioRecording.NumSamples + ", " +
                newAudioRecording.LengthMilliseconds + ", " +
                newAudioRecording.MinValueRelative + ", " +
                newAudioRecording.MaxValueRelative + ", " +
                newAudioRecording.AvgValueRelative + ", " +
                newAudioRecording.ModeValueRelative + ", " +
                newAudioRecording.ModeValueOccurrences + ", " +
                "NULL, " + //newAudioRecording.MedianValueRelative + ", " +
                "NULL, " + //newAudioRecording.PeakToPeakLevel + ", " +
                (newAudioRecording.IsMute ? 1 : 0) + ")";
            return query;
        }

        private static string GetAudioVolumeInsertQuery(List<AmplitudeData> NewAudioVolumeData)
        {
            string query = "INSERT INTO " + Settings.AUDIO_VOLUME_TABLE_NAME +
                " (audio_timestamp, rms_relative_value, rms_dbfs) VALUES ";
            foreach (AmplitudeData CurrentAmplitudeData in NewAudioVolumeData)
            {
                query += "('" + CurrentAmplitudeData.StartTime + "', " + CurrentAmplitudeData.RelativeAmplitudeRMSValue + ", " + CurrentAmplitudeData.AmplitudeDBFS + "), ";
            }
            query = query.Substring(0, query.Length - 2);

            return query;
        }

        private static string GetLiumAnalysisClusterInsertQuery(LiumCluster newLiumCluster, string liumFilename)
        {
            string MergeTextForInsertion = "NULL";
            if (newLiumCluster.MergeText != null)
            {
                MergeTextForInsertion = "'" + newLiumCluster.MergeText + "'";
            }
            string MergeWithForInsertion = "NULL";
            if (newLiumCluster.MergeWith != null)
            {
                MergeWithForInsertion = newLiumCluster.MergeWith.ToString();
            }
            string query = "INSERT INTO " + Settings.LIUM_ANALYSIS_CLUSTERS_TABLE_NAME +
                " (cluster_label, filename_lium, score_fs, score_ft, score_ms, score_mt, merge_text, merge_with_value) VALUES (" + 
                "'" + newLiumCluster.ClusterLabel + "'" + ", " +
                "'" + liumFilename + "'" + ", " +
                newLiumCluster.ScoreFS + ", " +
                newLiumCluster.ScoreFT + ", " +
                newLiumCluster.ScoreMS + ", " +
                newLiumCluster.ScoreMT + ", " +
                MergeTextForInsertion + ", " +
                MergeWithForInsertion + ")";
            return query;
        }

        private static string GetLiumAnalysisSegmentInsertQuery(LiumSegment NewLiumSegment, string LiumFilename, int ClusterID)
        {
            string query = "INSERT INTO " + Settings.LIUM_ANALYSIS_SEGMENTS_TABLE_NAME +
                " (start_absolute_time, end_absolute_time, filename_lium, cluster_id, show_name, channel_number, start_index, length, " +
                "speaker_label, speaker_gender, speaker_band_type, speaker_environment) VALUES (" +
                "'" + NewLiumSegment.StartTime.ToString("yyyy-MM-dd HH:mm:ss.ffff") + "'" + ", " +
                "'" + NewLiumSegment.EndTime.ToString("yyyy-MM-dd HH:mm:ss.ffff") + "'" + ", " +
                "'" + LiumFilename + "'" + ", " +
                ClusterID + ", " +
                "'" + "" + "'" + ", " +
                NewLiumSegment.ChannelNumber + ", " +
                NewLiumSegment.StartIndexInFeatures + ", " +
                NewLiumSegment.LenghtInFeatures + ", " +
                "'" + NewLiumSegment.SpeakerLabel + "'" + ", " + // TODO: redundant
                "'" + NewLiumSegment.SpeakerGender + "'" + ", " +
                "'" + NewLiumSegment.BandType + "'" + ", " +
                "'" + NewLiumSegment.Environment + "'" + ")";
            return query;
        }

        /// <summary>
        /// Inserts meta data about an audio recording into the audio table
        /// </summary>
        /// <param name="NewAudioRecording">audio recording object with meta data to store into the database</param>
        internal static void StoreAudioRecording(AudioRecording NewAudioRecording)
        {
            //private static readonly string QUERY_INSERT = "INSERT INTO " + Settings.DbTable + " (time, tsStart, tsEnd, window, process) VALUES ({0}, {1}, {2}, {3}, {4});";
            try
            {
                string QueryRecording = GetAudioRecordingInsertQuery(NewAudioRecording);
                Database.GetInstance().ExecuteDefaultQuery(QueryRecording);
                //long? newRowID = Database.GetInstance().ExecuteInsertQuery(query);
                //Logger.WriteToConsole("Inserted new audio recording with row ID " + newRowID + ".");
            }
            catch (Exception e)
            {
                Logger.WriteToLogFile(e);
            }
        }

        /// <summary>
        /// Inserts a set of audio volume data points into the audio volume data
        /// </summary>
        /// <param name="NewAudioVolumeData">List with AmplitudeData to store in the database</param>
        internal static void StoreAudioVolumeData(List<AmplitudeData> NewAudioVolumeData)
        {
            try
            {
                string QueryVolume = GetAudioVolumeInsertQuery(NewAudioVolumeData);
                Database.GetInstance().ExecuteDefaultQuery(QueryVolume);
            }
            catch (Exception e)
            {
                Logger.WriteToLogFile(e);
            }
        }

        /// <summary>
        /// Inserts LIUM analysis data to the database
        /// </summary>
        /// <param name="NewLiumClusterSet">list of LIUM clusters to store into the database</param>
        internal static void StoreLiumAnalysisResult(List<LiumCluster> NewLiumClusterSet, string LiumFilename)
        {
            try
            {
                foreach (LiumCluster CurrentCluster in NewLiumClusterSet)
                {
                    string queryCluster = GetLiumAnalysisClusterInsertQuery(CurrentCluster, LiumFilename);
                    Database.GetInstance().ExecuteDefaultQuery(queryCluster);

                    if (CurrentCluster.SegmentSet == null)
                    {
                        // this should not be possible to happen
                        Logger.WriteToConsole("Error: Segment set empty.");
                    }
                    else
                    {
                        foreach (LiumSegment CurrentSegment in CurrentCluster.SegmentSet)
                        {
                            int ClusterID = 0;
                            string querySegment = GetLiumAnalysisSegmentInsertQuery(CurrentSegment, LiumFilename, ClusterID);
                            Database.GetInstance().ExecuteDefaultQuery(querySegment);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.WriteToLogFile(e);
            }
        }

        //internal static void StoreAudioRecording(AudioRecording newAudioRecording, LIUMResult newLIUMResult)

        //TODO: move to Shared
        internal static void UpdateAllColumnsOfTable(string TableName, KeyValuePair<string, string>[] ColumnNamesAndTypes)
        {
            try
            {
                if (Database.GetInstance().HasTable(TableName))
                {
                    // update columns
                    foreach (KeyValuePair<string, string> Entry in ColumnNamesAndTypes)
                    {
                        try
                        {
                            if (!Database.GetInstance().HasTableColumn(TableName, Entry.Key))
                            {
                                Database.GetInstance().ExecuteDefaultQuery("ALTER TABLE " + TableName + " ADD COLUMN " + Entry.Key + " " + Entry.Value + ";");
                                Logger.WriteToConsole("Updating database table '" + TableName + "': inserting column '" + Entry.Key + "'.");
                            }
                        }
                        catch (Exception e)
                        {
                            Logger.WriteToLogFile(e);
                        }
                    }

                    //TODO: what happens if the type of an already existing table column is changed?

                    //TODO: Database.GetInstance().ExecuteDefaultQuery(QUERY_INDEX);

                    //TODO: migrate data
                    //MigrateWindowsActivityTable();
                }
            }
            catch (Exception e)
            {
                Logger.WriteToLogFile(e);
            }
        }

        /*
        internal static void UpdateDatabaseTables(int version)
        {
            try
            {
                // database update 2017-07-20 (added two columns to 'windows_activity' table: tsStart and tsEnd)
                // need to migrate the existing values in the table
                if (version == 4)
                {
                    if (Database.GetInstance().HasTable(Settings.DbTable))
                    {
                        // update table: add columns & index
                        Database.GetInstance().ExecuteDefaultQuery("ALTER TABLE " + Settings.DbTable + " ADD COLUMN tsStart TEXT;");
                        Database.GetInstance().ExecuteDefaultQuery("ALTER TABLE " + Settings.DbTable + " ADD COLUMN tsEnd TEXT;");
                        Database.GetInstance().ExecuteDefaultQuery(QUERY_INDEX);

                        // migrate data (set tsStart / tsEnd)
                        MigrateWindowsActivityTable();
                    }
                }
            }
            catch (Exception e)
            {
                Logger.WriteToLogFile(e);
            }
        }
        */

        /*
        /// <summary>
        /// Updates all entries of the audio recordings table
        /// </summary>
        private static void MigrateAudioRecordingsTable()
        {
            try
            {
                // copy time -> tsStart
                Database.GetInstance().ExecuteDefaultQuery("UPDATE " + Settings.DbTable + " set tsStart = time;");

                // copy tsStart of next item to tsEnd of this item
                var QUERY_UPDATE_TSEND = "UPDATE " + Settings.DbTable + " SET tsEnd = (SELECT t2.tsStart FROM " + Settings.DbTable + " t2 WHERE windows_activity.id + 1 = t2.id LIMIT 1);";
                Database.GetInstance().ExecuteDefaultQuery(QUERY_UPDATE_TSEND);

                // set tsEnd
                //var querySelect = "SELECT id, tsStart FROM " + Settings.DbTable + ";"; // LIMIT 10000;";
                //var table = Database.GetInstance().ExecuteReadQuery(querySelect);

                //if (table != null)
                //{
                //    WindowsActivity _previousItem = null;

                //    foreach (DataRow row in table.Rows)
                //    {
                //        // read values for this item
                //        var currentItem_Id = (long)row["id"];
                //        var urrentItem_tsStart = DateTime.Parse((string)row["tsStart"], CultureInfo.InvariantCulture);

                //        // update and store previous item
                //        if (_previousItem != null)
                //        {
                //            var tsEndString = (_previousItem.StartTime.Day == urrentItem_tsStart.Day)
                //                                ? Database.GetInstance().QTime2(urrentItem_tsStart) // previous items' tsEnd is current items' tsStart
                //                                : "''"; // if end of day: keep empty

                //            var queryUpdate = "UPDATE " + Settings.DbTable + " SET tsEnd = " + tsEndString + " WHERE id = '" + _previousItem.Id + "';";
                //            Database.GetInstance().ExecuteDefaultQuery(queryUpdate);
                //            //Logger.WriteToConsole(queryUpdate);
                //        }

                //        // set new previous item
                //        _previousItem = new WindowsActivity() { Id = (int)currentItem_Id, StartTime = urrentItem_tsStart }; //tsEnd is not yet known
                //    }
                //    table.Dispose();
                //}
            }
            catch (Exception e)
            {
                Logger.WriteToLogFile(e);
            }
        }
        */

        /*
        /// <summary>
        /// Saves the timestamp, start and end, process name and window title into the database.
        /// 
        /// In case the user doesn't want the window title to be stored (For privacy reasons),
        /// it is obfuscated.
        /// </summary>
        /// <param name="window"></param>
        /// <param name="process"></param>
        internal static void InsertSnapshot(WindowsActivityEntry entry)
        {
            try
            {
                // if user is browsing in InPrivate-mode, obfuscate window title (doesn't work in Google Chrome!)
                if (ProcessToActivityMapper.IsBrowser(entry.Process) && Settings.InkognitoBrowsingTerms.Any(entry.WindowTitle.ToLower().Contains))
                {
                    entry.WindowTitle = Dict.Anonymized;  // obfuscate window title
                }

                // if user enabled private tracking, obfuscate window title
                if (Shared.Settings.AnonymizeSensitiveData)
                {
                    var activityCategory = ProcessToActivityMapper.Map(entry.Process, entry.WindowTitle);
                    entry.WindowTitle = string.Format("{0} (category: {1})", Dict.Anonymized, activityCategory);  // obfuscate window title
                }

                // if end time is missing, don't store anything
                if (entry.TsEnd == DateTime.MinValue)
                {
                    Database.GetInstance().LogWarning("TsEnd of WindowsActivitySwitch was empty.");
                    return;
                }

                var query = string.Format(QUERY_INSERT,
                                          "strftime('%Y-%m-%d %H:%M:%f', 'now', 'localtime')",
                                          Database.GetInstance().QTime2(entry.TsStart),
                                          Database.GetInstance().QTime2(entry.TsEnd),
                                          Database.GetInstance().Q(entry.WindowTitle),
                                          Database.GetInstance().Q(entry.Process));

                Database.GetInstance().ExecuteDefaultQuery(query);
            }
            catch (Exception e)
            {
                Logger.WriteToLogFile(e);
            }
        }

        internal static bool UserInputTableExists()
        {
            var res = Database.GetInstance().HasTable(Shared.Settings.UserInputTable);
            return res;
        }

        /// <summary>
        /// Returns a list with tsStart and tsEnd of all missed sleep events
        /// </summary>
        /// <param name="ts_checkFrom"></param>
        /// <param name="ts_checkTo"></param>
        /// <returns></returns>
        internal static List<Tuple<long, DateTime, DateTime>> GetMissedSleepEvents(DateTime ts_checkFrom, DateTime ts_checkTo)
        {
            var results = new List<Tuple<long, DateTime, DateTime>>();

            try
            {
                var query = "SELECT wa.id, wa.tsStart, wa.tsEnd, ( "
                          + "SELECT sum(ui.keyTotal) + sum(ui.clickTotal) + sum(ui.ScrollDelta) + sum(ui.movedDistance) "
                          + "FROM " + Shared.Settings.UserInputTable + " AS ui "
                          + "WHERE (ui.tsStart between wa.tsStart and wa.tsEnd) AND (ui.tsEnd between wa.tsStart and wa.tsEnd) "
                          + ") as 'sumUserInput' "
                          + "FROM " + Settings.DbTable + " AS wa "
                          + "WHERE wa.process <> '" + Dict.Idle + "' " // we are looking for cases where the IDLE event was not catched
                          + "AND wa.process <> 'skype' AND wa.process <> 'lync' " // IDLE during calls are okay
                          + "AND (wa.tsStart between " + Database.GetInstance().QTime(ts_checkFrom) + " AND " + Database.GetInstance().QTime(ts_checkTo) + ") " // perf
                          + "AND (strftime('%s', wa.tsEnd) - strftime('%s', wa.tsStart)) > " + Settings.IdleSleepValidate_ThresholdIdleBlocks_s + ";"; // IDLE time window we are looking for

                var table = Database.GetInstance().ExecuteReadQuery(query);

                foreach (DataRow row in table.Rows)
                {
                    if (row["sumUserInput"] == DBNull.Value || Convert.ToInt32(row["sumUserInput"]) == 0)
                    {
                        var id = (long)row["id"];
                        var tsStart = DateTime.Parse((string)row["tsStart"], CultureInfo.InvariantCulture);
                        var tsEnd = DateTime.Parse((string)row["tsEnd"], CultureInfo.InvariantCulture);
                        var tuple = new Tuple<long, DateTime, DateTime>(id, tsStart, tsEnd);
                        results.Add(tuple);
                    }
                }
                table.Dispose();
            }
            catch (Exception e)
            {
                Logger.WriteToLogFile(e);
            }

            return results;
        }

        internal static void AddMissedSleepIdleEntry(List<Tuple<long, DateTime, DateTime>> toFix)
        {
            foreach (var item in toFix)
            {
                var idleTimeFix = item.Item2.AddMilliseconds(Settings.NotCountingAsIdleInterval_ms);
                var tsEnd = item.Item3;

                // add missed sleep idle entry
                var tempItem = new WindowsActivityEntry(idleTimeFix, tsEnd, Settings.ManualSleepIdle, Dict.Idle, IntPtr.Zero);
                InsertSnapshot(tempItem);

                // update tsEnd of previous (wrong entry)
                var query = "UPDATE " + Settings.DbTable + " SET tsEnd = " + Database.GetInstance().QTime2(idleTimeFix) + " WHERE id = " + item.Item1;
                Database.GetInstance().ExecuteDefaultQuery(query);
            }

            if (toFix.Count > 0) Database.GetInstance().LogInfo("Fixed " + toFix.Count + " missed IDLE sleep entries.");
        }
        */

        #endregion

        #region Visualization Queries

        /*
        /// <summary>
        /// Returns the program where the user focused on the longest
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        internal static FocusedWorkDto GetLongestFocusOnProgram(DateTimeOffset date)
        {
            try
            {
                var query = "SELECT process, (strftime('%s', tsEnd) - strftime('%s', tsStart)) as 'difference', tsStart, tsEnd "
                          + "FROM " + Settings.DbTable + " "
                          + "WHERE " + Database.GetInstance().GetDateFilteringStringForQuery(VisType.Day, date, "tsStart") + " AND " + Database.GetInstance().GetDateFilteringStringForQuery(VisType.Day, date, "tsEnd") + " "
                          + "AND process <> '" + Dict.Idle + "' "
                          + "GROUP BY id, tsStart "
                          + "ORDER BY difference DESC "
                          + "LIMIT 1;";

                var table = Database.GetInstance().ExecuteReadQuery(query);

                if (table != null && table.Rows.Count == 1)
                {
                    var row = table.Rows[0];
                    var process = Shared.Helpers.ProcessNameHelper.GetFileDescriptionFromProcess((string)row["process"]);
                    //var window = (string)row["window"];
                    var difference = Convert.ToInt32(row["difference"], CultureInfo.InvariantCulture);
                    var tsStart = DateTime.Parse((string)row["tsStart"], CultureInfo.InvariantCulture);
                    var tsEnd = DateTime.Parse((string)row["tsEnd"], CultureInfo.InvariantCulture);

                    table.Dispose();
                    return new FocusedWorkDto(process, difference, tsStart, tsEnd);
                }
                else
                {
                    table.Dispose();
                    return null;
                }  
            }
            catch (Exception e)
            {
                Logger.WriteToLogFile(e);
                return null;
            }
        }

        /// <summary>
        /// Fetches the activities a developer has on his computer for a given date in an
        /// ordered list according to time.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        internal static List<WindowsActivity> GetDayTimelineData(DateTimeOffset date)
        {
            var orderedActivityList = new List<WindowsActivity>();

            try
            {
                var query = "SELECT tsStart, tsEnd, window, process, (strftime('%s', tsEnd) - strftime('%s', tsStart)) as 'durInSec' "
                              + "FROM " + Settings.DbTable + " "
                              + "WHERE " + Database.GetInstance().GetDateFilteringStringForQuery(VisType.Day, date, "tsStart") + " AND " + Database.GetInstance().GetDateFilteringStringForQuery(VisType.Day, date, "tsEnd") + " "
                              + "ORDER BY tsStart;";

                var table = Database.GetInstance().ExecuteReadQuery(query);

                if (table != null)
                {
                    WindowsActivity previousWindowsActivityEntry = null;

                    foreach (DataRow row in table.Rows)
                    {
                        // fetch items from database
                        var e = new WindowsActivity();
                        e.StartTime = DateTime.Parse((string)row["tsStart"], CultureInfo.InvariantCulture);
                        e.EndTime = DateTime.Parse((string)row["tsEnd"], CultureInfo.InvariantCulture);
                        e.DurationInSeconds = row.IsNull("durInSec") ? 0 : Convert.ToInt32(row["durInSec"], CultureInfo.InvariantCulture);
                        var processName = (string)row["process"];

                        // make window titles more readable (TODO: improve!)
                        var windowTitle = (string)row["window"];
                        windowTitle = WindowTitleWebsitesExtractor.GetWebsiteDetails(processName, windowTitle);
                        //windowTitle = WindowTitleArtifactExtractor.GetArtifactDetails(processName, windowTitle);
                        //windowTitle = WindowTitleCodeExtractor.GetProjectName(windowTitle);

                        // map process and window to activity
                        e.ActivityCategory = ProcessToActivityMapper.Map(processName, windowTitle);


                        // check if we add a new item, or merge with the previous one
                        if (previousWindowsActivityEntry != null)
                        {
                            // previous item is same, update it (duration and tsEnd)
                            if (e.ActivityCategory == previousWindowsActivityEntry.ActivityCategory)
                            {
                                var lastItem = orderedActivityList.Last();
                                lastItem.DurationInSeconds += e.DurationInSeconds;
                                lastItem.EndTime = e.EndTime;
                                lastItem.WindowProcessList.Add(new WindowProcessItem(processName, windowTitle));
                            }
                            // previous item is different, add it to list
                            else
                            {
                                e.WindowProcessList.Add(new WindowProcessItem(processName, windowTitle));
                                orderedActivityList.Add(e);
                            }
                        }
                        else // first item
                        {
                            orderedActivityList.Add(e);
                        }
                        previousWindowsActivityEntry = e;
                    }
                    table.Dispose();
                }
            }
            catch (Exception e)
            {
                Logger.WriteToLogFile(e);
            }

            return orderedActivityList;
        }

        /// <summary>
        /// Fetches the activities a developer has on his computer for a given date and prepares the data
        /// to be visualized as a pie chart.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        internal static Dictionary<string, double> GetActivityPieChartData(DateTimeOffset date)
        {
            var dto = new Dictionary<string, double>();

            try
            {
                var query = "SELECT process, sum(difference) / 60.0 / 60.0  as 'durInHrs' "
                          + "FROM (	" 
                          + "SELECT process, (strftime('%s', tsEnd) - strftime('%s', tsStart)) as 'difference' "
                          + "FROM " + Settings.DbTable + " "
                          + "WHERE " + Database.GetInstance().GetDateFilteringStringForQuery(VisType.Day, date, "tsStart") + " and " + Database.GetInstance().GetDateFilteringStringForQuery(VisType.Day, date, "tsEnd") + " "
                          + "GROUP BY id, tsStart"
                          + ") "
                          + "WHERE difference > 0 and process <> '" + Dict.Idle + "' "
                          + "GROUP BY process;";

                var table = Database.GetInstance().ExecuteReadQuery(query);

                if (table != null)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        var process = (string)row["process"];
                        var fileDesc = Shared.Helpers.ProcessNameHelper.GetFileDescription(process);
                        var share = (double)row["durInHrs"];

                        if (dto.ContainsKey(fileDesc))
                        {
                            dto[fileDesc] += share;
                        }
                        else
                        {
                            dto.Add(fileDesc, share);
                        }
                    }
                    table.Dispose();
                }
            }
            catch (Exception e)
            {
                Logger.WriteToLogFile(e);
            }

            return dto;
        }

        /// <summary>
        /// For a given date, return the total time spent at work (from first to last input)
        /// and the total time spent on the computer.
        /// In HOURS.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        internal static Tuple<double, double> GetWorkTimeDetails(DateTimeOffset date)
        {
            try
            {
                var query = "SELECT sum(difference) / 60.0 / 60.0  as 'durInHrs' "
                            + "FROM ( "
                            + "SELECT (strftime('%s', tsEnd) - strftime('%s', tsStart)) as 'difference' "
                            + "FROM " + Settings.DbTable + " "
                            + "WHERE " + Database.GetInstance().GetDateFilteringStringForQuery(VisType.Day, date, "tsStart") + " and " + Database.GetInstance().GetDateFilteringStringForQuery(VisType.Day, date, "tsEnd")
                            + "AND process <> 'IDLE' );";

                var timeSpentActive = Database.GetInstance().ExecuteScalar3(query);
                var timeFirstEntry = Database.GetInstance().GetUserWorkStart(date);
                var timeLastEntry = Database.GetInstance().GetUserWorkEnd(date);
                var totalWorkTime = (timeLastEntry - timeFirstEntry).TotalHours;

                if (totalWorkTime < 0.2) totalWorkTime = 0.0;
                if (timeSpentActive < 0.2) timeSpentActive = 0.0;

                return new Tuple<double, double>(totalWorkTime, timeSpentActive);
            }
            catch (Exception e)
            {
                Logger.WriteToLogFile(e);
                return null;
            }
        }
        */

        #endregion

    }
}
