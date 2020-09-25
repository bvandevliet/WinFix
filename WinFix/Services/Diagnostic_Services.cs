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
    class Diagnostic_Services : _IFeature
    {
        public string Name => "Diagnostic Services (Inventory)";

        public string Description =>
            "Enables problem detection, troubleshooting and resolution for Windows components." +
            "\r\n\r\n" +
            "While this is sometimes useful, all the other times, it is useless and it may send PC usage data to Microsoft." +
            "\r\n" +
            "Disabling this service also disables Inventory Collector and parts of Telemetry (Privacy).";

        public bool Default => true;

        public dynamic Recommended => false;

        public bool Optimized => false;

        public bool Enabled
        {
            get
            {
                return Service.IsEnabled(
                    "diagsvc",
                    "DPS",
                    "WdiServiceHost",
                    "WdiSystemHost",
                    "DiagTrack",                                // ALSO FOR TELEMETRY !!
                    "dmwappushsvc",                             // ALSO FOR TELEMETRY !!
                    "diagnosticshub.standardcollector.service", // ALSO FOR TELEMETRY !!
                    "pla"
                );
            }
        }

        public void Enable(bool Enable)
        {
            Service.StartStop("diagsvc", Enable, ServiceStartMode.Manual);

            Service.StartStop("DPS", Enable, ServiceStartMode.Automatic);

            Service.StartStop("WdiServiceHost", Enable, ServiceStartMode.Manual);

            Service.StartStop("WdiSystemHost", Enable, ServiceStartMode.Manual);

            if (!Enable)
            {
                Service.StartStop("DiagTrack", Enable, ServiceStartMode.Automatic);                             // ALSO FOR TELEMETRY !!

                Service.StartStop("dmwappushsvc", Enable, ServiceStartMode.Manual);                             // ALSO FOR TELEMETRY !!

                Service.StartStop("diagnosticshub.standardcollector.service", Enable, ServiceStartMode.Manual); // ALSO FOR TELEMETRY !!

                string key = @"HKEY_LOCAL_MACHINE\Software\Policies\Microsoft\Windows\AppCompat";
                //RegEdit.SetValue(key, "DisableUAR", 1);     // Steps Recorder
                RegEdit.SetValue(key, "DisableInventory", 1); // Inventory Collector

                Dir.Delete(@"C:\ProgramData\Microsoft\Diagnosis\ETLLogs");

                RegEdit.SetValue(
                    @"HKEY_LOCAL_MACHINE\System\CurrentControlSet\Control\WMI\Autologger\AutoLogger-Diagtrack-Listener",
                    "Start", 0
                ); // ALSO FOR TELEMETRY ..
            }

            /**
             * Always disable requests for feedback (diagnostic usage data).
             */
            RegEdit.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Siuf\Rules", "NumberOfSIUFInPeriod", 0);

            Service.StartStop("pla", Enable, ServiceStartMode.Manual);
        }
    }
}
