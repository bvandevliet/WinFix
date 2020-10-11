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
    class UserAccessControl : _IFeature
    {
        public string Name => "User Access Control";

        public string Description =>
            "UAC throws a notification when an application attempts to make changes to the system." +
            "\r\n" +
            "If you know your applications to be legit, this feature can be very annoying.";

        public bool Default => true;

        public dynamic Recommended => false;

        public bool Optimized => false;

        public bool Enabled
        {
            get
            {
                return
                    !RegEdit.IsValue(
                        @"HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\Policies\System",
                        "EnableLUA", 0
                    ) ||
                    Service.IsEnabled("luafv");
            }
        }

        public void Enable(bool Enable)
        {
            Service.EnableDisable("luafv", Enable, ServiceStartMode.Automatic);

            RegEdit.SetValue(
                @"HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\Policies\System",
                "EnableLUA", Enable ? 1 : 0
            );
        }
    }
}
