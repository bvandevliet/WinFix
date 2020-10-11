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
    class Disable_Sensors : _IFeature
    {
        public string Name => "Disable Sensors";

        public string Description =>
            "While this is useful for tablets to recognize different conditions such as screen orientation," +
            "\r\n" +
            "it is not required for laptops or PC's and safe to disable to free up resources.";

        public bool Default => false;

        public dynamic Recommended => true;

        public bool Optimized => true;

        public bool Enabled
        {
            get
            {
                return
                    RegEdit.IsValue(
                        @"HKEY_LOCAL_MACHINE\Software\Policies\Microsoft\Windows\LocationAndSensors",
                        "DisableSensors", 1
                    ) &&
                    !Service.IsEnabled(
                        "SensorService",
                        "SensrSvc",
                        "SensorDataService"
                    );
            }
        }

        public void Enable(bool Enable)
        {
            Service.EnableDisable("SensorService", !Enable, ServiceStartMode.Manual);

            Service.EnableDisable("SensrSvc", !Enable, ServiceStartMode.Manual);

            Service.EnableDisable("SensorDataService", !Enable, ServiceStartMode.Manual);

            string key = @"HKEY_LOCAL_MACHINE\Software\Policies\Microsoft\Windows\LocationAndSensors";

            RegEdit.SetValue(key, "DisableSensors", Enable ? 1 : 0);
        }
    }
}
