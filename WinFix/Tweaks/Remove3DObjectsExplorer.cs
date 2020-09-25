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
    class Remove3DObjectsExplorer : _IFeature
    {
        public string Name => "Remove \"3D Objects\" from Explorer";

        public string Description =>
            "Remove the \"3D Objects\" folder from the left-side folder tree in Windows Explorer. NO UNDO !!";

        public bool Default => false;

        public dynamic Recommended => true;

        public bool Optimized => true;

        public bool Enabled
        {
            get
            {
                return false;
            }
        }

        public void Enable(bool Enable)
        {
            if (!Enable)
            {
                // !!!!
            }
            else
            {
                KeyLoop(@"Software\Microsoft\Windows\CurrentVersion\Explorer\FolderDescriptions");
                KeyLoop(@"Software\Wow6432Node\Microsoft\Windows\CurrentVersion\Explorer\FolderDescriptions");
                KeyLoop(@"Software\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace");
                KeyLoop(@"Software\Wow6432Node\Microsoft\Windows\CurrentVersion\Explorer\MyComputer\NameSpace");
            }
        }

        private void KeyLoop(string key_str)
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(key_str, true))
                {
                    key.DeleteSubKeyTree("{0DB7E03F-FC29-4DC6-9020-FF41B59E513A}");

                    foreach (string subkey_str in key.GetSubKeyNames())
                    {
                        using (RegistryKey subkey = key.OpenSubKey(subkey_str, false))
                        {
                            dynamic name = subkey.GetValue("Name");

                            if (
                                name != null &&
                                (string)name == "3D Objects"
                            )
                            {
                                try
                                {
                                    key.DeleteSubKeyTree(subkey_str);
                                }
                                catch (Exception)
                                {
                                }
                            }

                            subkey.Close();
                        }
                    }

                    key.Close();
                }
            }
            catch (Exception)
            {
            }
        }
    }
}