namespace QoL_Mods.Private
{
    partial class DynamicHighlightsForm
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
            this.eh_enableSweat = new System.Windows.Forms.CheckBox();
            this.label62 = new System.Windows.Forms.Label();
            this.eh_sweatSpeed = new System.Windows.Forms.ComboBox();
            this.eh_sweatLevel = new System.Windows.Forms.ComboBox();
            this.eh_isDefaultLevel = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // eh_enableSweat
            // 
            this.eh_enableSweat.AutoSize = true;
            this.eh_enableSweat.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.eh_enableSweat.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.eh_enableSweat.Location = new System.Drawing.Point(36, 21);
            this.eh_enableSweat.Name = "eh_enableSweat";
            this.eh_enableSweat.Size = new System.Drawing.Size(117, 19);
            this.eh_enableSweat.TabIndex = 10;
            this.eh_enableSweat.Text = "Enabled Sweat?";
            this.eh_enableSweat.UseVisualStyleBackColor = true;
            // 
            // label62
            // 
            this.label62.AutoSize = true;
            this.label62.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label62.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label62.Location = new System.Drawing.Point(33, 107);
            this.label62.Name = "label62";
            this.label62.Size = new System.Drawing.Size(126, 15);
            this.label62.TabIndex = 9;
            this.label62.Text = "Default Sweat Speed";
            // 
            // eh_sweatSpeed
            // 
            this.eh_sweatSpeed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.eh_sweatSpeed.FormattingEnabled = true;
            this.eh_sweatSpeed.Items.AddRange(new object[] {
            "1",
            "2"});
            this.eh_sweatSpeed.Location = new System.Drawing.Point(186, 99);
            this.eh_sweatSpeed.Name = "eh_sweatSpeed";
            this.eh_sweatSpeed.Size = new System.Drawing.Size(121, 21);
            this.eh_sweatSpeed.TabIndex = 8;
            // 
            // eh_sweatLevel
            // 
            this.eh_sweatLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.eh_sweatLevel.FormattingEnabled = true;
            this.eh_sweatLevel.Items.AddRange(new object[] {
            "0",
            "10",
            "20",
            "30",
            "40",
            "50",
            "60",
            "70",
            "80",
            "90",
            "100"});
            this.eh_sweatLevel.Location = new System.Drawing.Point(186, 61);
            this.eh_sweatLevel.Name = "eh_sweatLevel";
            this.eh_sweatLevel.Size = new System.Drawing.Size(121, 21);
            this.eh_sweatLevel.TabIndex = 7;
            // 
            // eh_isDefaultLevel
            // 
            this.eh_isDefaultLevel.AutoSize = true;
            this.eh_isDefaultLevel.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.eh_isDefaultLevel.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.eh_isDefaultLevel.Location = new System.Drawing.Point(36, 61);
            this.eh_isDefaultLevel.Name = "eh_isDefaultLevel";
            this.eh_isDefaultLevel.Size = new System.Drawing.Size(140, 19);
            this.eh_isDefaultLevel.TabIndex = 6;
            this.eh_isDefaultLevel.Text = "Default Sweat Level";
            this.eh_isDefaultLevel.UseVisualStyleBackColor = true;
            // 
            // DynamicHighlightsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.RoyalBlue;
            this.ClientSize = new System.Drawing.Size(317, 130);
            this.Controls.Add(this.eh_enableSweat);
            this.Controls.Add(this.label62);
            this.Controls.Add(this.eh_sweatSpeed);
            this.Controls.Add(this.eh_sweatLevel);
            this.Controls.Add(this.eh_isDefaultLevel);
            this.Name = "DynamicHighlightsForm";
            this.Text = "Dynamic Highlights Form";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.CheckBox eh_enableSweat;
        private System.Windows.Forms.Label label62;
        public System.Windows.Forms.ComboBox eh_sweatSpeed;
        public System.Windows.Forms.ComboBox eh_sweatLevel;
        public System.Windows.Forms.CheckBox eh_isDefaultLevel;
    }
}