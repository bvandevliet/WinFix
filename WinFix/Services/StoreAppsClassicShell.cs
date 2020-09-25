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
    class StoreAppsClassicShell : _IFeature
    {
        public string Name => "Store/Apps (+OpenShell)";

        public string Description =>
            "If you prefer a full desktop experience and don't use the Store and Apps," +
            "\r\n" +
            "it's strongly recommended to disable these services to free up resources." +
            "\r\n\r\n" +
            "Because the Metro StartMenu is filled with Apps and does not function properly if Store and App services are disabled," +
            "\r\n" +
            "the good ol' StartMenu known from previous versions of Windows will be restored, powered by Open-Shell 4.4.152 (formely ClassicShell)";

        public bool Default => true;

        public dynamic Recommended => null;

        public bool Optimized => false;

        public bool Enabled
        {
            get
            {
                return Service.IsEnabled(
                    "WSService",
                    "InstallService",
                    "NcbService",
                    "UserDataSvc",
                    "UnistoreSvc",
                    "PimIndexMaintenanceSvc",
                    "OneSyncSvc",
                    "DsSvc",
                    "MapsBroker"
                );
            }
        }

        public void Enable(bool Enable)
        {
            Service.StartStop("WSService", Enable, ServiceStartMode.Manual); // DEFAULT VALUES NOT VERIFIED !!

            Service.StartStop("InstallService", Enable, ServiceStartMode.Manual); // DEFAULT VALUES NOT VERIFIED !!

            Service.StartStop("NcbService", Enable, ServiceStartMode.Manual);

            Service.StartStop("UserDataSvc", Enable, ServiceStartMode.Manual);

            Service.StartStop("UnistoreSvc", Enable, ServiceStartMode.Manual);

            Service.StartStop("PimIndexMaintenanceSvc", Enable, ServiceStartMode.Manual);

            Service.StartStop("OneSyncSvc", Enable, ServiceStartMode.Automatic);

            Service.StartStop("DsSvc", Enable, ServiceStartMode.Manual);

            Service.StartStop("MapsBroker", Enable, ServiceStartMode.Automatic);

            RegEdit.SetValue(
                @"HKEY_LOCAL_MACHINE\Software\Policies\Microsoft\Windows\Explorer",
                "NoUseStoreOpenWith", Enable ? 0 : 1
            );

            RegEdit.SetValue(
                @"HKEY_LOCAL_MACHINE\Software\Policies\Microsoft\Windows\SettingSync",
                "DisableSettingSync", Enable ? 0 : 2
            );
            RegEdit.SetValue(
                @"HKEY_LOCAL_MACHINE\Software\Policies\Microsoft\Windows\SettingSync",
                "DisableSettingSyncUserOverride", Enable ? 0 : 1
            );

            RegEdit.SetValue(
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PushToInstall",
                "DisablePushToInstall", Enable ? 0 : 1
            );

            /**
             * Kill and remove ClassicShell.
             */
            string programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

            Process[] classicshellProcesses = Process.GetProcessesByName("ClassicShell");
            foreach (Process classicshellProcess in classicshellProcesses)
            {
                try
                {
                    classicshellProcess.Kill();
                }
                catch (Exception)
                {
                    Commands.taskkill("ClassicShell");
                }
            }

            Dir.Delete($@"{programFiles}\ClassicShell");

            if (Enable)
            {
                /**
                 * Restore StartMenuExperienceHost ..
                 */
                TakeOwnership.Folder(@"C:\Windows\SystemApps\Microsoft.Windows.StartMenuExperienceHost_cw5n1h2txyewy.bak");

                if (Dir.CopyAll(
                    @"C:\Windows\SystemApps\Microsoft.Windows.StartMenuExperienceHost_cw5n1h2txyewy.bak",
                    @"C:\Windows\SystemApps\Microsoft.Windows.StartMenuExperienceHost_cw5n1h2txyewy"
                ))
                {
                    Dir.Delete(@"C:\Windows\SystemApps\Microsoft.Windows.StartMenuExperienceHost_cw5n1h2txyewy.bak");
                }
            }
            else
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

                /**
                 * Remove StartMenuExperienceHost ..
                 */
                int loop = 0;

                while (Directory.Exists(@"C:\Windows\SystemApps\Microsoft.Windows.StartMenuExperienceHost_cw5n1h2txyewy") && loop < 20)
                {
                    Process[] startmenuexpProcesses = Process.GetProcessesByName("StartMenuExperienceHost");

                    foreach (Process startmenuexpProcess in startmenuexpProcesses)
                    {
                        try
                        {
                            startmenuexpProcess.Kill();
                        }
                        catch (Exception)
                        {
                            Commands.taskkill("SearchUI");
                        }
                    }

                    TakeOwnership.Folder(@"C:\Windows\SystemApps\Microsoft.Windows.StartMenuExperienceHost_cw5n1h2txyewy");

                    if (Dir.CopyAll(
                        @"C:\Windows\SystemApps\Microsoft.Windows.StartMenuExperienceHost_cw5n1h2txyewy",
                        @"C:\Windows\SystemApps\Microsoft.Windows.StartMenuExperienceHost_cw5n1h2txyewy.bak"
                    ))
                    {
                        Dir.Delete(@"C:\Windows\SystemApps\Microsoft.Windows.StartMenuExperienceHost_cw5n1h2txyewy");
                    }

                    loop++;
                }

                if (loop == 20)
                {
                    Console.WriteLine("Failed to disable \"StartMenuExperienceHost\"!");
                }
            }
        }
    }
}
