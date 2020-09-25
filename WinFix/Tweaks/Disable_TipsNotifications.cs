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
    class Disable_TipsNotifications : _IFeature
    {
        public string Name => "Disable tips notifications";

        public string Description =>
            "Disable notifications of tips that contain helpful info on using Windows.";

        public bool Default => false;

        public dynamic Recommended => null;

        public bool Optimized => true;

        public bool Enabled
        {
            get
            {
                if (
                    RegEdit.IsValue(
                        @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                        "SoftLandingEnabled", 0
                    ) &&
                    RegEdit.IsValue(
                        @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                        "SubscribedContent-338389Enabled", 0
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
                @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                "SoftLandingEnabled", Enable ? 0 : 1
            );
            RegEdit.SetValue(
                @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                "SubscribedContent-338389Enabled", Enable ? 0 : 1
            );
        }
    }
}
