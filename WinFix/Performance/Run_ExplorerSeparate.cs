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

namespace WinFix.Performance
{
    class Run_ExplorerSeparate : _IFeature
    {
        public string Name => "Run Explorer processes separately";

        public string Description =>
            "Improves stability, but requires additional system resources.";

        public bool Default => false;

        public dynamic Recommended => true;

        public bool Optimized => true;

        public bool Enabled
        {
            get
            {
                string key = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer";

                if (
                    /**
                     * Desktop ..
                     */
                    RegEdit.IsValue(
                        key,
                        "DesktopProcess", 1
                    ) &&
                    /**
                     * Folder windows ..
                     */
                    RegEdit.IsValue(
                        $@"{key}\Advanced",
                        "SeparateProcess", 1
                    ) &&
                    /**
                     * Browser ..
                     */
                    RegEdit.IsValue(
                        $@"{key}\BrowseNewProcess",
                        "BrowseNewProcess", "Yes"
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
            string key = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer";

            RegEdit.SetValue(
                key,
                "DesktopProcess", Enable ? 1 : 0
            );
            RegEdit.SetValue(
                $@"{key}\Advanced",
                "SeparateProcess", Enable ? 1 : 0
            );
            RegEdit.SetValue(
                $@"{key}\BrowseNewProcess",
                "BrowseNewProcess", Enable ? "Yes" : "No"
            );
        }
    }
}
