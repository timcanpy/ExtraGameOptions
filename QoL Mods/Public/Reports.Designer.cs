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
            this.btn_teamSave = new System.Windows.Forms.Button();
            this.critTestbtn = new System.Windows.Forms.Button();
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
            // btn_teamSave
            // 
            this.btn_teamSave.Location = new System.Drawing.Point(57, 58);
            this.btn_teamSave.Name = "btn_teamSave";
            this.btn_teamSave.Size = new System.Drawing.Size(75, 23);
            this.btn_teamSave.TabIndex = 2;
            this.btn_teamSave.Text = "Teams";
            this.btn_teamSave.UseVisualStyleBackColor = true;
            this.btn_teamSave.Visible = false;
            this.btn_teamSave.Click += new System.EventHandler(this.btn_teamSave_Click);
            // 
            // critTestbtn
            // 
            this.critTestbtn.Location = new System.Drawing.Point(57, 87);
            this.critTestbtn.Name = "critTestbtn";
            this.critTestbtn.Size = new System.Drawing.Size(75, 23);
            this.critTestbtn.TabIndex = 3;
            this.critTestbtn.Text = "Crit Test";
            this.critTestbtn.UseVisualStyleBackColor = true;
            this.critTestbtn.Visible = false;
            this.critTestbtn.Click += new System.EventHandler(this.critTestbtn_Click);
            // 
            // Reports
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(248, 139);
            this.Controls.Add(this.critTestbtn);
            this.Controls.Add(this.btn_teamSave);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.reports_recovery);
            this.Name = "Reports";
            this.Text = "Low Recovery Edit Report";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button reports_recovery;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_teamSave;
        private System.Windows.Forms.Button critTestbtn;
    }
}