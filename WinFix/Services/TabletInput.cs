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
    class TabletInput : _IFeature
    {
        public string Name => "Tablet Input";

        public string Description =>
            "Touch Keyboard and Handwriting Panel Service.";

        public bool Default => true;

        public dynamic Recommended => null;

        public bool Optimized => false;

        public bool Enabled
        {
            get
            {
                return Service.IsEnabled("TabletInputService");
            }
        }

        public void Enable(bool Enable)
        {
            Service.EnableDisable("TabletInputService", Enable, ServiceStartMode.Manual);
        }
    }
}