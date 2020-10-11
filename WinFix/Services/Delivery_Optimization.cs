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
    class Delivery_Optimization : _IFeature
    {
        public string Name => "Delivery Optimization Service";

        public string Description =>
            "Helps to speed up the Windows Update process by sharing parts of downloaded updates with other PC's." +
            "\r\n\r\n" +
            "Though, your system also serves as a channel for such P2P updates, which may lead to reduced bandwidth and network stability." +
            "\r\n" +
            "It may also cause you to use too much paid internet traffic.";

        public bool Default => true;

        public dynamic Recommended => false;

        public bool Optimized => false;

        public bool Enabled
        {
            get
            {
                return
                    !RegEdit.IsValue(
                        @"HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\DeliveryOptimization\Config",
                        "DODownloadMode", 0
                    ) ||
                    !RegEdit.IsValue(
                        @"HKEY_LOCAL_MACHINE\Software\Policies\Microsoft\Windows\DeliveryOptimization",
                        "DODownloadMode", 0
                    ) ||
!
                    /**
                     * P2P usage mode for update downloads
                     */
                    RegEdit.IsValue(
                        @"HKEY_USERS\S-1-5-20\Software\Microsoft\Windows\CurrentVersion\DeliveryOptimization\Settings",
                        "DownloadMode", 0
                    );
                    //|| Service.IsEnabled("DoSvc");
            }
        }

        public void Enable(bool Enable)
        {
            Service.EnableDisable("DoSvc", Enable, ServiceStartMode.Automatic);

            RegEdit.SetValue(
                @"HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\DeliveryOptimization\Config",
                "DODownloadMode", Enable ? 3 : 0
            );
            RegEdit.SetValue(
                @"HKEY_LOCAL_MACHINE\Software\Policies\Microsoft\Windows\DeliveryOptimization",
                "DODownloadMode", Enable ? 3 : 0
            );
            RegEdit.SetValue(
                @"HKEY_USERS\S-1-5-20\Software\Microsoft\Windows\CurrentVersion\DeliveryOptimization\Settings",
                "DownloadMode", Enable ? 2 : 0
            );
        }
    }
}
