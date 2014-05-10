namespace FKKVSFixer
{
    partial class DataViewForm
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
            this.fkkvsView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.fkkvsView)).BeginInit();
            this.SuspendLayout();
            // 
            // fkkvsView
            // 
            this.fkkvsView.AllowUserToAddRows = false;
            this.fkkvsView.AllowUserToDeleteRows = false;
            this.fkkvsView.AllowUserToResizeColumns = false;
            this.fkkvsView.AllowUserToResizeRows = false;
            this.fkkvsView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.fkkvsView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fkkvsView.Location = new System.Drawing.Point(0, 0);
            this.fkkvsView.Name = "fkkvsView";
            this.fkkvsView.ReadOnly = true;
            this.fkkvsView.RowTemplate.Height = 28;
            this.fkkvsView.ShowCellErrors = false;
            this.fkkvsView.ShowCellToolTips = false;
            this.fkkvsView.ShowEditingIcon = false;
            this.fkkvsView.ShowRowErrors = false;
            this.fkkvsView.Size = new System.Drawing.Size(2244, 963);
            this.fkkvsView.TabIndex = 0;
            // 
            // DataViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2244, 963);
            this.Controls.Add(this.fkkvsView);
            this.Name = "DataViewForm";
            this.Text = "DataViewForm";
            this.Load += new System.EventHandler(this.DataViewForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.fkkvsView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView fkkvsView;
    }
}