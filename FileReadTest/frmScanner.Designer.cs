namespace FileReadTest
{
    partial class frmScanner
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
            this.pbTotal = new System.Windows.Forms.ProgressBar();
            this.pbFile = new System.Windows.Forms.ProgressBar();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblFile = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // pbTotal
            // 
            this.pbTotal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbTotal.Location = new System.Drawing.Point(12, 73);
            this.pbTotal.Name = "pbTotal";
            this.pbTotal.Size = new System.Drawing.Size(627, 23);
            this.pbTotal.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pbTotal.TabIndex = 0;
            // 
            // pbFile
            // 
            this.pbFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbFile.Location = new System.Drawing.Point(12, 44);
            this.pbFile.Name = "pbFile";
            this.pbFile.Size = new System.Drawing.Size(627, 23);
            this.pbFile.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pbFile.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(564, 12);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Abort";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblFile
            // 
            this.lblFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFile.Location = new System.Drawing.Point(12, 17);
            this.lblFile.Name = "lblFile";
            this.lblFile.Size = new System.Drawing.Size(546, 20);
            this.lblFile.TabIndex = 2;
            this.lblFile.Text = "Initializing...";
            // 
            // frmScanner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(651, 108);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.pbFile);
            this.Controls.Add(this.pbTotal);
            this.Controls.Add(this.lblFile);
            this.Name = "frmScanner";
            this.Text = "File Reader";
            this.Shown += new System.EventHandler(this.frmScanner_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar pbTotal;
        private System.Windows.Forms.ProgressBar pbFile;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblFile;
    }
}