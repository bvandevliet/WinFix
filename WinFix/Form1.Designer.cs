using System.Drawing;

namespace WinFix
{
    partial class WinFix
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WinFix));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SetRecommended = new System.Windows.Forms.Button();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.RestoreCurrent = new System.Windows.Forms.Button();
            this.SetOptimized = new System.Windows.Forms.Button();
            this.RestoreDefaults = new System.Windows.Forms.Button();
            this.Apply = new System.Windows.Forms.Button();
            this.ServicesFeatures_box = new System.Windows.Forms.GroupBox();
            this.Privacy_box = new System.Windows.Forms.GroupBox();
            this.PerformanceStability_box = new System.Windows.Forms.GroupBox();
            this.Tweaks_box = new System.Windows.Forms.GroupBox();
            this.errorLog = new System.Windows.Forms.TextBox();
            this.descriptionBox = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.SetRecommended);
            this.groupBox1.Controls.Add(this.linkLabel1);
            this.groupBox1.Controls.Add(this.RestoreCurrent);
            this.groupBox1.Controls.Add(this.SetOptimized);
            this.groupBox1.Controls.Add(this.RestoreDefaults);
            this.groupBox1.Controls.Add(this.Apply);
            this.groupBox1.Location = new System.Drawing.Point(12, 485);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(712, 47);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            // 
            // SetRecommended
            // 
            this.SetRecommended.Location = new System.Drawing.Point(460, 17);
            this.SetRecommended.Name = "SetRecommended";
            this.SetRecommended.Size = new System.Drawing.Size(94, 23);
            this.SetRecommended.TabIndex = 3;
            this.SetRecommended.Text = "Recommended";
            this.SetRecommended.UseVisualStyleBackColor = true;
            this.SetRecommended.Click += new System.EventHandler(this.SetRecommended_Click);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.LinkArea = new System.Windows.Forms.LinkArea(3, 14);
            this.linkLabel1.Location = new System.Drawing.Point(10, 22);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(167, 17);
            this.linkLabel1.TabIndex = 0;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "by Bob Vandevliet | v2020.11.03";
            this.linkLabel1.UseCompatibleTextRendering = true;
            // 
            // RestoreCurrent
            // 
            this.RestoreCurrent.Location = new System.Drawing.Point(308, 17);
            this.RestoreCurrent.Name = "RestoreCurrent";
            this.RestoreCurrent.Size = new System.Drawing.Size(70, 23);
            this.RestoreCurrent.TabIndex = 1;
            this.RestoreCurrent.Text = "Current";
            this.RestoreCurrent.UseVisualStyleBackColor = true;
            this.RestoreCurrent.Click += new System.EventHandler(this.RestoreCurrent_Click);
            // 
            // SetOptimized
            // 
            this.SetOptimized.Location = new System.Drawing.Point(560, 17);
            this.SetOptimized.Name = "SetOptimized";
            this.SetOptimized.Size = new System.Drawing.Size(70, 23);
            this.SetOptimized.TabIndex = 4;
            this.SetOptimized.Text = "Optimized";
            this.SetOptimized.UseVisualStyleBackColor = true;
            this.SetOptimized.Click += new System.EventHandler(this.SetOptimized_Click);
            // 
            // RestoreDefaults
            // 
            this.RestoreDefaults.Location = new System.Drawing.Point(384, 17);
            this.RestoreDefaults.Name = "RestoreDefaults";
            this.RestoreDefaults.Size = new System.Drawing.Size(70, 23);
            this.RestoreDefaults.TabIndex = 2;
            this.RestoreDefaults.Text = "Default";
            this.RestoreDefaults.UseVisualStyleBackColor = true;
            this.RestoreDefaults.Click += new System.EventHandler(this.RestoreDefaults_Click);
            // 
            // Apply
            // 
            this.Apply.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Apply.Location = new System.Drawing.Point(636, 17);
            this.Apply.Name = "Apply";
            this.Apply.Size = new System.Drawing.Size(70, 23);
            this.Apply.TabIndex = 5;
            this.Apply.Text = "Apply";
            this.Apply.UseVisualStyleBackColor = true;
            this.Apply.Click += new System.EventHandler(this.Apply_Click);
            // 
            // ServicesFeatures_box
            // 
            this.ServicesFeatures_box.Location = new System.Drawing.Point(12, 13);
            this.ServicesFeatures_box.Name = "ServicesFeatures_box";
            this.ServicesFeatures_box.Size = new System.Drawing.Size(200, 446);
            this.ServicesFeatures_box.TabIndex = 0;
            this.ServicesFeatures_box.TabStop = false;
            this.ServicesFeatures_box.Text = "Services and Features";
            // 
            // Privacy_box
            // 
            this.Privacy_box.Location = new System.Drawing.Point(218, 13);
            this.Privacy_box.Name = "Privacy_box";
            this.Privacy_box.Size = new System.Drawing.Size(250, 446);
            this.Privacy_box.TabIndex = 1;
            this.Privacy_box.TabStop = false;
            this.Privacy_box.Text = "Privacy";
            // 
            // PerformanceStability_box
            // 
            this.PerformanceStability_box.Location = new System.Drawing.Point(474, 13);
            this.PerformanceStability_box.Name = "PerformanceStability_box";
            this.PerformanceStability_box.Size = new System.Drawing.Size(250, 178);
            this.PerformanceStability_box.TabIndex = 2;
            this.PerformanceStability_box.TabStop = false;
            this.PerformanceStability_box.Text = "Performance and Stability";
            // 
            // Tweaks_box
            // 
            this.Tweaks_box.Location = new System.Drawing.Point(474, 197);
            this.Tweaks_box.Name = "Tweaks_box";
            this.Tweaks_box.Size = new System.Drawing.Size(250, 262);
            this.Tweaks_box.TabIndex = 3;
            this.Tweaks_box.TabStop = false;
            this.Tweaks_box.Text = "Tweaks";
            // 
            // errorLog
            // 
            this.errorLog.BackColor = System.Drawing.SystemColors.Window;
            this.errorLog.Cursor = System.Windows.Forms.Cursors.Default;
            this.errorLog.Location = new System.Drawing.Point(12, 625);
            this.errorLog.Multiline = true;
            this.errorLog.Name = "errorLog";
            this.errorLog.ReadOnly = true;
            this.errorLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.errorLog.ShortcutsEnabled = false;
            this.errorLog.Size = new System.Drawing.Size(712, 99);
            this.errorLog.TabIndex = 99;
            this.errorLog.Text = "Any errors will be logged here ..";
            // 
            // descriptionBox
            // 
            this.descriptionBox.BackColor = System.Drawing.SystemColors.Window;
            this.descriptionBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.descriptionBox.Location = new System.Drawing.Point(12, 542);
            this.descriptionBox.Multiline = true;
            this.descriptionBox.Name = "descriptionBox";
            this.descriptionBox.ReadOnly = true;
            this.descriptionBox.ShortcutsEnabled = false;
            this.descriptionBox.Size = new System.Drawing.Size(712, 73);
            this.descriptionBox.TabIndex = 99;
            this.descriptionBox.Text = "Hover over a tweak to find out more ..";
            // 
            // WinFix
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(736, 736);
            this.Controls.Add(this.descriptionBox);
            this.Controls.Add(this.Tweaks_box);
            this.Controls.Add(this.errorLog);
            this.Controls.Add(this.PerformanceStability_box);
            this.Controls.Add(this.Privacy_box);
            this.Controls.Add(this.ServicesFeatures_box);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "WinFix";
            this.Text = "WinFix";
            this.Load += new System.EventHandler(this.WinFix_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button RestoreCurrent;
        private System.Windows.Forms.Button SetOptimized;
        private System.Windows.Forms.Button RestoreDefaults;
        private System.Windows.Forms.Button Apply;
        private System.Windows.Forms.GroupBox ServicesFeatures_box;
        private System.Windows.Forms.GroupBox Privacy_box;
        private System.Windows.Forms.GroupBox PerformanceStability_box;
        private System.Windows.Forms.TextBox errorLog;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.GroupBox Tweaks_box;
        private System.Windows.Forms.TextBox descriptionBox;
        private System.Windows.Forms.Button SetRecommended;
    }
}

