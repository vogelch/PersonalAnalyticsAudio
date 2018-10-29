// Created by Christoph Vogel (christoph.vogel@uzh.ch) from the University of Zurich
// Created: 2018-10-01
// 
// Licensed under the MIT License.

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Timers;
using Shared;
using Shared.Data;
using System.Collections.Generic;
using WindowsActivityTracker.Visualizations;
using AudioTracker.Data;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Reflection;
using System.Linq;
using AudioTracker.Views;
using NAudio.Wave;
using NAudio.CoreAudioApi;
using System.IO;
using System.ComponentModel;
using System.Threading;

namespace AudioTracker
{
    /// <summary>
    /// This tracker stores audio data in the database.
    /// </summary>
    public sealed class Daemon : BaseTrackerDisposable, ITracker
    {
        private bool _disposed = false;
        private System.Timers.Timer _idleCheckTimer;
        private System.Timers.Timer _idleSleepValidator;
        private DateTime _previousIdleSleepValidated = DateTime.MinValue;

        private bool _isConnectedAudioDevice = false;
        private bool _wasFirstStart = true;
        private bool _isPAPaused = false;

        // Timer for saving file (currently not used)
        //private Timer _saveToFileTimer;
        private DateTime _tsStart;

        // audio device and recording
        public static MMDevice inputAudioDevice { get; set; } //TODO: look into access modifier
        private static WaveIn waveSource = null;
        private static int recordingSampleRate = 16000;
        private static int recordingChannels = 1;
        private static string recordingFilePrefix = "audio";

        // Buffers for user input, they are emptied every x seconds (see Settings.UserInputAggregationInterval)
        /*
        private static readonly ConcurrentQueue<KeystrokeEvent> KeystrokeBuffer = new ConcurrentQueue<KeystrokeEvent>();
        private static readonly ConcurrentQueue<MouseClickEvent> MouseClickBuffer = new ConcurrentQueue<MouseClickEvent>();
        private static readonly ConcurrentQueue<MouseMovementSnapshot> MouseMovementBuffer = new ConcurrentQueue<MouseMovementSnapshot>();
        private static readonly ConcurrentQueue<MouseScrollSnapshot> MouseScrollsBuffer = new ConcurrentQueue<MouseScrollSnapshot>();
        */

        // Lists which temporarily store all user input data until they are saved as an aggregate in the database
        /*
        private static readonly List<KeystrokeEvent> KeystrokeListToSave = new List<KeystrokeEvent>();
        private static readonly List<MouseClickEvent> MouseClickListToSave = new List<MouseClickEvent>();
        private static readonly List<MouseMovementSnapshot> MouseMovementListToSave = new List<MouseMovementSnapshot>();
        private static readonly List<MouseScrollSnapshot> MouseScrollsListToSave = new List<MouseScrollSnapshot>();
        */


        #region ITracker Stuff

        public Daemon()
        {
            Name = Settings.TRACKER_NAME;
            inputAudioDevice = null;
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _idleCheckTimer.Dispose();
                    _idleSleepValidator.Dispose();
                }
                _disposed = true;
            }
            base.Dispose(disposing);
        }

        /*
        void t1_Tick(object sender, EventArgs e)
        {
            // dieser Code wird ausgeführt, wenn der Timer abgelaufen ist
        }
        */
        
        public override void Start()
        {
            try
            {
                // Register Save-To-Database Timer
                /*
                if (_saveToFileTimer != null)
                {
                    Stop();
                }
                _saveToFileTimer = new Timer();
                _saveToFileTimer.Interval = Settings.UserInputAggregationInterval.TotalMilliseconds;
                _saveToFileTimer.Elapsed += SaveToFileTick;
                //t1.Tick += new EventHandler(t1_Tick); // Eventhandler ezeugen der beim Timerablauf aufgerufen wird
                _saveToFileTimer.Start();

                // Set start timestamp for tracking
                _tsStart = DateTime.Now;
                */

                // Check whether Java is available, copy LIUM jar file resource to executing location
                if (isJavaAvailable())
                {
                    Logger.WriteToConsole("Java is available on the system.");
                    var msg = new Exception("Java is available on the system.");
                    Logger.WriteToLogFile(msg);
                }
                else
                {
                    Logger.WriteToConsole("Java is NOT available on the system.");
                    var msg = new Exception("Java is NOT available on the system.");
                    Logger.WriteToLogFile(msg);
                }
                WriteResourceToFile("AudioTracker.Resources.LIUM.LIUM_SpkDiarization-8.4.1.jar", "lium.jar");

                // Start Audio recording
                if (Settings.IS_RAW_RECORDING_ENABLED)
                {
                    waveSource = new WaveIn(WaveCallbackInfo.FunctionCallback());
                    waveSource.DeviceNumber = 0; // inputAudioDevice. // InputDeviceIndex
                    waveSource.WaveFormat = new WaveFormat(recordingSampleRate, recordingChannels);
                    waveSource.BufferMilliseconds = 30000; // TODO: get this number from settings
                    waveSource.DataAvailable += new EventHandler<WaveInEventArgs>(waveSource_DataAvailable);
                    waveSource.RecordingStopped += new EventHandler<StoppedEventArgs>(waveSource_RecordingStopped);
                    try
                    {
                        waveSource.StartRecording();
                        Logger.WriteToConsole("Audio recording has started.");
                    }
                    catch (Exception e)
                    {
                        Logger.WriteToLogFile(e);
                    }
                }

                IsRunning = true;
            }
            catch (Exception e)
            {
                Logger.WriteToLogFile(e);
                //Database.GetInstance().LogWarning("Registering events failed: " + e.Message);

                IsRunning = false;
            }
        }

        void waveSource_DataAvailable(object sender, WaveInEventArgs e)
        {
            string fileNameDateTime = DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss");
            string audioFilename = Shared.Settings.ExportFilePath + "\\" + recordingFilePrefix + "-" + fileNameDateTime + ".wav";
            WaveFileWriter waveFile = new WaveFileWriter(audioFilename, waveSource.WaveFormat);
            waveFile.Write(e.Buffer, 0, e.BytesRecorded);
            //int seconds = (int)(waveFile.Length / waveFile.WaveFormat.AverageBytesPerSecond);
            //Logger.WriteToConsole("Writing " + seconds + " seconds of audio to file.");
            waveFile.Close();
            waveFile.Dispose();
            waveFile = null;

            // start analysis of new audio chunk
            string outputFilename = Shared.Settings.ExportFilePath + "\\" + "lium-" + fileNameDateTime + ".seg";
            liumAnalysis(audioFilename, outputFilename);
        }

        void waveSource_RecordingStopped(object sender, EventArgs e)
        {
            if (waveSource != null)
            {
                waveSource.Dispose();
                waveSource = null;
            }
        }

        public override void Stop()
        {
            try
            {
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
                if (Settings.IS_RAW_RECORDING_ENABLED)
                {
                    waveSource.StopRecording();
                }
            }
            catch (Exception e)
            {
                Logger.WriteToLogFile(e);
                //Database.GetInstance().LogWarning("Un-Registering events failed: " + e.Message);
            }

            IsRunning = false;
        }

        /// <summary>
        /// Saves the buffer to a file and clears it afterwards.
        /// </summary>
        private async void SaveToFileTick(object sender, EventArgs e)
        {
            await Task.Run(() => SaveAudioBufferToFile());
        }

        /// <summary>
        /// dequeues the currently counted number of elements from the buffer and saves them to the database
        /// (it is probable that more samples will be added to the end of audio input queue while this happens,
        /// those samples will be saved to a file with the next run of this method)
        /// </summary>
        private void SaveAudioBufferToFile()
        {
            try
            {
                // TODO: get date and time for file name
                var now = DateTime.Now;
                var tsEnd = now.AddSeconds(-now.Second).AddSeconds(-Settings.UserInputAggregationIntervalInSeconds); // round to minute, - 60s
                var tsStart = tsEnd.AddSeconds(-Settings.UserInputAggregationIntervalInSeconds); // tsEnd - 60s
                //aggregate.TsStart = tsStart;
                //aggregate.TsEnd = tsEnd;

                // sum up user input types in aggregate
                //AddKeystrokesToAggregate(aggregate, tsStart, tsEnd);


                // TODO: save audio to file
            }
            catch (Exception e)
            {
                Database.GetInstance().LogWarning("Saving audio file failed: " + e.Message);
            }
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
            return IsRunning ? (Name + " is running. An audio device is " + (_isConnectedAudioDevice ? "connected." : "NOT connected.")) : (Name + " is NOT running.");
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
            var vis1 = new DayProgramsUsedPieChart(date);
            var vis2 = new DayMostFocusedProgram(date);
            var vis3 = new DayFragmentationTimeline(date);
            return new List<IVisualization> { vis1, vis2, vis3 };
        }

        public override List<IVisualization> GetVisualizationsWeek(DateTimeOffset date)
        {
            var vis1 = new WeekProgramsUsedTable(date);
            var vis2 = new WeekWorkTimeBarChart(date);
            return new List<IVisualization> { vis1, vis2 };
        }

        public override List<IFirstStartScreen> GetStartScreens()
        {
            return new List<IFirstStartScreen>() { new FirstStartWindow() };
        }

        #endregion

        /// <summary>
        /// Checks whether Java is available by spawning a process and executing "java - version"
        /// Returns true if Java is available, returns false if Java could not be executed for some reason
        /// </summary>
        private bool isJavaAvailable()
        {
            //TODO: possibly use IKVM.NET to avoid this mess completely?

            //alternative: look into registry, but does not guarantee that it will actually run; also: could be at a different place
            RegistryKey rk = Registry.LocalMachine;
            RegistryKey subKey = rk.OpenSubKey("SOFTWARE\\JavaSoft\\Java Runtime Environment");
            string currentVerion = subKey.GetValue("CurrentVersion").ToString();
            Logger.WriteToConsole("Java version: " + currentVerion);

            bool isAvailable = false;
            List<String> output = new List<string>();

            try
            {
                Process process = new Process();
                string arguments = "-version" + " \"";
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.FileName = @"java";
                process.StartInfo.Arguments = arguments;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.Arguments = arguments;

                process.OutputDataReceived += new DataReceivedEventHandler((s, e) =>
                {
                    if (e.Data != null)
                    {
                        output.Add((string)e.Data);
                    }
                });
                process.ErrorDataReceived += new DataReceivedEventHandler((s, e) =>
                {
                    if (e.Data != null)
                    {
                        output.Add((String)e.Data);
                    }
                });

                process.Start();
                process.WaitForExit();

                string result = null;
                result = process.StandardOutput.ReadToEnd();
                string error = null;
                error = process.StandardError.ReadToEnd();
                Logger.WriteToConsole("Java test: " + result + "//" + error);

                isAvailable = (process.ExitCode == 0);
            }
            catch (Exception ex)
            {
                Logger.WriteToLogFile(ex);
            }

            return isAvailable;
        }

        /// <summary>
        /// Starts Java application with LIUM.jar to analyse last n minutes of audio (without showing window)
        /// Returns console output as a string for further parsing.
        /// </summary>
        /// <param name="tsStart"></param>
        /// <param name="tsEnd"></param>
        /// <param name="windowTitle"></param>
        /// <param name="process"></param>
        private void runJarFile(string liumInputFilename, string liumOutputFilename)
        {
            try
            {
                //TODO: get binary name "lium.jar" from settings
                //string epubCheckPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "lium.jar");
                Process liumProcess = new Process();
                //liumProcess.StartInfo.FileName = "CMD.EXE";
                liumProcess.StartInfo.FileName = @"java";
                string arguments = "-Xmx2024m -jar lium.jar --fInputMask=./ " + liumInputFilename + " --sOutputMask=./ " + liumOutputFilename + " --doCEClustering showName \"";
                liumProcess.StartInfo.Arguments = arguments;
                //liumProcess.EnableRaisingEvents = true;
                Logger.WriteToConsole("arguments: " + liumProcess.StartInfo.Arguments);
                liumProcess.StartInfo.UseShellExecute = false;
                liumProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                liumProcess.StartInfo.CreateNoWindow = false; //TODO: set to true
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


                /*
                string output = liumProcess.StandardOutput.ReadToEnd();
                Console.WriteLine(output);
                string err = liumProcess.StandardError.ReadToEnd();
                Console.WriteLine(err);
                liumProcess.WaitForExit();
                */

                /*
                liumProcess.BeginOutputReadLine();
                liumProcess.BeginErrorReadLine();
                liumProcess.WaitForExit();
                */

                Logger.WriteToConsole("LIUM console output: " + result);
                //return result;
            }
            catch (Exception ex)
            {
                Logger.WriteToLogFile(ex);
                //return null;
            }
        }

        static void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            //TODO: feed it back to a string and give this to method
            Console.WriteLine(outLine.Data);
        }

        public void WriteResourceToFile(string resourceName, string fileName)
        {
            //TODO: instead loop through the folder and take the most recent version present
            const string subfolder = "PersonalAnalytics.Resources.LIUM.";
            var assembly = Assembly.GetExecutingAssembly();
            foreach (var name in assembly.GetManifestResourceNames())
            {
                /*
                // Skip names outside of your desired subfolder
                if (!name.StartsWith(subfolder))
                {
                    continue;
                }
                using (Stream input = assembly.GetManifestResourceStream(name))
                using (Stream output = File.Create(path + name.Substring(subfolder.Length)))
                {
                    input.CopyTo(output);
                }
                */
            }

            using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                using (var file = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    resource.CopyTo(file);
                }
            }
            //TODO: resource exists? file is writable?
        }

        private void liumAnalysis(string liumInputFilename, string liumOutputFilename)
        {
            //runJarFile("test.wav", "test.seg");
            //Logger.WriteToConsole("LIUM result: " + result);
            string arguments = "-Xmx2024m -jar lium.jar --fInputMask=\"" + liumInputFilename + "\" --sOutputMask=\"" + liumOutputFilename + "\" --doCEClustering showName";
            //string arguments = "-Xmx2024m -jar lium.jar";

            var worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync();

            void worker_DoWork(object sender, DoWorkEventArgs e)
            {
                //List<String> output = new List<string>();
                int timeout = 30000;

                try
                {
                    Process liumProcess = new Process();
                    liumProcess.StartInfo.FileName = @"java";
                    liumProcess.StartInfo.Arguments = arguments;
                    liumProcess.EnableRaisingEvents = true;
                    Logger.WriteToConsole("arguments: " + liumProcess.StartInfo.Arguments);
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
                            Logger.WriteToConsole("LIUM output event received.");
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
                            Logger.WriteToConsole("LIUM error event received.");
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

                        if (liumProcess.WaitForExit(timeout) && outputWaitHandle.WaitOne(timeout) && errorWaitHandle.WaitOne(timeout))
                        {
                            Logger.WriteToConsole("LIUM process completed.");
                            Logger.WriteToConsole("LIUM output: " + output.ToString());
                            Logger.WriteToConsole("LIUM error messages: " + error.ToString());
                            // Process completed.
                            //TODO: Check process.ExitCode here.
                        }
                        else
                        {
                            Logger.WriteToConsole("LIUM process timeout.");
                            // Timed out.
                        }
                    }

                    /*
                    liumProcess.OutputDataReceived += new DataReceivedEventHandler((s, evt) =>
                    {
                        if (evt.Data != null)
                        {
                            output.Add((string)evt.Data);
                        }
                        Logger.WriteToConsole("LIUM output received: " + (String)evt.Data);
                    });
                    liumProcess.ErrorDataReceived += new DataReceivedEventHandler((s, evt) =>
                    {
                        if (evt.Data != null)
                        {
                            output.Add((String)evt.Data);
                        }
                        Logger.WriteToConsole("LIUM error received: " + (String)evt.Data);
                        //p.Responding
                        //p.Kill();
                    });
                    */

                    /*
                    try
                    {
                        liumProcess.Start();
                        Logger.WriteToConsole("LIUM process started.");
                        liumProcess.WaitForExit();

                        string result = null;
                        result = liumProcess.StandardOutput.ReadToEnd();
                        string error = null;
                        error = liumProcess.StandardError.ReadToEnd();
                        Logger.WriteToConsole("LIUM process output read.");

                        var outputString = String.Join("\n", output);
                        //Logger.WriteToConsole("LIUM test: " + result + "//" + error);
                        Logger.WriteToConsole("LIUM output: " + outputString);
                    }
                    catch (Exception error)
                    {
                        Logger.WriteToLogFile(error);
                    }
                    */

                }
                catch (Exception ex)
                {
                    Logger.WriteToLogFile(ex);
                }

            }

            void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
            {
                Logger.WriteToConsole("Worker completed.");
            }

            /*
            try
            {
                var liumTask = Helpers.ProcessAsyncHelper.ProcessAsyncHelper.ExecuteShellCommand(@"java", arguments, 30000);
                liumTask.RunSynchronously();
            }
            catch (Exception e)
            {
                Logger.WriteToLogFile(e);
            }
            //Helpers.ProcessAsyncHelper.ProcessResult result = task.Result;
            //Logger.WriteToConsole("LIUM output: " + result.Output);
            */

        }

        /*
        private LiumResult parseLiumConsoleOutput(String consoleOutput)
        {
            ...
        }
        */

    }

}
