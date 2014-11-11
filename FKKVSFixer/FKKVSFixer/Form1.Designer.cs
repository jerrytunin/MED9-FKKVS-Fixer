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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.txtLog = new System.Windows.Forms.TextBox();
            this.cmdLog = new System.Windows.Forms.Button();
            this.cmdProcess = new System.Windows.Forms.Button();
            this.cmdChooseFKKVS = new System.Windows.Forms.Button();
            this.txtFKKVS = new System.Windows.Forms.TextBox();
            this.chkDelta = new System.Windows.Forms.CheckBox();
            this.trkSmoothing = new System.Windows.Forms.TrackBar();
            this.numSmoothPasses = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trkSmoothing)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSmoothPasses)).BeginInit();
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
            this.cmdProcess.Location = new System.Drawing.Point(301, 463);
            this.cmdProcess.Name = "cmdProcess";
            this.cmdProcess.Size = new System.Drawing.Size(147, 35);
            this.cmdProcess.TabIndex = 2;
            this.cmdProcess.Text = "Process FKKVS";
            this.cmdProcess.UseVisualStyleBackColor = true;
            this.cmdProcess.Click += new System.EventHandler(this.cmdProcess_Click);
            // 
            // cmdChooseFKKVS
            // 
            this.cmdChooseFKKVS.Location = new System.Drawing.Point(301, 270);
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
            // chkDelta
            // 
            this.chkDelta.AutoSize = true;
            this.chkDelta.Location = new System.Drawing.Point(502, 469);
            this.chkDelta.Name = "chkDelta";
            this.chkDelta.Size = new System.Drawing.Size(170, 24);
            this.chkDelta.TabIndex = 5;
            this.chkDelta.Text = "Display Delta Mask";
            this.chkDelta.UseVisualStyleBackColor = true;
            // 
            // trkSmoothing
            // 
            this.trkSmoothing.Location = new System.Drawing.Point(76, 388);
            this.trkSmoothing.Maximum = 100;
            this.trkSmoothing.Name = "trkSmoothing";
            this.trkSmoothing.Size = new System.Drawing.Size(596, 69);
            this.trkSmoothing.TabIndex = 6;
            this.trkSmoothing.Value = 50;
            // 
            // numSmoothPasses
            // 
            this.numSmoothPasses.Location = new System.Drawing.Point(456, 324);
            this.numSmoothPasses.Name = "numSmoothPasses";
            this.numSmoothPasses.Size = new System.Drawing.Size(66, 26);
            this.numSmoothPasses.TabIndex = 7;
            this.numSmoothPasses.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(304, 362);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 20);
            this.label1.TabIndex = 8;
            this.label1.Text = "Smoothing Factor:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(226, 326);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(224, 20);
            this.label2.TabIndex = 9;
            this.label2.Text = "Number of Smoothing Passes:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(748, 513);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numSmoothPasses);
            this.Controls.Add(this.trkSmoothing);
            this.Controls.Add(this.chkDelta);
            this.Controls.Add(this.cmdChooseFKKVS);
            this.Controls.Add(this.txtFKKVS);
            this.Controls.Add(this.cmdProcess);
            this.Controls.Add(this.cmdLog);
            this.Controls.Add(this.txtLog);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FKKVS Fixer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trkSmoothing)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSmoothPasses)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button cmdLog;
        private System.Windows.Forms.Button cmdProcess;
        private System.Windows.Forms.Button cmdChooseFKKVS;
        private System.Windows.Forms.TextBox txtFKKVS;
        private System.Windows.Forms.CheckBox chkDelta;
        private System.Windows.Forms.TrackBar trkSmoothing;
        private System.Windows.Forms.NumericUpDown numSmoothPasses;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;


    }
}

