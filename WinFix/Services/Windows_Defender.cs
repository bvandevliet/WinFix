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
    class Windows_Defender : _IFeature
    {
        public string Name => "Windows Defender";

        public string Description =>
            "The Windows built-in anti-virus.";

        public bool Default => true;

        public dynamic Recommended => true;

        public bool Optimized => false;

        public bool Enabled
        {
            get
            {
                return
                    !RegEdit.IsValue(
                        @"HKEY_LOCAL_MACHINE\Software\Policies\Microsoft\Windows Defender",
                        "DisableAntiSpyware", 1
                    ) ||
                    Service.IsEnabled(
                        "WinDefend",
                        "WdNisSvc",
                        "SecurityHealthService"
                    );
            }
        }

        public void Enable(bool Enable)
        {
            Commands.schtasks("Microsoft\\Windows\\Windows Defender\\Windows Defender Cache Maintenance", Enable);
            Commands.schtasks("Microsoft\\Windows\\Windows Defender\\Windows Defender Cleanup", Enable);
            Commands.schtasks("Microsoft\\Windows\\Windows Defender\\Windows Defender Scheduled Scan", Enable);
            Commands.schtasks("Microsoft\\Windows\\Windows Defender\\Windows Defender Verification", Enable);

            RegEdit.SetValue(
                @"HKEY_LOCAL_MACHINE\Software\Policies\Microsoft\Windows Defender",
                "DisableAntiSpyware",
                Enable ? 0 : 1
            );

            //string key = @"HKEY_LOCAL_MACHINE\Software\Policies\Microsoft\Windows Defender\Real-Time Protection";

            //RegEdit.SetValue(key, "DisableBehaviorMonitoring", Enable ? 0 : 1);
            //RegEdit.SetValue(key, "DisableOnAccessProtection", Enable ? 0 : 1);
            //RegEdit.SetValue(key, "DisableScanOnRealtimeEnable", Enable ? 0 : 1);

            /**
             * Always disable Microsoft SpyNet participation.
             */
            RegEdit.SetValue(
                @"HKEY_LOCAL_MACHINE\Software\Microsoft\Windows Defender\Spynet",
                "SpyNetReporting", 0
            );
            RegEdit.SetValue(
                @"HKEY_LOCAL_MACHINE\Software\Microsoft\Windows Defender\Spynet",
                "SubmitSamplesConsent", 2
            );
            RegEdit.SetValue(
                @"HKEY_LOCAL_MACHINE\Software\Policies\Microsoft\Windows Defender\Spynet",
                "SpyNetReporting", 0
            );
            RegEdit.SetValue(
                @"HKEY_LOCAL_MACHINE\Software\Policies\Microsoft\Windows Defender\Spynet",
                "SubmitSamplesConsent", 2
            );

            Service.EnableDisable("WinDefend", Enable, ServiceStartMode.Automatic, true);

            Service.EnableDisable("WdNisSvc", Enable, ServiceStartMode.Automatic, true);

            Service.EnableDisable("SecurityHealthService", Enable, ServiceStartMode.Automatic);
        }
    }
}