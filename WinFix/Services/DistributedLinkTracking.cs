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
    class DistributedLinkTracking : _IFeature
    {
        public string Name => "Distributed Link Tracking Client";

        public string Description =>
            "Maintains links between NTFS files within a computer or across computers in a network." +
            "\r\n\r\n" +
            "If this service is disabled, shortcuts do not automatically update but get broken after moving the targeted file or folder." +
            "\r\n" +
            "If this is not important for you, it's safe to disable this service to free up resources.";

        public bool Default => true;

        public dynamic Recommended => false;

        public bool Optimized => false;

        public bool Enabled
        {
            get
            {
                return Service.IsEnabled("TrkWks");
            }
        }

        public void Enable(bool Enable)
        {
            Service.StartStop("TrkWks", Enable, ServiceStartMode.Automatic);
        }
    }
}
