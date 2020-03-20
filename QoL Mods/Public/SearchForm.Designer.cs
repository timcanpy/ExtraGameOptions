using System;

namespace QoL_Mods
{
    partial class SearchForm
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
            this.ql_Controls = new System.Windows.Forms.TabControl();
            this.ql_WrestlerEdit = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.searchWiki = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.ws_promotionLbl = new System.Windows.Forms.Label();
            this.we_refresh = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.we_resultList = new System.Windows.Forms.ComboBox();
            this.we_promotionBox = new System.Windows.Forms.ComboBox();
            this.we_edit = new System.Windows.Forms.Button();
            this.we_searchBox = new System.Windows.Forms.TextBox();
            this.ql_RefereeEdit = new System.Windows.Forms.TabPage();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.refEdit = new System.Windows.Forms.Button();
            this.re_refresh = new System.Windows.Forms.Button();
            this.we_refList = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ql_Controls.SuspendLayout();
            this.ql_WrestlerEdit.SuspendLayout();
            this.panel1.SuspendLayout();
            this.ql_RefereeEdit.SuspendLayout();
            this.panel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // ql_Controls
            // 
            this.ql_Controls.Controls.Add(this.ql_WrestlerEdit);
            this.ql_Controls.Controls.Add(this.ql_RefereeEdit);
            this.ql_Controls.Location = new System.Drawing.Point(12, 12);
            this.ql_Controls.Name = "ql_Controls";
            this.ql_Controls.SelectedIndex = 0;
            this.ql_Controls.Size = new System.Drawing.Size(569, 337);
            this.ql_Controls.TabIndex = 1;
            // 
            // ql_WrestlerEdit
            // 
            this.ql_WrestlerEdit.BackColor = System.Drawing.Color.Transparent;
            this.ql_WrestlerEdit.Controls.Add(this.panel1);
            this.ql_WrestlerEdit.Location = new System.Drawing.Point(4, 22);
            this.ql_WrestlerEdit.Name = "ql_WrestlerEdit";
            this.ql_WrestlerEdit.Padding = new System.Windows.Forms.Padding(3);
            this.ql_WrestlerEdit.Size = new System.Drawing.Size(561, 311);
            this.ql_WrestlerEdit.TabIndex = 0;
            this.ql_WrestlerEdit.Text = "Wrestlers";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.searchWiki);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.ws_promotionLbl);
            this.panel1.Controls.Add(this.we_refresh);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.we_resultList);
            this.panel1.Controls.Add(this.we_promotionBox);
            this.panel1.Controls.Add(this.we_edit);
            this.panel1.Controls.Add(this.we_searchBox);
            this.panel1.Location = new System.Drawing.Point(6, 38);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(543, 214);
            this.panel1.TabIndex = 16;
            // 
            // searchWiki
            // 
            this.searchWiki.AutoSize = true;
            this.searchWiki.Location = new System.Drawing.Point(450, 141);
            this.searchWiki.Name = "searchWiki";
            this.searchWiki.Size = new System.Drawing.Size(54, 13);
            this.searchWiki.TabIndex = 19;
            this.searchWiki.TabStop = true;
            this.searchWiki.Text = "Wikipedia";
            this.searchWiki.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.searchWiki_LinkClicked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(200, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Wrestler Search";
            // 
            // ws_promotionLbl
            // 
            this.ws_promotionLbl.AutoSize = true;
            this.ws_promotionLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ws_promotionLbl.ForeColor = System.Drawing.Color.DarkGreen;
            this.ws_promotionLbl.Location = new System.Drawing.Point(99, 112);
            this.ws_promotionLbl.Name = "ws_promotionLbl";
            this.ws_promotionLbl.Size = new System.Drawing.Size(93, 13);
            this.ws_promotionLbl.TabIndex = 7;
            this.ws_promotionLbl.Text = "Search Results";
            // 
            // we_refresh
            // 
            this.we_refresh.Location = new System.Drawing.Point(345, 170);
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
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(205, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Promotion List";
            // 
            // we_resultList
            // 
            this.we_resultList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.we_resultList.FormattingEnabled = true;
            this.we_resultList.Location = new System.Drawing.Point(84, 133);
            this.we_resultList.Name = "we_resultList";
            this.we_resultList.Size = new System.Drawing.Size(360, 21);
            this.we_resultList.Sorted = true;
            this.we_resultList.TabIndex = 9;
            this.we_resultList.SelectedIndexChanged += new System.EventHandler(this.we_resultList_SelectedIndexChanged);
            // 
            // we_promotionBox
            // 
            this.we_promotionBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.we_promotionBox.FormattingEnabled = true;
            this.we_promotionBox.Location = new System.Drawing.Point(84, 83);
            this.we_promotionBox.Name = "we_promotionBox";
            this.we_promotionBox.Size = new System.Drawing.Size(360, 21);
            this.we_promotionBox.Sorted = true;
            this.we_promotionBox.TabIndex = 10;
            this.we_promotionBox.SelectedIndexChanged += new System.EventHandler(this.we_promotionBox_SelectedIndexChanged);
            // 
            // we_edit
            // 
            this.we_edit.Location = new System.Drawing.Point(84, 170);
            this.we_edit.Name = "we_edit";
            this.we_edit.Size = new System.Drawing.Size(75, 23);
            this.we_edit.TabIndex = 0;
            this.we_edit.Text = "Edit";
            this.we_edit.UseVisualStyleBackColor = true;
            this.we_edit.Click += new System.EventHandler(this.we_edit_Click);
            // 
            // we_searchBox
            // 
            this.we_searchBox.Location = new System.Drawing.Point(84, 33);
            this.we_searchBox.Name = "we_searchBox";
            this.we_searchBox.Size = new System.Drawing.Size(360, 20);
            this.we_searchBox.TabIndex = 11;
            // 
            // ql_RefereeEdit
            // 
            this.ql_RefereeEdit.BackColor = System.Drawing.SystemColors.Control;
            this.ql_RefereeEdit.Controls.Add(this.label3);
            this.ql_RefereeEdit.Controls.Add(this.panel6);
            this.ql_RefereeEdit.Location = new System.Drawing.Point(4, 22);
            this.ql_RefereeEdit.Name = "ql_RefereeEdit";
            this.ql_RefereeEdit.Size = new System.Drawing.Size(561, 311);
            this.ql_RefereeEdit.TabIndex = 1;
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
            this.panel6.Location = new System.Drawing.Point(103, 68);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(353, 164);
            this.panel6.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Franklin Gothic Medium", 18F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(99, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(151, 30);
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
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Franklin Gothic Medium", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(35, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(506, 21);
            this.label3.TabIndex = 7;
            this.label3.Text = "Changes to Referee Automatically Save When Exiting Edit Menu";
            // 
            // SearchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(589, 351);
            this.Controls.Add(this.ql_Controls);
            this.Name = "SearchForm";
            this.Text = "Edit Search";
            this.Load += new System.EventHandler(this.QoL_Form_Load);
            this.ql_Controls.ResumeLayout(false);
            this.ql_WrestlerEdit.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ql_RefereeEdit.ResumeLayout(false);
            this.ql_RefereeEdit.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.TabControl ql_Controls;
        public System.Windows.Forms.TabPage ql_WrestlerEdit;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label ws_promotionLbl;
        private System.Windows.Forms.Button we_refresh;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.ComboBox we_resultList;
        public System.Windows.Forms.ComboBox we_promotionBox;
        public System.Windows.Forms.Button we_edit;
        public System.Windows.Forms.TextBox we_searchBox;
        private System.Windows.Forms.TabPage ql_RefereeEdit;
        private System.Windows.Forms.LinkLabel searchWiki;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button refEdit;
        private System.Windows.Forms.Button re_refresh;
        private System.Windows.Forms.ComboBox we_refList;
        private System.Windows.Forms.Label label3;
    }
}