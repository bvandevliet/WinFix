﻿using ExtensionsIO;
using ExtensionsRegex;
using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceProcess;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Diagnostics;

/**
 * FIX ACTION CENTER !!
 */

namespace WinFix.Services
{
    class Security_Center : _IFeature
    {
        public string Name => "Security Center (Action Center)";

        public string Description =>
            "Monitors and reports security health settings on the computer and provides systray alerts " +
            "and a graphical view of the security health states in the Security and Maintenance control panel." +
            "\r\n\r\n" +
            "If you don't want or need notifications about the security health state of your system, " +
            "you can safely disable it." +
            "\r\n" +
            "This will also attempt to disable Action Center, which will complain about Security Center being disabled.";

        public bool Default => true;

        public dynamic Recommended => null;

        public bool Optimized => false;

        public bool Enabled
        {
            get
            {
                return Service.IsEnabled("wscsvc");
            }
        }

        public void Enable(bool Enable)
        {
            Service.EnableDisable("wscsvc", Enable, ServiceStartMode.Manual);

            RegEdit.SetValue(
                @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Explorer",
                "DisableNotificationCenter",
                Enable ? 0 : 1
            );

            RegEdit.SetValue(
                @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows\Explorer",
                "HideSCAHealth",
                Enable ? 0 : 1
            );

            RegEdit.SetValue(
                @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings\Windows.SystemToast.SecurityAndMaintenance",
                "Enabled",
                Enable ? 1 : 0
            );
        }
    }
}
