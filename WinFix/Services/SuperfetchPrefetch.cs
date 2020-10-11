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
    class SuperfetchPrefetch : _IFeature
    {
        public string Name => "Superfetch and Prefetch";

        public string Description =>
            "Prefetch groups application files together on the hard disk to reduce access time." +
            "\r\n" +
            "This is useful but it takes time for itself too." +
            "\r\n\r\n" +
            "Superfetch preloads often used applications into the memory." +
            "\r\n" +
            "Disabling this can speed up a computer with low memory or a slower CPU," +
            "\r\n" +
            "while its questionable if it's even needed on a super fast computer.";

        public bool Default => true;

        public dynamic Recommended => false;

        public bool Optimized => false;

        public bool Enabled
        {
            get
            {
                return
                    !RegEdit.IsValue(
                        @"HKEY_LOCAL_MACHINE\System\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",
                        "EnablePrefetcher", 0
                    ) ||
                    !RegEdit.IsValue(
                        @"HKEY_LOCAL_MACHINE\System\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",
                        "EnableSuperfetcher", 0
                    ) ||
                    Service.IsEnabled("SysMain");
            }
        }

        public void Enable(bool Enable)
        {
            Service.EnableDisable("SysMain", Enable, ServiceStartMode.Automatic);

            string key = @"HKEY_LOCAL_MACHINE\System\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters";

            RegEdit.SetValue(key, "EnablePrefetcher", Enable ? 3 : 0);
            RegEdit.SetValue(key, "EnableSuperfetcher", Enable ? 3 : 0);
        }
    }
}
