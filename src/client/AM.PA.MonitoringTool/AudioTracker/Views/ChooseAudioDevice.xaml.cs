// Created by Christoph Vogel (christoph.vogel@uzh.ch) from the University of Zurich
// Created: 2018-10-01
// 
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Shared;
using System;
using NAudio.CoreAudioApi;

namespace AudioTracker.Views
{
    /// <summary>
    /// Interaction logic for ChooseBluetoothDevice.xaml
    /// </summary>
    public partial class ChooseAudioDevice : Window
    {
        public ChooseAudioDevice()
        {
            InitializeComponent();
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

            //TODO: show visualization of audio input here such that the user can check that he has selected the correct audio device and that it works
            //TODO: possibly run some checks here:
            //TODO: check whether chosen input is plausible
            //TODO: check whether audio device is muted

            //var device = AudioDevicesSelectionList.SelectedItem as PortableBluetoothDeviceInformation;
            //await Connector.Instance.Connect(device);
            //ConnectionEstablishedEvent?.Invoke(device.Name);
        }

    }
}