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
    class Disable_Experiments : _IFeature
    {
        public string Name => "Disable conducting experiments";

        public string Description =>
            "Disallow Microsoft to conduct experiments with the settings on your PC.";

        public bool Default => false;

        public dynamic Recommended => true;

        public bool Optimized => true;

        public bool Enabled
        {
            get
            {
                if (
                    RegEdit.IsValue(
                        @"HKEY_LOCAL_MACHINE\Software\Microsoft\PolicyManager\current\device\System",
                        "AllowExperimentation", 0
                    )
                )
                {
                    return true;
                }
                return false;
            }
        }

        public void Enable(bool Enable)
        {
            string key = @"HKEY_LOCAL_MACHINE\Software\Microsoft\PolicyManager\current\device\System";

            RegEdit.SetValue(key, "AllowExperimentation", Enable ? 0 : 1);
        }
    }
}
