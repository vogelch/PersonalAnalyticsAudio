// Created by Christoph Vogel (christoph.vogel@uzh.ch) from the University of Zurich
// Created: 2018-10-01
// 
// Licensed under the MIT License.

using AudioTracker.Helpers;
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
        private int? SelectedDeviceNumber;

        public FirstStartWindow()
        {
            InitializeComponent();
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
            try
            {
                AudioDeviceList.Visibility = Visibility.Hidden;
                FindButton.IsEnabled = false;

                //TODO: check Windows settings (?)
                //TODO: preselect plausible input

                //MMDeviceEnumerator deviceEnumerator = new MMDeviceEnumerator();
                //MMDeviceCollection AudioDevices = deviceEnumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active); // DataFlow.All
                int waveInDevices = WaveIn.DeviceCount;
                Database.GetInstance().LogInfo("AudioTracker startup window: Finsihed looking for audio devices; number of devices found: " + waveInDevices);

                if (waveInDevices > 0)
                {
                    AudioDeviceList.Visibility = Visibility.Visible;
                }
                else
                {
                    //TODO: show warning/error message
                    Database.GetInstance().LogError("AudioTracker: no audio devices found.");
                }

                AudioDevicesSelectionList.Items.Clear();
                //TODO: refactor: functional code should not be in view class
                string listOfAudioDeviceInformation = "";
                for (int waveInDevice = 0; waveInDevice < waveInDevices; waveInDevice++)
                {
                    WaveInCapabilities deviceInfo = WaveIn.GetCapabilities(waveInDevice);
                    AudioDevicesSelectionList.Items.Add(deviceInfo.ProductName);
                    listOfAudioDeviceInformation += "Device " + waveInDevice + ": " + deviceInfo.ProductName + ", " + deviceInfo.Channels + " channels, ID: " + deviceInfo.ProductGuid + "\n";
                }
                Database.GetInstance().LogInfo("AudioTracker: List of audio devices found at startup:" + '\n' + listOfAudioDeviceInformation.TrimEnd('\n'));
                FindButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                Logger.WriteToLogFile(ex);
            }
        }

        private async void OnDeviceSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FindButton.IsEnabled = false;
            SelectedDeviceNumber = AudioDeviceHelper.GetDeviceNumberFromDeviceName(AudioDevicesSelectionList.SelectedItem.ToString());

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
            Settings.inputAudioDevice = AudioDeviceHelper.GetAudioDeviceFromDeviceNumber((int)SelectedDeviceNumber);
            Settings.inputAudioDeviceNumber = SelectedDeviceNumber;
            Settings.inputAudioDeviceName = Settings.inputAudioDevice.DeviceFriendlyName; // FriendlyName
            if (SelectedDeviceNumber != null)
            {
                Database.GetInstance().LogInfo("AudioTracker: The user has selected the device number " + SelectedDeviceNumber + " at startup.");
                Logger.WriteToConsole("AudioTracker: The user has selected the device number " + SelectedDeviceNumber + " at startup.");
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