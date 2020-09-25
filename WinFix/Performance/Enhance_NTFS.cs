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
    class Enhance_NTFS : _IFeature
    {
        public string Name => "Enhance NTFS";

        public string Description =>
            "Increase the Master File Table zone reservation by one step to increase file access performance over a long term." +
            "\r\n\r\n" +
            "Since disk defragmenters can't defragment the MFT," +
            "\r\n" +
            "Windows reserves a certain amount of extra space for it to grow, in an effort to reduce its eventual fragmentation." +
            "\r\n" +
            "The more fragmented the MFT gets, the more it will hamper overall disk performance.";

        public bool Default => false;

        public dynamic Recommended => true;

        public bool Optimized => true;

        public bool Enabled
        {
            get
            {
                string key = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem";

                if (
                    RegEdit.IsValue(key, "NtfsDisableLastAccessUpdate", 80000003) &&
                    RegEdit.IsValue(key, "NtfsMftZoneReservation", 2)
                )
                {
                    return true;
                }
                return false;
            }
        }

        public void Enable(bool Enable)
        {
            string key = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem";

            RegEdit.SetValue(key, "NtfsMftZoneReservation", Enable ? 2 : 1);

            RegEdit.SetValue(key, "NtfsDisableLastAccessUpdate", 80000003); // ENABLED BY DEFAULT ..
            RegEdit.SetValue(key, "NtfsDisable8dot3NameCreation", 2); // DEFAULT AND APPEARS TO BE JUST FINE ..
        }
    }
}
