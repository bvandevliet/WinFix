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
    class Disable_Hibernate : _IFeature
    {
        public string Name => "Disable hibernate";

        public string Description =>
            "In order to resume your work after booting from hibernate, Windows needs to store its memory in a file on the hard disk (hiberfil.sys)." +
            "\r\n" +
            "This file has the same size as your installed memory. If you don't use hibernate, there might be some extra storage space to win.";

        public bool Default => false;

        public dynamic Recommended => true;

        public bool Optimized => true;

        public bool Enabled
        {
            get
            {
                return
                    RegEdit.IsValue(
                        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power",
                        "HibernateEnabled", 0
                    );
            }
        }

        public void Enable(bool Enable)
        {
            using (Process proc = Commands._getInstance())
            {
                proc.StartInfo.FileName = "powercfg.exe";
                proc.StartInfo.Arguments = "-h " + (Enable ? "off" : "on");

                Commands._runInstance(proc);

                proc.Close();
            }
        }
    }
}
