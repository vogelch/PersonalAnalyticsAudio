using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Shared;
using System.Threading;

namespace AudioTracker.Helpers
{

    /*
    class DeviceChangeNotifier : Form
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

    public class DeviceNotification
    {
        //https://msdn.microsoft.com/en-us/library/aa363480(v=vs.85).aspx
        public const int DbtDeviceArrival = 0x8000; // system detected a new device        
        public const int DbtDeviceRemoveComplete = 0x8004; // device is gone     
        public const int DbtDevNodesChanged = 0x0007; //A device has been added to or removed from the system.

        public const int WmDevicechange = 0x0219; // device change event      
        private const int DbtDevtypDeviceinterface = 5;
        //https://msdn.microsoft.com/en-us/library/aa363431(v=vs.85).aspx
        private const int DEVICE_NOTIFY_ALL_INTERFACE_CLASSES = 4;
        private static readonly Guid GuidDevinterfaceUSBDevice = new Guid("A5DCBF10-6530-11D2-901F-00C04FB951ED"); // USB devices
        private static IntPtr notificationHandle;

        /// <summary>
        /// Registers a window to receive notifications when devices are plugged or unplugged.
        /// </summary>
        /// <param name="windowHandle">Handle to the window receiving notifications.</param>
        /// <param name="usbOnly">true to filter to USB devices only, false to be notified for all devices.</param>
        public static void RegisterDeviceNotification(IntPtr windowHandle, bool usbOnly = false)
        {
            var dbi = new DevBroadcastDeviceinterface
            {
                DeviceType = DbtDevtypDeviceinterface,
                Reserved = 0,
                ClassGuid = GuidDevinterfaceUSBDevice,
                Name = 0
            };

            dbi.Size = Marshal.SizeOf(dbi);
            IntPtr buffer = Marshal.AllocHGlobal(dbi.Size);
            Marshal.StructureToPtr(dbi, buffer, true);

            notificationHandle = RegisterDeviceNotification(windowHandle, buffer, usbOnly ? 0 : DEVICE_NOTIFY_ALL_INTERFACE_CLASSES);
        }

        /// <summary>
        /// Unregisters the window for device notifications
        /// </summary>
        public static void UnregisterDeviceNotification()
        {
            UnregisterDeviceNotification(notificationHandle);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr RegisterDeviceNotification(IntPtr recipient, IntPtr notificationFilter, int flags);

        [DllImport("user32.dll")]
        private static extern bool UnregisterDeviceNotification(IntPtr handle);

        [StructLayout(LayoutKind.Sequential)]
        private struct DevBroadcastDeviceinterface
        {
            internal int Size;
            internal int DeviceType;
            internal int Reserved;
            internal Guid ClassGuid;
            internal short Name;
        }
    }

}
