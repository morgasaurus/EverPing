namespace EverPingUi
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.Label_Host = new System.Windows.Forms.Label();
            this.TextBox_Host = new System.Windows.Forms.TextBox();
            this.TextBox_Timeout = new System.Windows.Forms.TextBox();
            this.Label_Timeout = new System.Windows.Forms.Label();
            this.TextBox_Bytes = new System.Windows.Forms.TextBox();
            this.Label_Bytes = new System.Windows.Forms.Label();
            this.Button_Start = new System.Windows.Forms.Button();
            this.Button_Stop = new System.Windows.Forms.Button();
            this.RichTextBox_Log = new System.Windows.Forms.RichTextBox();
            this.Button_Save = new System.Windows.Forms.Button();
            this.SaveFileDialog_SaveResult = new System.Windows.Forms.SaveFileDialog();
            this.Button_Defaults = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Label_Host
            // 
            this.Label_Host.AutoSize = true;
            this.Label_Host.Location = new System.Drawing.Point(12, 15);
            this.Label_Host.Name = "Label_Host";
            this.Label_Host.Size = new System.Drawing.Size(29, 13);
            this.Label_Host.TabIndex = 0;
            this.Label_Host.Text = "Host";
            // 
            // TextBox_Host
            // 
            this.TextBox_Host.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBox_Host.Location = new System.Drawing.Point(47, 12);
            this.TextBox_Host.Name = "TextBox_Host";
            this.TextBox_Host.Size = new System.Drawing.Size(189, 20);
            this.TextBox_Host.TabIndex = 1;
            this.TextBox_Host.Click += new System.EventHandler(this.OnTextBoxFocusEnter);
            // 
            // TextBox_Timeout
            // 
            this.TextBox_Timeout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBox_Timeout.Location = new System.Drawing.Point(293, 12);
            this.TextBox_Timeout.Name = "TextBox_Timeout";
            this.TextBox_Timeout.Size = new System.Drawing.Size(47, 20);
            this.TextBox_Timeout.TabIndex = 3;
            this.TextBox_Timeout.Click += new System.EventHandler(this.OnTextBoxFocusEnter);
            // 
            // Label_Timeout
            // 
            this.Label_Timeout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Label_Timeout.AutoSize = true;
            this.Label_Timeout.Location = new System.Drawing.Point(242, 15);
            this.Label_Timeout.Name = "Label_Timeout";
            this.Label_Timeout.Size = new System.Drawing.Size(45, 13);
            this.Label_Timeout.TabIndex = 2;
            this.Label_Timeout.Text = "Timeout";
            // 
            // TextBox_Bytes
            // 
            this.TextBox_Bytes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBox_Bytes.Location = new System.Drawing.Point(385, 12);
            this.TextBox_Bytes.Name = "TextBox_Bytes";
            this.TextBox_Bytes.Size = new System.Drawing.Size(47, 20);
            this.TextBox_Bytes.TabIndex = 5;
            this.TextBox_Bytes.Click += new System.EventHandler(this.OnTextBoxFocusEnter);
            // 
            // Label_Bytes
            // 
            this.Label_Bytes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Label_Bytes.AutoSize = true;
            this.Label_Bytes.Location = new System.Drawing.Point(346, 15);
            this.Label_Bytes.Name = "Label_Bytes";
            this.Label_Bytes.Size = new System.Drawing.Size(33, 13);
            this.Label_Bytes.TabIndex = 4;
            this.Label_Bytes.Text = "Bytes";
            // 
            // Button_Start
            // 
            this.Button_Start.Location = new System.Drawing.Point(15, 38);
            this.Button_Start.Name = "Button_Start";
            this.Button_Start.Size = new System.Drawing.Size(75, 23);
            this.Button_Start.TabIndex = 6;
            this.Button_Start.Text = "Start";
            this.Button_Start.UseVisualStyleBackColor = true;
            this.Button_Start.Click += new System.EventHandler(this.Button_Start_Click);
            // 
            // Button_Stop
            // 
            this.Button_Stop.Location = new System.Drawing.Point(96, 38);
            this.Button_Stop.Name = "Button_Stop";
            this.Button_Stop.Size = new System.Drawing.Size(75, 23);
            this.Button_Stop.TabIndex = 7;
            this.Button_Stop.Text = "Stop";
            this.Button_Stop.UseVisualStyleBackColor = true;
            this.Button_Stop.Click += new System.EventHandler(this.Button_Stop_Click);
            // 
            // RichTextBox_Log
            // 
            this.RichTextBox_Log.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RichTextBox_Log.DetectUrls = false;
            this.RichTextBox_Log.Location = new System.Drawing.Point(15, 67);
            this.RichTextBox_Log.Name = "RichTextBox_Log";
            this.RichTextBox_Log.ReadOnly = true;
            this.RichTextBox_Log.Size = new System.Drawing.Size(417, 482);
            this.RichTextBox_Log.TabIndex = 8;
            this.RichTextBox_Log.Text = "";
            this.RichTextBox_Log.WordWrap = false;
            // 
            // Button_Save
            // 
            this.Button_Save.Location = new System.Drawing.Point(177, 38);
            this.Button_Save.Name = "Button_Save";
            this.Button_Save.Size = new System.Drawing.Size(75, 23);
            this.Button_Save.TabIndex = 9;
            this.Button_Save.Text = "Save";
            this.Button_Save.UseVisualStyleBackColor = true;
            this.Button_Save.Click += new System.EventHandler(this.Button_Save_Click);
            // 
            // SaveFileDialog_SaveResult
            // 
            this.SaveFileDialog_SaveResult.DefaultExt = "txt";
            this.SaveFileDialog_SaveResult.Filter = "Text Files|*.txt|All Files|*.*";
            this.SaveFileDialog_SaveResult.Title = "Save Report";
            // 
            // Button_Defaults
            // 
            this.Button_Defaults.Location = new System.Drawing.Point(258, 38);
            this.Button_Defaults.Name = "Button_Defaults";
            this.Button_Defaults.Size = new System.Drawing.Size(75, 23);
            this.Button_Defaults.TabIndex = 10;
            this.Button_Defaults.Text = "Defaults";
            this.Button_Defaults.UseVisualStyleBackColor = true;
            this.Button_Defaults.Click += new System.EventHandler(this.Button_Defaults_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(444, 561);
            this.Controls.Add(this.Button_Defaults);
            this.Controls.Add(this.Button_Save);
            this.Controls.Add(this.RichTextBox_Log);
            this.Controls.Add(this.Button_Stop);
            this.Controls.Add(this.Button_Start);
            this.Controls.Add(this.TextBox_Bytes);
            this.Controls.Add(this.Label_Bytes);
            this.Controls.Add(this.TextBox_Timeout);
            this.Controls.Add(this.Label_Timeout);
            this.Controls.Add(this.TextBox_Host);
            this.Controls.Add(this.Label_Host);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(200, 400);
            this.Name = "Form1";
            this.Text = "Ever Ping";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Label_Host;
        private System.Windows.Forms.TextBox TextBox_Host;
        private System.Windows.Forms.TextBox TextBox_Timeout;
        private System.Windows.Forms.Label Label_Timeout;
        private System.Windows.Forms.TextBox TextBox_Bytes;
        private System.Windows.Forms.Label Label_Bytes;
        private System.Windows.Forms.Button Button_Start;
        private System.Windows.Forms.Button Button_Stop;
        private System.Windows.Forms.RichTextBox RichTextBox_Log;
        private System.Windows.Forms.Button Button_Save;
        private System.Windows.Forms.SaveFileDialog SaveFileDialog_SaveResult;
        private System.Windows.Forms.Button Button_Defaults;
    }
}

