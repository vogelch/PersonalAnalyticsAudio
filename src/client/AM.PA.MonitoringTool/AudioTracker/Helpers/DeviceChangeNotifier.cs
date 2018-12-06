// Created by Christoph Vogel (christoph.vogel@uzh.ch) from the University of Zurich
// Created: 2018-10-01
// 
// Licensed under the MIT License.

using System;
using System.Windows.Forms;
using System.Threading;

namespace AudioTracker.Helpers
{
    // adapted from https://stackoverflow.com/questions/2061167/how-to-receive-plug-play-device-notifications-without-a-windows-form

    internal class DeviceChangeNotifier : Form
    {
        public delegate void DeviceNotifyDelegate(Message msg);
        public static event DeviceNotifyDelegate DeviceNotify;
        private static DeviceChangeNotifier mInstance;

        public static void Start()
        {
            Thread t = new Thread(runForm);
            t.SetApartmentState(ApartmentState.STA);
            t.IsBackground = true;
            t.Start();
        }

        public static void Stop()
        {
            if (mInstance == null)
            {
                throw new InvalidOperationException("Notifier not started");
            }
            DeviceNotify = null;
            mInstance.Invoke(new MethodInvoker(mInstance.endForm));
        }

        private static void runForm()
        {
            Application.Run(new DeviceChangeNotifier());
        }

        private void endForm()
        {
            Close();
        }

        protected override void SetVisibleCore(bool value)
        {
            // Prevent window getting visible
            if (mInstance == null) CreateHandle();
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
                if (handler != null)
                {
                    handler(m);
                }
            }
            base.WndProc(ref m);
        }
    }
}
