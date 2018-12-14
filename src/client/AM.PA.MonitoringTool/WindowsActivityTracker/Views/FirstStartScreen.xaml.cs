﻿using Shared;
using Shared.Data;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;

namespace WindowsActivityTracker.Views
{
    /// <summary>
    /// Interaction logic for FirstStartScreen.xaml
    /// </summary>
    public partial class FirstStartScreen : System.Windows.Controls.UserControl, IFirstStartScreen
    {
        public FirstStartScreen()
        {
            InitializeComponent();
        }

        public string GetTitle()
        {
            return Settings.TrackerName;
        }

        public void PreviousClicked()
        {
            //not needed
        }

        public async void NextClicked()
        {
            if (RecordWindowTitlesCheckbox.IsChecked.HasValue)
            {
                if (RecordWindowTitlesCheckbox.IsChecked.Value)
                {
                    /*
                    bool success = await Office365Api.GetInstance().Authenticate();

                    if (!success)
                    {
                        NotifyIcon notification = new NotifyIcon();
                        notification.Visible = true;
                        notification.BalloonTipTitle = "PersonalAnalytics";
                        notification.BalloonTipText = Settings.TrackerName + " was disabled as the authentication failed.";
                        notification.Icon = SystemIcons.Exclamation;
                        notification.Text = Settings.TrackerName  + " was disabled as the authentication failed.";
                        notification.ShowBalloonTip(60 * 1000);
                    }

                    Leave(success);
                    */
                }
                else
                {
                    Leave(false);
                }
            }
            else
            {
                Leave(false);
            }
        }

        private void Leave(bool enabled)
        {
            //Database.GetInstance().SetSettings("WindowsActivityTrackerEnabled", enabled);
        }

        private void RecordWindowTitles_Checked(object sender, RoutedEventArgs e)
        {
            Settings.IsStoreWindowTitle = true;
        }

        private void RecordWindowTitles_Unchecked(object sender, RoutedEventArgs e)
        {
            Settings.IsStoreWindowTitle = false;
        }

        private void RecordProcessNames_Checked(object sender, RoutedEventArgs e)
        {
            Settings.IsStoreProcess = true;
        }

        private void RecordProcessNames_Unchecked(object sender, RoutedEventArgs e)
        {
            Settings.IsStoreProcess = false;
        }

    }
}
