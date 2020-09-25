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
    class Disable_Telemetry : _IFeature
    {
        public string Name => "Disable Telemetry and Data Collection";

        public string Description =>
            "Stop user tracking, recording user actions, collecting advertising info and other data and sending it to Microsoft.";

        public bool Default => false;

        public dynamic Recommended => true;

        public bool Optimized => true;

        public bool Enabled
        {
            get
            {
                return
                    !Service.IsEnabled(
                        "DiagTrack",                               // ALSO FOR DIAGNOSTIC SVCs !!
                        "dmwappushsvc",                            // ALSO FOR DIAGNOSTIC SVCs !!
                        "diagnosticshub.standardcollector.service" // ALSO FOR DIAGNOSTIC SVCs !!
                    ) &&
                    /**
                     * DiagTrack logger
                     */
                    RegEdit.IsValue(
                        @"HKEY_LOCAL_MACHINE\System\CurrentControlSet\Control\WMI\Autologger\AutoLogger-Diagtrack-Listener",
                        "Start", 0
                    ) &&
                    /**
                     * Application Impact Telemetry
                     */
                    RegEdit.IsValue(
                        @"HKEY_LOCAL_MACHINE\Software\Policies\Microsoft\Windows\AppCompat",
                        "AITEnable", 0
                    ) &&
                    /**
                     * Customer Experience Improvement Program
                     */
                    RegEdit.IsValue(
                        @"HKEY_LOCAL_MACHINE\Software\Policies\Microsoft\SQMClient\Windows",
                        "CEIPEnable", 0
                    ) &&
                    /**
                     * Data Collection
                     */
                    RegEdit.IsValue(
                        @"HKEY_LOCAL_MACHINE\Software\Policies\Microsoft\Windows\DataCollection",
                        "AllowTelemetry", 0
                    ) &&
                    /**
                     * Steps Recorder
                     */
                    RegEdit.IsValue(
                        @"HKEY_LOCAL_MACHINE\Software\Policies\Microsoft\Windows\AppCompat",
                        "DisableUAR", 1
                    ) &&
                    /**
                     * Sending/sharing handwriting data
                     */
                    RegEdit.IsValue(
                        @"HKEY_CURRENT_USER\Software\Microsoft\Input\TIPC",
                        "Enabled", 0
                    ) &&
                    RegEdit.IsValue(
                        @"HKEY_LOCAL_MACHINE\Software\Policies\Microsoft\Windows\TabletPC",
                        "PreventHandwritingDataSharing", 1
                    ) &&
                    /**
                     * User tracking
                     */
                    RegEdit.IsValue(
                        @"HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer",
                        "NoInstrumentation", 1
                    ) &&
                    /**
                     * Advertising info
                     */
                    RegEdit.IsValue(
                        @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\AdvertisingInfo",
                        "Enabled", 0
                    );
            }
        }

        public void Enable(bool Enable)
        {
            Service.StartStop("DiagTrack", !Enable, ServiceStartMode.Automatic);                             // ALSO FOR DIAGNOSTIC SVCs !!

            Service.StartStop("dmwappushsvc", !Enable, ServiceStartMode.Manual);                             // ALSO FOR DIAGNOSTIC SVCs !!

            Service.StartStop("diagnosticshub.standardcollector.service", !Enable, ServiceStartMode.Manual); // ALSO FOR DIAGNOSTIC SVCs !!

            RegEdit.SetValue(
                @"HKEY_LOCAL_MACHINE\System\CurrentControlSet\Control\WMI\Autologger\AutoLogger-Diagtrack-Listener",
                "Start", Enable ? 0 : 1
            );
            RegEdit.SetValue(
                @"HKEY_LOCAL_MACHINE\Software\Policies\Microsoft\Windows\AppCompat",
                "AITEnable", Enable ? 0 : 1
            );
            RegEdit.SetValue(
                @"HKEY_LOCAL_MACHINE\Software\Policies\Microsoft\SQMClient\Windows",
                "CEIPEnable", Enable ? 0 : 1
            );
            RegEdit.SetValue(
                @"HKEY_LOCAL_MACHINE\Software\Policies\Microsoft\Windows\DataCollection",
                "AllowTelemetry", Enable ? 0 : 1
            );
            RegEdit.SetValue(
                @"HKEY_LOCAL_MACHINE\Software\Policies\Microsoft\Windows\AppCompat",
                "DisableUAR", Enable ? 1 : 0
            );
            RegEdit.SetValue(
                @"HKEY_CURRENT_USER\Software\Microsoft\Input\TIPC",
                "Enabled", Enable ? 0 : 1
            );
            RegEdit.SetValue(
                @"HKEY_LOCAL_MACHINE\Software\Policies\Microsoft\Windows\TabletPC",
                "PreventHandwritingDataSharing", Enable ? 1 : 0
            );
            RegEdit.SetValue(
                @"HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer",
                "NoInstrumentation", Enable ? 1 : 0
            );
            RegEdit.SetValue(
                @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\AdvertisingInfo",
                "Enabled", Enable ? 0 : 1
            );
        }
    }
}
