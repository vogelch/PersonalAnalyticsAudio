﻿// Created by Christoph Vogel (christoph.vogel@uzh.ch) from the University of Zurich
// Created: 2018-10-01
// 
// Licensed under the MIT License.

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Shared;
using Shared.Data;
using System.Collections.Generic;
using WindowsActivityTracker.Visualizations;
using AudioTracker.Data;
using System.Globalization;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq;
using AudioTracker.Views;
using NAudio.Wave;
using NAudio.CoreAudioApi;
using NAudio.Lame;
using System.IO;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using Hardcodet.Wpf.TaskbarNotification;
using System.Drawing;
using System.Management;
using AudioTracker.Helpers;
using AudioTracker.Models;

namespace AudioTracker
{
    /// <summary>
    /// This tracker stores audio data in the database.
    /// </summary>
    public sealed class Daemon : BaseTrackerDisposable, ITracker
    {
        private bool IsDisposed = false;
        //private System.Timers.Timer _idleCheckTimer;
        //private System.Timers.Timer _idleSleepValidator;
        //private DateTime _previousIdleSleepValidated = DateTime.MinValue;

        private System.Timers.Timer checkAudioDeviceTimer;
        private bool _isConnectedAudioDevice = false;
        private bool _wasFirstStart = true;
        private bool isPaused = false;

        // audio device notification
        private NotifyIcon NotificationHandle;

        // audio device and recording
        private WaveIn waveSource = null;
        private int lastNumberOfAudioDevices = 0;
        private int lastNumberOfAudioDevicesTick = 0;
        private AudioDevice LastInputAudioDevice;
        private short[] currentAudioInputBuffer = null;
        private DateTime lastAbnormalRecordingAbort = new DateTime();
        private bool isCurrentStopAbnormal = false;
        private DateTime startOfCurrentRecording = new DateTime();

        #region ITracker Stuff

        public Daemon()
        {
            Name = Settings.TRACKER_NAME;
            if (Settings.IS_RAW_RECORDING_ENABLED)
            {
                Name += " (with raw recording)";
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    //_idleCheckTimer.Dispose();
                    //_idleSleepValidator.Dispose();
                }
                IsDisposed = true;
            }
            base.Dispose(disposing);
        }

        public override void Start()
        {
            lastNumberOfAudioDevices = WaveIn.DeviceCount; //TODO: should this be in the constructor?

            string[] resourceNames = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            var msg1 = new Exception("Resource names: " + string.Join(" ; ", resourceNames));
            Logger.WriteToLogFile(msg1);

            //TODO: this should not be necessary
            //TODO: move this to the constructor?
            //TODO: do this only if raw recording is activated?
            JavaHelper.WriteResourceToFile("AudioTracker.Resources.LibMP3Lame.libmp3lame.32.dll", "libmp3lame.32.dll");
            JavaHelper.WriteResourceToFile("AudioTracker.Resources.LibMP3Lame.libmp3lame.64.dll", "libmp3lame.64.dll");

            // start device notifications
            DeviceChangeNotifier.Start();
            DeviceChangeNotifier.DeviceNotify += DeviceNotificationHandler;

            isPaused = false;
            try
            {
                // Register Audio Device Timer
                checkAudioDeviceTimer = new System.Timers.Timer();
                checkAudioDeviceTimer.Interval = 5000; // Settings.UserInputAggregationInterval.TotalMilliseconds;
                checkAudioDeviceTimer.Elapsed += CheckAudioDeviceTick;
                //checkAudioDeviceTimer.Start();

                // Write system info to database
                ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
                ManagementObjectCollection moc = mc.GetInstances();
                if (moc.Count != 0)
                {
                    foreach (ManagementObject mo in mc.GetInstances())
                    {
                        string currentSystemInfo = "\nMachine Make: " + mo["Manufacturer"].ToString() + "\nMachine Model: " + mo["Model"].ToString() + "\nSystem Type: " +
                            mo["SystemType"].ToString() + "\nHost Name: " + mo["DNSHostName"].ToString() + "\nLogon User Name: " + mo["UserName"].ToString() + "\n";
                        Database.GetInstance().LogInfo(currentSystemInfo);
                    }
                }

                // Check whether Java is available, copy LIUM jar file resource to executing location
                if (!JavaHelper.IsJavaAvailable())
                {
                    //TODO: do not contine but abort gracefully under this condition
                    Logger.WriteToConsole("Java is NOT available on the system.");
                    var msg = new Exception("Java is NOT available on the system.");
                    Logger.WriteToLogFile(msg);
                }
                JavaHelper.WriteResourceToFile("AudioTracker.Resources.LIUM.LIUM_SpkDiarization-8.4.1.jar", "lium.jar");

                // Start Audio recording
                StartAudioRecording();

                IsRunning = true;
            }
            catch (Exception e)
            {
                Logger.WriteToLogFile(e);
                //Database.GetInstance().LogWarning("Registering events failed: " + e.Message);
                IsRunning = false;
            }

        }

        public override void Stop()
        {
            var msg = new Exception("Daemon stop called.");
            Logger.WriteToLogFile(msg);
            Database.GetInstance().LogInfo("AudioTracker: Daemon stop called.");

            isPaused = true;
            //TODO: properly delete potentially remaining .wav files at shutdown

            try
            {
                // stop device change notifier
                //DeviceChangeNotifier.Stop();

                // Unregister audio device time checker Timer
                /*
                if (checkAudioDeviceTimer != null)
                {
                    checkAudioDeviceTimer.Stop();
                    checkAudioDeviceTimer.Dispose();
                    checkAudioDeviceTimer = null;
                }
                */

                // Stop audio recording
                StopAudioRecording();
                if (waveSource != null)
                {
                    waveSource.StopRecording();
                    waveSource.Dispose();
                    waveSource = null;
                }

                // Dispose NotificationHandle if necessary
                if (NotificationHandle != null)
                {
                    NotificationHandle.Visible = false;
                    NotificationHandle.Dispose();
                    NotificationHandle = null;
                }
            }
            catch (Exception e)
            {
                Logger.WriteToLogFile(e);
                //Database.GetInstance().LogWarning("Un-Registering events failed: " + e.Message);
            }

            IsRunning = false;
        }

        public override void CreateDatabaseTablesIfNotExist()
        {
            Queries.CreateAudioRecordingsTable();
            Queries.UpdateAllColumnsOfTable(Settings.AUDIO_RECORDINGS_TABLE_NAME, Queries.AUDIO_RECORDINGS_COLUMN_NAMES);

            Queries.CreateAudioVolumeTable();
            Queries.UpdateAllColumnsOfTable(Settings.AUDIO_VOLUME_TABLE_NAME, Queries.AUDIO_VOLUME_COLUMN_NAMES);
        }

        public override void UpdateDatabaseTables(int version)
        {
            //Queries.UpdateDatabaseTables(version);
        }

        public override bool IsFirstStart
        {
            get { _wasFirstStart = !Database.GetInstance().HasSetting(Settings.TRACKER_ENEABLED_SETTING); return !Database.GetInstance().HasSetting(Settings.TRACKER_ENEABLED_SETTING); }
        }

        public override string GetStatus()
        {
            //TODO: _isConnectedAudioDevice has to be implemented or insert name of microphone currently in use
            //return IsRunning ? (Name + " is running. An audio device is " + (_isConnectedAudioDevice ? "connected." : "NOT connected.")) : (Name + " is NOT running.");
            return IsRunning ? (Name + " is running.") : (Name + " is NOT running.");
        }

        public override bool IsEnabled()
        {
            return Settings.IsEnabled;
        }

        public override string GetVersion()
        {
            var v = new AssemblyName(Assembly.GetExecutingAssembly().FullName).Version;
            return Shared.Helpers.VersionHelper.GetFormattedVersion(v);
        }

        public override List<IVisualization> GetVisualizationsDay(DateTimeOffset date)
        {
            /*
            var vis1 = new DayProgramsUsedPieChart(date);
            var vis2 = new DayMostFocusedProgram(date);
            var vis3 = new DayFragmentationTimeline(date);
            return new List<IVisualization> { vis1, vis2, vis3 };
            */
            return null;
        }

        public override List<IVisualization> GetVisualizationsWeek(DateTimeOffset date)
        {
            /*
            var vis1 = new WeekProgramsUsedTable(date);
            var vis2 = new WeekWorkTimeBarChart(date);
            return new List<IVisualization> { vis1, vis2 };
            */
            return null;
        }

        public override List<IFirstStartScreen> GetStartScreens()
        {
            return new List<IFirstStartScreen>() { new FirstStartWindow() };
        }

        #endregion

        private async void CheckAudioDeviceTick(object sender, EventArgs e)
        {
            Logger.WriteToConsole("Tick!");
            /*
            MMDeviceEnumerator deviceEnumerator = new MMDeviceEnumerator();
            MMDeviceCollection AudioDevices = deviceEnumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);
            int currentNumberOfAudioDevices = WaveIn.DeviceCount;
            Logger.WriteToConsole("current: " + currentNumberOfAudioDevices + " / last: " + lastNumberOfAudioDevicesTick);
            if (currentNumberOfAudioDevices > lastNumberOfAudioDevicesTick)
            {
                lastNumberOfAudioDevicesTick = currentNumberOfAudioDevices;
                Logger.WriteToConsole("An audio device has been added.");
                var msg = new Exception("An audio device has been added.");
                Logger.WriteToLogFile(msg);

                //TODO: check if it is the same device which was removed

                StopAudioRecording();
                StartAudioRecording();
            }
            else if (currentNumberOfAudioDevices < lastNumberOfAudioDevicesTick)
            {
                lastNumberOfAudioDevicesTick = currentNumberOfAudioDevices;
                Logger.WriteToConsole("An audio device has been removed.");
            }
            else
            {
                Logger.WriteToConsole("Changed device was not an audio device.");
            }
            */
        }

        private bool StartAudioRecording()
        {
            LastInputAudioDevice = Settings.InputAudioDevice;

            waveSource = new WaveIn(WaveCallbackInfo.FunctionCallback());
            try
            {
                //TODO: check whether audio device has changed since startup
                waveSource.DeviceNumber = (int)Settings.InputAudioDevice.DeviceNumber; //TODO: null check!
                Database.GetInstance().LogInfo("AudioTracker: Audio input device number selected at start of recording: " + Settings.InputAudioDevice.DeviceNumber);
                waveSource.WaveFormat = new WaveFormat(Settings.RecordingSampleRate, Settings.RecordingNumberOfChannels);
                waveSource.BufferMilliseconds = Settings.AudioRecordingChunkLength;
                waveSource.DataAvailable += new EventHandler<WaveInEventArgs>(waveSource_DataAvailable);
                waveSource.RecordingStopped += new EventHandler<NAudio.Wave.StoppedEventArgs>(waveSource_RecordingStopped);
                startOfCurrentRecording = DateTime.Now;
                waveSource.StartRecording();

                var msg = new Exception("Audio recording has started.");
                Logger.WriteToLogFile(msg);
                Database.GetInstance().LogInfo("AudioTracker: Audio recording has started.");
                return true;
            }
            catch (Exception e)
            {
                Logger.WriteToLogFile(e);
                return false;
            }
        }

        private bool StopAudioRecording()
        {
            try
            {
                if (waveSource != null)
                {
                    waveSource.StopRecording();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Logger.WriteToLogFile(e);
                return false;
            }
        }

        void waveSource_DataAvailable(object sender, WaveInEventArgs e)
        {
            DateTime endOfThisRecording = DateTime.Now;
            DateTime startOfThisRecording = startOfCurrentRecording;
            startOfCurrentRecording = endOfThisRecording;
            try
            {
                string fileNameDateTime = DateTime.Now.ToString("yyyy-MM-dd--HH-mm-ss");
                string audioFilename = Shared.Settings.ExportFilePath + "\\" + Settings.RecordingFileNamePrefix + "-" + fileNameDateTime + ".wav";
                WaveFileWriter waveFile = new WaveFileWriter(audioFilename, waveSource.WaveFormat);
                waveFile.Write(e.Buffer, 0, e.BytesRecorded);

                // Calculate loudness and check whether microphone is (probably) muted
                double[] relativeAmplitudes = AmplitudeHelper.GetRelativeAmplitudes(e.Buffer, e.BytesRecorded);
                List<AmplitudeData> newAmplitudeDataList = AmplitudeHelper.GetAmplitudeData(e.Buffer, e.BytesRecorded, 30000);
                AmplitudeData newAmplitudeData = newAmplitudeDataList.ToArray()[0];
                bool isMicrophoneProbablyMuted = AmplitudeHelper.IsMicrophoneProbablyMuted(newAmplitudeData);

                //Logger.WriteToConsole("Device name: " + inputAudioDevice.DeviceFriendlyName);  <- throws an exception
                //Logger.WriteToConsole("Device mute: " + inputAudioDevice.AudioEndpointVolume.VolumeRange); // inputAudioDevice.AudioEndpointVolume.Mute.ToString()
                int lengthOfRecording = (int)(1000 * waveFile.Length / waveFile.WaveFormat.AverageBytesPerSecond);
                if (lengthOfRecording != Settings.AudioRecordingChunkLength)
                {
                    var msg = new Exception("Recording of audio segment has stopped early (after " + lengthOfRecording + " milliseconds).");
                    Logger.WriteToLogFile(msg);
                    Database.GetInstance().LogWarning("AudioTracker: Recording of audio segment has stopped early (after " + lengthOfRecording + " milliseconds).");
                    if (isPaused)
                    {
                        Logger.WriteToConsole("PersonalAnalytics was paused.");
                    }
                    else
                    {
                        lastAbnormalRecordingAbort = DateTime.Now;
                        Logger.WriteToConsole("PersonalAnalytics recording aborted abnormally!");
                    }
                    //..
                }
                waveFile.Close();
                waveFile.Dispose();
                waveFile = null;

                //save as MP3 file if raw recording option is enabled
                string audioFilenameMp3 = null;
                if (Settings.IS_RAW_RECORDING_ENABLED)
                {
                    audioFilenameMp3 = Settings.RecordingFileNamePrefix + "-" + fileNameDateTime + ".mp3";
                    string audioFilePathMp3 = Shared.Settings.ExportFilePath + "\\" + audioFilenameMp3;
                    ConvertWavToMp3(audioFilename, audioFilePathMp3);
                }

                // start analysis of new audio chunk
                string outputFileName = "lium-" + fileNameDateTime + ".seg";
                string outputFilePath = Shared.Settings.ExportFilePath + "\\" + outputFileName;
                string liumConsoleOutput = liumAnalysis(audioFilename, outputFilePath);

                //store audio recording meta data into database
                AudioRecording newAudioRecording = new AudioRecording(startOfThisRecording, endOfThisRecording, audioFilenameMp3, outputFileName, liumConsoleOutput, 0, lengthOfRecording,
                    newAmplitudeData.MinValue, newAmplitudeData.MaxValue, newAmplitudeData.AvgValue, newAmplitudeData.ModeValue, newAmplitudeData.ModeOccurrences, 0.0, 0.0, isMicrophoneProbablyMuted);
                Queries.StoreAudioRecording(newAudioRecording);

                //store amplitude data into database
                //TODO: implement
            }
            catch (Exception ex)
            {
                Logger.WriteToLogFile(ex);
                // catch System.IO.IOException
                if (IsDiskFull(ex))
                {
                    Database.GetInstance().LogError("AudioTracker: Could not save recording to file because there was not enough disk space. " + ex.Message);

                }
                else
                {
                    Database.GetInstance().LogError(ex.Message);
                }
            }

        }

        void waveSource_RecordingStopped(object sender, EventArgs e)
        {
            //TODO: remove abuse of log file and store result solely in database log table
            var msg = new Exception("Audio recording has stopped (waveSource_RecordingStopped).");
            Logger.WriteToLogFile(msg);
            Database.GetInstance().LogInfo("AudioTracker: Audio recording has stopped.");

            if (waveSource != null)
            {
                waveSource.Dispose();
                waveSource = null;
            }
        }

        /// <summary>
        /// Starts Java application with LIUM.jar to analyse last n minutes of audio (without showing window)
        /// Returns console output as a string for further parsing.
        /// </summary>
        /// <param name="liumInputFilename"></param>
        /// <param name="liumOutputFilename"></param>
        private string liumAnalysis(string liumInputFilename, string liumOutputFilename)
        {
            string lium_output_std = null;
            string lium_output_err = null;

            var worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync();

            void worker_DoWork(object sender, DoWorkEventArgs e)
            {
                try
                {
                    Process liumProcess = new Process();
                    liumProcess.StartInfo.FileName = @"java";
                    string arguments = "-Xmx2024m -jar lium.jar --fInputMask=\"" + liumInputFilename + "\" --sOutputMask=\"" + liumOutputFilename + "\" --doCEClustering showName";
                    liumProcess.StartInfo.Arguments = arguments;
                    liumProcess.EnableRaisingEvents = true;
                    liumProcess.StartInfo.UseShellExecute = false;
                    liumProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    liumProcess.StartInfo.CreateNoWindow = true;
                    liumProcess.StartInfo.RedirectStandardOutput = true;
                    liumProcess.StartInfo.RedirectStandardError = true;

                    StringBuilder output = new StringBuilder();
                    StringBuilder error = new StringBuilder();

                    using (AutoResetEvent outputWaitHandle = new AutoResetEvent(false))
                    using (AutoResetEvent errorWaitHandle = new AutoResetEvent(false))
                    {
                        liumProcess.OutputDataReceived += (senderObj, evt) => {
                            if (evt.Data == null)
                            {
                                outputWaitHandle.Set();
                            }
                            else
                            {
                                output.AppendLine(evt.Data);
                            }
                        };
                        liumProcess.ErrorDataReceived += (senderObj, evt) =>
                        {
                            if (evt.Data == null)
                            {
                                errorWaitHandle.Set();
                            }
                            else
                            {
                                error.AppendLine(evt.Data);
                            }
                        };

                        liumProcess.Start();
                        Logger.WriteToConsole("LIUM process started.");

                        liumProcess.BeginOutputReadLine();
                        liumProcess.BeginErrorReadLine();

                        int timeout = 30000;
                        if (liumProcess.WaitForExit(timeout) && outputWaitHandle.WaitOne(timeout) && errorWaitHandle.WaitOne(timeout))
                        {
                            Logger.WriteToConsole("LIUM process completed.");
                            lium_output_std = output.ToString();
                            lium_output_err = error.ToString();
                            //Logger.WriteToConsole("LIUM output: " + lium_output_std);
                            //Logger.WriteToConsole("LIUM error messages: " + lium_output_err);

                            //TODO: Check process.ExitCode here.
                            //TODO: write to database
                        }
                        else
                        {
                            Logger.WriteToConsole("LIUM process timeout.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteToLogFile(ex);
                }

            }

            void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
            {
                Logger.WriteToConsole("Worker completed.");
                ParseLiumClustersFromFile(liumOutputFilename);

                if (File.Exists(@liumInputFilename))
                {
                    File.Delete(@liumInputFilename);
                }
                else
                {
                    //TODO: write to error log
                }

            }

            return lium_output_err;

        }

        private static void ConvertWavToMp3(string WavFile, string Mp3FileName)
        {
            using (var resultMemoryStream = new MemoryStream())
            using (var reader = new WaveFileReader(WavFile))
            using (var writer = new LameMP3FileWriter(Mp3FileName, reader.WaveFormat, 16/*LAMEPreset.VBR_90*/))
            {
                reader.CopyTo(writer);
            }
        }

        private List<LiumCluster> ParseLiumClustersFromFile(string LiumInputFileName)
        {
            const int LIUM_FEATURE_LENGTH = 10; // in milliseconds, i.e. 3000 features for a 30 seconds recording //TODO: move to settings or to model
            List<LiumCluster> NewLiumClusterSet = new List<LiumCluster>();

            StreamReader reader = File.OpenText(LiumInputFileName);
            string line;
            bool isNextLineClusterInfo = false;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.Substring(0, 2) == ";;")
                {
                    isNextLineClusterInfo = true;
                }
                if (isNextLineClusterInfo)
                {
                    //LiumCluster CurrentNewLiumCluster = new LiumCluster();
                    Logger.WriteToConsole("Cluster: " + line);
                    //NewLiumClusterSet.Add(CurrentNewLiumCluster);
                }
                else
                {
                    string[] SegmentProperties = line.Split(' ');
                    LiumSegment NewLiumSegment = new LiumSegment(SegmentProperties);
                    //CurrentNewLiumCluster.SegmentSet.Add(NewLiumSegment);
                    Logger.WriteToConsole(NewLiumSegment.ToString());
                }
            }
            return null;
            //return NewLiumClusterSet;
        }

        static bool IsDiskFull(Exception ex)
        {
            const int HR_ERROR_HANDLE_DISK_FULL = unchecked((int)0x80070027);
            const int HR_ERROR_DISK_FULL = unchecked((int)0x80070070);
            return ex.HResult == HR_ERROR_HANDLE_DISK_FULL || ex.HResult == HR_ERROR_DISK_FULL;
        }

        internal void DeviceNotificationHandler(Message msg)
        {
            Logger.WriteToConsole("Device change event LParam: " + msg.LParam); // 0    depends on Wparam; set to zero for DBT_DEVNODES_CHANGED
            Logger.WriteToConsole("Device change event HWnd: " + msg.HWnd); // A handle to the window.
            Logger.WriteToConsole("Device change event Msg: " + msg.Msg); // 537 = WM_DEVICECHANGE
            Logger.WriteToConsole("Device change event Result: " + msg.Result);
            Logger.WriteToConsole("Device change event WParam: " + msg.WParam);

            const int WPARAM_DEVICE_CHANGE_EVENT = 0x0219; // 537 = WM_DEVICECHANGE
            const int WPARAM_DEVICE_NODE_CHANGED = 0x0007; // 7 = DBT_DEVNODES_CHANGED     A device has been added to or removed from the system.
            const int WPARAM_DEVICE_REMOVE_COMPLETE = 0x8004; // 32772 =       A device or piece of media has been removed.
            const int WPARAM_DEVICE_ARRIVAL = 0x8000; // 32768 = DBT_DEVICEARRIVAL             A device or piece of media has been inserted and is now available.

            if (msg.Msg == WPARAM_DEVICE_CHANGE_EVENT)
            {
                int wparamAsInt = msg.WParam.ToInt32();
                if (wparamAsInt == WPARAM_DEVICE_NODE_CHANGED)
                {
                    if (Settings.InputAudioDevice != null)
                    {
                        // If an audio device is currently correctly connected, check if current audio device is still connected
                        MMDeviceCollection AudioDevices = AudioDeviceHelper.GetActiveInputDevicesCollection();
                        if (!AudioDevices.Contains(Settings.InputAudioDevice.DeviceInstance))
                        {
                            Settings.InputAudioDevice = null;
                            if (Settings.IS_DEVICE_EVENT_NOTIFICATION_ENABLED)
                            {
                                Logger.WriteToConsole("Warning: The audio device currently in use has been unplugged!");
                                var msg1 = new Exception("Warning: The audio device currently in use has been unplugged!");
                                Logger.WriteToLogFile(msg1);
                                /*
                                NotificationHandle = new NotifyIcon();
                                NotificationHandle.BalloonTipTitle = "PersonalAnalytics: Audio device removed!";
                                NotificationHandle.BalloonTipText = "The microphone used by the audio tracker was removed. Audio recording has been stopped. Personal Analytics will try to automatically resume recording on reconnect."; // ": Audio device " + inputAudioDevice.DeviceFriendlyName + " was removed.";
                                NotificationHandle.Icon = SystemIcons.Exclamation;
                                NotificationHandle.BalloonTipClosed += (sender, e) => { var thisIcon = (NotifyIcon)sender; thisIcon.Visible = false; thisIcon.Dispose(); };
                                NotificationHandle.Text = Name + ": Audio device removed!";
                                NotificationHandle.Visible = true;
                                NotificationHandle.ShowBalloonTip(5000);
                                */
                            }
                        }
                    }
                    else
                    {
                        Thread.Sleep(500);
                        MMDeviceCollection AudioDevices = AudioDeviceHelper.GetActiveInputDevicesCollection();
                        List<string> AudioDeviceNames = new List<string>();
                        foreach (MMDevice CurrentDevice in AudioDevices)
                        {
                            Logger.WriteToConsole("Current audio device: " + CurrentDevice.DeviceFriendlyName);
                            AudioDeviceNames.Add(CurrentDevice.DeviceFriendlyName);
                        }
                        Logger.WriteToConsole("Checking whether the changed device was the previously removed audio device...");
                        Logger.WriteToConsole("Last device name: " + LastInputAudioDevice.FriendlyName);
                        if (AudioDeviceNames.Contains(LastInputAudioDevice.FriendlyName))
                        {
                            Logger.WriteToConsole("The previously disconnected audio device has been plugged in again. Trying to resume recording...");
                            var msg2 = new Exception("The previously disconnected audio device has been plugged in again. Trying to resume recording...");
                            Logger.WriteToLogFile(msg2);
                            // TODO: set the audio device again as the current audio device
                            /*
                            Settings.InputAudioDevice = AudioDeviceHelper.GetDeviceFromDeviceName(LastInputAudioDeviceName);
                            Settings.InputAudioDeviceName = Settings.InputAudioDevice.DeviceFriendlyName;
                            Settings.InputAudioDeviceNumber = AudioDeviceHelper.GetDeviceNumberFromDeviceName(Settings.InputAudioDeviceName);
                            */
                            //TODO: show a message again at this point which tells the user that recording resumes?
                            StopAudioRecording();
                            StartAudioRecording();
                        }
                    }

                    /*
                    int currentNumberOfAudioDevices = WaveIn.DeviceCount;
                    Logger.WriteToConsole("current: " + currentNumberOfAudioDevices + " / last: " + lastNumberOfAudioDevices);
                    if (currentNumberOfAudioDevices > lastNumberOfAudioDevices)
                    {
                        lastNumberOfAudioDevices = currentNumberOfAudioDevices;
                        Logger.WriteToConsole("An audio device has been added.");
                    }
                    else if (currentNumberOfAudioDevices < lastNumberOfAudioDevices)
                    {
                        lastNumberOfAudioDevices = currentNumberOfAudioDevices;
                        Logger.WriteToConsole("An audio device has been removed.");
                        // check if the removed audio device was the device currently in use

                        if (Settings.IS_DEVICE_EVENT_NOTIFICATION_ENABLED)
                        {
                            Logger.WriteToConsole("Showing warning balloon tip...");
                            NotificationHandle = new NotifyIcon();
                            NotificationHandle.BalloonTipTitle = "PersonalAnalytics: Audio device removed!";
                            NotificationHandle.BalloonTipText = "The microphone used by the audio tracker was removed. Audio recording has been stopped. Personal Analytics will try to automatically resume recording on reconnect."; // ": Audio device " + inputAudioDevice.DeviceFriendlyName + " was removed.";
                            NotificationHandle.Icon = SystemIcons.Exclamation;
                            NotificationHandle.Text = Name + ": Audio device removed!";
                            NotificationHandle.Visible = true;
                            NotificationHandle.ShowBalloonTip(5000);
                        }
                    }
                    else
                    {
                        Logger.WriteToConsole("Changed device was not an audio device.");
                    }
                    */
                }
            }

            if (Settings.IS_DEVICE_EVENT_RECORDING_ENABLED)
            {
                //TODO: store device event in database
                //DatabaseConnector.AddHeartMeasurementsToDatabase(measurements, false);
            }

        }

    }

    public enum EventType
    {
        INSERTED = 2,
        REMOVED = 3
    }

}
