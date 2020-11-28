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
    class Disable_LogonBackgroundImage : _IFeature
    {
        public string Name => "Disable lock screen background image";

        public string Description =>
            "Disable the lock screen background image to login right away.";

        public bool Default => false;

        public dynamic Recommended => null;

        public bool Optimized => true;

        public bool Enabled
        {
            get
            {
                if (
                    RegEdit.IsValue(
                        @"HKEY_LOCAL_MACHINE\Software\Policies\Microsoft\Windows\System",
                        "DisableLogonBackgroundImage", 1
                    ) &&
                    RegEdit.IsValue(
                        @"HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\Authentication\LogonUI\SessionData",
                        "AllowLockScreen", 0
                    ) &&
                    RegEdit.IsValue(
                        @"HKEY_LOCAL_MACHINE\Software\Policies\Microsoft\Windows\Personalization",
                        "NoLockScreen", 1
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
                @"HKEY_LOCAL_MACHINE\Software\Policies\Microsoft\Windows\System",
                "DisableLogonBackgroundImage", Enable ? 1 : 0
            );
            RegEdit.SetValue(
                @"HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\Authentication\LogonUI\SessionData",
                "AllowLockScreen", Enable ? 0 : 1
            );
            RegEdit.SetValue(
                @"HKEY_LOCAL_MACHINE\Software\Policies\Microsoft\Windows\Personalization",
                "NoLockScreen", Enable ? 1 : 0
            );
        }
    }
}
