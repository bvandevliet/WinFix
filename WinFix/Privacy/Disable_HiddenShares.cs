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
    class Disable_HiddenShares : _IFeature
    {
        public string Name => "Disable hidden shares";

        public string Description =>
            "Disable hidden shared folders to prevent administrators from seeing your files.";

        public bool Default => false;

        public dynamic Recommended => true;

        public bool Optimized => true;

        public bool Enabled
        {
            get
            {
                if (
                    RegEdit.IsValue(
                        @"HKEY_LOCAL_MACHINE\System\CurrentControlSet\Services\LanmanServer\Parameters",
                        "AutoShareWks", 0
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
            RegEdit.SetValue(
                @"HKEY_LOCAL_MACHINE\System\CurrentControlSet\Services\LanmanServer\Parameters",
                "AutoShareWks", Enable ? 0 : 1
            );
        }
    }
}
