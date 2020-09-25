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
    class Disable_Anonymous_User_Access : _IFeature
    {
        public string Name => "Disable Anonymous user access";

        public string Description =>
            "It's highly recommended to deny anonymous user access to prevent unauthorized use of your computer.";

        public bool Default => false;

        public dynamic Recommended => true;

        public bool Optimized => true;

        public bool Enabled
        {
            get
            {
                if (
                    RegEdit.IsValue(
                        @"HKEY_LOCAL_MACHINE\System\CurrentControlSet\Control\Lsa",
                        "RestrictAnonymous", 1
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
                @"HKEY_LOCAL_MACHINE\System\CurrentControlSet\Control\Lsa",
                "RestrictAnonymous", Enable ? 1 : 0
            );
        }
    }
}
