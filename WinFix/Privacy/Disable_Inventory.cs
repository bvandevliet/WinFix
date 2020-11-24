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
    class Disable_Inventory : _IFeature
    {
        public string Name => "Disable Inventory Collector";

        public string Description =>
            "Collects information on installed applications, devices and system information from all computers in a network.";

        public bool Default => false;

        public dynamic Recommended => true;

        public bool Optimized => true;

        public bool Enabled
        {
            get
            {
                return
                    RegEdit.IsValue(
                        @"HKEY_LOCAL_MACHINE\Software\Policies\Microsoft\Windows\AppCompat",
                        "DisableInventory", 1
                    );
            }
        }

        public void Enable(bool Enable)
        {
            RegEdit.SetValue(@"HKEY_LOCAL_MACHINE\Software\Policies\Microsoft\Windows\AppCompat", "DisableInventory", Enable ? 1 : 0);
        }
    }
}
