namespace QoL_Mods.Public
{
    partial class UkemiNotificationForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.uk_wrestlerTab = new System.Windows.Forms.TabPage();
            this.uk_removeWrestlerNotification = new System.Windows.Forms.Button();
            this.uk_addWrestlerNoticiation = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.uk_wrestlerNotification = new System.Windows.Forms.ComboBox();
            this.uk_wrestlers = new System.Windows.Forms.ComboBox();
            this.label23 = new System.Windows.Forms.Label();
            this.panel19 = new System.Windows.Forms.Panel();
            this.label60 = new System.Windows.Forms.Label();
            this.uk_removeWrestler = new System.Windows.Forms.Button();
            this.uk_wrestlerSearch = new System.Windows.Forms.TextBox();
            this.uk_addWrestler = new System.Windows.Forms.Button();
            this.label61 = new System.Windows.Forms.Label();
            this.uk_wrestlerResults = new System.Windows.Forms.ComboBox();
            this.uk_promotionTab = new System.Windows.Forms.TabPage();
            this.uk_removeRingNotification = new System.Windows.Forms.Button();
            this.uk_addRingNotification = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.uk_ringNotifications = new System.Windows.Forms.ComboBox();
            this.uk_ringsList = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.uk_removeRing = new System.Windows.Forms.Button();
            this.uk_ringSearch = new System.Windows.Forms.TextBox();
            this.uk_addRing = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.uk_ringResults = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.uk_notificationList = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.uk_load = new System.Windows.Forms.Button();
            this.uk_save = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.uk_wrestlerTab.SuspendLayout();
            this.panel19.SuspendLayout();
            this.uk_promotionTab.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.uk_wrestlerTab);
            this.tabControl1.Controls.Add(this.uk_promotionTab);
            this.tabControl1.Location = new System.Drawing.Point(156, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(417, 250);
            this.tabControl1.TabIndex = 0;
            // 
            // uk_wrestlerTab
            // 
            this.uk_wrestlerTab.Controls.Add(this.uk_removeWrestlerNotification);
            this.uk_wrestlerTab.Controls.Add(this.uk_addWrestlerNoticiation);
            this.uk_wrestlerTab.Controls.Add(this.label1);
            this.uk_wrestlerTab.Controls.Add(this.uk_wrestlerNotification);
            this.uk_wrestlerTab.Controls.Add(this.uk_wrestlers);
            this.uk_wrestlerTab.Controls.Add(this.label23);
            this.uk_wrestlerTab.Controls.Add(this.panel19);
            this.uk_wrestlerTab.Location = new System.Drawing.Point(4, 22);
            this.uk_wrestlerTab.Name = "uk_wrestlerTab";
            this.uk_wrestlerTab.Padding = new System.Windows.Forms.Padding(3);
            this.uk_wrestlerTab.Size = new System.Drawing.Size(409, 224);
            this.uk_wrestlerTab.TabIndex = 0;
            this.uk_wrestlerTab.Text = "Wrestlers";
            this.uk_wrestlerTab.UseVisualStyleBackColor = true;
            // 
            // uk_removeWrestlerNotification
            // 
            this.uk_removeWrestlerNotification.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uk_removeWrestlerNotification.Location = new System.Drawing.Point(304, 146);
            this.uk_removeWrestlerNotification.Name = "uk_removeWrestlerNotification";
            this.uk_removeWrestlerNotification.Size = new System.Drawing.Size(75, 23);
            this.uk_removeWrestlerNotification.TabIndex = 65;
            this.uk_removeWrestlerNotification.Text = "Remove";
            this.uk_removeWrestlerNotification.UseVisualStyleBackColor = true;
            this.uk_removeWrestlerNotification.Click += new System.EventHandler(this.uk_removeWrestlerNotification_Click);
            // 
            // uk_addWrestlerNoticiation
            // 
            this.uk_addWrestlerNoticiation.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uk_addWrestlerNoticiation.Location = new System.Drawing.Point(211, 146);
            this.uk_addWrestlerNoticiation.Name = "uk_addWrestlerNoticiation";
            this.uk_addWrestlerNoticiation.Size = new System.Drawing.Size(75, 23);
            this.uk_addWrestlerNoticiation.TabIndex = 64;
            this.uk_addWrestlerNoticiation.Text = "Add";
            this.uk_addWrestlerNoticiation.UseVisualStyleBackColor = true;
            this.uk_addWrestlerNoticiation.Click += new System.EventHandler(this.uk_addWrestlerNoticiation_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(243, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 15);
            this.label1.TabIndex = 62;
            this.label1.Text = "Notification Sound";
            // 
            // uk_wrestlerNotification
            // 
            this.uk_wrestlerNotification.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.uk_wrestlerNotification.FormattingEnabled = true;
            this.uk_wrestlerNotification.Location = new System.Drawing.Point(211, 100);
            this.uk_wrestlerNotification.Name = "uk_wrestlerNotification";
            this.uk_wrestlerNotification.Size = new System.Drawing.Size(168, 21);
            this.uk_wrestlerNotification.TabIndex = 63;
            // 
            // uk_wrestlers
            // 
            this.uk_wrestlers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.uk_wrestlers.FormattingEnabled = true;
            this.uk_wrestlers.Location = new System.Drawing.Point(212, 39);
            this.uk_wrestlers.Name = "uk_wrestlers";
            this.uk_wrestlers.Size = new System.Drawing.Size(168, 21);
            this.uk_wrestlers.TabIndex = 60;
            this.uk_wrestlers.SelectedIndexChanged += new System.EventHandler(this.uk_wrestlers_SelectedIndexChanged);
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.Location = new System.Drawing.Point(270, 18);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(62, 15);
            this.label23.TabIndex = 61;
            this.label23.Text = "Wrestlers";
            // 
            // panel19
            // 
            this.panel19.BackColor = System.Drawing.Color.White;
            this.panel19.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel19.Controls.Add(this.label60);
            this.panel19.Controls.Add(this.uk_removeWrestler);
            this.panel19.Controls.Add(this.uk_wrestlerSearch);
            this.panel19.Controls.Add(this.uk_addWrestler);
            this.panel19.Controls.Add(this.label61);
            this.panel19.Controls.Add(this.uk_wrestlerResults);
            this.panel19.Location = new System.Drawing.Point(6, 6);
            this.panel19.Name = "panel19";
            this.panel19.Size = new System.Drawing.Size(185, 210);
            this.panel19.TabIndex = 19;
            // 
            // label60
            // 
            this.label60.AutoSize = true;
            this.label60.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label60.Location = new System.Drawing.Point(50, 10);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(99, 15);
            this.label60.TabIndex = 12;
            this.label60.Text = "Wrestler Search";
            // 
            // uk_removeWrestler
            // 
            this.uk_removeWrestler.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uk_removeWrestler.Location = new System.Drawing.Point(53, 170);
            this.uk_removeWrestler.Name = "uk_removeWrestler";
            this.uk_removeWrestler.Size = new System.Drawing.Size(75, 23);
            this.uk_removeWrestler.TabIndex = 17;
            this.uk_removeWrestler.Text = "Remove";
            this.uk_removeWrestler.UseVisualStyleBackColor = true;
            this.uk_removeWrestler.Click += new System.EventHandler(this.uk_removeWrestler_Click);
            // 
            // uk_wrestlerSearch
            // 
            this.uk_wrestlerSearch.Location = new System.Drawing.Point(6, 36);
            this.uk_wrestlerSearch.Name = "uk_wrestlerSearch";
            this.uk_wrestlerSearch.Size = new System.Drawing.Size(168, 20);
            this.uk_wrestlerSearch.TabIndex = 13;
            // 
            // uk_addWrestler
            // 
            this.uk_addWrestler.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uk_addWrestler.Location = new System.Drawing.Point(53, 130);
            this.uk_addWrestler.Name = "uk_addWrestler";
            this.uk_addWrestler.Size = new System.Drawing.Size(75, 23);
            this.uk_addWrestler.TabIndex = 16;
            this.uk_addWrestler.Text = "Add";
            this.uk_addWrestler.UseVisualStyleBackColor = true;
            this.uk_addWrestler.Click += new System.EventHandler(this.uk_addWrestler_Click);
            // 
            // label61
            // 
            this.label61.AutoSize = true;
            this.label61.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label61.Location = new System.Drawing.Point(59, 71);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(81, 15);
            this.label61.TabIndex = 14;
            this.label61.Text = "Wrestler List";
            // 
            // uk_wrestlerResults
            // 
            this.uk_wrestlerResults.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.uk_wrestlerResults.FormattingEnabled = true;
            this.uk_wrestlerResults.Location = new System.Drawing.Point(6, 91);
            this.uk_wrestlerResults.Name = "uk_wrestlerResults";
            this.uk_wrestlerResults.Size = new System.Drawing.Size(168, 21);
            this.uk_wrestlerResults.TabIndex = 15;
            // 
            // uk_promotionTab
            // 
            this.uk_promotionTab.Controls.Add(this.uk_removeRingNotification);
            this.uk_promotionTab.Controls.Add(this.uk_addRingNotification);
            this.uk_promotionTab.Controls.Add(this.label4);
            this.uk_promotionTab.Controls.Add(this.uk_ringNotifications);
            this.uk_promotionTab.Controls.Add(this.uk_ringsList);
            this.uk_promotionTab.Controls.Add(this.label5);
            this.uk_promotionTab.Controls.Add(this.panel1);
            this.uk_promotionTab.Location = new System.Drawing.Point(4, 22);
            this.uk_promotionTab.Name = "uk_promotionTab";
            this.uk_promotionTab.Padding = new System.Windows.Forms.Padding(3);
            this.uk_promotionTab.Size = new System.Drawing.Size(409, 224);
            this.uk_promotionTab.TabIndex = 1;
            this.uk_promotionTab.Text = "Promotions";
            this.uk_promotionTab.UseVisualStyleBackColor = true;
            // 
            // uk_removeRingNotification
            // 
            this.uk_removeRingNotification.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uk_removeRingNotification.Location = new System.Drawing.Point(306, 148);
            this.uk_removeRingNotification.Name = "uk_removeRingNotification";
            this.uk_removeRingNotification.Size = new System.Drawing.Size(75, 23);
            this.uk_removeRingNotification.TabIndex = 71;
            this.uk_removeRingNotification.Text = "Remove";
            this.uk_removeRingNotification.UseVisualStyleBackColor = true;
            this.uk_removeRingNotification.Click += new System.EventHandler(this.uk_removeRingNotification_Click);
            // 
            // uk_addRingNotification
            // 
            this.uk_addRingNotification.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uk_addRingNotification.Location = new System.Drawing.Point(213, 148);
            this.uk_addRingNotification.Name = "uk_addRingNotification";
            this.uk_addRingNotification.Size = new System.Drawing.Size(75, 23);
            this.uk_addRingNotification.TabIndex = 70;
            this.uk_addRingNotification.Text = "Add";
            this.uk_addRingNotification.UseVisualStyleBackColor = true;
            this.uk_addRingNotification.Click += new System.EventHandler(this.uk_addRingNotification_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(245, 79);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(120, 15);
            this.label4.TabIndex = 68;
            this.label4.Text = "Notification Sounds";
            // 
            // uk_ringNotifications
            // 
            this.uk_ringNotifications.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.uk_ringNotifications.FormattingEnabled = true;
            this.uk_ringNotifications.Location = new System.Drawing.Point(213, 102);
            this.uk_ringNotifications.Name = "uk_ringNotifications";
            this.uk_ringNotifications.Size = new System.Drawing.Size(168, 21);
            this.uk_ringNotifications.TabIndex = 69;
            // 
            // uk_ringsList
            // 
            this.uk_ringsList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.uk_ringsList.FormattingEnabled = true;
            this.uk_ringsList.Location = new System.Drawing.Point(214, 41);
            this.uk_ringsList.Name = "uk_ringsList";
            this.uk_ringsList.Size = new System.Drawing.Size(168, 21);
            this.uk_ringsList.TabIndex = 66;
            this.uk_ringsList.SelectedIndexChanged += new System.EventHandler(this.uk_rings_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(276, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 15);
            this.label5.TabIndex = 67;
            this.label5.Text = "Rings";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.uk_removeRing);
            this.panel1.Controls.Add(this.uk_ringSearch);
            this.panel1.Controls.Add(this.uk_addRing);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.uk_ringResults);
            this.panel1.Location = new System.Drawing.Point(6, 8);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(185, 210);
            this.panel1.TabIndex = 20;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(52, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 15);
            this.label2.TabIndex = 12;
            this.label2.Text = "Ring Search";
            // 
            // uk_removeRing
            // 
            this.uk_removeRing.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uk_removeRing.Location = new System.Drawing.Point(53, 170);
            this.uk_removeRing.Name = "uk_removeRing";
            this.uk_removeRing.Size = new System.Drawing.Size(75, 23);
            this.uk_removeRing.TabIndex = 17;
            this.uk_removeRing.Text = "Remove";
            this.uk_removeRing.UseVisualStyleBackColor = true;
            this.uk_removeRing.Click += new System.EventHandler(this.uk_removeRing_Click);
            // 
            // uk_ringSearch
            // 
            this.uk_ringSearch.Location = new System.Drawing.Point(6, 36);
            this.uk_ringSearch.Name = "uk_ringSearch";
            this.uk_ringSearch.Size = new System.Drawing.Size(168, 20);
            this.uk_ringSearch.TabIndex = 13;
            // 
            // uk_addRing
            // 
            this.uk_addRing.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uk_addRing.Location = new System.Drawing.Point(53, 130);
            this.uk_addRing.Name = "uk_addRing";
            this.uk_addRing.Size = new System.Drawing.Size(75, 23);
            this.uk_addRing.TabIndex = 16;
            this.uk_addRing.Text = "Add";
            this.uk_addRing.UseVisualStyleBackColor = true;
            this.uk_addRing.Click += new System.EventHandler(this.uk_addRing_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(60, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 15);
            this.label3.TabIndex = 14;
            this.label3.Text = "Ring List";
            // 
            // uk_ringResults
            // 
            this.uk_ringResults.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.uk_ringResults.FormattingEnabled = true;
            this.uk_ringResults.Location = new System.Drawing.Point(6, 91);
            this.uk_ringResults.Name = "uk_ringResults";
            this.uk_ringResults.Size = new System.Drawing.Size(168, 21);
            this.uk_ringResults.TabIndex = 15;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.uk_load);
            this.panel2.Controls.Add(this.uk_save);
            this.panel2.Controls.Add(this.uk_notificationList);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Location = new System.Drawing.Point(12, 28);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(138, 230);
            this.panel2.TabIndex = 1;
            // 
            // uk_notificationList
            // 
            this.uk_notificationList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.uk_notificationList.FormattingEnabled = true;
            this.uk_notificationList.Location = new System.Drawing.Point(5, 41);
            this.uk_notificationList.Name = "uk_notificationList";
            this.uk_notificationList.Size = new System.Drawing.Size(126, 21);
            this.uk_notificationList.TabIndex = 67;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(25, 12);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 15);
            this.label6.TabIndex = 66;
            this.label6.Text = "Notifications";
            // 
            // uk_load
            // 
            this.uk_load.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uk_load.Location = new System.Drawing.Point(28, 124);
            this.uk_load.Name = "uk_load";
            this.uk_load.Size = new System.Drawing.Size(75, 23);
            this.uk_load.TabIndex = 69;
            this.uk_load.Text = "Load";
            this.uk_load.UseVisualStyleBackColor = true;
            this.uk_load.Click += new System.EventHandler(this.uk_load_Click);
            // 
            // uk_save
            // 
            this.uk_save.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uk_save.Location = new System.Drawing.Point(28, 84);
            this.uk_save.Name = "uk_save";
            this.uk_save.Size = new System.Drawing.Size(75, 23);
            this.uk_save.TabIndex = 68;
            this.uk_save.Text = "Save";
            this.uk_save.UseVisualStyleBackColor = true;
            this.uk_save.Click += new System.EventHandler(this.uk_save_Click);
            // 
            // UkemiNotificationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 266);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.tabControl1);
            this.Name = "UkemiNotificationForm";
            this.Text = "Ukemi Notification Form";
            this.tabControl1.ResumeLayout(false);
            this.uk_wrestlerTab.ResumeLayout(false);
            this.uk_wrestlerTab.PerformLayout();
            this.panel19.ResumeLayout(false);
            this.panel19.PerformLayout();
            this.uk_promotionTab.ResumeLayout(false);
            this.uk_promotionTab.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage uk_wrestlerTab;
        private System.Windows.Forms.TabPage uk_promotionTab;
        private System.Windows.Forms.Panel panel19;
        private System.Windows.Forms.Label label60;
        private System.Windows.Forms.Button uk_removeWrestler;
        public System.Windows.Forms.TextBox uk_wrestlerSearch;
        private System.Windows.Forms.Button uk_addWrestler;
        private System.Windows.Forms.Label label61;
        private System.Windows.Forms.ComboBox uk_wrestlerResults;
        public System.Windows.Forms.ComboBox uk_wrestlers;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox uk_wrestlerNotification;
        private System.Windows.Forms.Button uk_removeWrestlerNotification;
        private System.Windows.Forms.Button uk_addWrestlerNoticiation;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button uk_removeRing;
        public System.Windows.Forms.TextBox uk_ringSearch;
        private System.Windows.Forms.Button uk_addRing;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox uk_ringResults;
        private System.Windows.Forms.Button uk_removeRingNotification;
        private System.Windows.Forms.Button uk_addRingNotification;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox uk_ringNotifications;
        public System.Windows.Forms.ComboBox uk_ringsList;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox uk_notificationList;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button uk_load;
        private System.Windows.Forms.Button uk_save;
    }
}