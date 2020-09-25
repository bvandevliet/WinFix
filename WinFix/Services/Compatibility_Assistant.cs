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
    class Compatibility_Assistant : _IFeature
    {
        public string Name => "Program Compatibility Assistant";

        public string Description =>
            "Monitors programs installed and run by the user and detects known compatibility problems." +
            "\r\n\r\n" +
            "Since use cases are very rare, it is safe to disable this service to free up resources.";

        public bool Default => true;

        public dynamic Recommended => false;

        public bool Optimized => false;

        public bool Enabled
        {
            get
            {
                return
                    !RegEdit.IsValue(
                        @"HKEY_LOCAL_MACHINE\Software\Policies\Microsoft\Windows\AppCompat",
                        "DisableEngine", 1
                    ) ||
                    Service.IsEnabled("PcaSvc");
            }
        }

        public void Enable(bool Enable)
        {
            Service.StartStop("PcaSvc", Enable, ServiceStartMode.Automatic);

            string key = @"HKEY_LOCAL_MACHINE\Software\Policies\Microsoft\Windows\AppCompat";

            RegEdit.SetValue(key, "DisableEngine", Enable ? 0 : 1);
        }
    }
}
