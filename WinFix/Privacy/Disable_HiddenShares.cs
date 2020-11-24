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
        public string Name => "Disable unauthorized access";

        public string Description =>
            "Disable hidden shared folders to prevent administrators from seeing your files" +
            "\nAnd deny anonymous user access to prevent unauthorized use of your computer.";

        public bool Default => false;

        public dynamic Recommended => true;

        public bool Optimized => true;

        public bool Enabled
        {
            get
            {
                return
                    RegEdit.IsValue(
                        @"HKEY_LOCAL_MACHINE\System\CurrentControlSet\Services\LanmanServer\Parameters",
                        "AutoShareWks", 0
                    ) &&
                    RegEdit.IsValue(
                        @"HKEY_LOCAL_MACHINE\System\CurrentControlSet\Control\Lsa",
                        "RestrictAnonymous", 1
                    );
            }
        }

        public void Enable(bool Enable)
        {
            RegEdit.SetValue(
                @"HKEY_LOCAL_MACHINE\System\CurrentControlSet\Services\LanmanServer\Parameters",
                "AutoShareWks", Enable ? 0 : 1
            );

            RegEdit.SetValue(
                @"HKEY_LOCAL_MACHINE\System\CurrentControlSet\Control\Lsa",
                "RestrictAnonymous", Enable ? 1 : 0
            );
        }
    }
}
