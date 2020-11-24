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
    class Disable_ShareAcrossDevices : _IFeature
    {
        public string Name => "Disable share across devices";

        public string Description =>
            "Disable sharing of messages between apps on other devices.";

        public bool Default => false;

        public dynamic Recommended => null;

        public bool Optimized => true;

        public bool Enabled
        {
            get
            {
                return
                    RegEdit.IsValue(
                        @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP",
                        "CdpSessionUserAuthzPolicy", 0
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
                @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP",
                "CdpSessionUserAuthzPolicy", Enable ? 0 : 2
            );

            RegEdit.SetValue(
                @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP",
                "EnableRemoteLaunchToast", Enable ? 0 : 1
            );

            RegEdit.SetValue(
                @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP",
                "NearShareChannelUserAuthzPolicy", Enable ? 0 : 2
            );

            RegEdit.SetValue(
                @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\CDP",
                "RomeSdkChannelUserAuthzPolicy", Enable ? 0 : 1
            );
        }
    }
}