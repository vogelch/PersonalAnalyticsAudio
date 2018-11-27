using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioTracker.Models
{
    public class DeviceEvent
    {
        public int LParam { get; set; }
        public string HWnd { get; set; }
        public object Msg { get; set; }
        public object Result { get; set; }
        public object WParam { get; set; }

        //    key / int_val / text

        /* WPARAM
            DBT_CONFIGCHANGECANCELED    0x0019  A request to change the current configuration (dock or undock) has been canceled.
            DBT_CONFIGCHANGED   0x0018  The current configuration has changed, due to a dock or undock.
            DBT_CUSTOMEVENT 0x8006  A custom event has occurred.
            DBT_DEVICEQUERYREMOVE   0x8001  Permission is requested to remove a device or piece of media. Any application can deny this request and cancel the removal.
            DBT_DEVICEQUERYREMOVEFAILED 0x8002  A request to remove a device or piece of media has been canceled.
            DBT_DEVICEREMOVECOMPLETE    0x8004  32772  A device or piece of media has been removed.
            DBT_DEVICEREMOVEPENDING 0x8003  A device or piece of media is about to be removed. Cannot be denied.
            DBT_DEVICETYPESPECIFIC  0x8005  A device-specific event has occurred.
            DBT_DEVNODES_CHANGED    0x0007  7  A device has been added to or removed from the system.
            DBT_QUERYCHANGECONFIG   0x0017  Permission is requested to change the current configuration (dock or undock).
            DBT_USERDEFINED 0xFFFF  The meaning of this message is user-defined.

            Private Const DBT_DEVICEARRIVAL As Integer = 32768
            Private Const DBT_DEVICEREMOVECOMPLETE As Integer = 32772
        */

        public override string ToString()
        {
            //TODO: implement
            //return Name.ToString();
            return null;
        }
    }
}
