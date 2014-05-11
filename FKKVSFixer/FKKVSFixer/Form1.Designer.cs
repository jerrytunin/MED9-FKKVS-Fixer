namespace FKKVSFixer
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
            this.txtLog = new System.Windows.Forms.TextBox();
            this.cmdLog = new System.Windows.Forms.Button();
            this.cmdProcess = new System.Windows.Forms.Button();
            this.cmdChooseFKKVS = new System.Windows.Forms.Button();
            this.txtFKKVS = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(76, 115);
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.Size = new System.Drawing.Size(596, 26);
            this.txtLog.TabIndex = 0;
            // 
            // cmdLog
            // 
            this.cmdLog.Location = new System.Drawing.Point(301, 169);
            this.cmdLog.Name = "cmdLog";
            this.cmdLog.Size = new System.Drawing.Size(147, 35);
            this.cmdLog.TabIndex = 1;
            this.cmdLog.Text = "Choose Log File";
            this.cmdLog.UseVisualStyleBackColor = true;
            this.cmdLog.Click += new System.EventHandler(this.cmdLog_Click);
            // 
            // cmdProcess
            // 
            this.cmdProcess.Location = new System.Drawing.Point(301, 337);
            this.cmdProcess.Name = "cmdProcess";
            this.cmdProcess.Size = new System.Drawing.Size(147, 35);
            this.cmdProcess.TabIndex = 2;
            this.cmdProcess.Text = "Process FKKVS";
            this.cmdProcess.UseVisualStyleBackColor = true;
            this.cmdProcess.Click += new System.EventHandler(this.cmdProcess_Click);
            // 
            // cmdChooseFKKVS
            // 
            this.cmdChooseFKKVS.Location = new System.Drawing.Point(301, 276);
            this.cmdChooseFKKVS.Name = "cmdChooseFKKVS";
            this.cmdChooseFKKVS.Size = new System.Drawing.Size(147, 35);
            this.cmdChooseFKKVS.TabIndex = 4;
            this.cmdChooseFKKVS.Text = "Choose FKKVS File";
            this.cmdChooseFKKVS.UseVisualStyleBackColor = true;
            this.cmdChooseFKKVS.Click += new System.EventHandler(this.cmdChooseFKKVS_Click);
            // 
            // txtFKKVS
            // 
            this.txtFKKVS.Location = new System.Drawing.Point(76, 228);
            this.txtFKKVS.Name = "txtFKKVS";
            this.txtFKKVS.ReadOnly = true;
            this.txtFKKVS.Size = new System.Drawing.Size(596, 26);
            this.txtFKKVS.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(748, 387);
            this.Controls.Add(this.cmdChooseFKKVS);
            this.Controls.Add(this.txtFKKVS);
            this.Controls.Add(this.cmdProcess);
            this.Controls.Add(this.cmdLog);
            this.Controls.Add(this.txtLog);
            this.Name = "Form1";
            this.Text = "FKKVS Fixer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button cmdLog;
        private System.Windows.Forms.Button cmdProcess;
        private System.Windows.Forms.Button cmdChooseFKKVS;
        private System.Windows.Forms.TextBox txtFKKVS;


    }
}

