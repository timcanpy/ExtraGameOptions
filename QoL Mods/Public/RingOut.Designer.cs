namespace QoL_Mods.Public
{
    partial class RingOut
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
            this.ringoutcb = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // ringoutcb
            // 
            this.ringoutcb.AutoSize = true;
            this.ringoutcb.Checked = true;
            this.ringoutcb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ringoutcb.Location = new System.Drawing.Point(40, 37);
            this.ringoutcb.Name = "ringoutcb";
            this.ringoutcb.Size = new System.Drawing.Size(120, 17);
            this.ringoutcb.TabIndex = 0;
            this.ringoutcb.Text = "No Move Ring Outs";
            this.ringoutcb.UseVisualStyleBackColor = true;
            // 
            // RingOut
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(221, 164);
            this.Controls.Add(this.ringoutcb);
            this.Name = "RingOut";
            this.Text = "Ring Out Moves";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.CheckBox ringoutcb;
    }
}