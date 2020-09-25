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

namespace WinFix.Privacy
{
    class Disable_TypingInkingDictionary : _IFeature
    {
        public string Name => "Disable typing and inking dictionary";

        public string Description =>
            "Your personal typing and inking dictionary will be cleared and disabled.";

        public bool Default => false;

        public dynamic Recommended => true;

        public bool Optimized => true;

        public bool Enabled
        {
            get
            {
                return
                    RegEdit.IsValue(
                        @"HKEY_CURRENT_USER\Software\Microsoft\Personalization\Settings",
                        "AcceptedPrivacyPolicy", 0
                    ) &&
                    RegEdit.IsValue(
                        @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SettingSync\Groups\Language",
                        "Enabled", 0
                    ) &&
                    RegEdit.IsValue(
                        @"HKEY_CURRENT_USER\Software\Microsoft\InputPersonalization",
                        "RestrictImplicitTextCollection", 1
                    ) &&
                    RegEdit.IsValue(
                        @"HKEY_CURRENT_USER\Software\Microsoft\InputPersonalization",
                        "RestrictImplicitInkCollection", 1
                    ) &&
                    RegEdit.IsValue(
                        @"HKEY_CURRENT_USER\Software\Microsoft\InputPersonalization\TrainedDataStore",
                        "HarvestContacts", 0
                    );
            }
        }

        public void Enable(bool Enable)
        {
            RegEdit.SetValue(
                @"HKEY_CURRENT_USER\Software\Microsoft\Personalization\Settings",
                "AcceptedPrivacyPolicy", Enable ? 0 : 1
            );
            RegEdit.SetValue(
                @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\SettingSync\Groups\Language",
                "Enabled", Enable ? 0 : 1
            );
            RegEdit.SetValue(
                @"HKEY_CURRENT_USER\Software\Microsoft\InputPersonalization",
                "RestrictImplicitTextCollection", Enable ? 1 : 0
            );
            RegEdit.SetValue(
                @"HKEY_CURRENT_USER\Software\Microsoft\InputPersonalization",
                "RestrictImplicitInkCollection", Enable ? 1 : 0
            );
            RegEdit.SetValue(
                @"HKEY_CURRENT_USER\Software\Microsoft\InputPersonalization\TrainedDataStore",
                "HarvestContacts", Enable ? 0 : 1
            );

            if (!Enable)
            {
                return;
            }

            Dir.Delete(@"C:\Users\Bob Vandevliet\AppData\Roaming\Microsoft\Spelling");
        }
    }
}
