using ExtensionsIO;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;

namespace WinFix.Privacy
{
    class Disable_Cortana : _IFeature
    {
        public string Name => "Disable Cortana";

        public string Description =>
            "Disable Cortana and improve your privacy. I recommend to install Open-Shell as the search functionality in Start Menu will break.";

        public bool Default => false;

        public dynamic Recommended => true;

        public bool Optimized => true;

        public bool Enabled
        {
            get
            {
                return
                    RegEdit.IsValue(
                        @"HKEY_LOCAL_MACHINE\Software\Policies\Microsoft\Windows\Windows Search",
                        "AllowCortana", 0
                    ) &&
                    RegEdit.IsValue(
                        @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Cortana",
                        "IsAvailable", 0
                    ) &&
                    !Directory.Exists(@"C:\Windows\SystemApps\Microsoft.Windows.Cortana_cw5n1h2txyewy");
            }
        }

        public void Enable(bool Enable)
        {
            RegEdit.SetValue(
                @"HKEY_LOCAL_MACHINE\Software\Policies\Microsoft\Windows\Windows Search",
                "AllowCortana", Enable ? 0 : 1
            );

            RegEdit.SetValue(
                @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Cortana",
                "IsAvailable", Enable ? 0 : 1
            );

            RegEdit.SetValue(
                @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced",
                "ShowCortanaButton", Enable ? 0 : 1
            );

            //RegEdit.SetValue(
            //    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search",
            //    "SearchboxTaskbarMode", Enable ? 0 : 1
            //);

            if (Enable)
            {
                /**
                     * Remove Cortana ..
                     */
                Commands.InvokePS("Get-AppxPackage -AllUsers Microsoft.Windows.Cortana | Remove-AppxPackage"); // <= 1909
                Commands.InvokePS("Get-AppxPackage -AllUsers Microsoft.549981C3F5F10 | Remove-AppxPackage");   // >= 2004

                int loop_cortana = 0;

                while (Directory.Exists(@"C:\Windows\SystemApps\Microsoft.Windows.Cortana_cw5n1h2txyewy") && loop_cortana < 20)
                {
                    Process[] searchProcesses = Process.GetProcessesByName("SearchUI");

                    foreach (Process searchProcess in searchProcesses)
                    {
                        try
                        {
                            searchProcess.Kill();
                        }
                        catch (Exception)
                        {
                            Commands.taskkill("SearchUI");
                        }
                    }

                    try
                    {
                        if (loop_cortana % 2 == 0)
                        {
                            TakeOwnership.Folder(@"C:\Windows\SystemApps\Microsoft.Windows.Cortana_cw5n1h2txyewy");
                        }

                        Dir.DeleteDir(@"C:\Windows\SystemApps\Microsoft.Windows.Cortana_cw5n1h2txyewy");
                    }
                    catch (Exception)
                    {
                    }

                    loop_cortana++;
                }

                if (loop_cortana >= 20)
                {
                    Console.WriteLine("Failed to properly disable \"Cortana\"!");
                }

                /**
                 * Remove SearchApp ..
                 */
                Commands.InvokePS("Get-AppxPackage -AllUsers Microsoft.Windows.Search | Remove-AppxPackage");

                int loop_searchapp = 0;

                while (Directory.Exists(@"Microsoft.Windows.Search_cw5n1h2txyewy") && loop_searchapp < 20)
                {
                    Process[] searchProcesses = Process.GetProcessesByName("SearchApp");

                    foreach (Process searchProcess in searchProcesses)
                    {
                        try
                        {
                            searchProcess.Kill();
                        }
                        catch (Exception)
                        {
                            Commands.taskkill("SearchApp");
                        }
                    }

                    try
                    {
                        if (loop_searchapp % 2 == 0)
                        {
                            TakeOwnership.Folder(@"Microsoft.Windows.Search_cw5n1h2txyewy");
                        }

                        Dir.DeleteDir(@"Microsoft.Windows.Search_cw5n1h2txyewy");
                    }
                    catch (Exception)
                    {
                    }

                    loop_searchapp++;
                }

                if (loop_searchapp >= 20)
                {
                    Console.WriteLine("Failed to properly disable \"Search App\"!");
                }
            }
            else
            {
                dynamic WinVer = Registry.GetValue(@"HKEY_LOCAL_MACHINE\Software\Microsoft\Windows NT\CurrentVersion", "ReleaseId", 0);

                /**
                 * Re-enable Cortana ..
                 */
                if ((int)WinVer <= 1909)
                {
                    Commands.InvokePS("Get-AppxPackage -AllUsers Microsoft.Windows.Cortana | Foreach {Add-AppxPackage -DisableDevelopmentMode -Register \"$($_.InstallLocation)\\AppXManifest.xml\"}"); // <= 1909
                }
                else
                {
                    Commands.InvokePS("Get-AppxPackage -AllUsers Microsoft.549981C3F5F10 | Foreach {Add-AppxPackage -DisableDevelopmentMode -Register \"$($_.InstallLocation)\\AppXManifest.xml\"}");   // >= 2004
                }

                /**
                 * Re-enable SearchApp ..
                 */
                Commands.InvokePS("Get-AppxPackage -AllUsers Microsoft.Windows.Search | Foreach {Add-AppxPackage -DisableDevelopmentMode -Register \"$($_.InstallLocation)\\AppXManifest.xml\"}");
            }
        }
    }
}