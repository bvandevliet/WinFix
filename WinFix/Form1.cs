using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

/**
 * - more often, the "key" string variable can be removed, string be placed inside "SetValue()"
 * - check if all "/Software/" subkeys are all uppercase
 */

namespace WinFix
{
    partial class WinFix : Form
    {
        private readonly Dictionary<CheckBox, _IFeature> _register = new Dictionary<CheckBox, _IFeature>();

        public StringWriter _output;

        public WinFix()
        {
            Enabled = false;
            InitializeComponent();
        }

        private void WinFix_Load(object sender, EventArgs e)
        {
            _output = new TextBoxStreamWriter(errorLog);
            Console.SetOut(_output);

            /**
             * Privacy
             */
            _IFeature[] Privacy_items = new _IFeature[]
            {
                new Privacy.Disable_Cortana(),
                new Privacy.Disable_Telemetry(),
                new Privacy.Disable_Biometry(),
                new Privacy.Disable_Geolocation(),
                new Privacy.Disable_Sensors(),
                new Privacy.Disable_Experiments(),
                new Privacy.Disable_HiddenShares(),
                new Privacy.Disable_RemoteReg(),
                new Privacy.Disable_ShareAcrossDevices(),
                new Privacy.Disable_Inventory(),
                new Privacy.Disable_WifiSense(),
                new Privacy.Disable_TypingInkingDictionary(),
                //Disallow apps to sync and access data
            };
            Register(Privacy_items, Privacy_box);

            /**
             * Services and Features
             */
            _IFeature[] ServicesFeatures_items = new _IFeature[]
            {
                new Services.Backup_Shadow_Copy(),
                new Services.Diagnostic_Services(),
                new Services.Error_Report(),
                new Services.Delivery_Optimization(),
                new Services.Data_Usage_Subscription(),
                new Services.DistributedLinkTracking(),
                new Services.Compatibility_Assistant(),
                new Services.FontCache(),
                new Services.SuperfetchPrefetch(),
                new Services.NFC_Support(),
                new Services.Wifi_Direct(),
                new Services.Phone_Services(),
                new Services.InkWorkspace(),
                new Services.TabletInput(),
                new Services.UserAccessControl(),
                new Services.Security_Center(),
                new Services.Windows_Defender(),
                new Services.Windows_Search(),
                new Services.StoreAppsClassicShell(),
            };
            Register(ServicesFeatures_items, ServicesFeatures_box);

            /**
             * Performance and Stability
             */
            _IFeature[] PerformanceStability_items = new _IFeature[]
            {
                new Performance.Disable_AutolaunchDelay(),
                new Performance.Disable_BackgroundApps(),
                new Performance.Enhance_NTFS(),
                new Performance.Run_ExplorerSeparate(),
            };
            Register(PerformanceStability_items, PerformanceStability_box);

            /**
             * Tweaks
             */
            _IFeature[] Tweaks_items = new _IFeature[]
            {
                new Tweaks.NumlockOnBoot(),
                new Tweaks.Disable_LogonBackgroundImage(),
                new Tweaks.Disable_TipsNotifications(),
                new Tweaks.TakeOwnership_CM(),
                new Tweaks.AlwaysShowExtension(),
                new Tweaks.Remove3DObjectsExplorer(),
            };
            Register(Tweaks_items, Tweaks_box);

            foreach (KeyValuePair<CheckBox, _IFeature> Feature in _register)
            {
                Feature.Key.MouseEnter += new EventHandler(delegate (object obj, EventArgs ev)
                {
                    descriptionBox.Text = Feature.Value.Description;
                });
                Feature.Key.MouseLeave += new EventHandler(delegate (object obj, EventArgs ev)
                {
                    descriptionBox.Text = "Hover over a tweak to find out more ..";
                });
            }

            GetCurrent();

            Enabled = true;
        }

        private void Register(_IFeature[] features, GroupBox groupbox)
        {
            int length = features.Length;
            groupbox.Height = 28 + length * 23;

            for (int index = 0; index < length; index++)
            {
                _IFeature feature = features[index];

                CheckBox checkbox = new CheckBox()
                {
                    Text = string.Concat(feature.Name),
                    Size = new Size(groupbox.Width - 12, 17),
                    Location = new Point(10, 22 + index * 23),
                };
                _register.Add(checkbox, feature);
                groupbox.Controls.Add(checkbox);
            }
        }

        private void GetCurrent()
        {
            foreach (KeyValuePair<CheckBox, _IFeature> Feature in _register)
            {
                Feature.Key.Checked = Feature.Value.Enabled;
            }
        }

        private void RestoreCurrent_Click(object sender, EventArgs e)
        {
            Enabled = false;

            GetCurrent();

            Enabled = true;
        }

        private void Apply_Click(object sender, EventArgs e)
        {
            Enabled = false;

            errorLog.Clear();

            foreach (KeyValuePair<CheckBox, _IFeature> Feature in _register)
            {
                Feature.Value.Enable(Feature.Key.Checked);
            }

            GetCurrent();

            errorLog.AppendText("\r\n\r\nAll done! Thank you for using WinFix!\r\nA reboot might be required ..");

            Enabled = true;
        }

        private void RestoreDefaults_Click(object sender, EventArgs e)
        {
            Enabled = false;

            foreach (KeyValuePair<CheckBox, _IFeature> Feature in _register)
            {
                Feature.Key.Checked = Feature.Value.Default;
            }

            Enabled = true;
        }

        private void SetOptimized_Click(object sender, EventArgs e)
        {
            Enabled = false;

            foreach (KeyValuePair<CheckBox, _IFeature> Feature in _register)
            {
                Feature.Key.Checked = Feature.Value.Optimized;
            }

            Enabled = true;
        }

        private void SetRecommended_Click(object sender, EventArgs e)
        {
            Enabled = false;

            foreach (KeyValuePair<CheckBox, _IFeature> Feature in _register)
            {
                if (Feature.Value.Recommended == true)
                {
                    Feature.Key.Checked = true;
                }
                else if (Feature.Value.Recommended == false)
                {
                    Feature.Key.Checked = false;
                }
                // else, leave it ..
            }

            Enabled = true;
        }
    }
}
