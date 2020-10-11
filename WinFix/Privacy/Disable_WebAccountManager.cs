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
    class Disable_WebAccountManager : _IFeature
    {
        public string Name => "Disable Sign-in and Web Account Manager";

        public string Description =>
            "Enables single sign-in for apps and services but reduces security.";

        public bool Default => false;

        public dynamic Recommended => true;

        public bool Optimized => true;

        public bool Enabled
        {
            get
            {
                return !Service.IsEnabled("TokenBroker", "wlidsvc");
            }
        }

        public void Enable(bool Enable)
        {
            Service.EnableDisable("TokenBroker", !Enable, ServiceStartMode.Manual);

            Service.EnableDisable("wlidsvc", !Enable, ServiceStartMode.System);
        }
    }
}
