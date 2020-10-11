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
    class Disable_Biometry : _IFeature
    {
        public string Name => "Disable Biometric services";

        public string Description =>
            "Disables face recognition (Windows Hello) and camera access at the logon screen to improve privacy.";

        public bool Default => false;

        public dynamic Recommended => true;

        public bool Optimized => true;

        public bool Enabled
        {
            get
            {
                return
                    RegEdit.IsValue(
                        @"HKEY_LOCAL_MACHINE\Software\Policies\Microsoft\Biometrics",
                        "Enabled", 0
                    ) &&
                    !Service.IsEnabled("WbioSrvc");
            }
        }

        public void Enable(bool Enable)
        {
            Service.EnableDisable("WbioSrvc", !Enable, ServiceStartMode.Manual);

            RegEdit.SetValue(
                @"HKEY_LOCAL_MACHINE\Software\Policies\Microsoft\Biometrics",
                "Enabled", Enable ? 0 : 1
            );
            RegEdit.SetValue(
                @"HKEY_LOCAL_MACHINE\Software\Policies\Microsoft\Windows\Personalization",
                "NoLockScreenCamera", Enable ? 1 : 0
            );
            RegEdit.SetValue(
                @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Authentication\LogonUI\AccessPage\Camera",
                "CameraEnabled", Enable ? 0 : 1
            );
            RegEdit.SetValue(
                @"HKEY_LOCAL_MACHINE\Software\Microsoft\PolicyManager\default\Settings",
                "AllowSignInOptions", Enable ? 0 : 1
            );
        }
    }
}
