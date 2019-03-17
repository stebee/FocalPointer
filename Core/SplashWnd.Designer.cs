namespace FocalPointer
{
    partial class SplashWnd
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SplashWnd));
            this._titleLabel = new System.Windows.Forms.Label();
            this._authorLabel = new System.Windows.Forms.Label();
            this._versionLabel = new System.Windows.Forms.Label();
            this._copyrightLabel = new System.Windows.Forms.Label();
            this._trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.SuspendLayout();
            // 
            // _titleLabel
            // 
            this._titleLabel.Font = new System.Drawing.Font("Verdana", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._titleLabel.Location = new System.Drawing.Point(5, 0);
            this._titleLabel.Name = "_titleLabel";
            this._titleLabel.Size = new System.Drawing.Size(780, 118);
            this._titleLabel.TabIndex = 0;
            this._titleLabel.Text = "FocusPointer";
            this._titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _authorLabel
            // 
            this._authorLabel.Font = new System.Drawing.Font("Verdana", 16.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._authorLabel.Location = new System.Drawing.Point(6, 110);
            this._authorLabel.Name = "_authorLabel";
            this._authorLabel.Size = new System.Drawing.Size(780, 80);
            this._authorLabel.TabIndex = 1;
            this._authorLabel.Text = "by Siobhan Beeman";
            this._authorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _versionLabel
            // 
            this._versionLabel.Font = new System.Drawing.Font("Verdana", 16.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._versionLabel.Location = new System.Drawing.Point(5, 214);
            this._versionLabel.Name = "_versionLabel";
            this._versionLabel.Size = new System.Drawing.Size(780, 70);
            this._versionLabel.TabIndex = 2;
            this._versionLabel.Text = "Version 1.0.0";
            this._versionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _copyrightLabel
            // 
            this._copyrightLabel.Font = new System.Drawing.Font("Verdana", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._copyrightLabel.Location = new System.Drawing.Point(6, 288);
            this._copyrightLabel.Name = "_copyrightLabel";
            this._copyrightLabel.Size = new System.Drawing.Size(780, 50);
            this._copyrightLabel.TabIndex = 3;
            this._copyrightLabel.Text = "by Siobhan Beeman";
            this._copyrightLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _trayIcon
            // 
            this._trayIcon.Text = "notifyIcon1";
            this._trayIcon.Visible = true;
            // 
            // SplashWnd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(790, 340);
            this.ControlBox = false;
            this.Controls.Add(this._copyrightLabel);
            this.Controls.Add(this._versionLabel);
            this.Controls.Add(this._authorLabel);
            this.Controls.Add(this._titleLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SplashWnd";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label _titleLabel;
        private System.Windows.Forms.Label _authorLabel;
        private System.Windows.Forms.Label _versionLabel;
        private System.Windows.Forms.Label _copyrightLabel;
        private System.Windows.Forms.NotifyIcon _trayIcon;
    }
}

