namespace UCRobotics_AutoCamUploader
{
    partial class Form1
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
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnCamSetup = new System.Windows.Forms.Button();
            this.display = new System.Windows.Forms.PictureBox();
            this.portSelect = new System.Windows.Forms.ComboBox();
            this.autoSelect = new System.Windows.Forms.CheckBox();
            this.indicatorPanel = new System.Windows.Forms.Panel();
            this.serialIndicator = new System.Windows.Forms.CheckBox();
            this.camIndicator = new System.Windows.Forms.CheckBox();
            this.youtubeIndicator = new System.Windows.Forms.CheckBox();
            this.refreshPortsTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.display)).BeginInit();
            this.indicatorPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(11, 11);
            this.btnLogin.Margin = new System.Windows.Forms.Padding(2);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(53, 26);
            this.btnLogin.TabIndex = 0;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnCamSetup
            // 
            this.btnCamSetup.Location = new System.Drawing.Point(68, 11);
            this.btnCamSetup.Margin = new System.Windows.Forms.Padding(2);
            this.btnCamSetup.Name = "btnCamSetup";
            this.btnCamSetup.Size = new System.Drawing.Size(84, 26);
            this.btnCamSetup.TabIndex = 4;
            this.btnCamSetup.Text = "Setup Camera";
            this.btnCamSetup.UseVisualStyleBackColor = true;
            this.btnCamSetup.Click += new System.EventHandler(this.btnCamSetup_Click);
            // 
            // display
            // 
            this.display.Location = new System.Drawing.Point(11, 71);
            this.display.Margin = new System.Windows.Forms.Padding(2);
            this.display.Name = "display";
            this.display.Size = new System.Drawing.Size(303, 184);
            this.display.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.display.TabIndex = 5;
            this.display.TabStop = false;
            // 
            // portSelect
            // 
            this.portSelect.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.portSelect.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.portSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.portSelect.FormattingEnabled = true;
            this.portSelect.Location = new System.Drawing.Point(157, 15);
            this.portSelect.Name = "portSelect";
            this.portSelect.Size = new System.Drawing.Size(62, 21);
            this.portSelect.Sorted = true;
            this.portSelect.TabIndex = 6;
            this.portSelect.SelectionChangeCommitted += new System.EventHandler(this.portSelect_SelectionChangeCommitted);
            // 
            // autoSelect
            // 
            this.autoSelect.AutoSize = true;
            this.autoSelect.Location = new System.Drawing.Point(225, 17);
            this.autoSelect.Name = "autoSelect";
            this.autoSelect.Size = new System.Drawing.Size(89, 17);
            this.autoSelect.TabIndex = 7;
            this.autoSelect.Text = "Auto-Set Port";
            this.autoSelect.UseVisualStyleBackColor = true;
            this.autoSelect.CheckedChanged += new System.EventHandler(this.autoSelect_CheckedChanged);
            // 
            // indicatorPanel
            // 
            this.indicatorPanel.BackColor = System.Drawing.Color.Red;
            this.indicatorPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.indicatorPanel.Controls.Add(this.serialIndicator);
            this.indicatorPanel.Controls.Add(this.camIndicator);
            this.indicatorPanel.Controls.Add(this.youtubeIndicator);
            this.indicatorPanel.Location = new System.Drawing.Point(11, 40);
            this.indicatorPanel.Name = "indicatorPanel";
            this.indicatorPanel.Size = new System.Drawing.Size(303, 26);
            this.indicatorPanel.TabIndex = 8;
            // 
            // serialIndicator
            // 
            this.serialIndicator.AutoSize = true;
            this.serialIndicator.Enabled = false;
            this.serialIndicator.FlatAppearance.CheckedBackColor = System.Drawing.Color.Lime;
            this.serialIndicator.Location = new System.Drawing.Point(207, 3);
            this.serialIndicator.Name = "serialIndicator";
            this.serialIndicator.Size = new System.Drawing.Size(74, 17);
            this.serialIndicator.TabIndex = 2;
            this.serialIndicator.Text = "Serial Port";
            this.serialIndicator.UseVisualStyleBackColor = false;
            // 
            // camIndicator
            // 
            this.camIndicator.AutoSize = true;
            this.camIndicator.Enabled = false;
            this.camIndicator.FlatAppearance.CheckedBackColor = System.Drawing.Color.Lime;
            this.camIndicator.Location = new System.Drawing.Point(108, 2);
            this.camIndicator.Name = "camIndicator";
            this.camIndicator.Size = new System.Drawing.Size(93, 17);
            this.camIndicator.TabIndex = 1;
            this.camIndicator.Text = "Camera Setup";
            this.camIndicator.UseVisualStyleBackColor = false;
            // 
            // youtubeIndicator
            // 
            this.youtubeIndicator.AutoSize = true;
            this.youtubeIndicator.Enabled = false;
            this.youtubeIndicator.FlatAppearance.CheckedBackColor = System.Drawing.Color.Lime;
            this.youtubeIndicator.Location = new System.Drawing.Point(3, 3);
            this.youtubeIndicator.Name = "youtubeIndicator";
            this.youtubeIndicator.Size = new System.Drawing.Size(99, 17);
            this.youtubeIndicator.TabIndex = 0;
            this.youtubeIndicator.Text = "YouTube Login";
            this.youtubeIndicator.UseVisualStyleBackColor = false;
            // 
            // refreshPortsTimer
            // 
            this.refreshPortsTimer.Enabled = true;
            this.refreshPortsTimer.Interval = 1000;
            this.refreshPortsTimer.Tick += new System.EventHandler(this.refreshPortsTimer_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(325, 266);
            this.Controls.Add(this.indicatorPanel);
            this.Controls.Add(this.autoSelect);
            this.Controls.Add(this.portSelect);
            this.Controls.Add(this.display);
            this.Controls.Add(this.btnCamSetup);
            this.Controls.Add(this.btnLogin);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.display)).EndInit();
            this.indicatorPanel.ResumeLayout(false);
            this.indicatorPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnCamSetup;
        private System.Windows.Forms.PictureBox display;
        private System.Windows.Forms.CheckBox autoSelect;
        private System.Windows.Forms.Panel indicatorPanel;
        private System.Windows.Forms.CheckBox youtubeIndicator;
        private System.Windows.Forms.CheckBox serialIndicator;
        private System.Windows.Forms.CheckBox camIndicator;
        private System.Windows.Forms.ComboBox portSelect;
        private System.Windows.Forms.Timer refreshPortsTimer;
    }
}

