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
    class NFC_Support : _IFeature
    {
        public string Name => "NFC support";

        public string Description =>
            "Disable if no requirement for NFC devices is desired.";

        public bool Default => true;

        public dynamic Recommended => false;

        public bool Optimized => false;

        public bool Enabled
        {
            get
            {
                return Service.IsEnabled("SEMgrSvc");
            }
        }

        public void Enable(bool Enable)
        {
            Service.EnableDisable("SEMgrSvc", Enable, ServiceStartMode.Manual);
        }
    }
}
