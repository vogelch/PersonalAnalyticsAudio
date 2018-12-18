// Created by Christoph Vogel (christoph.vogel@uzh.ch) from the University of Zurich
// Created: 2018-10-01
// 
// Licensed under the MIT License.

using AudioTracker.Helpers;
using AudioTracker.Models;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using Shared;
using Shared.Data;
using System;
using System.Windows;
using System.Windows.Controls;

namespace AudioTracker.Views
{
    /// <summary>
    /// Interaction logic for FirstStartWindow.xaml
    /// </summary>
    public partial class FirstStartWindow : UserControl, IFirstStartScreen
    {
        private AudioDevice SelectedDevice = new AudioDevice();
        /*
        private int? SelectedDeviceNumber = null;
        private string SelectedDeviceDeviceFriendlyName = null;
        private string SelectedDeviceFriendlyName = null;
        private MMDevice SelectedDevice = null;
        private string SelectedDeviceID = null;
        */

        public FirstStartWindow()
        {
            InitializeComponent();

            JavaUnavailableMessage.Visibility = Visibility.Hidden;
            if (!JavaHelper.IsJavaAvailable())
            {
                JavaUnavailableMessage.Visibility = Visibility.Visible;
            }
            /*
            Tuple<bool, string> IsVersionProblem = JavaHelper.IsJavaVersionWrong();
            string AdjustedErrorMessage;
            if (IsVersionProblem.Item1)
            {
                AdjustedErrorMessage = JavaUnavailableMessage.Text + " Error due to conflict of Java version in Windows registry and executable (error message: " + IsVersionProblem.Item2 + ")";
            }
            else
            {
                AdjustedErrorMessage = JavaUnavailableMessage.Text + " (unknown error)";
            }
            JavaUnavailableMessage.Text = AdjustedErrorMessage;
            */
        }

        public string GetTitle()
        {
            return Settings.Name;
        }

        public void PreviousClicked()
        {

        }

        private async void FindDevices(object sender, RoutedEventArgs e)
        {
            //TODO: check Windows settings (?)
            //TODO: preselect plausible input
            try
            {
                AudioDeviceList.Visibility = Visibility.Hidden;
                FindButton.IsEnabled = false;

                // count number of audio devices available
                int numberOfWaveInDevices = WaveIn.DeviceCount;
                Database.GetInstance().LogInfo("AudioTracker startup window: Finsihed looking for audio devices; number of devices found: " + numberOfWaveInDevices);
                if (numberOfWaveInDevices > 0)
                {
                    AudioDeviceList.Visibility = Visibility.Visible;
                }
                else
                {
                    //TODO: show warning/error message
                    Database.GetInstance().LogError("AudioTracker: no audio devices found.");
                }
                AudioDevicesSelectionList.Items.Clear();

                // add audio devices to selection list
                MMDeviceCollection AudioDevices = AudioDeviceHelper.GetActiveInputDevicesCollection();
                foreach (MMDevice CurrentAudioDevice in AudioDevices)
                {
                    AudioDevicesSelectionList.Items.Add(CurrentAudioDevice);
                }
                /*
                string listOfAudioDeviceInformation = "";
                for (int waveInDevice = 0; waveInDevice < numberOfWaveInDevices; waveInDevice++)
                {
                    WaveInCapabilities deviceInfo = WaveIn.GetCapabilities(waveInDevice);
                    AudioDevicesSelectionList.Items.Add(deviceInfo.ProductName);
                    listOfAudioDeviceInformation += "Device " + waveInDevice + ": " + deviceInfo.ProductName + ", " + deviceInfo.Channels + " channels, ID: " + deviceInfo.ProductGuid + "\n";
                }
                Database.GetInstance().LogInfo("AudioTracker: List of audio devices found at startup:" + '\n' + listOfAudioDeviceInformation.TrimEnd('\n'));
                */

                FindButton.IsEnabled = true;

            }
            catch (Exception ex)
            {
                Logger.WriteToLogFile(ex);
            }
        }

        private void OnDeviceSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FindButton.IsEnabled = false;

            SelectedDevice.DeviceInstance = AudioDevicesSelectionList.SelectedItem as MMDevice;
            SelectedDevice.ID = SelectedDevice.DeviceInstance.ID;
            SelectedDevice.DeviceFriendlyName = SelectedDevice.DeviceInstance.DeviceFriendlyName;
            SelectedDevice.FriendlyName = SelectedDevice.DeviceInstance.FriendlyName;

            SelectedDevice.DeviceNumber = AudioDeviceHelper.GetDeviceNumberFromFriendlyName(SelectedDevice.FriendlyName);
            AudioDeviceHelper.SetPropertiesFromDeviceNumber(SelectedDevice, (int)SelectedDevice.DeviceNumber);

            Logger.WriteToConsole("The participant selected the device with the friendly name " + SelectedDevice.FriendlyName);
            Logger.WriteToConsole("The corresponding device number is: " + SelectedDevice.DeviceNumber);

            /*
            if (selectedAudioDevice.DeviceFriendlyName != "USB PnP Sound Device")
            {
                //TODO: warning
            }
            */

            //TODO: show visualization of audio input here such that the user can check that he has selected the correct audio device and that it works
            //TODO: possibly run some checks here:
            //TODO: check whether chosen input is plausible (should be something along the lines of "Mikrofon (USB PnP Sound Device)")
            //TODO: check whether audio device is muted

        }

        public void NextClicked()
        {
            // write selected audio device to settings; needed to communicate this information to the AudioTracker
            Settings.InputAudioDevice = SelectedDevice;

            if (SelectedDevice.DeviceNumber != null)
            {
                Database.GetInstance().LogInfo("AudioTracker: The user has selected the device number " + SelectedDevice.DeviceNumber + " at startup.");
                Logger.WriteToConsole("AudioTracker: The user has selected the device number " + SelectedDevice.DeviceNumber + " at startup.");
            }
            else
            {
                Logger.WriteToConsole("AudioTracker: Error assigning device number for selected audio device.");
            }

            //TODO: write more info to log database table
            /*
            Logger.WriteToConsole("Selected audio device: " + selectedAudioDevice.DeviceFriendlyName + " / "); // USB PnP Sound Device
            Logger.WriteToConsole(selectedAudioDevice.FriendlyName + " / "); // Mikrofon (USB PnP Sound Device)
            Logger.WriteToConsole(selectedAudioDevice.ID + " / "); // {0.0.1.00000000}.{fc598ff8-af26-467b-b29b-ea9b906dda9a}
            Logger.WriteToConsole(selectedAudioDevice.State + " / "); // Active
            Logger.WriteToConsole(selectedAudioDevice.Properties + " / ");
            */

            /*
            if (Enable.IsChecked.HasValue)
            {
                if (Enable.IsChecked.Value)
                {
                    _chooser = new ChooseAudioDevice();
                    //_chooser.ConnectionEstablishedEvent += OnConnectionEstablished;
                    //_chooser.TrackerDisabledEvent += OnTrackerDisabled;
                    _chooser.ShowDialog();
                }
                else
                {
                    Database.GetInstance().SetSettings(Settings.TRACKER_ENEABLED_SETTING, false);
                    Logger.WriteToConsole("The participant updated the setting '" + Settings.TRACKER_ENEABLED_SETTING + "' to False");
                }
            }
            else
            {
                Database.GetInstance().SetSettings(Settings.TRACKER_ENEABLED_SETTING, false);
                Logger.WriteToConsole("The participant updated the setting '" + Settings.TRACKER_ENEABLED_SETTING + "' to False");
            }
            */
        }

    }
}