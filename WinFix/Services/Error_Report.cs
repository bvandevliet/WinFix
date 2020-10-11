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
    class Error_Report : _IFeature
    {
        public string Name => "Error Reporting (Steps Recorder)";

        public string Description =>
            "Allows errors to be reported when programs stop working or responding and allows existing solutions to be delivered." +
            "\r\n" +
            "Also allows logs to be generated for diagnostic and repair services." +
            "\r\n\r\n" +
            "If this service is stopped, error reporting might not work correctly " +
            "and results of diagnostic services and repairs might not be displayed." +
            "\r\n" +
            "Disabling this service also disables Steps Recorder (Privacy).";

        public bool Default => true;

        public dynamic Recommended => false;

        public bool Optimized => false;

        public bool Enabled
        {
            get
            {
                return (
                    Service.IsEnabled(
                        "WerSvc",
                        "wercplsupport"
                    ) ||
                    !RegEdit.IsValue(
                        @"HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\Windows Error Reporting",
                        "Disabled", 1
                    )
                );
            }
        }

        public void Enable(bool Enable)
        {
            Service.EnableDisable("WerSvc", Enable, ServiceStartMode.Manual);

            Service.EnableDisable("wercplsupport", Enable, ServiceStartMode.Manual);

            if (!Enable)
            {
                string key = @"HKEY_LOCAL_MACHINE\Software\Policies\Microsoft\Windows\AppCompat";
                RegEdit.SetValue(key, "DisableUAR", 1);         // Steps Recorder
                //RegEdit.SetValue(key, "DisableInventory", 1); // Inventory Collector

                RegEdit.SetValue(
                    @"HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\Windows Error Reporting",
                    "Disabled", 1
                );
            }
            else
            {
                RegEdit.SetValue(
                    @"HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\Windows Error Reporting",
                    "Disabled", 0
                );
            }

            /**
             * Always disable handwriting error reports.
             */
            RegEdit.SetValue(
                @"HKEY_LOCAL_MACHINE\Software\Policies\Microsoft\Windows\HandwritingErrorReports",
                "PreventHandwritingErrorReports", 1
            );

            /**
             * Never send additional (potentially sensitive data) information in an error report (enhance privacy).
             */
            RegEdit.SetValue(
                @"HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\Windows Error Reporting",
                "DontSendAdditionalData", 1
            );
            RegEdit.SetValue(
                @"HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\Windows Error Reporting\Consent",
                "DefaultConsent", 3
            );
            RegEdit.SetValue(
                @"HKEY_LOCAL_MACHINE\Software\WOW6432Node\Microsoft\Windows\Windows Error Reporting\Consent",
                "DefaultConsent", 3
            );
        }
    }
}
