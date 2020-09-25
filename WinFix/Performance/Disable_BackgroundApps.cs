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

namespace WinFix.Performance
{
    class Disable_BackgroundApps : _IFeature
    {
        public string Name => "Disallow apps to run in background";

        public string Description =>
            "By default, apps can receive updates and send notifications, even if they are not being used." +
            "\r\n\r\n" +
            "Disallow background apps to save battery and slightly improve overall performance.";

        public bool Default => false;

        public dynamic Recommended => true;

        public bool Optimized => true;

        public bool Enabled
        {
            get
            {
                return
                    RegEdit.IsValue(
                        @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\BackgroundAccessApplications",
                        "GlobalUserDisabled", 1
                    ) &&
                    RegEdit.IsValue(
                        @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search",
                        "BackgroundAppGlobalToggle", 0
                    );
            }
        }

        public void Enable(bool Enable)
        {
            RegEdit.SetValue(
                @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\BackgroundAccessApplications",
                "GlobalUserDisabled", Enable ? 1 : 0
            );
            RegEdit.SetValue(
                @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search",
                "BackgroundAppGlobalToggle", Enable ? 0 : 1
            );
        }
    }
}
