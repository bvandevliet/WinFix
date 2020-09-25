using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceProcess;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Diagnostics;

namespace WinFix.Features
{
    class Sensor_Services : IFeature
    {
        public string Name => "Sensor Monitoring";

        public string Description => throw new NotImplementedException();

        public bool Default => true;

        public bool Optimized => false;

        public bool Enabled
        {
            get
            {
                return SvcController.OneOrMoreRunning(
                    new string[] {
                        "SensorDataService",
                        "SensrSvc",
                        "SensorService"
                    }
                );
            }
        }

        public void Enable(bool Enable)
        {
            sc.StartStop("SensorDataService", Enable, 3, 1);

            sc.StartStop("SensrSvc", Enable, 3, 1);

            sc.StartStop("SensorService", Enable, 3, 1);
        }
    }
}
