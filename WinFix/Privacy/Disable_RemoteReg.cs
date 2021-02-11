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
    class Disable_RemoteReg : _IFeature
    {
        public string Name => "Disable Remote Registry";

        public string Description =>
            "Disable access to the Windows Registry by a remote host.";

        public bool Default => true;

        public dynamic Recommended => true;

        public bool Optimized => true;

        public bool Enabled
        {
            get
            {
                return !Service.IsEnabled("RemoteRegistry");
            }
        }

        public void Enable(bool Enable)
        {
            Service.EnableDisable("RemoteRegistry", !Enable, ServiceStartMode.Disabled);

            RegEdit.SetValue(
                @"HKEY_LOCAL_MACHINE\System\CurrentControlSet\Control\Remote Assistance",
                "fAllowToGetHelp", 0 // screenshot 6 - put tweak somewhere else !!
            );
        }
    }
}
