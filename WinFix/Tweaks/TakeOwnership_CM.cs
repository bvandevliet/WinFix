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
    class TakeOwnership_CM : _IFeature
    {
        public string Name => "Take Ownership contextmenu item";

        public string Description =>
            "Take ownership of folders and files using a simple right-click context menu item.";

        public bool Default => false;

        public dynamic Recommended => null;

        public bool Optimized => true;

        public bool Enabled
        {
            get
            {
                return
                    Registry.ClassesRoot.OpenSubKey(@"*\shell\takeownership") != null &&
                    Registry.ClassesRoot.OpenSubKey(@"Directory\shell\takeownership") != null;
            }
        }

        public void Enable(bool Enable)
        {

            if (Enable)
            {
                string TEMP = Path.GetTempPath();

                /**
                 * Import registry entries ..
                 */
                try
                {
                    File.WriteAllText($@"{TEMP}\TakeOwnership_add.reg", Regedit.Resource1.TakeOwnership_add);

                    Commands.regimport($@"{TEMP}\TakeOwnership_add.reg");
                }
                catch (Exception)
                {
                }
                try
                {
                    File.Delete($@"{TEMP}\TakeOwnership_add.reg");
                }
                catch (Exception)
                {
                }
            }
            else
            {
                try
                {
                    Registry.ClassesRoot.DeleteSubKeyTree(@"*\shell\takeownership");
                }
                catch (Exception)
                {
                }
                try
                {
                    Registry.ClassesRoot.DeleteSubKeyTree(@"Directory\shell\takeownership");
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
