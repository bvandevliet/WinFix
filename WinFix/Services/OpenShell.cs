using ExtensionsIO;
using ExtensionsRegex;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.ServiceProcess;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Diagnostics;

namespace WinFix.Services
{
    class OpenShell : _IFeature
    {
        public string Name => "Install Open-Shell";

        public string Description =>
            "Installs Open-Shell 4.4.160 (formely ClassicShell)";

        public bool Default => false;

        public dynamic Recommended => null;

        public bool Optimized => true;

        public bool Enabled
        {
            get
            {
                return Process.GetProcessesByName("StartMenu").Length > 0;
            }
        }

        public void Enable(bool Enable)
        {
            /**
             * Kill and remove ClassicShell.
             */
            string programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

            Process[] classicshellProcesses = Process.GetProcessesByName("StartMenu");
            foreach (Process classicshellProcess in classicshellProcesses)
            {
                try
                {
                    classicshellProcess.Kill();
                }
                catch (Exception)
                {
                    Commands.taskkill("StartMenu");
                }
            }

            Dir.DeleteDir($@"{programFiles}\ClassicShell");
            Dir.DeleteDir($@"{programFiles}\Open-Shell");

            if (Enable)
            {
                string TEMP = Path.GetTempPath();

                /**
                 * Import ClassicShell settings and autorun ..
                 */
                try
                {
                    File.WriteAllText($@"{TEMP}\Open-Shell.reg", ClassicShell.Resource1.Settings);

                    Commands.regimport($@"{TEMP}\Open-Shell.reg");
                }
                catch (Exception)
                {
                }
                try
                {
                    File.Delete($@"{TEMP}\Open-Shell.reg");
                }
                catch (Exception)
                {
                }

                /**
                 * Install ClassicShell ..
                 */
                try
                {
                    File.WriteAllBytes($@"{TEMP}\Open-Shell.zip", ClassicShell.Resource1.Open_Shell);

                    ZipFile.ExtractToDirectory($@"{TEMP}\Open-Shell.zip", $"{programFiles}");
                }
                catch (Exception)
                {
                }
                try
                {
                    File.Delete($@"{TEMP}\Open-Shell.zip");
                }
                catch (Exception)
                {
                }

                /**
                 * Start ClassicShell ..
                 */
                try
                {
                    Process.Start($@"{programFiles}\Open-Shell\StartMenu.exe", "-autorun");
                }
                catch (Exception)
                {
                }
            }
        }
    }
}