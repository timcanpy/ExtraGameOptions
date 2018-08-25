using System;

namespace QoL_Mods
{
    partial class QoL_Form
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
            this.cb_comment = new System.Windows.Forms.CheckBox();
            this.ego_MainTabs = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.ql_Controls = new System.Windows.Forms.TabControl();
            this.ql_WrestlerEdit = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.we_refresh = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_mainMenu = new System.Windows.Forms.Button();
            this.we_resultList = new System.Windows.Forms.ComboBox();
            this.we_search = new System.Windows.Forms.Button();
            this.we_promotionBox = new System.Windows.Forms.ComboBox();
            this.we_edit = new System.Windows.Forms.Button();
            this.we_searchBox = new System.Windows.Forms.TextBox();
            this.we_unsubscribe = new System.Windows.Forms.CheckBox();
            this.ql_RefereeEdit = new System.Windows.Forms.TabPage();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.refEdit = new System.Windows.Forms.Button();
            this.re_refresh = new System.Windows.Forms.Button();
            this.we_refList = new System.Windows.Forms.ComboBox();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.em_injuries = new System.Windows.Forms.CheckBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.ij_matches = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.ij_recoveryRate = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.cb_injuryModData = new System.Windows.Forms.CheckBox();
            this.ij_load = new System.Windows.Forms.Button();
            this.ij_save = new System.Windows.Forms.Button();
            this.ij_bodyHP = new System.Windows.Forms.ComboBox();
            this.ij_armHP = new System.Windows.Forms.ComboBox();
            this.ij_legHP = new System.Windows.Forms.ComboBox();
            this.ij_neckHP = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.ij_removeAll = new System.Windows.Forms.Button();
            this.ij_add = new System.Windows.Forms.Button();
            this.ij_removeOne = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.ij_wrestlerList = new System.Windows.Forms.ListBox();
            this.ij_result = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.ij_wrestlerSearch = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.overrideTabs = new System.Windows.Forms.TabControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.or_default = new System.Windows.Forms.CheckBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.or_ring = new System.Windows.Forms.CheckBox();
            this.or_ringList = new System.Windows.Forms.ComboBox();
            this.or_venueList = new System.Windows.Forms.ComboBox();
            this.or_venue = new System.Windows.Forms.CheckBox();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.panel7 = new System.Windows.Forms.Panel();
            this.ma_forceTag = new System.Windows.Forms.CheckBox();
            this.ma_aiCall = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.refBum_minLogic = new System.Windows.Forms.DomainUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.btn_bumpRef = new System.Windows.Forms.Button();
            this.ma_convertSeconds = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.ij_recoveryDate = new System.Windows.Forms.TextBox();
            this.ego_MainTabs.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.ql_Controls.SuspendLayout();
            this.ql_WrestlerEdit.SuspendLayout();
            this.panel1.SuspendLayout();
            this.ql_RefereeEdit.SuspendLayout();
            this.panel6.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.overrideTabs.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cb_comment
            // 
            this.cb_comment.AutoSize = true;
            this.cb_comment.Location = new System.Drawing.Point(297, 12);
            this.cb_comment.Name = "cb_comment";
            this.cb_comment.Size = new System.Drawing.Size(148, 17);
            this.cb_comment.TabIndex = 0;
            this.cb_comment.Text = "Track Crowd Excitement?";
            this.cb_comment.UseVisualStyleBackColor = true;
            // 
            // ego_MainTabs
            // 
            this.ego_MainTabs.Controls.Add(this.tabPage1);
            this.ego_MainTabs.Controls.Add(this.tabPage3);
            this.ego_MainTabs.Controls.Add(this.tabPage2);
            this.ego_MainTabs.Location = new System.Drawing.Point(12, 12);
            this.ego_MainTabs.Name = "ego_MainTabs";
            this.ego_MainTabs.SelectedIndex = 0;
            this.ego_MainTabs.Size = new System.Drawing.Size(852, 340);
            this.ego_MainTabs.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.ql_Controls);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(844, 314);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Edit Management";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // ql_Controls
            // 
            this.ql_Controls.Controls.Add(this.ql_WrestlerEdit);
            this.ql_Controls.Controls.Add(this.ql_RefereeEdit);
            this.ql_Controls.Controls.Add(this.tabPage6);
            this.ql_Controls.Location = new System.Drawing.Point(6, 6);
            this.ql_Controls.Name = "ql_Controls";
            this.ql_Controls.SelectedIndex = 0;
            this.ql_Controls.Size = new System.Drawing.Size(832, 311);
            this.ql_Controls.TabIndex = 0;
            // 
            // ql_WrestlerEdit
            // 
            this.ql_WrestlerEdit.BackColor = System.Drawing.Color.Transparent;
            this.ql_WrestlerEdit.Controls.Add(this.panel1);
            this.ql_WrestlerEdit.Controls.Add(this.we_unsubscribe);
            this.ql_WrestlerEdit.Location = new System.Drawing.Point(4, 22);
            this.ql_WrestlerEdit.Name = "ql_WrestlerEdit";
            this.ql_WrestlerEdit.Padding = new System.Windows.Forms.Padding(3);
            this.ql_WrestlerEdit.Size = new System.Drawing.Size(824, 285);
            this.ql_WrestlerEdit.TabIndex = 0;
            this.ql_WrestlerEdit.Text = "Wrestlers";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.we_refresh);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.btn_mainMenu);
            this.panel1.Controls.Add(this.we_resultList);
            this.panel1.Controls.Add(this.we_search);
            this.panel1.Controls.Add(this.we_promotionBox);
            this.panel1.Controls.Add(this.we_edit);
            this.panel1.Controls.Add(this.we_searchBox);
            this.panel1.Location = new System.Drawing.Point(126, 46);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(543, 214);
            this.panel1.TabIndex = 16;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(231, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Wrestler Search";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(234, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Search Results";
            // 
            // we_refresh
            // 
            this.we_refresh.Location = new System.Drawing.Point(283, 170);
            this.we_refresh.Name = "we_refresh";
            this.we_refresh.Size = new System.Drawing.Size(99, 23);
            this.we_refresh.TabIndex = 14;
            this.we_refresh.Text = "Refresh Lists";
            this.we_refresh.UseVisualStyleBackColor = true;
            this.we_refresh.Click += new System.EventHandler(this.we_refresh_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(236, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Promotion List";
            // 
            // btn_mainMenu
            // 
            this.btn_mainMenu.Location = new System.Drawing.Point(388, 170);
            this.btn_mainMenu.Name = "btn_mainMenu";
            this.btn_mainMenu.Size = new System.Drawing.Size(87, 23);
            this.btn_mainMenu.TabIndex = 13;
            this.btn_mainMenu.Text = "Main Menu";
            this.btn_mainMenu.UseVisualStyleBackColor = true;
            this.btn_mainMenu.Click += new System.EventHandler(this.btn_mainMenu_Click);
            // 
            // we_resultList
            // 
            this.we_resultList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.we_resultList.FormattingEnabled = true;
            this.we_resultList.Location = new System.Drawing.Point(115, 133);
            this.we_resultList.Name = "we_resultList";
            this.we_resultList.Size = new System.Drawing.Size(360, 21);
            this.we_resultList.Sorted = true;
            this.we_resultList.TabIndex = 9;
            // 
            // we_search
            // 
            this.we_search.Location = new System.Drawing.Point(115, 170);
            this.we_search.Name = "we_search";
            this.we_search.Size = new System.Drawing.Size(75, 23);
            this.we_search.TabIndex = 12;
            this.we_search.Text = "Search";
            this.we_search.UseVisualStyleBackColor = true;
            this.we_search.Click += new System.EventHandler(this.we_search_Click);
            // 
            // we_promotionBox
            // 
            this.we_promotionBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.we_promotionBox.FormattingEnabled = true;
            this.we_promotionBox.Location = new System.Drawing.Point(115, 83);
            this.we_promotionBox.Name = "we_promotionBox";
            this.we_promotionBox.Size = new System.Drawing.Size(360, 21);
            this.we_promotionBox.Sorted = true;
            this.we_promotionBox.TabIndex = 10;
            this.we_promotionBox.SelectedIndexChanged += new System.EventHandler(this.we_promotionBox_SelectedIndexChanged);
            // 
            // we_edit
            // 
            this.we_edit.Location = new System.Drawing.Point(202, 170);
            this.we_edit.Name = "we_edit";
            this.we_edit.Size = new System.Drawing.Size(75, 23);
            this.we_edit.TabIndex = 0;
            this.we_edit.Text = "Edit";
            this.we_edit.UseVisualStyleBackColor = true;
            this.we_edit.Click += new System.EventHandler(this.we_edit_Click);
            // 
            // we_searchBox
            // 
            this.we_searchBox.Location = new System.Drawing.Point(115, 33);
            this.we_searchBox.Name = "we_searchBox";
            this.we_searchBox.Size = new System.Drawing.Size(360, 20);
            this.we_searchBox.TabIndex = 11;
            this.we_searchBox.LostFocus += new System.EventHandler(this.we_SearchBox_LostFocus);
            // 
            // we_unsubscribe
            // 
            this.we_unsubscribe.AutoSize = true;
            this.we_unsubscribe.Font = new System.Drawing.Font("NCAA Utah St Aggies Bold", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.we_unsubscribe.Location = new System.Drawing.Point(171, 16);
            this.we_unsubscribe.Name = "we_unsubscribe";
            this.we_unsubscribe.Size = new System.Drawing.Size(438, 24);
            this.we_unsubscribe.TabIndex = 15;
            this.we_unsubscribe.Text = "Unsubscribe Wrestler From Workshop";
            this.we_unsubscribe.UseVisualStyleBackColor = true;
            // 
            // ql_RefereeEdit
            // 
            this.ql_RefereeEdit.BackColor = System.Drawing.Color.Transparent;
            this.ql_RefereeEdit.Controls.Add(this.panel6);
            this.ql_RefereeEdit.Location = new System.Drawing.Point(4, 22);
            this.ql_RefereeEdit.Name = "ql_RefereeEdit";
            this.ql_RefereeEdit.Size = new System.Drawing.Size(824, 285);
            this.ql_RefereeEdit.TabIndex = 2;
            this.ql_RefereeEdit.Text = "Referees";
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.White;
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel6.Controls.Add(this.label4);
            this.panel6.Controls.Add(this.refEdit);
            this.panel6.Controls.Add(this.re_refresh);
            this.panel6.Controls.Add(this.we_refList);
            this.panel6.Location = new System.Drawing.Point(246, 43);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(353, 164);
            this.panel6.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(99, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(148, 31);
            this.label4.TabIndex = 2;
            this.label4.Text = "Referee List";
            // 
            // refEdit
            // 
            this.refEdit.Location = new System.Drawing.Point(36, 113);
            this.refEdit.Name = "refEdit";
            this.refEdit.Size = new System.Drawing.Size(75, 29);
            this.refEdit.TabIndex = 3;
            this.refEdit.Text = "Edit";
            this.refEdit.UseVisualStyleBackColor = true;
            this.refEdit.Click += new System.EventHandler(this.refEdit_Click);
            // 
            // re_refresh
            // 
            this.re_refresh.Location = new System.Drawing.Point(230, 113);
            this.re_refresh.Name = "re_refresh";
            this.re_refresh.Size = new System.Drawing.Size(75, 29);
            this.re_refresh.TabIndex = 4;
            this.re_refresh.Text = "Refresh List";
            this.re_refresh.UseVisualStyleBackColor = true;
            this.re_refresh.Click += new System.EventHandler(this.re_refresh_Click);
            // 
            // we_refList
            // 
            this.we_refList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.we_refList.FormattingEnabled = true;
            this.we_refList.Location = new System.Drawing.Point(36, 65);
            this.we_refList.Name = "we_refList";
            this.we_refList.Size = new System.Drawing.Size(269, 21);
            this.we_refList.TabIndex = 1;
            // 
            // tabPage6
            // 
            this.tabPage6.BackColor = System.Drawing.Color.Transparent;
            this.tabPage6.Controls.Add(this.em_injuries);
            this.tabPage6.Controls.Add(this.panel5);
            this.tabPage6.Controls.Add(this.panel4);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Size = new System.Drawing.Size(824, 285);
            this.tabPage6.TabIndex = 3;
            this.tabPage6.Text = "Injuries";
            // 
            // em_injuries
            // 
            this.em_injuries.AutoSize = true;
            this.em_injuries.Location = new System.Drawing.Point(16, 7);
            this.em_injuries.Name = "em_injuries";
            this.em_injuries.Size = new System.Drawing.Size(95, 17);
            this.em_injuries.TabIndex = 2;
            this.em_injuries.Text = "Enable Injuries";
            this.em_injuries.UseVisualStyleBackColor = true;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.White;
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel5.Controls.Add(this.ij_recoveryDate);
            this.panel5.Controls.Add(this.label18);
            this.panel5.Controls.Add(this.ij_matches);
            this.panel5.Controls.Add(this.label17);
            this.panel5.Controls.Add(this.ij_recoveryRate);
            this.panel5.Controls.Add(this.label16);
            this.panel5.Controls.Add(this.cb_injuryModData);
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
            this.panel5.Controls.Add(this.label11);
            this.panel5.Location = new System.Drawing.Point(376, 30);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(427, 247);
            this.panel5.TabIndex = 1;
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
            // ij_recoveryRate
            // 
            this.ij_recoveryRate.Location = new System.Drawing.Point(158, 70);
            this.ij_recoveryRate.Name = "ij_recoveryRate";
            this.ij_recoveryRate.Size = new System.Drawing.Size(126, 20);
            this.ij_recoveryRate.TabIndex = 19;
            this.ij_recoveryRate.LostFocus += new System.EventHandler(this.ij_recoveryRate_LostFocus);
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
            // cb_injuryModData
            // 
            this.cb_injuryModData.AutoSize = true;
            this.cb_injuryModData.Location = new System.Drawing.Point(133, 31);
            this.cb_injuryModData.Name = "cb_injuryModData";
            this.cb_injuryModData.Size = new System.Drawing.Size(162, 17);
            this.cb_injuryModData.TabIndex = 17;
            this.cb_injuryModData.Text = "Injuries Can Modify Edit Data";
            this.cb_injuryModData.UseVisualStyleBackColor = true;
            // 
            // ij_load
            // 
            this.ij_load.Location = new System.Drawing.Point(300, 221);
            this.ij_load.Name = "ij_load";
            this.ij_load.Size = new System.Drawing.Size(94, 23);
            this.ij_load.TabIndex = 9;
            this.ij_load.Text = "Load Data";
            this.ij_load.UseVisualStyleBackColor = true;
            this.ij_load.Click += new System.EventHandler(this.ij_load_Click);
            // 
            // ij_save
            // 
            this.ij_save.Location = new System.Drawing.Point(40, 221);
            this.ij_save.Name = "ij_save";
            this.ij_save.Size = new System.Drawing.Size(94, 23);
            this.ij_save.TabIndex = 10;
            this.ij_save.Text = "Save Data";
            this.ij_save.UseVisualStyleBackColor = true;
            this.ij_save.Click += new System.EventHandler(this.ij_save_Click);
            // 
            // ij_bodyHP
            // 
            this.ij_bodyHP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ij_bodyHP.FormattingEnabled = true;
            this.ij_bodyHP.Location = new System.Drawing.Point(300, 70);
            this.ij_bodyHP.Name = "ij_bodyHP";
            this.ij_bodyHP.Size = new System.Drawing.Size(88, 21);
            this.ij_bodyHP.TabIndex = 16;
            this.ij_bodyHP.SelectedIndexChanged += new System.EventHandler(this.ij_bodyHP_SelectedIndexChanged);
            // 
            // ij_armHP
            // 
            this.ij_armHP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ij_armHP.FormattingEnabled = true;
            this.ij_armHP.Location = new System.Drawing.Point(46, 138);
            this.ij_armHP.Name = "ij_armHP";
            this.ij_armHP.Size = new System.Drawing.Size(88, 21);
            this.ij_armHP.TabIndex = 15;
            this.ij_armHP.SelectedIndexChanged += new System.EventHandler(this.ij_armHP_SelectedIndexChanged);
            // 
            // ij_legHP
            // 
            this.ij_legHP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ij_legHP.FormattingEnabled = true;
            this.ij_legHP.Location = new System.Drawing.Point(300, 138);
            this.ij_legHP.Name = "ij_legHP";
            this.ij_legHP.Size = new System.Drawing.Size(88, 21);
            this.ij_legHP.TabIndex = 14;
            this.ij_legHP.SelectedIndexChanged += new System.EventHandler(this.ij_legHP_SelectedIndexChanged);
            // 
            // ij_neckHP
            // 
            this.ij_neckHP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ij_neckHP.FormattingEnabled = true;
            this.ij_neckHP.Location = new System.Drawing.Point(46, 70);
            this.ij_neckHP.Name = "ij_neckHP";
            this.ij_neckHP.Size = new System.Drawing.Size(88, 21);
            this.ij_neckHP.TabIndex = 13;
            this.ij_neckHP.SelectedIndexChanged += new System.EventHandler(this.ij_neckHP_SelectedIndexChanged);
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
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(56, 54);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(67, 13);
            this.label12.TabIndex = 9;
            this.label12.Text = "Neck Health";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft YaHei UI", 14.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(175, 3);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(79, 25);
            this.label11.TabIndex = 9;
            this.label11.Text = "Injuries";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.White;
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel4.Controls.Add(this.ij_removeAll);
            this.panel4.Controls.Add(this.ij_add);
            this.panel4.Controls.Add(this.ij_removeOne);
            this.panel4.Controls.Add(this.label10);
            this.panel4.Controls.Add(this.ij_wrestlerList);
            this.panel4.Controls.Add(this.ij_result);
            this.panel4.Controls.Add(this.label6);
            this.panel4.Controls.Add(this.ij_wrestlerSearch);
            this.panel4.Controls.Add(this.label5);
            this.panel4.Location = new System.Drawing.Point(12, 30);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(342, 247);
            this.panel4.TabIndex = 0;
            // 
            // ij_removeAll
            // 
            this.ij_removeAll.Location = new System.Drawing.Point(234, 221);
            this.ij_removeAll.Name = "ij_removeAll";
            this.ij_removeAll.Size = new System.Drawing.Size(85, 23);
            this.ij_removeAll.TabIndex = 8;
            this.ij_removeAll.Text = "Remove All";
            this.ij_removeAll.UseVisualStyleBackColor = true;
            this.ij_removeAll.Click += new System.EventHandler(this.ij_removeAll_Click);
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
            this.ij_removeOne.Click += new System.EventHandler(this.ij_removeOne_Click);
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
            // ij_wrestlerList
            // 
            this.ij_wrestlerList.FormattingEnabled = true;
            this.ij_wrestlerList.Location = new System.Drawing.Point(26, 117);
            this.ij_wrestlerList.Name = "ij_wrestlerList";
            this.ij_wrestlerList.ScrollAlwaysVisible = true;
            this.ij_wrestlerList.Size = new System.Drawing.Size(293, 95);
            this.ij_wrestlerList.TabIndex = 4;
            this.ij_wrestlerList.SelectedIndexChanged += new System.EventHandler(this.ij_wrestlerList_SelectedIndexChanged);
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
            this.ij_wrestlerSearch.LostFocus += new System.EventHandler(this.ij_wrestlerSearch_LostFocus);
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
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(844, 314);
            this.tabPage3.TabIndex = 1;
            this.tabPage3.Text = "AI Behaviour";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.overrideTabs);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(844, 314);
            this.tabPage2.TabIndex = 2;
            this.tabPage2.Text = "Overrides";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // overrideTabs
            // 
            this.overrideTabs.Controls.Add(this.tabPage4);
            this.overrideTabs.Controls.Add(this.tabPage5);
            this.overrideTabs.Location = new System.Drawing.Point(3, 3);
            this.overrideTabs.Name = "overrideTabs";
            this.overrideTabs.SelectedIndex = 0;
            this.overrideTabs.Size = new System.Drawing.Size(841, 315);
            this.overrideTabs.TabIndex = 6;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.or_default);
            this.tabPage4.Controls.Add(this.panel3);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(833, 289);
            this.tabPage4.TabIndex = 0;
            this.tabPage4.Text = "Match Configuration";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // or_default
            // 
            this.or_default.AutoSize = true;
            this.or_default.Location = new System.Drawing.Point(169, 29);
            this.or_default.Name = "or_default";
            this.or_default.Size = new System.Drawing.Size(129, 17);
            this.or_default.TabIndex = 7;
            this.or_default.Text = "Override Default Edits";
            this.or_default.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.or_ring);
            this.panel3.Controls.Add(this.or_ringList);
            this.panel3.Controls.Add(this.or_venueList);
            this.panel3.Controls.Add(this.or_venue);
            this.panel3.Location = new System.Drawing.Point(3, 26);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(140, 220);
            this.panel3.TabIndex = 6;
            // 
            // or_ring
            // 
            this.or_ring.AutoSize = true;
            this.or_ring.Location = new System.Drawing.Point(3, 3);
            this.or_ring.Name = "or_ring";
            this.or_ring.Size = new System.Drawing.Size(91, 17);
            this.or_ring.TabIndex = 0;
            this.or_ring.Text = "Override Ring";
            this.or_ring.UseVisualStyleBackColor = true;
            // 
            // or_ringList
            // 
            this.or_ringList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.or_ringList.FormattingEnabled = true;
            this.or_ringList.Location = new System.Drawing.Point(3, 26);
            this.or_ringList.Name = "or_ringList";
            this.or_ringList.Size = new System.Drawing.Size(121, 21);
            this.or_ringList.TabIndex = 2;
            // 
            // or_venueList
            // 
            this.or_venueList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.or_venueList.FormattingEnabled = true;
            this.or_venueList.Location = new System.Drawing.Point(3, 87);
            this.or_venueList.Name = "or_venueList";
            this.or_venueList.Size = new System.Drawing.Size(121, 21);
            this.or_venueList.TabIndex = 3;
            // 
            // or_venue
            // 
            this.or_venue.AutoSize = true;
            this.or_venue.Location = new System.Drawing.Point(3, 64);
            this.or_venue.Name = "or_venue";
            this.or_venue.Size = new System.Drawing.Size(100, 17);
            this.or_venue.TabIndex = 1;
            this.or_venue.Text = "Override Venue";
            this.or_venue.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            this.tabPage5.BackColor = System.Drawing.Color.Transparent;
            this.tabPage5.Controls.Add(this.panel7);
            this.tabPage5.Controls.Add(this.panel2);
            this.tabPage5.Controls.Add(this.label8);
            this.tabPage5.Controls.Add(this.label7);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(833, 289);
            this.tabPage5.TabIndex = 1;
            this.tabPage5.Text = "Match Actions";
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.White;
            this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel7.Controls.Add(this.ma_forceTag);
            this.panel7.Controls.Add(this.ma_aiCall);
            this.panel7.Location = new System.Drawing.Point(6, 33);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(221, 106);
            this.panel7.TabIndex = 7;
            // 
            // ma_forceTag
            // 
            this.ma_forceTag.AutoSize = true;
            this.ma_forceTag.Checked = true;
            this.ma_forceTag.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ma_forceTag.Location = new System.Drawing.Point(17, 12);
            this.ma_forceTag.Name = "ma_forceTag";
            this.ma_forceTag.Size = new System.Drawing.Size(158, 17);
            this.ma_forceTag.TabIndex = 0;
            this.ma_forceTag.Text = "Force partner to tag if called";
            this.ma_forceTag.UseVisualStyleBackColor = true;
            // 
            // ma_aiCall
            // 
            this.ma_aiCall.AutoSize = true;
            this.ma_aiCall.Location = new System.Drawing.Point(17, 32);
            this.ma_aiCall.Name = "ma_aiCall";
            this.ma_aiCall.Size = new System.Drawing.Size(98, 17);
            this.ma_aiCall.TabIndex = 6;
            this.ma_aiCall.Text = "AI calls for tags";
            this.ma_aiCall.UseVisualStyleBackColor = true;
            this.ma_aiCall.Visible = false;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.refBum_minLogic);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.btn_bumpRef);
            this.panel2.Controls.Add(this.ma_convertSeconds);
            this.panel2.Location = new System.Drawing.Point(260, 33);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(258, 106);
            this.panel2.TabIndex = 5;
            // 
            // refBum_minLogic
            // 
            this.refBum_minLogic.Location = new System.Drawing.Point(99, 48);
            this.refBum_minLogic.Name = "refBum_minLogic";
            this.refBum_minLogic.Size = new System.Drawing.Size(50, 20);
            this.refBum_minLogic.TabIndex = 1;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(96, 32);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "RNG Ceiling";
            // 
            // btn_bumpRef
            // 
            this.btn_bumpRef.Enabled = false;
            this.btn_bumpRef.Location = new System.Drawing.Point(86, 83);
            this.btn_bumpRef.Name = "btn_bumpRef";
            this.btn_bumpRef.Size = new System.Drawing.Size(75, 23);
            this.btn_bumpRef.TabIndex = 4;
            this.btn_bumpRef.Text = "Bump Ref";
            this.btn_bumpRef.UseVisualStyleBackColor = true;
            this.btn_bumpRef.Visible = false;
            this.btn_bumpRef.Click += new System.EventHandler(this.btn_bumpRef_Click);
            // 
            // ma_convertSeconds
            // 
            this.ma_convertSeconds.AutoSize = true;
            this.ma_convertSeconds.Checked = true;
            this.ma_convertSeconds.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ma_convertSeconds.Location = new System.Drawing.Point(13, 12);
            this.ma_convertSeconds.Name = "ma_convertSeconds";
            this.ma_convertSeconds.Size = new System.Drawing.Size(239, 17);
            this.ma_convertSeconds.TabIndex = 1;
            this.ma_convertSeconds.Text = "If referee is down, seconds can enter the ring";
            this.ma_convertSeconds.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(54, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 21);
            this.label8.TabIndex = 3;
            this.label8.Text = "Wrestlers";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(347, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(73, 21);
            this.label7.TabIndex = 2;
            this.label7.Text = "Seconds";
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
            // ij_recoveryDate
            // 
            this.ij_recoveryDate.Enabled = false;
            this.ij_recoveryDate.Location = new System.Drawing.Point(158, 139);
            this.ij_recoveryDate.Name = "ij_recoveryDate";
            this.ij_recoveryDate.Size = new System.Drawing.Size(126, 20);
            this.ij_recoveryDate.TabIndex = 23;
            // 
            // QoL_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(866, 351);
            this.Controls.Add(this.ego_MainTabs);
            this.Controls.Add(this.cb_comment);
            this.Name = "QoL_Form";
            this.Text = "Extra Game Options";
            this.Load += new System.EventHandler(this.QoL_Form_Load);
            this.ego_MainTabs.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ql_Controls.ResumeLayout(false);
            this.ql_WrestlerEdit.ResumeLayout(false);
            this.ql_WrestlerEdit.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ql_RefereeEdit.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.tabPage6.ResumeLayout(false);
            this.tabPage6.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.overrideTabs.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        
        #endregion
        public System.Windows.Forms.CheckBox cb_comment;
        private System.Windows.Forms.TabControl ego_MainTabs;
        private System.Windows.Forms.TabPage tabPage1;
        public System.Windows.Forms.TabControl ql_Controls;
        public System.Windows.Forms.TabPage ql_WrestlerEdit;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button we_refresh;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_mainMenu;
        public System.Windows.Forms.ComboBox we_resultList;
        public System.Windows.Forms.Button we_search;
        public System.Windows.Forms.ComboBox we_promotionBox;
        public System.Windows.Forms.Button we_edit;
        public System.Windows.Forms.TextBox we_searchBox;
        private System.Windows.Forms.CheckBox we_unsubscribe;
        private System.Windows.Forms.TabPage ql_RefereeEdit;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button refEdit;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox we_refList;
        private System.Windows.Forms.Button re_refresh;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabControl overrideTabs;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        public System.Windows.Forms.CheckBox ma_convertSeconds;
        private System.Windows.Forms.Button btn_bumpRef;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label9;
        public System.Windows.Forms.DomainUpDown refBum_minLogic;
        public System.Windows.Forms.CheckBox ma_forceTag;
        private System.Windows.Forms.CheckBox ma_aiCall;
        private System.Windows.Forms.Panel panel3;
        public System.Windows.Forms.CheckBox or_default;
        public System.Windows.Forms.ComboBox or_venueList;
        public System.Windows.Forms.ComboBox or_ringList;
        public System.Windows.Forms.CheckBox or_venue;
        public System.Windows.Forms.CheckBox or_ring;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox ij_wrestlerSearch;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox ij_result;
        private System.Windows.Forms.Label label10;
        public System.Windows.Forms.ListBox ij_wrestlerList;
        private System.Windows.Forms.Button ij_removeAll;
        private System.Windows.Forms.Button ij_add;
        private System.Windows.Forms.Button ij_removeOne;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button ij_load;
        private System.Windows.Forms.Button ij_save;
        public System.Windows.Forms.CheckBox em_injuries;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        public System.Windows.Forms.ComboBox ij_bodyHP;
        public System.Windows.Forms.ComboBox ij_armHP;
        public System.Windows.Forms.ComboBox ij_legHP;
        public System.Windows.Forms.ComboBox ij_neckHP;
        public System.Windows.Forms.CheckBox cb_injuryModData;
        public System.Windows.Forms.TextBox ij_recoveryRate;
        public System.Windows.Forms.TextBox ij_matches;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox ij_recoveryDate;
    }
}