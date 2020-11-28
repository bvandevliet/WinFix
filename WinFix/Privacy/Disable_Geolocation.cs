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
    class Disable_Geolocation : _IFeature
    {
        public string Name => "Disable Geolocation";

        public string Description =>
            "Disable tracking of your location and storing location history.";

        public bool Default => false;

        public dynamic Recommended => null;

        public bool Optimized => true;

        public bool Enabled
        {
            get
            {
                string key = @"HKEY_LOCAL_MACHINE\Software\Policies\Microsoft\Windows\LocationAndSensors";

                return
                    RegEdit.IsValue(key, "DisableLocation", 1) &&
                    RegEdit.IsValue(key, "DisableWindowsLocationProvider", 1) &&
                    RegEdit.IsValue(key, "DisableLocationScripting", 1) &&
                    !Service.IsEnabled("lfsvc");
            }
        }

        public void Enable(bool Enable)
        {
            Service.EnableDisable("lfsvc", !Enable, ServiceStartMode.Manual);

            string key = @"HKEY_LOCAL_MACHINE\Software\Policies\Microsoft\Windows\LocationAndSensors";

            RegEdit.SetValue(key, "DisableLocation", Enable ? 1 : 0);
            RegEdit.SetValue(key, "DisableWindowsLocationProvider", Enable ? 1 : 0);
            RegEdit.SetValue(key, "DisableLocationScripting", Enable ? 1 : 0);

            /**
             * Prohibit Cortana and Windows Search to access location.
             */
            RegEdit.SetValue(
                @"HKEY_LOCAL_MACHINE\Software\Policies\Microsoft\Windows\Windows Search",
                "AllowSearchToUseLocation", Enable ? 0 : 1
            );
        }
    }
}
