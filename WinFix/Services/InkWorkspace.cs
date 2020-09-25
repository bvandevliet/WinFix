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
    class InkWorkspace : _IFeature
    {
        public string Name => "Windows Ink Workspace";

        public string Description => 
            "Aside from being a dedicated launcher for pen-enabled apps," +
            "\r\n" +
            "the Windows Ink Workspace includes new Sticky Notes, Sketchpad, and Screen Sketch applications." +
            "\r\n\r\n" +
            "Consider for yourself if you need these services.";

        public bool Default => true;

        public dynamic Recommended => null;

        public bool Optimized => false;

        public bool Enabled
        {
            get
            {
                if (
                    RegEdit.IsValue(
                        @"HKEY_LOCAL_MACHINE\Software\Policies\Microsoft\WindowsInkWorkspace",
                        "AllowWindowsInkWorkspace",
                        0
                    )
                )
                {
                    return false;
                }
                return true;
            }
        }

        public void Enable(bool Enable)
        {
            RegEdit.SetValue(
                @"HKEY_LOCAL_MACHINE\Software\Policies\Microsoft\WindowsInkWorkspace",
                "AllowWindowsInkWorkspace",
                Enable ? 1 : 0
            );
        }
    }
}
