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
            this.ego_MainTabs.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.ql_Controls.SuspendLayout();
            this.ql_WrestlerEdit.SuspendLayout();
            this.panel1.SuspendLayout();
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
            this.label1.Text = "WrestlerSearch";
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
            // QoL_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(866, 351);
            this.Controls.Add(this.ego_MainTabs);
            this.Controls.Add(this.cb_comment);
            this.Name = "QoL_Form";
            this.Text = "WrestlerSearch";
            this.Load += new System.EventHandler(this.QoL_Form_Load);
            this.ego_MainTabs.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ql_Controls.ResumeLayout(false);
            this.ql_WrestlerEdit.ResumeLayout(false);
            this.ql_WrestlerEdit.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
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
    }
}