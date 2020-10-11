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
    class Backup_Shadow_Copy : _IFeature
    {
        public string Name => "Backup and Shadow Copy";

        public string Description =>
            "Provides Windows Backup and Restore capabilities." +
            "\r\n\r\n" +
            "Keep in mind that System Restore does not backup your personal files, " +
            "emails, documents or pictures, but only system files and settings." +
            "\r\n" +
            "Disabling System Restore improves performance and saves disk space.";

        public bool Default => true;

        public dynamic Recommended => null;

        public bool Optimized => false;

        public bool Enabled
        {
            get
            {
                return Service.IsEnabled(
                    "SDRSVC",
                    "wbengine",
                    "VSS",
                    "swprv",
                    "SQLWriter"
                );
            }
        }

        public void Enable(bool Enable)
        {
            Service.EnableDisable("SDRSVC", Enable, ServiceStartMode.Manual);

            Service.EnableDisable("wbengine", Enable, ServiceStartMode.Manual);

            Service.EnableDisable("VSS", Enable, ServiceStartMode.Manual);

            Service.EnableDisable("swprv", Enable, ServiceStartMode.Manual);

            Service.EnableDisable("SQLWriter", Enable, ServiceStartMode.Manual); // DEFAULT VALUES NOT FOUND, NEED TO VERIFY !!

            if (!Enable)
            {
                Dir.DeleteDir($@"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\Microsoft\Windows\FileHistory");
            }
        }
    }
}
