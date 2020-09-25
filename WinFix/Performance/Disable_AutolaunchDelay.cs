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

namespace WinFix.Performance
{
    class Disable_AutolaunchDelay : _IFeature
    {
        public string Name => "Disable application autolauch delay";

        public string Description =>
            "By default, Windows waits a certain amount of time before triggering programs that are configured to run at startup." +
            "\r\n\r\n" +
            "Disabling this delay reduces overall system startup time.";

        public bool Default => false;

        public dynamic Recommended => true;

        public bool Optimized => true;

        public bool Enabled
        {
            get
            {
                try
                {
                    if (
                        (int)Registry.GetValue(
                            @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Serialize",
                            "Startupdelayinmsec", 1001
                        ) <= 1000
                    )
                    {
                        return true;
                    }
                }
                catch (Exception)
                {
                }

                return false;
            }
        }

        public void Enable(bool Enable)
        {
            string key_string = @"Software\Microsoft\Windows\CurrentVersion\Explorer\Serialize";

            if (Enable)
            {
                RegEdit.SetValue($@"HKEY_CURRENT_USER\{key_string}", "Startupdelayinmsec", 1000);
            }
            else
            {
                try
                {
                    using (RegistryKey key = Registry.CurrentUser.OpenSubKey(key_string, true))
                    {
                        key.DeleteValue("Startupdelayinmsec");

                        key.Close();
                    }
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
