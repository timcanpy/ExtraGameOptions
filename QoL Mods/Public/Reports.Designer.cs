namespace QoL_Mods
{
    partial class Reports
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
            this.reports_recovery = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // reports_recovery
            // 
            this.reports_recovery.Location = new System.Drawing.Point(59, 29);
            this.reports_recovery.Name = "reports_recovery";
            this.reports_recovery.Size = new System.Drawing.Size(75, 23);
            this.reports_recovery.TabIndex = 0;
            this.reports_recovery.Text = "Generate";
            this.reports_recovery.UseVisualStyleBackColor = true;
            this.reports_recovery.Click += new System.EventHandler(this.reports_recovery_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(48, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Low Recovery Edits";
            // 
            // Reports
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(186, 139);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.reports_recovery);
            this.Name = "Reports";
            this.Text = "Reports";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button reports_recovery;
        private System.Windows.Forms.Label label1;
    }
}