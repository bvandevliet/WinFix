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
    class HyperV : _IFeature
    {
        public string Name => "Hyper-V";

        public string Description =>
            "Provides tools for services and management for making and running virtual machines and related resources.";

        public bool Default => throw new NotImplementedException();

        public dynamic Recommended => false;

        public bool Optimized => false;

        public bool Enabled
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Enable(bool Enable)
        {
            Commands.dism("Microsoft-Hyper-V-All", Enable);
        }
    }
}
