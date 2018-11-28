// Created by Christoph Vogel (christoph.vogel@uzh.ch) from the University of Zurich
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

namespace AudioTracker
{

    public enum EventType
    {
        INSERTED = 2,
        REMOVED = 3
    }

    /// <summary>
    /// This tracker stores audio data in the database.
    /// </summary>
    public sealed class Daemon : BaseTrackerDisposable, ITracker
    {
        private bool IsDisposed = false;
        private System.Timers.Timer _idleCheckTimer;
        private System.Timers.Timer _idleSleepValidator;
        private DateTime _previousIdleSleepValidated = DateTime.MinValue;

        private System.Timers.Timer checkAudioDeviceTimer;
        private bool _isConnectedAudioDevice = false;
        private bool _wasFirstStart = true;
        private bool isPaused = false;

        // audio device notification
        private NotifyIcon NotificationHandle;

        // audio device and recording
        public MMDevice inputAudioDevice { get; set; } //TODO: look into access modifier
        private WaveIn waveSource = null;
        public static int lastNumberOfAudioDevices = 0; //TODO: look into access modifier //TODO: bad, should be a instance variable (which is probably not possible) or stored at another location
        public static int lastNumberOfAudioDevicesTick = 0;
        private const int recordingSampleRate = 16000;
        private int recordingChannels = 1;
        private string recordingFilePrefix = "audio";
        private short[] currentAudioInputBuffer = null;
        private DateTime lastAbnormalRecordingAbort = new DateTime();
        private bool isCurrentStopAbnormal = false;

        #region ITracker Stuff

        public Daemon()
        {
            Name = Settings.TRACKER_NAME;
            if (Settings.IS_RAW_RECORDING_ENABLED)
            {
                Name += " (with raw recording)";
            }
            inputAudioDevice = null;
        }

        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    _idleCheckTimer.Dispose();
                    _idleSleepValidator.Dispose();
                }
                IsDisposed = true;
            }
            base.Dispose(disposing);
        }

        public override void Start()
        {

            string[] resourceNames = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            var msg1 = new Exception("Resource names: " + string.Join(" ; ", resourceNames));
            Logger.WriteToLogFile(msg1);

            //TODO: this should not be necessary
            JavaHelper.WriteResourceToFile("AudioTracker.Resources.LibMP3Lame.libmp3lame.32.dll", "libmp3lame.32.dll");
            JavaHelper.WriteResourceToFile("AudioTracker.Resources.LibMP3Lame.libmp3lame.64.dll", "libmp3lame.64.dll");

            isPaused = false;
            try
            {
                // Register Audio Device Timer
                if (checkAudioDeviceTimer != null)
                {
                    Stop();
                }
                checkAudioDeviceTimer = new System.Timers.Timer();
                checkAudioDeviceTimer.Interval = 5000; // Settings.UserInputAggregationInterval.TotalMilliseconds;
                checkAudioDeviceTimer.Elapsed += CheckAudioDeviceTick;
                //checkAudioDeviceTimer.Start();

                // start device notifications
                
                //DeviceChangeNotifier.Stop();
                //DeviceChangeNotifier.DeviceNotify += HandleCustomEvent;
                //DeviceChangeNotifier.Start();
                /*

                DeviceNotification myDeviceNotifier = new DeviceNotification();
                DeviceNotification.

                System.Windows.Forms.Application.Run(new DeviceChangeNotifier());
                UsbNotification.RegisterUsbDeviceNotification(this.Handle);
                */

                // create management class object
                //ManagementClass mc = new ManagementClass("Win32_ComputerSystem");

                /*
                Machine Make: LENOVO
                Machine Model: 20AWS18T00
                System Type: x64-based PC
                Host Name: DESKTOP-23JVN3Q
                Logon User Name: DESKTOP-23JVN3Q\Christoph
                */

                //collection to store all management objects
                /*
                ManagementObjectCollection moc = mc.GetInstances();
                if (moc.Count != 0)
                {
                    foreach (ManagementObject mo in mc.GetInstances())
                    {
                        // display general system information
                        Console.WriteLine("\nMachine Make: {0}\nMachine Model: {1}\nSystem Type: {2}\nHost Name: {3}\nLogon User Name: {4}\n",
                                          mo["Manufacturer"].ToString(),
                                          mo["Model"].ToString(),
                                          mo["SystemType"].ToString(),
                                          mo["DNSHostName"].ToString(),
                                          mo["UserName"].ToString());
                    }
                }
                */

                /*
                var watcher = new ManagementEventWatcher();
                string WqlEventQueryString = "SELECT * FROM Win32_VolumeChangeEvent WHERE EventType = " + EventType.INSERTED + " or EventType = " + EventType.REMOVED;
                WqlEventQuery query = new WqlEventQuery(WqlEventQueryString);

                watcher.EventArrived += (s, e) =>
                {
                    string driveName = e.NewEvent.Properties["DriveName"].Value.ToString();
                    EventType eventType = (EventType)(Convert.ToInt16(e.NewEvent.Properties["EventType"].Value));

                    string eventName = Enum.GetName(typeof(EventType), eventType);

                    Console.WriteLine("{0}: {1} {2}", DateTime.Now, driveName, eventName);
                };
                watcher.Query = query;
                try
                {
                    watcher.Start();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    //Logger.WriteToLogFile(e);
                    //Database.GetInstance().LogWarning("Registering events failed: " + e.Message);
                }

                //wait for user action
                Console.ReadLine();

                // Accessing TPM-related information
                bool isTpmPresent;
                UInt32 status = 0;
                object[] wmiParams = null;
                */

                /*
                // create management class object
                ManagementClass mc = new ManagementClass("/root/CIMv2/Security/MicrosoftTpm:Win32_Tpm");

                //collection to store all management objects
                ManagementObjectCollection moc = mc.GetInstances();

                // Retrieve single instance of WMI management object
                ManagementObjectCollection.ManagementObjectEnumerator moe = moc.GetEnumerator();
                moe.MoveNext();
                ManagementObject mo = (ManagementObject)moe.Current;
                if (null == mo)
                {
                    isTpmPresent = false;
                    Console.WriteLine("\nTPM Present: {0}\n", isTpmPresent.ToString());
                }
                else
                {
                    isTpmPresent = true;
                    Console.WriteLine("\nTPM Present: {0}\n", isTpmPresent.ToString());
                }

                if (isTpmPresent) // Query if TPM is in activated state
                {
                    wmiParams = new object[1];
                    wmiParams[0] = false;
                    status = (UInt32) mo.InvokeMethod("IsActivated", wmiParams);
                    if (0 != status)
                    {
                        Console.WriteLine("The WMI method call {0} returned error status {1}", "IsActivated", status);
                    }
                    else
                    {
                        Console.WriteLine("TPM Status: {0}", status);
                    }
                }
                */

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

        private async void CheckAudioDeviceTick(object sender, EventArgs e)
        {
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

        }

        private void StartAudioRecording()
        {
            //TODO: return value should be boolean

            if (Settings.IS_RAW_RECORDING_ENABLED) // TODO: this check should not be here
            {
                waveSource = new WaveIn(WaveCallbackInfo.FunctionCallback());
                try
                {
                    //TODO: move this to the settings
                    int waveInDevices = WaveIn.DeviceCount;
                    for (int waveInDevice = 0; waveInDevice < waveInDevices; waveInDevice++)
                    {
                        if (waveInDevice == Settings.inputAudioDeviceNumber)
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
                                    inputAudioDevice = CurrentDevice;
                                }
                            }
                        }
                    }

                    waveSource.DeviceNumber = Settings.inputAudioDeviceNumber;
                    Logger.WriteToConsole("Selected audio input device number: " + Settings.inputAudioDeviceNumber);
                    waveSource.WaveFormat = new WaveFormat(recordingSampleRate, recordingChannels);
                    waveSource.BufferMilliseconds = Settings.AudioRecordingChunkLength;
                    waveSource.DataAvailable += new EventHandler<WaveInEventArgs>(waveSource_DataAvailable);
                    waveSource.RecordingStopped += new EventHandler<NAudio.Wave.StoppedEventArgs>(waveSource_RecordingStopped);
                    waveSource.StartRecording();

                    var msg = new Exception("Audio recording has started.");
                    Logger.WriteToLogFile(msg);
                    Database.GetInstance().LogInfo("Audio recording has started.");
                }
                catch (Exception e)
                {
                    Logger.WriteToLogFile(e);
                }
            }
        }

        private void StopAudioRecording()
        {
            // TODO: return value should be boolean
            if (Settings.IS_RAW_RECORDING_ENABLED) // TODO: this check should not be necessary at this point; it should actually be IS_RAW_STORING_ENABLED
            {
                if (waveSource != null)
                {
                    waveSource.StopRecording();
                }
            }
        }

        void waveSource_DataAvailable(object sender, WaveInEventArgs e)
        {
            try
            {
                string fileNameDateTime = DateTime.Now.ToString("yyyy-MM-dd--HH-mm-ss");
                string audioFilename = Shared.Settings.ExportFilePath + "\\" + recordingFilePrefix + "-" + fileNameDateTime + ".wav";
                WaveFileWriter waveFile = new WaveFileWriter(audioFilename, waveSource.WaveFormat);
                waveFile.Write(e.Buffer, 0, e.BytesRecorded);

                // Calculate loudness and check whether microphone is (probably) muted
                float[] relativeAmplitudes = new float[e.BytesRecorded / 2];
                int j = 0;
                for (int i = 0; i < e.BytesRecorded; i += 2)
                {
                    short sample = (short)((e.Buffer[i + 1] << 8) | e.Buffer[i + 0]);
                    float sample32 = sample / 32768.0f; // shouldn't this be 32767?
                    relativeAmplitudes[j] = sample32;
                    j++;
                }
                float maxValue = relativeAmplitudes.Max();
                float minValue = relativeAmplitudes.Min();

                Dictionary<float, int> bucket = new Dictionary<float, int>();
                float modeAmplitudeValue = float.MinValue;
                int maxCount = 0;
                int count;
                foreach (float sample in relativeAmplitudes)
                {
                    if (bucket.TryGetValue(sample, out count))
                    {
                        count++;
                        bucket[sample] = count;
                    }
                    else
                    {
                        count = 1;
                        bucket.Add(sample, count);
                    }

                    if (count >= maxCount)
                    {
                        modeAmplitudeValue = sample;
                        maxCount = count;
                    }

                }

                //TODO: calculate median
                //TODO: average should be calculated as average of *absolute values*
                //TODO: display number of samples
                //TODO: sum of samples non-relative, max, min, avg non-relative
                //TODO: chech endianess of system (BitConverter.IsLittleEndian)

                if (maxValue < 0.0015 && minValue > -0.0015 && maxCount > 25000)
                {
                    Console.WriteLine("Microphone is probably muted...");
                    Console.WriteLine("Maximum value: " + maxValue);
                    Console.WriteLine("Minimum value: " + minValue);
                    Console.WriteLine("Average value: " + relativeAmplitudes.Sum() / relativeAmplitudes.Length);
                    Console.WriteLine("Mode value: " + modeAmplitudeValue + " (" + maxCount + " occurrences)");
                }

                // loudness: 20 * log10(Abs(amplitude))
                // square samples, take average of these squares, take root
                // relative loudness: last second: average |amplitude|

                //Logger.WriteToConsole("Device name: " + inputAudioDevice.DeviceFriendlyName);  <- throws an exception
                //Logger.WriteToConsole("Device mute: " + inputAudioDevice.AudioEndpointVolume.VolumeRange); // inputAudioDevice.AudioEndpointVolume.Mute.ToString()
                int lengthOfRecording = (int)(1000 * waveFile.Length / waveFile.WaveFormat.AverageBytesPerSecond);
                if (lengthOfRecording != Settings.AudioRecordingChunkLength)
                {
                    var msg = new Exception("Recording of audio segment has stopped early (after " + lengthOfRecording  + " milliseconds).");
                    Logger.WriteToLogFile(msg);
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

                //save as MP3 file
                string audioFilenameMp3 = Shared.Settings.ExportFilePath + "\\" + recordingFilePrefix + "-" + fileNameDateTime + ".mp3";
                ConvertWavToMp3(audioFilename, audioFilenameMp3);

                // start analysis of new audio chunk
                string outputFilename = Shared.Settings.ExportFilePath + "\\" + "lium-" + fileNameDateTime + ".seg";
                liumAnalysis(audioFilename, outputFilename);
            }
            catch (Exception ex)
            {
                Logger.WriteToLogFile(ex);
            }

        }

        void waveSource_RecordingStopped(object sender, EventArgs e)
        {
            Logger.WriteToConsole("Event args: " + e.ToString());

            //TODO: remove abuse of log file and store result solely in database log table
            var msg = new Exception("Audio recording has stopped.");
            Logger.WriteToLogFile(msg);
            Database.GetInstance().LogInfo("Audio recording has stopped.");

            if (waveSource != null)
            {
                waveSource.Dispose();
                waveSource = null;
            }
        }

        void HandleCustomEvent(Message msg)
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

            //TODO: check
            if (msg.Msg == WPARAM_DEVICE_CHANGE_EVENT)
            {
                int wparamAsInt = msg.WParam.ToInt32();
                /*
                if (wparamAsInt == WPARAM_DEVICE_ARRIVAL) // neu eingesteckt, funktioniert für USB-Stick
                {
                    Logger.WriteToConsole("Device plugged in (e.g. USB stick)");
                    //if
                }
                */
                if (wparamAsInt == WPARAM_DEVICE_NODE_CHANGED)
                {
                    //Logger.WriteToConsole("A device has been plugged in or plugged out.");
                    Thread.Sleep(250);
                    TimeSpan timeSinceLastAbnormalAbortOfRecording = lastAbnormalRecordingAbort - DateTime.Now;
                    Logger.WriteToConsole("Time span since last abnormal termination: " + timeSinceLastAbnormalAbortOfRecording.TotalMilliseconds);
                    if (Math.Abs(timeSinceLastAbnormalAbortOfRecording.TotalMilliseconds) < 250)
                    {
                        Logger.WriteToConsole("Microphone probably unplugged...");
                        //isCurrentStopAbnormal = true;
                        if (Settings.IS_DEVICE_EVENT_NOTIFICATION_ENABLED)
                        {
                            NotificationHandle = new NotifyIcon();
                            NotificationHandle.Visible = true;
                            NotificationHandle.BalloonTipTitle = "PersonalAnalytics: Audio device removed!";
                            NotificationHandle.BalloonTipText = "The microphone used by the audio tracker was removed. Audio recording has been stopped. Personal Analytics will try to automatically resume recording on reconnect."; // ": Audio device " + inputAudioDevice.DeviceFriendlyName + " was removed.";
                            NotificationHandle.Icon = SystemIcons.Exclamation;
                            NotificationHandle.Text = Name + ": Audio device removed!";
                            NotificationHandle.ShowBalloonTip(10 * 1000);
                        }
                        //DeviceChangeNotifier.Start();
                        //Stop();
                    }

                    // check if new device is an audio device
                    MMDeviceEnumerator deviceEnumerator = new MMDeviceEnumerator();
                    MMDeviceCollection AudioDevices = deviceEnumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);
                    int currentNumberOfAudioDevices = WaveIn.DeviceCount;
                    Logger.WriteToConsole("current: " + currentNumberOfAudioDevices + " / last: " + lastNumberOfAudioDevices);
                    if (currentNumberOfAudioDevices > lastNumberOfAudioDevices)
                    {
                        lastNumberOfAudioDevices = currentNumberOfAudioDevices;
                        Logger.WriteToConsole("An audio device has been added.");

                        //TODO: check if it is the same device which was removed

                        /*
                        if (Settings.IS_DEVICE_EVENT_NOTIFICATION_ENABLED)
                        {
                            NotificationHandle = new NotifyIcon();
                            NotificationHandle.Visible = true;
                            NotificationHandle.BalloonTipTitle = "PersonalAnalytics: Audio device added!";
                            NotificationHandle.BalloonTipText = Name + ": Audio device has been added."; // ": Audio device " + inputAudioDevice.DeviceFriendlyName + " was removed.";
                            NotificationHandle.Icon = SystemIcons.Exclamation;
                            NotificationHandle.Text = Name + ": Audio device added!";
                            NotificationHandle.ShowBalloonTip(60 * 1000);
                        }
                        */

                    }
                    else if (currentNumberOfAudioDevices < lastNumberOfAudioDevices)
                    {
                        lastNumberOfAudioDevices = currentNumberOfAudioDevices;
                        Logger.WriteToConsole("An audio device has been removed.");
                    }
                    else
                    {
                        Logger.WriteToConsole("Changed device was not an audio device.");
                    }

                }
                /*
                else if (wparamAsInt == WPARAM_DEVICE_REMOVE_COMPLETE) // ausgesteckt
                {
                    Logger.WriteToConsole("Device plugged out.");
                    //if
                }
                */
            }

            if (Settings.IS_DEVICE_EVENT_RECORDING_ENABLED)
            {
                //TODO: store device event in database
                //DatabaseConnector.AddHeartMeasurementsToDatabase(measurements, false);
            }

        }


        /*
        using System.Runtime.InteropServices;
        const int WM_DEVICECHANGE = 0x0219;
        // new device is pluggedin
        const int DBT_DEVICEARRIVAL = 0x8000; 
        //device is removed 
        const int DBT_DEVICEREMOVECOMPLETE = 0x8004; 
        //device is changed
        const int DBT_DEVNODES_CHANGED = 0x0007; 
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_DEVICECHANGE
            {
                //Your code here.
            }
            base.WndProc(ref m);
        }
        */

        public override void Stop()
        {
            Logger.WriteToConsole("Daemon stop called.");
            isPaused = true;

            try
            {
                // stop device change notifier if not an abnormal stop
                // DeviceChangeNotifier.Stop();
                /*
                if (!isCurrentStopAbnormal)
                {
                    DeviceChangeNotifier.Start();
                    isCurrentStopAbnormal = false;
                }
                */

                checkAudioDeviceTimer = null;
                checkAudioDeviceTimer.Dispose();

                // Unregister idle time checker Timer
                if (_idleCheckTimer != null)
                {
                    _idleCheckTimer.Stop();
                    _idleCheckTimer.Dispose();
                    _idleCheckTimer = null;
                }

                // Unregister idle resume validator Timer
                if (_idleSleepValidator != null)
                {
                    _idleSleepValidator.Stop();
                    _idleSleepValidator.Dispose();
                    _idleSleepValidator = null;
                }

                // Stop audio recording
                StopAudioRecording();
                if (Settings.IS_RAW_RECORDING_ENABLED)
                {
                    if (waveSource != null)
                    {
                        waveSource.StopRecording();
                    }
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

        /// <summary>
        /// Starts Java application with LIUM.jar to analyse last n minutes of audio (without showing window)
        /// Returns console output as a string for further parsing.
        /// </summary>
        /// <param name="liumInputFilename"></param>
        /// <param name="liumOutputFilename"></param>
        private void runJarFile(string liumInputFilename, string liumOutputFilename)
        {
            try
            {
                //TODO: get binary name "lium.jar" from settings
                //string epubCheckPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "lium.jar");
                Process liumProcess = new Process();
                liumProcess.StartInfo.FileName = @"java";
                string arguments = "-Xmx2024m -jar lium.jar --fInputMask=./ " + liumInputFilename + " --sOutputMask=./ " + liumOutputFilename + " --doCEClustering showName \"";
                liumProcess.StartInfo.Arguments = arguments;
                //liumProcess.EnableRaisingEvents = true;
                liumProcess.StartInfo.UseShellExecute = false;
                liumProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                liumProcess.StartInfo.CreateNoWindow = true;
                liumProcess.StartInfo.RedirectStandardOutput = true;
                liumProcess.StartInfo.RedirectStandardError = true;
                //liumProcess.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
                //liumProcess.ErrorDataReceived += new DataReceivedEventHandler(OutputHandler);

                //Start the process
                string result = null;
                try
                {
                    liumProcess.Start();

                    /*
                    Logger.WriteToConsole("LIUM console output: ");
                    while ((output = liumProcess.StandardOutput.ReadLine()) != null)
                    {
                        Logger.WriteToConsole(output);
                    }
                    liumProcess.WaitForExit();
                    */

                    var output = new List<string>();
                    while (liumProcess.StandardOutput.Peek() > -1)
                    {
                        output.Add(liumProcess.StandardOutput.ReadLine());
                    }
                    while (liumProcess.StandardError.Peek() > -1)
                    {
                        output.Add(liumProcess.StandardError.ReadLine());
                    }
                    liumProcess.WaitForExit();
                    result = String.Join("\n", output);
                }
                catch (Exception e)
                {
                    Logger.WriteToLogFile(e);
                }

                //Logger.WriteToConsole("LIUM console output: " + result);
                //return result;
            }
            catch (Exception ex)
            {
                Logger.WriteToLogFile(ex);
                //return null;
            }
        }

        /*
        static void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            //TODO: feed it back to a string and give this to method

            //Console.WriteLine(outLine.Data);
        }
        */

        private void liumAnalysis(string liumInputFilename, string liumOutputFilename)
        {
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
                            //Logger.WriteToConsole("LIUM output: " + output.ToString());
                            //Logger.WriteToConsole("LIUM error messages: " + error.ToString());
                            
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

                if (File.Exists(@liumInputFilename))
                {
                    File.Delete(@liumInputFilename);
                }
                else
                {
                    //TODO: write to error log
                }
            }

        }

        private static void ConvertWavToMp3(string WavFile, string Mp3FileName)
        {
            //CheckAddBinPath();

            //const string libname = @"libmp3lame.dll";
            //[DllImport(libname, CallingConvention = CallingConvention.Cdecl)]

            using (var resultMemoryStream = new MemoryStream())
            using (var reader = new WaveFileReader(WavFile))
            using (var writer = new LameMP3FileWriter(Mp3FileName, reader.WaveFormat, 16/*LAMEPreset.VBR_90*/))
            {
                reader.CopyTo(writer);
            }
        }

        // Audio input devices
        public static List<string> GetAudioInputDevices()
        {
            var deviceList = new List<string>();
            int waveInDevices = WaveIn.DeviceCount;
            for (int waveInDevice = 0; waveInDevice < waveInDevices; waveInDevice++)
            {
                string deviceProductName = WaveIn.GetCapabilities(waveInDevice).ProductName;
                deviceList.Add(deviceProductName);
            }
            return deviceList;
        }

        /*
        private LiumResult parseLiumConsoleOutput(String consoleOutput)
        {
            ...
        }
        */

        public static void CheckAddBinPath()
        {
            // find path to 'bin' folder
            var binPath = Path.Combine(new string[] { AppDomain.CurrentDomain.BaseDirectory, "bin" });
            // get current search path from environment
            var path = Environment.GetEnvironmentVariable("PATH") ?? "";

            // add 'bin' folder to search path if not already present
            if (!path.Split(Path.PathSeparator).Contains(binPath, StringComparer.CurrentCultureIgnoreCase))
            {
                path = string.Join(Path.PathSeparator.ToString(), new string[] { path, binPath });
                Environment.SetEnvironmentVariable("PATH", path);
            }
        }

        public bool ByteArrayToFile(string byteArrayTargetFileName, byte[] inputByteArray)
        {
            try
            {
                using (var fs = new FileStream(byteArrayTargetFileName, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(inputByteArray, 0, inputByteArray.Length);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in process: {0}", ex);
                return false;
            }
        }


    }

    /*
    static partial class DeviceChangeNotifier : Form
    {
        //Implement an event handler for the DeviceChangeNotifier.DeviceNotify event to get notifications.
        public delegate void DeviceNotifyDelegate(Message msg);
        public event DeviceNotifyDelegate DeviceNotify;
        private DeviceChangeNotifier mInstance;

        public static void Start()
        {
            Thread t = new Thread(runForm);
            t.SetApartmentState(ApartmentState.STA);
            t.IsBackground = true;
            t.Start();
            Logger.WriteToConsole("DeviceNotifier started.");
        }

        public static void Stop()
        {
            //if (mInstance == null) throw new InvalidOperationException("Notifier not started");
            DeviceNotify = null;
            //mInstance.Invoke(new MethodInvoker(mInstance.endForm));
            Logger.WriteToConsole("DeviceNotifier stopped.");
        }

        private static void runForm()
        {
            System.Windows.Forms.Application.Run(new DeviceChangeNotifier());
        }

        private void endForm()
        {
            this.Close();
        }

        protected override void SetVisibleCore(bool value)
        {
            // Prevent window getting visible
            if (mInstance == null)
            {
                CreateHandle();
            }
            mInstance = this;
            value = false;
            base.SetVisibleCore(value);
        }

        protected override void WndProc(ref Message m)
        {
            // Trap WM_DEVICECHANGE
            if (m.Msg == 0x219)
            {
                DeviceNotifyDelegate handler = DeviceNotify;
                if (handler != null) handler(m);
            }
            base.WndProc(ref m);
        }
    }
    */

}
