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
    class FontCache : _IFeature
    {
        public string Name => "Font Cache Service";

        public string Description => 
            "Optimizes performance of applications by caching commonly used font data." +
            "\r\n\r\n" +
            "While this has almost no drawback on performance, it still is a non-essential extra resource.";

        public bool Default => true;

        public dynamic Recommended => false;

        public bool Optimized => false;

        public bool Enabled
        {
            get
            {
                return Service.IsEnabled("FontCache");
            }
        }

        public void Enable(bool Enable)
        {
            Service.StartStop("FontCache", Enable, ServiceStartMode.Automatic);
            Service.StartStop("FontCache3.0.0.0", Enable, ServiceStartMode.Automatic);
        }
    }
}