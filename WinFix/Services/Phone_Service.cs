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
    class Phone_Services : _IFeature
    {
        public string Name => "Phone Services";

        public string Description =>
            "Disable if no requirement for Telephony devices is desired.";

        public bool Default => true;

        public dynamic Recommended => false;

        public bool Optimized => false;

        public bool Enabled
        {
            get
            {
                return Service.IsEnabled(
                    "PhoneSvc",
                    "SmsRouter",
                    "TapiSrv",
                    "icssvc"
                );
            }
        }

        public void Enable(bool Enable)
        {
            Service.EnableDisable("PhoneSvc", Enable, ServiceStartMode.Manual);

            Service.EnableDisable("SmsRouter", Enable, ServiceStartMode.Manual);

            Service.EnableDisable("TapiSrv", Enable, ServiceStartMode.Manual);

            Service.EnableDisable("icssvc", Enable, ServiceStartMode.Manual);

            RegEdit.SetValue(
                @"HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\Policies\MobilityCenter",
                "NoMobilityCenter", Enable ? 0 : 1
            );
        }
    }
}
