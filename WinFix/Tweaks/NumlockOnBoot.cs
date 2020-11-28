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

namespace WinFix.Tweaks
{
    class NumlockOnBoot : _IFeature
    {
        public string Name => "Enable numlock on boot";

        public string Description =>
            "Numlock will be turned on by default.";

        public bool Default => false;

        public dynamic Recommended => true;

        public bool Optimized => true;

        public bool Enabled
        {
            get
            {
                if (
                    RegEdit.IsValue(
                        @"HKEY_CURRENT_USER\Control Panel\Keyboard",
                        "InitialKeyboardIndicators", "2"
                    ) &&
                    RegEdit.IsValue(
                        @"HKEY_USERS\.DEFAULT\Control Panel\Keyboard",
                        "InitialKeyboardIndicators", "2"
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
                @"HKEY_CURRENT_USER\Control Panel\Keyboard",
                "InitialKeyboardIndicators", Enable ? 2 : 2147483648
            );
            RegEdit.SetValue(
                @"HKEY_USERS\.DEFAULT\Control Panel\Keyboard",
                "InitialKeyboardIndicators", Enable ? 2 : 2147483648
            );
        }
    }
}
