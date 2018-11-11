namespace QoL_Mods
{
    partial class InjuryForm
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
            this.panel4 = new System.Windows.Forms.Panel();
            this.ij_wrestlerList = new System.Windows.Forms.ListBox();
            this.ij_removeAll = new System.Windows.Forms.Button();
            this.ij_add = new System.Windows.Forms.Button();
            this.ij_removeOne = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.ij_result = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.ij_wrestlerSearch = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.ij_recoveryDate = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.ij_matches = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.ij_load = new System.Windows.Forms.Button();
            this.ij_save = new System.Windows.Forms.Button();
            this.ij_armHP = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.ij_notifications = new System.Windows.Forms.CheckBox();
            this.ij_highRisk = new System.Windows.Forms.CheckBox();
            this.ij_recoveryRate = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.ij_bodyHP = new System.Windows.Forms.ComboBox();
            this.ij_legHP = new System.Windows.Forms.ComboBox();
            this.ij_neckHP = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.em_injuries = new System.Windows.Forms.CheckBox();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.White;
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel4.Controls.Add(this.ij_wrestlerList);
            this.panel4.Controls.Add(this.ij_removeAll);
            this.panel4.Controls.Add(this.ij_add);
            this.panel4.Controls.Add(this.ij_removeOne);
            this.panel4.Controls.Add(this.label10);
            this.panel4.Controls.Add(this.ij_result);
            this.panel4.Controls.Add(this.label6);
            this.panel4.Controls.Add(this.ij_wrestlerSearch);
            this.panel4.Controls.Add(this.label5);
            this.panel4.Location = new System.Drawing.Point(12, 32);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(342, 247);
            this.panel4.TabIndex = 1;
            // 
            // ij_wrestlerList
            // 
            this.ij_wrestlerList.FormattingEnabled = true;
            this.ij_wrestlerList.Location = new System.Drawing.Point(26, 115);
            this.ij_wrestlerList.Name = "ij_wrestlerList";
            this.ij_wrestlerList.ScrollAlwaysVisible = true;
            this.ij_wrestlerList.Size = new System.Drawing.Size(293, 95);
            this.ij_wrestlerList.TabIndex = 9;
            // 
            // ij_removeAll
            // 
            this.ij_removeAll.Location = new System.Drawing.Point(234, 221);
            this.ij_removeAll.Name = "ij_removeAll";
            this.ij_removeAll.Size = new System.Drawing.Size(85, 23);
            this.ij_removeAll.TabIndex = 8;
            this.ij_removeAll.Text = "Remove All";
            this.ij_removeAll.UseVisualStyleBackColor = true;
            // 
            // ij_add
            // 
            this.ij_add.Location = new System.Drawing.Point(26, 221);
            this.ij_add.Name = "ij_add";
            this.ij_add.Size = new System.Drawing.Size(94, 23);
            this.ij_add.TabIndex = 7;
            this.ij_add.Text = "Add Wrestler";
            this.ij_add.UseVisualStyleBackColor = true;
            this.ij_add.Click += new System.EventHandler(this.ij_add_Click);
            // 
            // ij_removeOne
            // 
            this.ij_removeOne.Location = new System.Drawing.Point(131, 221);
            this.ij_removeOne.Name = "ij_removeOne";
            this.ij_removeOne.Size = new System.Drawing.Size(85, 23);
            this.ij_removeOne.TabIndex = 6;
            this.ij_removeOne.Text = "Remove One";
            this.ij_removeOne.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(128, 99);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 13);
            this.label10.TabIndex = 5;
            this.label10.Text = "Wrestler List";
            // 
            // ij_result
            // 
            this.ij_result.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ij_result.FormattingEnabled = true;
            this.ij_result.Location = new System.Drawing.Point(26, 73);
            this.ij_result.Name = "ij_result";
            this.ij_result.Size = new System.Drawing.Size(293, 21);
            this.ij_result.Sorted = true;
            this.ij_result.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(124, 54);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Search Results";
            // 
            // ij_wrestlerSearch
            // 
            this.ij_wrestlerSearch.Location = new System.Drawing.Point(26, 27);
            this.ij_wrestlerSearch.Name = "ij_wrestlerSearch";
            this.ij_wrestlerSearch.Size = new System.Drawing.Size(293, 20);
            this.ij_wrestlerSearch.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(124, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Wrestler Search";
            // 
            // ij_recoveryDate
            // 
            this.ij_recoveryDate.Enabled = false;
            this.ij_recoveryDate.Location = new System.Drawing.Point(158, 139);
            this.ij_recoveryDate.Name = "ij_recoveryDate";
            this.ij_recoveryDate.Size = new System.Drawing.Size(126, 20);
            this.ij_recoveryDate.TabIndex = 23;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(173, 122);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(103, 13);
            this.label18.TabIndex = 22;
            this.label18.Text = "Fully Recovery Date";
            // 
            // ij_matches
            // 
            this.ij_matches.Enabled = false;
            this.ij_matches.Location = new System.Drawing.Point(158, 192);
            this.ij_matches.Name = "ij_matches";
            this.ij_matches.Size = new System.Drawing.Size(126, 20);
            this.ij_matches.TabIndex = 21;
            this.ij_matches.Visible = false;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(158, 176);
            this.label17.Name = "label17";
            this.label17.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label17.Size = new System.Drawing.Size(122, 13);
            this.label17.TabIndex = 20;
            this.label17.Text = "Matches since last injury";
            this.label17.Visible = false;
            // 
            // ij_load
            // 
            this.ij_load.Location = new System.Drawing.Point(315, 221);
            this.ij_load.Name = "ij_load";
            this.ij_load.Size = new System.Drawing.Size(94, 23);
            this.ij_load.TabIndex = 9;
            this.ij_load.Text = "Load Data";
            this.ij_load.UseVisualStyleBackColor = true;
            // 
            // ij_save
            // 
            this.ij_save.Location = new System.Drawing.Point(29, 221);
            this.ij_save.Name = "ij_save";
            this.ij_save.Size = new System.Drawing.Size(94, 23);
            this.ij_save.TabIndex = 10;
            this.ij_save.Text = "Save Data";
            this.ij_save.UseVisualStyleBackColor = true;
            // 
            // ij_armHP
            // 
            this.ij_armHP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ij_armHP.FormattingEnabled = true;
            this.ij_armHP.Location = new System.Drawing.Point(46, 138);
            this.ij_armHP.Name = "ij_armHP";
            this.ij_armHP.Size = new System.Drawing.Size(88, 21);
            this.ij_armHP.TabIndex = 15;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(56, 122);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(59, 13);
            this.label14.TabIndex = 11;
            this.label14.Text = "Arm Health";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(312, 122);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(59, 13);
            this.label13.TabIndex = 10;
            this.label13.Text = "Leg Health";
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.White;
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel5.Controls.Add(this.ij_notifications);
            this.panel5.Controls.Add(this.ij_highRisk);
            this.panel5.Controls.Add(this.ij_recoveryDate);
            this.panel5.Controls.Add(this.label18);
            this.panel5.Controls.Add(this.ij_matches);
            this.panel5.Controls.Add(this.label17);
            this.panel5.Controls.Add(this.ij_recoveryRate);
            this.panel5.Controls.Add(this.label16);
            this.panel5.Controls.Add(this.ij_load);
            this.panel5.Controls.Add(this.ij_save);
            this.panel5.Controls.Add(this.ij_bodyHP);
            this.panel5.Controls.Add(this.ij_armHP);
            this.panel5.Controls.Add(this.ij_legHP);
            this.panel5.Controls.Add(this.ij_neckHP);
            this.panel5.Controls.Add(this.label15);
            this.panel5.Controls.Add(this.label14);
            this.panel5.Controls.Add(this.label13);
            this.panel5.Controls.Add(this.label12);
            this.panel5.Location = new System.Drawing.Point(375, 31);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(427, 247);
            this.panel5.TabIndex = 2;
            // 
            // ij_notifications
            // 
            this.ij_notifications.AutoSize = true;
            this.ij_notifications.Location = new System.Drawing.Point(129, 29);
            this.ij_notifications.Name = "ij_notifications";
            this.ij_notifications.Size = new System.Drawing.Size(148, 17);
            this.ij_notifications.TabIndex = 25;
            this.ij_notifications.Text = "Enable Injury Notifications";
            this.ij_notifications.UseVisualStyleBackColor = true;
            // 
            // ij_highRisk
            // 
            this.ij_highRisk.AutoSize = true;
            this.ij_highRisk.Location = new System.Drawing.Point(129, 8);
            this.ij_highRisk.Name = "ij_highRisk";
            this.ij_highRisk.Size = new System.Drawing.Size(198, 17);
            this.ij_highRisk.TabIndex = 24;
            this.ij_highRisk.Text = "High Risk Moves Can Cause Injuries";
            this.ij_highRisk.UseVisualStyleBackColor = true;
            // 
            // ij_recoveryRate
            // 
            this.ij_recoveryRate.Location = new System.Drawing.Point(158, 70);
            this.ij_recoveryRate.Name = "ij_recoveryRate";
            this.ij_recoveryRate.Size = new System.Drawing.Size(126, 20);
            this.ij_recoveryRate.TabIndex = 19;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(177, 54);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(79, 13);
            this.label16.TabIndex = 18;
            this.label16.Text = "Recovery Rate";
            // 
            // ij_bodyHP
            // 
            this.ij_bodyHP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ij_bodyHP.FormattingEnabled = true;
            this.ij_bodyHP.Location = new System.Drawing.Point(300, 70);
            this.ij_bodyHP.Name = "ij_bodyHP";
            this.ij_bodyHP.Size = new System.Drawing.Size(88, 21);
            this.ij_bodyHP.TabIndex = 16;
            // 
            // ij_legHP
            // 
            this.ij_legHP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ij_legHP.FormattingEnabled = true;
            this.ij_legHP.Location = new System.Drawing.Point(300, 138);
            this.ij_legHP.Name = "ij_legHP";
            this.ij_legHP.Size = new System.Drawing.Size(88, 21);
            this.ij_legHP.TabIndex = 14;
            // 
            // ij_neckHP
            // 
            this.ij_neckHP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ij_neckHP.FormattingEnabled = true;
            this.ij_neckHP.Location = new System.Drawing.Point(46, 70);
            this.ij_neckHP.Name = "ij_neckHP";
            this.ij_neckHP.Size = new System.Drawing.Size(88, 21);
            this.ij_neckHP.TabIndex = 13;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(312, 54);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(65, 13);
            this.label15.TabIndex = 12;
            this.label15.Text = "Body Health";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(56, 54);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(67, 13);
            this.label12.TabIndex = 9;
            this.label12.Text = "Neck Health";
            // 
            // em_injuries
            // 
            this.em_injuries.AutoSize = true;
            this.em_injuries.Location = new System.Drawing.Point(12, 9);
            this.em_injuries.Name = "em_injuries";
            this.em_injuries.Size = new System.Drawing.Size(95, 17);
            this.em_injuries.TabIndex = 3;
            this.em_injuries.Text = "Enable Injuries";
            this.em_injuries.UseVisualStyleBackColor = true;
            // 
            // InjuryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(813, 284);
            this.Controls.Add(this.em_injuries);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Name = "InjuryForm";
            this.Text = "InjuryForm";
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel4;
        public System.Windows.Forms.ListBox ij_wrestlerList;
        public System.Windows.Forms.Button ij_removeAll;
        public System.Windows.Forms.Button ij_add;
        public System.Windows.Forms.Button ij_removeOne;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox ij_result;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox ij_wrestlerSearch;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox ij_recoveryDate;
        private System.Windows.Forms.Label label18;
        public System.Windows.Forms.TextBox ij_matches;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Button ij_load;
        private System.Windows.Forms.Button ij_save;
        public System.Windows.Forms.ComboBox ij_armHP;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Panel panel5;
        public System.Windows.Forms.CheckBox ij_notifications;
        public System.Windows.Forms.CheckBox ij_highRisk;
        public System.Windows.Forms.TextBox ij_recoveryRate;
        private System.Windows.Forms.Label label16;
        public System.Windows.Forms.ComboBox ij_bodyHP;
        public System.Windows.Forms.ComboBox ij_legHP;
        public System.Windows.Forms.ComboBox ij_neckHP;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label12;
        public System.Windows.Forms.CheckBox em_injuries;
    }
}