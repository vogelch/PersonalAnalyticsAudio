// Created by Christoph Vogel (christoph.vogel@uzh.ch) from the University of Zurich
// Created: 2018-10-01
// 
// Licensed under the MIT License.

using NAudio.CoreAudioApi;
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
        private ChooseAudioDevice _chooser;
        
        public FirstStartWindow()
        {
            InitializeComponent();
        }
    
        public string GetTitle()
        {
            return Settings.Name;
        }

        public void NextClicked()
        {
            //TODO: write log entry to database
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

        public void PreviousClicked()
        {

        }

        private async void FindDevices(object sender, RoutedEventArgs e)
        {
            try
            {
                Logger.WriteToConsole("Start looking for audio devices");
                AudioDeviceList.Visibility = Visibility.Hidden;
                FindButton.IsEnabled = false;

                //TODO: check Windows settings (?)
                //TODO: preselect plausible input

                MMDeviceEnumerator deviceEnumerator = new MMDeviceEnumerator();
                MMDeviceCollection AudioDevices = deviceEnumerator.EnumerateAudioEndPoints(DataFlow.All, DeviceState.Active);
                Logger.WriteToConsole("Finsihed looking for audio devices. Found " + AudioDevices.Count + " devices.");

                if (AudioDevices.Count > 0)
                {
                    AudioDeviceList.Visibility = Visibility.Visible;
                }
                else
                {
                    //TODO: warning/error message
                }

                AudioDevicesSelectionList.Items.Clear();
                foreach (MMDevice CurrentDevice in AudioDevices)
                {
                    AudioDevicesSelectionList.Items.Add(CurrentDevice);
                }
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
            MMDevice selectedAudioDevice = AudioDevicesSelectionList.SelectedItem as MMDevice;
            Daemon.inputAudioDevice = selectedAudioDevice;

            //Logger.WriteToConsole("Selected audio device: " + selectedAudioDevice.ProductName + " / " + );
            /*
            selectedAudioDevice.ProductName;
            selectedAudioDevice.Channels;
            selectedAudioDevice.FriendlyName;
            selectedAudioDevice.State;
            selectedAudioDevice.ID;
            */

            //TODO: show visualization of audio input here such that the user can check that he has selected the correct audio device and that it works
            //TODO: possibly run some checks here:
            //TODO: check whether chosen input is plausible (should be something along the lines of "Mikrofon (USB PnP Sound Device)")
            //TODO: check whether audio device is muted

        }


    }
}