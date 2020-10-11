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

namespace WinFix.Services
{
    class Wifi_Direct : _IFeature
    {
        public string Name => "Wifi Direct";

        public string Description =>
            "The form of Bluetooth but over Wifi.";

        public bool Default => true;

        public dynamic Recommended => null;

        public bool Optimized => false;

        public bool Enabled
        {
            get
            {
                return Service.IsEnabled("WFDSConMgrSvc");
            }
        }

        public void Enable(bool Enable)
        {
            Service.EnableDisable("WFDSConMgrSvc", Enable, ServiceStartMode.Manual);
        }
    }
}
