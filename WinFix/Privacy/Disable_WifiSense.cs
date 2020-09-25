using ExtensionsIO;
using ExtensionsRegex;
using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceProcess;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Diagnostics;

namespace WinFix.Privacy
{
    class Disable_WifiSense : _IFeature
    {
        public string Name => "Disable WifiSense";

        public string Description =>
            "Wifi Sense is a feature in that allows you to connect to your friends shared Wifi connections.";

        public bool Default => false;

        public dynamic Recommended => true;

        public bool Optimized => true;

        public bool Enabled
        {
            get
            {
                string key = @"HKEY_LOCAL_MACHINE\Software\Microsoft\WcmSvc\wifinetworkmanager";

                if (
                    RegEdit.IsValue($@"{key}\config", "AutoConnectAllowedOEM", 0) &&
                    RegEdit.IsValue($@"{key}\features", "WifiSenseCredShared", 0) &&
                    RegEdit.IsValue($@"{key}\features", "WifiSenseOpen", 0)
                )
                {
                    return true;
                }
                return false;
            }
        }

        public void Enable(bool Enable)
        {
            string key = @"HKEY_LOCAL_MACHINE\Software\Microsoft\WcmSvc\wifinetworkmanager";

            RegEdit.SetValue($@"{key}\config", "AutoConnectAllowedOEM", Enable ? 0 : 1);
            RegEdit.SetValue($@"{key}\features", "WifiSenseCredShared", Enable ? 0 : 1);
            RegEdit.SetValue($@"{key}\features", "WifiSenseOpen", Enable ? 0 : 1);
        }
    }
}
