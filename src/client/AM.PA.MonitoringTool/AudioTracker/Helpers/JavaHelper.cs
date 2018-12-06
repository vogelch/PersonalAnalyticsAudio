// Created by Christoph Vogel (christoph.vogel@uzh.ch) from the University of Zurich
// Created: 2018-10-01
// 
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.Win32;
using Shared;
using Shared.Data;

namespace AudioTracker.Helpers
{
    class JavaHelper
    {
        /// <summary>
        /// Checks whether Java is available by spawning a process and executing "java - version"
        /// Returns true if Java is available, returns false if Java could not be executed for some reason
        /// </summary>
        public static bool IsJavaAvailable()
        {
            //TODO: possibly use IKVM.NET to avoid this mess completely?

            //check whether there is an entry for Java int the Windows registry (but does not guarantee that it will actually run; could also be at a different place)
            RegistryKey rk = Registry.LocalMachine;
            RegistryKey subKey = rk.OpenSubKey("SOFTWARE\\JavaSoft\\Java Runtime Environment");
            string currentVerion = subKey.GetValue("CurrentVersion").ToString();
            Logger.WriteToConsole("Java version: " + currentVerion);
            Database.GetInstance().LogInfo("A registry entry for Java has been found on the participant's system. Version information: " + currentVerion);

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
                Database.GetInstance().LogInfo("Java is available on the participant's system. Java test command (java -version) has been executed successfully. Resulting information: " + result + " Detailed information: " + error.Trim());

                isAvailable = (process.ExitCode == 0);

                if (!isAvailable)
                {
                    Database.GetInstance().LogError("Java is NOT available on the participant's system. Java test command (java -version) failed. Resulting information: " + result + " Detailed information: " + error.Trim());
                }
            }
            catch (Exception ex)
            {
                Logger.WriteToLogFile(ex);
            }

            return isAvailable;
        }

        public static void WriteResourceToFile(string resourceName, string fileName)
        {
            //TODO: return a boolean indicating whether it was successful
            //TODO: move to another class
            var assembly = Assembly.GetExecutingAssembly();

            //TODO: instead loop through the folder and take the most recent version present
            const string subfolder = "PersonalAnalytics.Resources.LIUM.";
            /*
            foreach (var name in assembly.GetManifestResourceNames())
            {
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
            }
            */

            try
            {
                if (File.Exists(@fileName))
                {
                    Logger.WriteToConsole("LIUM JAR file already present. Trying to delete it...");
                    //TODO: delete the file already present and copy it to the folder again (in case there was a new version)
                    //File.Delete(@fileName);
                }
                else
                {
                    // this is necessary because there will be a "System.UnauthorizedAccessException" in mscorlib.dll when pausing and resuming...
                    using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                    {
                        using (var file = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                        {
                            resource.CopyTo(file);
                        }
                    }
                    //TODO: resource exists? file is writable?
                }
            }
            catch (Exception e)
            {
                Logger.WriteToLogFile(e);
            }
        }

    }
}
