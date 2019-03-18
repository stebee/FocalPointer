namespace FocalPointer
{
    partial class ManageIntervalWnd
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManageIntervalWnd));
            this._stateIcon = new System.Windows.Forms.PictureBox();
            this._taskCombo = new System.Windows.Forms.ComboBox();
            this._nameText = new System.Windows.Forms.TextBox();
            this._leftButton = new System.Windows.Forms.Button();
            this._rightButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this._stateIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // _stateIcon
            // 
            this._stateIcon.InitialImage = ((System.Drawing.Image)(resources.GetObject("_stateIcon.InitialImage")));
            this._stateIcon.Location = new System.Drawing.Point(12, 12);
            this._stateIcon.Name = "_stateIcon";
            this._stateIcon.Size = new System.Drawing.Size(128, 128);
            this._stateIcon.TabIndex = 0;
            this._stateIcon.TabStop = false;
            this._stateIcon.Click += new System.EventHandler(this.stateIcon_Click);
            // 
            // _taskCombo
            // 
            this._taskCombo.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._taskCombo.FormattingEnabled = true;
            this._taskCombo.Location = new System.Drawing.Point(156, 81);
            this._taskCombo.Name = "_taskCombo";
            this._taskCombo.Size = new System.Drawing.Size(699, 59);
            this._taskCombo.TabIndex = 1;
            // 
            // _nameText
            // 
            this._nameText.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._nameText.Location = new System.Drawing.Point(156, 12);
            this._nameText.Name = "_nameText";
            this._nameText.Size = new System.Drawing.Size(699, 56);
            this._nameText.TabIndex = 0;
            // 
            // _leftButton
            // 
            this._leftButton.Image = global::FocalPointer.Properties.Resources.update;
            this._leftButton.Location = new System.Drawing.Point(873, 12);
            this._leftButton.Name = "_leftButton";
            this._leftButton.Size = new System.Drawing.Size(128, 128);
            this._leftButton.TabIndex = 2;
            this._leftButton.UseVisualStyleBackColor = true;
            this._leftButton.Click += new System.EventHandler(this.leftButton_Click);
            // 
            // _rightButton
            // 
            this._rightButton.Image = global::FocalPointer.Properties.Resources.start;
            this._rightButton.Location = new System.Drawing.Point(1016, 12);
            this._rightButton.Name = "_rightButton";
            this._rightButton.Size = new System.Drawing.Size(128, 128);
            this._rightButton.TabIndex = 3;
            this._rightButton.UseVisualStyleBackColor = true;
            this._rightButton.Click += new System.EventHandler(this.rightButton_Click);
            // 
            // ManageIntervalWnd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1154, 152);
            this.ControlBox = false;
            this.Controls.Add(this._rightButton);
            this.Controls.Add(this._leftButton);
            this.Controls.Add(this._nameText);
            this.Controls.Add(this._taskCombo);
            this.Controls.Add(this._stateIcon);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ManageIntervalWnd";
            this.RightToLeftLayout = true;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            ((System.ComponentModel.ISupportInitialize)(this._stateIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox _stateIcon;
        private System.Windows.Forms.ComboBox _taskCombo;
        private System.Windows.Forms.TextBox _nameText;
        private System.Windows.Forms.Button _leftButton;
        private System.Windows.Forms.Button _rightButton;
    }
}