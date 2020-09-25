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
    class Data_Usage_Subscription : _IFeature
    {
        public string Name => "Data Usage Monitoring";

        public string Description => 
            "Network data usage, data limit, restrict background data, metered networks." +
            "\r\n\r\n" +
            "If you are a desktop user, you probably don't need to concern about data usage";

        public bool Default => true;

        public dynamic Recommended => false;

        public bool Optimized => false;

        public bool Enabled
        {
            get
            {
                return Service.IsEnabled("Dusmsvc");
            }
        }

        public void Enable(bool Enable)
        {
            Service.StartStop("Dusmsvc", Enable, ServiceStartMode.Automatic);
        }
    }
}
