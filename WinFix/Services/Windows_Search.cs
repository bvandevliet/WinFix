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

namespace WinFix.Services
{
    class Windows_Search : _IFeature
    {
        public string Name => "Windows Search";

        public string Description =>
            "Disable indexing of files to improve overall performance." +
            "\r\n" +
            "You can still search for files and folders in the Windows Explorer, but it might take just a little more time.";

        public bool Default => true;

        public dynamic Recommended => false;

        public bool Optimized => false;

        public bool Enabled
        {
            get
            {
                return
                    Process.GetProcessesByName("SearchUI").Length != 0 ||
                    Service.IsEnabled("WSearch");
            }
        }

        public void Enable(bool Enable)
        {
            Service.StartStop("WSearch", Enable, ServiceStartMode.Automatic);

            RegEdit.SetValue(
                @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Search\Preferences",
                "WholeFileSystem", Enable ? 0 : 1
            );
        }
    }
}
