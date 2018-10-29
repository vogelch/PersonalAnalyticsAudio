﻿// Created by Sebastian Mueller (smueller@ifi.uzh.ch) from the University of Zurich
// Created: 2017-01-23
// 
// Licensed under the MIT License.

using Shared;
using Shared.Data;
using System;
using System.Timers;
using FitbitTracker.Data;
using FitbitTracker.Model;
using System.Collections.Generic;
using FitbitTracker.Data.FitbitModel;
using System.Windows;
using System.Reflection;
using FitbitTracker.Views;

namespace FitbitTracker
{
    public sealed class Deamon : BaseTracker, ITracker
    {
        private Window _browserWindow;
        private Timer _fitbitTimer;
        private bool _isPApaused = false;

        public Deamon()
        {
            Name = Settings.TRACKER_NAME;
            if (Settings.IsDetailedCollectionEnabled)
            {
                Name += " (detailed)";
            }

            FitbitConnector.TokenRevoked += FitbitConnector_TokenRevoked;
        }

        private void FitbitConnector_TokenRevoked()
        {
            Stop();
            Database.GetInstance().SetSettings(Settings.TRACKER_ENEABLED_SETTING, false);
        }

        public override string GetVersion()
        {
            var v = new AssemblyName(Assembly.GetExecutingAssembly().FullName).Version;
            return Shared.Helpers.VersionHelper.GetFormattedVersion(v);
        }

        public override string GetStatus()
        {
            return IsRunning ? (Name + " is running.") : (Name + " is NOT running.");
        }

        public override void CreateDatabaseTablesIfNotExist()
        {
            DatabaseConnector.CreateFitbitTables();
        }

        public override bool IsEnabled()
        {
            return Database.GetInstance().GetSettingsBool(Settings.TRACKER_ENEABLED_SETTING, Settings.IsEnabledByDefault);
        }

        public override bool IsFirstStart { get { return !Database.GetInstance().HasSetting(Settings.TRACKER_ENEABLED_SETTING); } }

        public void InternalStart()
        {
            try
            {
                FitbitConnector.RefreshTokenFail += FitbitConnector_RefreshTokenFail;

                CheckIfSecretsAreAvailable();
                CheckIfTokenIsAvailable();

                if (IsEnabled())
                {
                    Logger.WriteToConsole("Start Fitibit Tracker");
                    CreateFitbitPullTimer();
                    IsRunning = true;
                }
            }
            catch (Exception e)
            {
                Logger.WriteToLogFile(e);
            }
        }

        public override void Start()
        {
            _isPApaused = false;
            InternalStart();
        }

        private void CheckIfSecretsAreAvailable()
        {
            //Check if credentials are there. If not, we get them from the server.
            //Also check if credentials are meaningful or just dummy credentials. We had
            //incidents where we stored a, b, or c as dummy credentials. In this case, the
            //following check should detect these dummy credentials and replace them with the real ones.
            if (SecretStorage.GetFibitFirstAuthorizationCode() == null ||
                SecretStorage.GetFitbitClientID() == null ||
                SecretStorage.GetFitbitClientSecret() == null ||
                SecretStorage.GetFibitFirstAuthorizationCode().Length <= 1 ||
                SecretStorage.GetFitbitClientID().Length <= 1 ||
                SecretStorage.GetFitbitClientSecret().Length <= 1)
            {
                try
                {
                    AccessDataService.AccessDataClient client = new AccessDataService.AccessDataClient();
                        
                    string authorizationCode = client.GetFitbitFirstAuthorizationCode();
                    if (authorizationCode != null)
                    {
                        SecretStorage.SaveFitbitFirstAuthorizationCode(authorizationCode);
                    }

                    string clientID = client.GetFitbitClientID();
                    if (clientID != null)
                    {
                        SecretStorage.SaveFitbitClientID(clientID);
                    }

                    string clientSecret = client.GetFitbitClientSecret();
                    if (clientSecret != null)
                    {
                        SecretStorage.SaveFitbitClientSecret(clientSecret);
                    }
                }
               
                catch (Exception e)
                {
                    Logger.WriteToLogFile(e);
                }
            }
        }

        //Called whenever refreshing the access or refresh token failed with a not authorized or bad request message
        private void FitbitConnector_RefreshTokenFail()
        {
            Logger.WriteToConsole("Refresh access token failed. Let the user register to get new tokens");
            GetNewTokens();
        }

        //Checks whether a token is stored. If not, new tokens are retrieved from fitbit
        private void CheckIfTokenIsAvailable()
        {
            if (SecretStorage.GetAccessToken() == null || SecretStorage.GetRefreshToken() == null)
            {
                GetNewTokens();
            }
            else
            {
                Logger.WriteToConsole("No need to refresh tokens");
            }
        }

        //Gets new tokens from fitbit
        internal void GetNewTokens()
        {
            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                var browser = new EmbeddedBrowser(Settings.REGISTRATION_URL);
                browser.FinishEvent += Browser_FinishEvent;
                browser.RegistrationTokenEvent += Browser_RegistrationTokenEvent;
                browser.ErrorEvent += Browser_ErrorEvent;

                _browserWindow = new Window
                {
                    Title = "Register PersonalAnalytics to let it access Fitbit data",
                    Content = browser
                };

                _browserWindow.ShowDialog();
            }));
        }

        //Called when getting new tokens from fitbit causes an error
        private void Browser_ErrorEvent()
        {
            Logger.WriteToConsole("Couldn't register Fibit. FitbitTracker will be disabled.");
            IsRunning = false;
            Database.GetInstance().SetSettings(Settings.TRACKER_ENEABLED_SETTING, false);
            Stop();
        }

        public void ChangeEnabledState(bool? fibtitTrackerEnabled)
        {
            Console.WriteLine(Settings.TRACKER_NAME + " is now " + (fibtitTrackerEnabled.Value ? "enabled" : "disabled"));
            Database.GetInstance().SetSettings(Settings.TRACKER_ENEABLED_SETTING, fibtitTrackerEnabled.Value);
            Database.GetInstance().LogInfo("The participant updated the setting '" + Settings.TRACKER_ENEABLED_SETTING + "' to " + fibtitTrackerEnabled.Value);

            if (fibtitTrackerEnabled.Value && !_isPApaused)
            {
                CreateDatabaseTablesIfNotExist();
                InternalStart();
            }
            else if (!fibtitTrackerEnabled.Value && !_isPApaused && IsRunning)
            {
                InternalStop();
            }
            else
            {
                Logger.WriteToConsole("Don't do anything, tracker is paused");
            }
        }

        //Called when new tokens were received from fitbit
        private void Browser_RegistrationTokenEvent(string token)
        {
            CheckIfSecretsAreAvailable();
            FitbitConnector.GetFirstAccessToken(token);
        }

        //Called when the browser window used to retrieve tokens from fitbit should be closed
        private void Browser_FinishEvent()
        {
            Application.Current.Dispatcher.Invoke((Action)(() =>
            {
                _browserWindow.Close();
            }));
        }

        //Creates a timer that is used to periodically pull data from the fitbit API
        private void CreateFitbitPullTimer()
        {
            _fitbitTimer = new Timer();
            _fitbitTimer.Elapsed += OnPullFromFitbit;
            _fitbitTimer.Interval = Settings.SYNCHRONIZE_INTERVALL_FIRST;
            _fitbitTimer.Enabled = true;
        }

        //Called when new data should be pull from the fitbit API
        private void OnPullFromFitbit(object sender, ElapsedEventArgs eventArgs)
        {
            _fitbitTimer.Interval = Settings.SYNCHRONIZE_INTERVALL_SECOND;

            Logger.WriteToConsole("Try to sync with Fitbit");

            try
            {
                DateTimeOffset latestSync = FitbitConnector.GetLatestSyncDate();
                if (latestSync == DateTimeOffset.MinValue)
                {
                    Logger.WriteToConsole("Can't sync now. No timestamp received.");
                }
                else
                {
                    Logger.WriteToConsole("Latest sync date: " + latestSync.ToString(Settings.FORMAT_DAY_AND_TIME));
                    Database.GetInstance().SetSettings(Settings.LAST_SYNCED_DATE, latestSync.ToString(Settings.FORMAT_DAY_AND_TIME));
                    latestSync = latestSync.AddDays(-1);

                    GetStepData(latestSync);
                    GetActivityData(latestSync);
                    GetHRData(latestSync);
                    GetSleepData(latestSync);
                }
            }
            catch (Exception e)
            {
                Logger.WriteToLogFile(e);
            }
        }

        //Retrieves the step data from fitbit
        private static void GetStepData(DateTimeOffset latestSync)
        {
            List<DateTimeOffset> days = DatabaseConnector.GetDaysToSynchronize(DataType.STEPS);

            foreach (DateTimeOffset day in days)
            {
                Logger.WriteToConsole("Sync Steps: " + day.ToString(Settings.FORMAT_DAY));

                if (Settings.IsDetailedCollectionEnabled)
                {
                    StepData stepData = FitbitConnector.GetStepDataForDay(day);
                    DatabaseConnector.SaveStepDataForDay(stepData, day, false);
                }

                StepData aggregatedData = FitbitConnector.GetStepDataAggregatedForDay(day);
                DatabaseConnector.SaveStepDataForDay(aggregatedData, day, true);

                if (day < latestSync)
                {
                    Logger.WriteToConsole("Finished syncing Steps for day: " + day.ToString(Settings.FORMAT_DAY));
                    DatabaseConnector.SetSynchronizedDay(day, DataType.STEPS);
                }
            }
        }

        //Retrieves activity data from fitbit
        private static void GetActivityData(DateTimeOffset latestSync)
        {
            List<DateTimeOffset> days = DatabaseConnector.GetDaysToSynchronize(DataType.ACTIVITIES);

            foreach (DateTimeOffset day in days)
            {
                Logger.WriteToConsole("Sync Activity: " + day.ToString(Settings.FORMAT_DAY));
                ActivityData activityData = FitbitConnector.GetActivityDataForDay(day);
                DatabaseConnector.SaveActivityData(activityData, day);
                if (day < latestSync)
                {
                    Logger.WriteToConsole("Finished syncing Activity for day: " + day.ToString(Settings.FORMAT_DAY));
                    DatabaseConnector.SetSynchronizedDay(day, DataType.ACTIVITIES);
                }
            }
        }

        //Retrieves HR data from fitbit
        private static void GetHRData(DateTimeOffset latestSync)
        {
            List<DateTimeOffset> days = DatabaseConnector.GetDaysToSynchronize(DataType.HR);

            foreach (DateTimeOffset day in days)
            {
                Logger.WriteToConsole("Sync HR: " + day.ToString(Settings.FORMAT_DAY));
                Tuple<List<HeartRateDayData>, List<HeartrateIntraDayData>> hrData = FitbitConnector.GetHeartrateForDay(day);
                DatabaseConnector.SaveHRData(hrData.Item1);

                if (Settings.IsDetailedCollectionEnabled)
                {
                    DatabaseConnector.SaveHRIntradayData(hrData.Item2);
                }

                if (day < latestSync)
                {
                    Logger.WriteToConsole("Finished syncing HR for day: " + day.ToString(Settings.FORMAT_DAY));
                    DatabaseConnector.SetSynchronizedDay(day, DataType.HR);
                }
            }
        }

        //Retrieves sleep data from fitbit
        private static void GetSleepData(DateTimeOffset latestSync)
        {
            List<DateTimeOffset> days = DatabaseConnector.GetDaysToSynchronize(DataType.SLEEP);

            foreach (DateTimeOffset day in days)
            {
                Logger.WriteToConsole("Sync sleep: " + day);
                SleepData sleepData = FitbitConnector.GetSleepDataForDay(day);
                DatabaseConnector.SaveSleepData(sleepData, day);
                if (day < latestSync)
                {
                    Logger.WriteToConsole("Finished syncing sleep for day: " + day);
                    DatabaseConnector.SetSynchronizedDay(day, DataType.SLEEP);
                }
            }
        }

        public void InternalStop()
        {
            if (_fitbitTimer != null)
            {
                _fitbitTimer.Enabled = false;
            }
            IsRunning = false;
        }

        public override void Stop()
        {
            _isPApaused = true;
            InternalStop();
        }

        public override void UpdateDatabaseTables(int version)
        {
            //No updates needed so far!
        }

        public override List<IVisualization> GetVisualizationsDay(DateTimeOffset date)
        {
            return new List<IVisualization> { new SleepVisualizationForDay(date), new StepVisualizationForDay(date) };
        }

        public override List<IVisualization> GetVisualizationsWeek(DateTimeOffset date)
        {
            return new List<IVisualization> { new SleepVisualizationForWeek(date), new StepVisualizationForWeek(date) };
        }

        public override List<IFirstStartScreen> GetStartScreens()
        {
            return new List<IFirstStartScreen>() { new FirstStartWindow() };
        }

    }
}