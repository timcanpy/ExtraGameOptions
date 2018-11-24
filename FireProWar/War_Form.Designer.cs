namespace FireProWar
{
    partial class War_Form
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.fpw_addPromotion = new System.Windows.Forms.Button();
            this.fpw_deletePromotion = new System.Windows.Forms.Button();
            this.fpw_updatePromotion = new System.Windows.Forms.Button();
            this.label45 = new System.Windows.Forms.Label();
            this.fpw_promoRingList = new System.Windows.Forms.ComboBox();
            this.label44 = new System.Windows.Forms.Label();
            this.fpw_promoRegionList = new System.Windows.Forms.ComboBox();
            this.label43 = new System.Windows.Forms.Label();
            this.fpw_promoStyleList = new System.Windows.Forms.ComboBox();
            this.fpw_promoName = new System.Windows.Forms.TextBox();
            this.label39 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.fpw_promoList = new System.Windows.Forms.ComboBox();
            this.label46 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.fpw_Enable = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.fpw_infoTabs = new System.Windows.Forms.TabControl();
            this.fpw_history = new System.Windows.Forms.TabPage();
            this.fpw_promoClearHistory = new System.Windows.Forms.Button();
            this.fpw_promoMthRating = new System.Windows.Forms.Label();
            this.fpw_promoMthCnt = new System.Windows.Forms.Label();
            this.fpw_promoHistory = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.fpw_management = new System.Windows.Forms.TabPage();
            this.label9 = new System.Windows.Forms.Label();
            this.ms_fireAll = new System.Windows.Forms.Button();
            this.ms_fireOne = new System.Windows.Forms.Button();
            this.ms_employeeList = new System.Windows.Forms.ListBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.ms_hireGroup = new System.Windows.Forms.Button();
            this.ms_groupList = new System.Windows.Forms.ComboBox();
            this.ms_refreshList = new System.Windows.Forms.Button();
            this.ms_hireWrestler = new System.Windows.Forms.Button();
            this.ms_searchResults = new System.Windows.Forms.ComboBox();
            this.ms_wrestlerSearch = new System.Windows.Forms.TextBox();
            this.fpw_details = new System.Windows.Forms.TabPage();
            this.resetPoints = new System.Windows.Forms.Button();
            this.ms_moralePoints = new System.Windows.Forms.Label();
            this.ms_empRecord = new System.Windows.Forms.Label();
            this.ms_empRating = new System.Windows.Forms.Label();
            this.ms_empMatches = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.ms_moraleRank = new System.Windows.Forms.ComboBox();
            this.ms_employeeCountry = new System.Windows.Forms.TextBox();
            this.ms_employeeStyle = new System.Windows.Forms.TextBox();
            this.ms_employeeName = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.ms_rosterList = new System.Windows.Forms.ListBox();
            this.btn_SaveData = new System.Windows.Forms.Button();
            this.btn_LoadData = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.fpw_infoTabs.SuspendLayout();
            this.fpw_history.SuspendLayout();
            this.fpw_management.SuspendLayout();
            this.fpw_details.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.fpw_addPromotion);
            this.panel1.Controls.Add(this.fpw_deletePromotion);
            this.panel1.Controls.Add(this.fpw_updatePromotion);
            this.panel1.Controls.Add(this.label45);
            this.panel1.Controls.Add(this.fpw_promoRingList);
            this.panel1.Controls.Add(this.label44);
            this.panel1.Controls.Add(this.fpw_promoRegionList);
            this.panel1.Controls.Add(this.label43);
            this.panel1.Controls.Add(this.fpw_promoStyleList);
            this.panel1.Controls.Add(this.fpw_promoName);
            this.panel1.Controls.Add(this.label39);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.fpw_promoList);
            this.panel1.Controls.Add(this.label46);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.fpw_Enable);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(225, 426);
            this.panel1.TabIndex = 0;
            // 
            // fpw_addPromotion
            // 
            this.fpw_addPromotion.Location = new System.Drawing.Point(52, 341);
            this.fpw_addPromotion.Name = "fpw_addPromotion";
            this.fpw_addPromotion.Size = new System.Drawing.Size(109, 23);
            this.fpw_addPromotion.TabIndex = 41;
            this.fpw_addPromotion.Text = "Add Promotion";
            this.fpw_addPromotion.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.fpw_addPromotion.UseVisualStyleBackColor = true;
            this.fpw_addPromotion.Click += new System.EventHandler(this.fpw_addPromotion_Click);
            // 
            // fpw_deletePromotion
            // 
            this.fpw_deletePromotion.Location = new System.Drawing.Point(52, 396);
            this.fpw_deletePromotion.Name = "fpw_deletePromotion";
            this.fpw_deletePromotion.Size = new System.Drawing.Size(109, 23);
            this.fpw_deletePromotion.TabIndex = 40;
            this.fpw_deletePromotion.Text = "Delete Promotion";
            this.fpw_deletePromotion.UseVisualStyleBackColor = true;
            this.fpw_deletePromotion.Click += new System.EventHandler(this.fpw_deletePromotion_Click);
            // 
            // fpw_updatePromotion
            // 
            this.fpw_updatePromotion.Location = new System.Drawing.Point(52, 370);
            this.fpw_updatePromotion.Name = "fpw_updatePromotion";
            this.fpw_updatePromotion.Size = new System.Drawing.Size(109, 23);
            this.fpw_updatePromotion.TabIndex = 39;
            this.fpw_updatePromotion.Text = "Update Promotion";
            this.fpw_updatePromotion.UseVisualStyleBackColor = true;
            this.fpw_updatePromotion.Click += new System.EventHandler(this.fpw_updatePromotion_Click);
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label45.Location = new System.Drawing.Point(78, 296);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(69, 13);
            this.label45.TabIndex = 38;
            this.label45.Text = "Home Ring";
            // 
            // fpw_promoRingList
            // 
            this.fpw_promoRingList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fpw_promoRingList.FormattingEnabled = true;
            this.fpw_promoRingList.Location = new System.Drawing.Point(7, 314);
            this.fpw_promoRingList.Name = "fpw_promoRingList";
            this.fpw_promoRingList.Size = new System.Drawing.Size(203, 21);
            this.fpw_promoRingList.Sorted = true;
            this.fpw_promoRingList.TabIndex = 37;
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label44.Location = new System.Drawing.Point(75, 254);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(83, 13);
            this.label44.TabIndex = 36;
            this.label44.Text = "Home Region";
            // 
            // fpw_promoRegionList
            // 
            this.fpw_promoRegionList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fpw_promoRegionList.FormattingEnabled = true;
            this.fpw_promoRegionList.Location = new System.Drawing.Point(7, 271);
            this.fpw_promoRegionList.Name = "fpw_promoRegionList";
            this.fpw_promoRegionList.Size = new System.Drawing.Size(203, 21);
            this.fpw_promoRegionList.Sorted = true;
            this.fpw_promoRegionList.TabIndex = 35;
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label43.Location = new System.Drawing.Point(77, 204);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(67, 13);
            this.label43.TabIndex = 34;
            this.label43.Text = "Fight Style";
            // 
            // fpw_promoStyleList
            // 
            this.fpw_promoStyleList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fpw_promoStyleList.FormattingEnabled = true;
            this.fpw_promoStyleList.Location = new System.Drawing.Point(7, 221);
            this.fpw_promoStyleList.Name = "fpw_promoStyleList";
            this.fpw_promoStyleList.Size = new System.Drawing.Size(203, 21);
            this.fpw_promoStyleList.Sorted = true;
            this.fpw_promoStyleList.TabIndex = 33;
            // 
            // fpw_promoName
            // 
            this.fpw_promoName.Location = new System.Drawing.Point(7, 178);
            this.fpw_promoName.Name = "fpw_promoName";
            this.fpw_promoName.Size = new System.Drawing.Size(203, 20);
            this.fpw_promoName.TabIndex = 32;
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label39.Location = new System.Drawing.Point(95, 160);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(39, 13);
            this.label39.TabIndex = 31;
            this.label39.Text = "Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(87, 132);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 16);
            this.label2.TabIndex = 30;
            this.label2.Text = "Details";
            // 
            // fpw_promoList
            // 
            this.fpw_promoList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fpw_promoList.FormattingEnabled = true;
            this.fpw_promoList.Location = new System.Drawing.Point(7, 95);
            this.fpw_promoList.Name = "fpw_promoList";
            this.fpw_promoList.Size = new System.Drawing.Size(203, 21);
            this.fpw_promoList.Sorted = true;
            this.fpw_promoList.TabIndex = 29;
            this.fpw_promoList.SelectedIndexChanged += new System.EventHandler(this.fpw_promoList_SelectedIndexChanged);
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label46.Location = new System.Drawing.Point(75, 73);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(86, 16);
            this.label46.TabIndex = 28;
            this.label46.Text = "Promotions";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("NCAA Utah St Aggies Bold", 12F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(33, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(177, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Promotion Details";
            // 
            // fpw_Enable
            // 
            this.fpw_Enable.AutoSize = true;
            this.fpw_Enable.Location = new System.Drawing.Point(7, 9);
            this.fpw_Enable.Name = "fpw_Enable";
            this.fpw_Enable.Size = new System.Drawing.Size(86, 17);
            this.fpw_Enable.TabIndex = 0;
            this.fpw_Enable.Text = "Enable FPW";
            this.fpw_Enable.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Gainsboro;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.fpw_infoTabs);
            this.panel2.Location = new System.Drawing.Point(244, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(544, 426);
            this.panel2.TabIndex = 1;
            // 
            // fpw_infoTabs
            // 
            this.fpw_infoTabs.Controls.Add(this.fpw_history);
            this.fpw_infoTabs.Controls.Add(this.fpw_management);
            this.fpw_infoTabs.Controls.Add(this.fpw_details);
            this.fpw_infoTabs.Location = new System.Drawing.Point(3, 3);
            this.fpw_infoTabs.Name = "fpw_infoTabs";
            this.fpw_infoTabs.SelectedIndex = 0;
            this.fpw_infoTabs.Size = new System.Drawing.Size(534, 416);
            this.fpw_infoTabs.TabIndex = 0;
            // 
            // fpw_history
            // 
            this.fpw_history.BackColor = System.Drawing.Color.WhiteSmoke;
            this.fpw_history.Controls.Add(this.fpw_promoClearHistory);
            this.fpw_history.Controls.Add(this.fpw_promoMthRating);
            this.fpw_history.Controls.Add(this.fpw_promoMthCnt);
            this.fpw_history.Controls.Add(this.fpw_promoHistory);
            this.fpw_history.Controls.Add(this.label5);
            this.fpw_history.Controls.Add(this.label4);
            this.fpw_history.Controls.Add(this.label3);
            this.fpw_history.Location = new System.Drawing.Point(4, 22);
            this.fpw_history.Name = "fpw_history";
            this.fpw_history.Size = new System.Drawing.Size(526, 390);
            this.fpw_history.TabIndex = 2;
            this.fpw_history.Text = "Promotion History";
            // 
            // fpw_promoClearHistory
            // 
            this.fpw_promoClearHistory.Location = new System.Drawing.Point(192, 362);
            this.fpw_promoClearHistory.Name = "fpw_promoClearHistory";
            this.fpw_promoClearHistory.Size = new System.Drawing.Size(109, 23);
            this.fpw_promoClearHistory.TabIndex = 42;
            this.fpw_promoClearHistory.Text = "Clear History";
            this.fpw_promoClearHistory.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.fpw_promoClearHistory.UseVisualStyleBackColor = true;
            this.fpw_promoClearHistory.Click += new System.EventHandler(this.fpw_promoClearHistory_Click);
            // 
            // fpw_promoMthRating
            // 
            this.fpw_promoMthRating.AutoSize = true;
            this.fpw_promoMthRating.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fpw_promoMthRating.Location = new System.Drawing.Point(246, 88);
            this.fpw_promoMthRating.Name = "fpw_promoMthRating";
            this.fpw_promoMthRating.Size = new System.Drawing.Size(19, 20);
            this.fpw_promoMthRating.TabIndex = 47;
            this.fpw_promoMthRating.Text = "0";
            // 
            // fpw_promoMthCnt
            // 
            this.fpw_promoMthCnt.AutoSize = true;
            this.fpw_promoMthCnt.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fpw_promoMthCnt.Location = new System.Drawing.Point(248, 40);
            this.fpw_promoMthCnt.Name = "fpw_promoMthCnt";
            this.fpw_promoMthCnt.Size = new System.Drawing.Size(19, 20);
            this.fpw_promoMthCnt.TabIndex = 46;
            this.fpw_promoMthCnt.Text = "0";
            this.fpw_promoMthCnt.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // fpw_promoHistory
            // 
            this.fpw_promoHistory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.fpw_promoHistory.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fpw_promoHistory.Location = new System.Drawing.Point(12, 137);
            this.fpw_promoHistory.Multiline = true;
            this.fpw_promoHistory.Name = "fpw_promoHistory";
            this.fpw_promoHistory.ReadOnly = true;
            this.fpw_promoHistory.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.fpw_promoHistory.Size = new System.Drawing.Size(498, 221);
            this.fpw_promoHistory.TabIndex = 45;
            this.fpw_promoHistory.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("NCAA Utah St Aggies Bold", 12F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(189, 114);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(146, 16);
            this.label5.TabIndex = 44;
            this.label5.Text = "Roster History";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("NCAA Utah St Aggies Bold", 12F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(151, 65);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(221, 16);
            this.label4.TabIndex = 43;
            this.label4.Text = "Average Match Rating";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("NCAA Utah St Aggies Bold", 12F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(186, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(159, 16);
            this.label3.TabIndex = 42;
            this.label3.Text = "Matches Booked";
            // 
            // fpw_management
            // 
            this.fpw_management.BackColor = System.Drawing.Color.WhiteSmoke;
            this.fpw_management.Controls.Add(this.btn_LoadData);
            this.fpw_management.Controls.Add(this.btn_SaveData);
            this.fpw_management.Controls.Add(this.label9);
            this.fpw_management.Controls.Add(this.ms_fireAll);
            this.fpw_management.Controls.Add(this.ms_fireOne);
            this.fpw_management.Controls.Add(this.ms_employeeList);
            this.fpw_management.Controls.Add(this.label8);
            this.fpw_management.Controls.Add(this.label7);
            this.fpw_management.Controls.Add(this.label6);
            this.fpw_management.Controls.Add(this.ms_hireGroup);
            this.fpw_management.Controls.Add(this.ms_groupList);
            this.fpw_management.Controls.Add(this.ms_refreshList);
            this.fpw_management.Controls.Add(this.ms_hireWrestler);
            this.fpw_management.Controls.Add(this.ms_searchResults);
            this.fpw_management.Controls.Add(this.ms_wrestlerSearch);
            this.fpw_management.Location = new System.Drawing.Point(4, 22);
            this.fpw_management.Name = "fpw_management";
            this.fpw_management.Padding = new System.Windows.Forms.Padding(3);
            this.fpw_management.Size = new System.Drawing.Size(526, 390);
            this.fpw_management.TabIndex = 0;
            this.fpw_management.Text = "Roster Management";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(367, 17);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(82, 16);
            this.label9.TabIndex = 35;
            this.label9.Text = "Roster List";
            // 
            // ms_fireAll
            // 
            this.ms_fireAll.Location = new System.Drawing.Point(438, 345);
            this.ms_fireAll.Name = "ms_fireAll";
            this.ms_fireAll.Size = new System.Drawing.Size(82, 23);
            this.ms_fireAll.TabIndex = 34;
            this.ms_fireAll.Text = "Fire All";
            this.ms_fireAll.UseVisualStyleBackColor = true;
            this.ms_fireAll.Click += new System.EventHandler(this.ms_fireAll_Click);
            // 
            // ms_fireOne
            // 
            this.ms_fireOne.Location = new System.Drawing.Point(307, 345);
            this.ms_fireOne.Name = "ms_fireOne";
            this.ms_fireOne.Size = new System.Drawing.Size(89, 23);
            this.ms_fireOne.TabIndex = 33;
            this.ms_fireOne.Text = "Fire One";
            this.ms_fireOne.UseVisualStyleBackColor = true;
            this.ms_fireOne.Click += new System.EventHandler(this.ms_fireOne_Click);
            // 
            // ms_employeeList
            // 
            this.ms_employeeList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ms_employeeList.FormattingEnabled = true;
            this.ms_employeeList.Location = new System.Drawing.Point(307, 44);
            this.ms_employeeList.Name = "ms_employeeList";
            this.ms_employeeList.ScrollAlwaysVisible = true;
            this.ms_employeeList.Size = new System.Drawing.Size(213, 290);
            this.ms_employeeList.Sorted = true;
            this.ms_employeeList.TabIndex = 32;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(90, 132);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(113, 16);
            this.label8.TabIndex = 31;
            this.label8.Text = "Search Results";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(104, 74);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(78, 16);
            this.label7.TabIndex = 30;
            this.label7.Text = "Group List";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(90, 17);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(119, 16);
            this.label6.TabIndex = 29;
            this.label6.Text = "Wrestler Search";
            // 
            // ms_hireGroup
            // 
            this.ms_hireGroup.Location = new System.Drawing.Point(107, 227);
            this.ms_hireGroup.Name = "ms_hireGroup";
            this.ms_hireGroup.Size = new System.Drawing.Size(87, 23);
            this.ms_hireGroup.TabIndex = 23;
            this.ms_hireGroup.Text = "Hire Group";
            this.ms_hireGroup.UseVisualStyleBackColor = true;
            this.ms_hireGroup.Click += new System.EventHandler(this.ms_hireGroup_Click);
            // 
            // ms_groupList
            // 
            this.ms_groupList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ms_groupList.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ms_groupList.FormattingEnabled = true;
            this.ms_groupList.Location = new System.Drawing.Point(6, 94);
            this.ms_groupList.Name = "ms_groupList";
            this.ms_groupList.Size = new System.Drawing.Size(282, 21);
            this.ms_groupList.Sorted = true;
            this.ms_groupList.TabIndex = 22;
            this.ms_groupList.SelectedIndexChanged += new System.EventHandler(this.ms_groupList_SelectedIndexChanged);
            this.ms_groupList.LostFocus += new System.EventHandler(this.GroupList_LostFocus);
            // 
            // ms_refreshList
            // 
            this.ms_refreshList.Location = new System.Drawing.Point(107, 261);
            this.ms_refreshList.Name = "ms_refreshList";
            this.ms_refreshList.Size = new System.Drawing.Size(87, 23);
            this.ms_refreshList.TabIndex = 20;
            this.ms_refreshList.Text = "Refresh List";
            this.ms_refreshList.UseVisualStyleBackColor = true;
            this.ms_refreshList.Click += new System.EventHandler(this.ms_refreshList_Click);
            // 
            // ms_hireWrestler
            // 
            this.ms_hireWrestler.Location = new System.Drawing.Point(107, 194);
            this.ms_hireWrestler.Name = "ms_hireWrestler";
            this.ms_hireWrestler.Size = new System.Drawing.Size(87, 23);
            this.ms_hireWrestler.TabIndex = 19;
            this.ms_hireWrestler.Text = "Hire Wrestler";
            this.ms_hireWrestler.UseVisualStyleBackColor = true;
            this.ms_hireWrestler.Click += new System.EventHandler(this.ms_hireWrestler_Click);
            // 
            // ms_searchResults
            // 
            this.ms_searchResults.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ms_searchResults.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ms_searchResults.FormattingEnabled = true;
            this.ms_searchResults.Location = new System.Drawing.Point(6, 152);
            this.ms_searchResults.Name = "ms_searchResults";
            this.ms_searchResults.Size = new System.Drawing.Size(282, 21);
            this.ms_searchResults.Sorted = true;
            this.ms_searchResults.TabIndex = 18;
            // 
            // ms_wrestlerSearch
            // 
            this.ms_wrestlerSearch.Location = new System.Drawing.Point(6, 44);
            this.ms_wrestlerSearch.Name = "ms_wrestlerSearch";
            this.ms_wrestlerSearch.Size = new System.Drawing.Size(282, 20);
            this.ms_wrestlerSearch.TabIndex = 16;
            this.ms_wrestlerSearch.LostFocus += new System.EventHandler(this.WrestlerSearch_LostFocus);
            // 
            // fpw_details
            // 
            this.fpw_details.Controls.Add(this.resetPoints);
            this.fpw_details.Controls.Add(this.ms_moralePoints);
            this.fpw_details.Controls.Add(this.ms_empRecord);
            this.fpw_details.Controls.Add(this.ms_empRating);
            this.fpw_details.Controls.Add(this.ms_empMatches);
            this.fpw_details.Controls.Add(this.label18);
            this.fpw_details.Controls.Add(this.label17);
            this.fpw_details.Controls.Add(this.label16);
            this.fpw_details.Controls.Add(this.label15);
            this.fpw_details.Controls.Add(this.label14);
            this.fpw_details.Controls.Add(this.label13);
            this.fpw_details.Controls.Add(this.label12);
            this.fpw_details.Controls.Add(this.label11);
            this.fpw_details.Controls.Add(this.ms_moraleRank);
            this.fpw_details.Controls.Add(this.ms_employeeCountry);
            this.fpw_details.Controls.Add(this.ms_employeeStyle);
            this.fpw_details.Controls.Add(this.ms_employeeName);
            this.fpw_details.Controls.Add(this.label10);
            this.fpw_details.Controls.Add(this.ms_rosterList);
            this.fpw_details.Location = new System.Drawing.Point(4, 22);
            this.fpw_details.Name = "fpw_details";
            this.fpw_details.Padding = new System.Windows.Forms.Padding(3);
            this.fpw_details.Size = new System.Drawing.Size(526, 390);
            this.fpw_details.TabIndex = 1;
            this.fpw_details.Text = "Roster Details";
            this.fpw_details.UseVisualStyleBackColor = true;
            // 
            // resetPoints
            // 
            this.resetPoints.BackColor = System.Drawing.Color.Red;
            this.resetPoints.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.resetPoints.ForeColor = System.Drawing.Color.Red;
            this.resetPoints.Location = new System.Drawing.Point(479, 200);
            this.resetPoints.Name = "resetPoints";
            this.resetPoints.Size = new System.Drawing.Size(27, 23);
            this.resetPoints.TabIndex = 70;
            this.resetPoints.UseVisualStyleBackColor = false;
            this.resetPoints.Click += new System.EventHandler(this.resetPoints_Click);
            // 
            // ms_moralePoints
            // 
            this.ms_moralePoints.AutoSize = true;
            this.ms_moralePoints.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ms_moralePoints.Location = new System.Drawing.Point(444, 205);
            this.ms_moralePoints.Name = "ms_moralePoints";
            this.ms_moralePoints.Size = new System.Drawing.Size(16, 16);
            this.ms_moralePoints.TabIndex = 69;
            this.ms_moralePoints.Text = "0";
            // 
            // ms_empRecord
            // 
            this.ms_empRecord.AutoSize = true;
            this.ms_empRecord.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ms_empRecord.Location = new System.Drawing.Point(384, 347);
            this.ms_empRecord.Name = "ms_empRecord";
            this.ms_empRecord.Size = new System.Drawing.Size(16, 16);
            this.ms_empRecord.TabIndex = 68;
            this.ms_empRecord.Text = "0";
            // 
            // ms_empRating
            // 
            this.ms_empRating.AutoSize = true;
            this.ms_empRating.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ms_empRating.Location = new System.Drawing.Point(386, 304);
            this.ms_empRating.Name = "ms_empRating";
            this.ms_empRating.Size = new System.Drawing.Size(16, 16);
            this.ms_empRating.TabIndex = 67;
            this.ms_empRating.Text = "0";
            // 
            // ms_empMatches
            // 
            this.ms_empMatches.AutoSize = true;
            this.ms_empMatches.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ms_empMatches.Location = new System.Drawing.Point(395, 258);
            this.ms_empMatches.Name = "ms_empMatches";
            this.ms_empMatches.Size = new System.Drawing.Size(16, 16);
            this.ms_empMatches.TabIndex = 66;
            this.ms_empMatches.Text = "0";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(327, 323);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(167, 16);
            this.label18.TabIndex = 65;
            this.label18.Text = "Win/Loss/Draw Record";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(333, 280);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(161, 16);
            this.label17.TabIndex = 64;
            this.label17.Text = "Average Match Rating";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(354, 242);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(106, 16);
            this.label16.TabIndex = 63;
            this.label16.Text = "Total Matches";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(403, 181);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(103, 16);
            this.label15.TabIndex = 62;
            this.label15.Text = "Morale Points";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(312, 181);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(44, 16);
            this.label14.TabIndex = 61;
            this.label14.Text = "Rank";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(360, 122);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(107, 16);
            this.label13.TabIndex = 60;
            this.label13.Text = "Native Region";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(389, 69);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(43, 16);
            this.label12.TabIndex = 59;
            this.label12.Text = "Style";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(389, 16);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(49, 16);
            this.label11.TabIndex = 58;
            this.label11.Text = "Name";
            // 
            // ms_moraleRank
            // 
            this.ms_moraleRank.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ms_moraleRank.FormattingEnabled = true;
            this.ms_moraleRank.Location = new System.Drawing.Point(315, 202);
            this.ms_moraleRank.Name = "ms_moraleRank";
            this.ms_moraleRank.Size = new System.Drawing.Size(35, 21);
            this.ms_moraleRank.TabIndex = 47;
            this.ms_moraleRank.SelectedIndexChanged += new System.EventHandler(this.ms_moraleRank_SelectedIndexChanged);
            // 
            // ms_employeeCountry
            // 
            this.ms_employeeCountry.Location = new System.Drawing.Point(315, 153);
            this.ms_employeeCountry.Name = "ms_employeeCountry";
            this.ms_employeeCountry.ReadOnly = true;
            this.ms_employeeCountry.Size = new System.Drawing.Size(191, 20);
            this.ms_employeeCountry.TabIndex = 51;
            // 
            // ms_employeeStyle
            // 
            this.ms_employeeStyle.Location = new System.Drawing.Point(315, 89);
            this.ms_employeeStyle.Name = "ms_employeeStyle";
            this.ms_employeeStyle.ReadOnly = true;
            this.ms_employeeStyle.Size = new System.Drawing.Size(191, 20);
            this.ms_employeeStyle.TabIndex = 49;
            // 
            // ms_employeeName
            // 
            this.ms_employeeName.Location = new System.Drawing.Point(315, 36);
            this.ms_employeeName.Name = "ms_employeeName";
            this.ms_employeeName.ReadOnly = true;
            this.ms_employeeName.Size = new System.Drawing.Size(191, 20);
            this.ms_employeeName.TabIndex = 46;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(75, 17);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(82, 16);
            this.label10.TabIndex = 37;
            this.label10.Text = "Roster List";
            // 
            // ms_rosterList
            // 
            this.ms_rosterList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ms_rosterList.FormattingEnabled = true;
            this.ms_rosterList.Location = new System.Drawing.Point(15, 44);
            this.ms_rosterList.Name = "ms_rosterList";
            this.ms_rosterList.ScrollAlwaysVisible = true;
            this.ms_rosterList.Size = new System.Drawing.Size(213, 290);
            this.ms_rosterList.Sorted = true;
            this.ms_rosterList.TabIndex = 36;
            this.ms_rosterList.SelectedIndexChanged += new System.EventHandler(this.ms_rosterList_SelectedIndexChanged);
            // 
            // btn_SaveData
            // 
            this.btn_SaveData.Location = new System.Drawing.Point(6, 345);
            this.btn_SaveData.Name = "btn_SaveData";
            this.btn_SaveData.Size = new System.Drawing.Size(75, 23);
            this.btn_SaveData.TabIndex = 36;
            this.btn_SaveData.Text = "Save Data";
            this.btn_SaveData.UseVisualStyleBackColor = true;
            this.btn_SaveData.Click += new System.EventHandler(this.btn_SaveData_Click);
            // 
            // btn_LoadData
            // 
            this.btn_LoadData.Location = new System.Drawing.Point(213, 345);
            this.btn_LoadData.Name = "btn_LoadData";
            this.btn_LoadData.Size = new System.Drawing.Size(75, 23);
            this.btn_LoadData.TabIndex = 37;
            this.btn_LoadData.Text = "Load Data";
            this.btn_LoadData.UseVisualStyleBackColor = true;
            this.btn_LoadData.Click += new System.EventHandler(this.btn_LoadData_Click);
            // 
            // War_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Maroon;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "War_Form";
            this.Text = "Fire Promotion Wars";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.fpw_infoTabs.ResumeLayout(false);
            this.fpw_history.ResumeLayout(false);
            this.fpw_history.PerformLayout();
            this.fpw_management.ResumeLayout(false);
            this.fpw_management.PerformLayout();
            this.fpw_details.ResumeLayout(false);
            this.fpw_details.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox fpw_promoList;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.Label label45;
        public System.Windows.Forms.ComboBox fpw_promoRingList;
        private System.Windows.Forms.Label label44;
        public System.Windows.Forms.ComboBox fpw_promoRegionList;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.ComboBox fpw_promoStyleList;
        private System.Windows.Forms.TextBox fpw_promoName;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.Button fpw_addPromotion;
        private System.Windows.Forms.Button fpw_deletePromotion;
        private System.Windows.Forms.Button fpw_updatePromotion;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TabControl fpw_infoTabs;
        private System.Windows.Forms.TabPage fpw_history;
        private System.Windows.Forms.TabPage fpw_management;
        private System.Windows.Forms.TabPage fpw_details;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox fpw_promoHistory;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button ms_hireGroup;
        private System.Windows.Forms.ComboBox ms_groupList;
        private System.Windows.Forms.Button ms_refreshList;
        private System.Windows.Forms.Button ms_hireWrestler;
        private System.Windows.Forms.ComboBox ms_searchResults;
        private System.Windows.Forms.TextBox ms_wrestlerSearch;
        public System.Windows.Forms.ListBox ms_employeeList;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button ms_fireAll;
        private System.Windows.Forms.Button ms_fireOne;
        private System.Windows.Forms.Label label10;
        public System.Windows.Forms.ListBox ms_rosterList;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox ms_moraleRank;
        private System.Windows.Forms.TextBox ms_employeeCountry;
        private System.Windows.Forms.TextBox ms_employeeStyle;
        private System.Windows.Forms.TextBox ms_employeeName;
        private System.Windows.Forms.Label fpw_promoMthRating;
        private System.Windows.Forms.Label fpw_promoMthCnt;
        private System.Windows.Forms.Label ms_empRecord;
        private System.Windows.Forms.Label ms_empRating;
        private System.Windows.Forms.Label ms_empMatches;
        private System.Windows.Forms.Label ms_moralePoints;
        private System.Windows.Forms.Button fpw_promoClearHistory;
        public System.Windows.Forms.CheckBox fpw_Enable;
        private System.Windows.Forms.Button resetPoints;
        private System.Windows.Forms.Button btn_LoadData;
        private System.Windows.Forms.Button btn_SaveData;
    }
}