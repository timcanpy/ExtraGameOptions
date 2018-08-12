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
            this.ql_WrestlerEdit = new System.Windows.Forms.TabPage();
            this.we_unsubscribe = new System.Windows.Forms.CheckBox();
            this.we_refresh = new System.Windows.Forms.Button();
            this.btn_mainMenu = new System.Windows.Forms.Button();
            this.we_search = new System.Windows.Forms.Button();
            this.we_edit = new System.Windows.Forms.Button();
            this.we_searchBox = new System.Windows.Forms.TextBox();
            this.we_promotionBox = new System.Windows.Forms.ComboBox();
            this.we_resultList = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ql_Controls = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.cb_comment = new System.Windows.Forms.CheckBox();
            this.ql_WrestlerEdit.SuspendLayout();
            this.ql_Controls.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ql_WrestlerEdit
            // 
            this.ql_WrestlerEdit.Controls.Add(this.we_unsubscribe);
            this.ql_WrestlerEdit.Controls.Add(this.we_refresh);
            this.ql_WrestlerEdit.Controls.Add(this.btn_mainMenu);
            this.ql_WrestlerEdit.Controls.Add(this.we_search);
            this.ql_WrestlerEdit.Controls.Add(this.we_edit);
            this.ql_WrestlerEdit.Controls.Add(this.we_searchBox);
            this.ql_WrestlerEdit.Controls.Add(this.we_promotionBox);
            this.ql_WrestlerEdit.Controls.Add(this.we_resultList);
            this.ql_WrestlerEdit.Controls.Add(this.label2);
            this.ql_WrestlerEdit.Controls.Add(this.label3);
            this.ql_WrestlerEdit.Controls.Add(this.label1);
            this.ql_WrestlerEdit.Location = new System.Drawing.Point(4, 22);
            this.ql_WrestlerEdit.Name = "ql_WrestlerEdit";
            this.ql_WrestlerEdit.Padding = new System.Windows.Forms.Padding(3);
            this.ql_WrestlerEdit.Size = new System.Drawing.Size(501, 194);
            this.ql_WrestlerEdit.TabIndex = 0;
            this.ql_WrestlerEdit.Text = "Wrestler Edit";
            this.ql_WrestlerEdit.UseVisualStyleBackColor = true;
            // 
            // we_unsubscribe
            // 
            this.we_unsubscribe.AutoSize = true;
            this.we_unsubscribe.Location = new System.Drawing.Point(3, 28);
            this.we_unsubscribe.Name = "we_unsubscribe";
            this.we_unsubscribe.Size = new System.Drawing.Size(85, 17);
            this.we_unsubscribe.TabIndex = 15;
            this.we_unsubscribe.Text = "Unsubscribe";
            this.we_unsubscribe.UseVisualStyleBackColor = true;
            // 
            // we_refresh
            // 
            this.we_refresh.Location = new System.Drawing.Point(269, 165);
            this.we_refresh.Name = "we_refresh";
            this.we_refresh.Size = new System.Drawing.Size(99, 23);
            this.we_refresh.TabIndex = 14;
            this.we_refresh.Text = "Refresh Lists";
            this.we_refresh.UseVisualStyleBackColor = true;
            this.we_refresh.Click += new System.EventHandler(this.we_refresh_Click);
            // 
            // btn_mainMenu
            // 
            this.btn_mainMenu.Location = new System.Drawing.Point(382, 165);
            this.btn_mainMenu.Name = "btn_mainMenu";
            this.btn_mainMenu.Size = new System.Drawing.Size(87, 23);
            this.btn_mainMenu.TabIndex = 13;
            this.btn_mainMenu.Text = "Main Menu";
            this.btn_mainMenu.UseVisualStyleBackColor = true;
            this.btn_mainMenu.Click += new System.EventHandler(this.btn_mainMenu_Click);
            // 
            // we_search
            // 
            this.we_search.Location = new System.Drawing.Point(97, 165);
            this.we_search.Name = "we_search";
            this.we_search.Size = new System.Drawing.Size(75, 23);
            this.we_search.TabIndex = 12;
            this.we_search.Text = "Search";
            this.we_search.UseVisualStyleBackColor = true;
            this.we_search.Click += new System.EventHandler(this.we_search_Click);
            // 
            // we_edit
            // 
            this.we_edit.Location = new System.Drawing.Point(178, 165);
            this.we_edit.Name = "we_edit";
            this.we_edit.Size = new System.Drawing.Size(75, 23);
            this.we_edit.TabIndex = 0;
            this.we_edit.Text = "Edit";
            this.we_edit.UseVisualStyleBackColor = true;
            this.we_edit.Click += new System.EventHandler(this.we_edit_Click);
            // 
            // we_searchBox
            // 
            this.we_searchBox.Location = new System.Drawing.Point(109, 28);
            this.we_searchBox.Name = "we_searchBox";
            this.we_searchBox.Size = new System.Drawing.Size(360, 20);
            this.we_searchBox.TabIndex = 11;
            // 
            // we_promotionBox
            // 
            this.we_promotionBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.we_promotionBox.FormattingEnabled = true;
            this.we_promotionBox.Location = new System.Drawing.Point(109, 78);
            this.we_promotionBox.Name = "we_promotionBox";
            this.we_promotionBox.Size = new System.Drawing.Size(360, 21);
            this.we_promotionBox.Sorted = true;
            this.we_promotionBox.TabIndex = 10;
            this.we_promotionBox.SelectedIndexChanged += new System.EventHandler(this.we_promotionBox_SelectedIndexChanged);
            // 
            // we_resultList
            // 
            this.we_resultList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.we_resultList.FormattingEnabled = true;
            this.we_resultList.Location = new System.Drawing.Point(109, 128);
            this.we_resultList.Name = "we_resultList";
            this.we_resultList.Size = new System.Drawing.Size(360, 21);
            this.we_resultList.Sorted = true;
            this.we_resultList.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(228, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Promotion List";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(228, 112);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Search Results";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(222, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Wrestler Search";
            // 
            // ql_Controls
            // 
            this.ql_Controls.Controls.Add(this.ql_WrestlerEdit);
            this.ql_Controls.Controls.Add(this.tabPage1);
            this.ql_Controls.Location = new System.Drawing.Point(12, 19);
            this.ql_Controls.Name = "ql_Controls";
            this.ql_Controls.SelectedIndex = 0;
            this.ql_Controls.Size = new System.Drawing.Size(509, 220);
            this.ql_Controls.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.cb_comment);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(501, 194);
            this.tabPage1.TabIndex = 1;
            this.tabPage1.Text = "Commentary";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // cb_comment
            // 
            this.cb_comment.AutoSize = true;
            this.cb_comment.Location = new System.Drawing.Point(3, 15);
            this.cb_comment.Name = "cb_comment";
            this.cb_comment.Size = new System.Drawing.Size(148, 17);
            this.cb_comment.TabIndex = 0;
            this.cb_comment.Text = "Track Crowd Excitement?";
            this.cb_comment.UseVisualStyleBackColor = true;
            // 
            // QoL_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(533, 251);
            this.Controls.Add(this.ql_Controls);
            this.Name = "QoL_Form";
            this.Text = "Extra Game Options";
            this.Load += new System.EventHandler(this.QoL_Form_Load_1);
            this.ql_WrestlerEdit.ResumeLayout(false);
            this.ql_WrestlerEdit.PerformLayout();
            this.ql_Controls.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.TabPage ql_WrestlerEdit;
        public System.Windows.Forms.Button we_edit;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TabControl ql_Controls;
        public System.Windows.Forms.Button we_search;
        public System.Windows.Forms.TextBox we_searchBox;
        public System.Windows.Forms.ComboBox we_promotionBox;
        public System.Windows.Forms.ComboBox we_resultList;
        private System.Windows.Forms.TabPage tabPage1;
        public System.Windows.Forms.CheckBox cb_comment;
        private System.Windows.Forms.Button btn_mainMenu;
        private System.Windows.Forms.Button we_refresh;
        private System.Windows.Forms.CheckBox we_unsubscribe;
    }
}