namespace QoL_Mods.Private
{
    partial class CustomReversalForm
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
            this.panel21 = new System.Windows.Forms.Panel();
            this.rev_save = new System.Windows.Forms.Button();
            this.rev_moveType = new System.Windows.Forms.ComboBox();
            this.label65 = new System.Windows.Forms.Label();
            this.rev_moveRefresh = new System.Windows.Forms.Button();
            this.rev_searchResults = new System.Windows.Forms.ComboBox();
            this.label66 = new System.Windows.Forms.Label();
            this.label67 = new System.Windows.Forms.Label();
            this.rev_moveSearch = new System.Windows.Forms.TextBox();
            this.panel22 = new System.Windows.Forms.Panel();
            this.rev_replacementType = new System.Windows.Forms.TextBox();
            this.rev_removeReplacement = new System.Windows.Forms.Button();
            this.rev_addReplacement = new System.Windows.Forms.Button();
            this.rev_replacementList = new System.Windows.Forms.ListBox();
            this.label71 = new System.Windows.Forms.Label();
            this.rev_executor = new System.Windows.Forms.ComboBox();
            this.label70 = new System.Windows.Forms.Label();
            this.label69 = new System.Windows.Forms.Label();
            this.rev_reversalRemove = new System.Windows.Forms.Button();
            this.rev_reversalAdd = new System.Windows.Forms.Button();
            this.rev_reversalList = new System.Windows.Forms.ListBox();
            this.label68 = new System.Windows.Forms.Label();
            this.panel21.SuspendLayout();
            this.panel22.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel21
            // 
            this.panel21.BackColor = System.Drawing.Color.White;
            this.panel21.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel21.Controls.Add(this.rev_save);
            this.panel21.Controls.Add(this.rev_moveType);
            this.panel21.Controls.Add(this.label65);
            this.panel21.Controls.Add(this.rev_moveRefresh);
            this.panel21.Controls.Add(this.rev_searchResults);
            this.panel21.Controls.Add(this.label66);
            this.panel21.Controls.Add(this.label67);
            this.panel21.Controls.Add(this.rev_moveSearch);
            this.panel21.Location = new System.Drawing.Point(12, 26);
            this.panel21.Name = "panel21";
            this.panel21.Size = new System.Drawing.Size(197, 264);
            this.panel21.TabIndex = 9;
            // 
            // rev_save
            // 
            this.rev_save.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rev_save.Location = new System.Drawing.Point(49, 219);
            this.rev_save.Name = "rev_save";
            this.rev_save.Size = new System.Drawing.Size(99, 23);
            this.rev_save.TabIndex = 18;
            this.rev_save.Text = "Save";
            this.rev_save.UseVisualStyleBackColor = true;
            this.rev_save.Click += new System.EventHandler(this.rev_save_Click);
            // 
            // rev_moveType
            // 
            this.rev_moveType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.rev_moveType.FormattingEnabled = true;
            this.rev_moveType.Location = new System.Drawing.Point(3, 80);
            this.rev_moveType.Name = "rev_moveType";
            this.rev_moveType.Size = new System.Drawing.Size(187, 21);
            this.rev_moveType.TabIndex = 17;
            this.rev_moveType.SelectedIndexChanged += new System.EventHandler(this.rev_moveType_SelectedIndexChanged);
            // 
            // label65
            // 
            this.label65.AutoSize = true;
            this.label65.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label65.Location = new System.Drawing.Point(65, 59);
            this.label65.Name = "label65";
            this.label65.Size = new System.Drawing.Size(67, 15);
            this.label65.TabIndex = 16;
            this.label65.Text = "Move Type";
            // 
            // rev_moveRefresh
            // 
            this.rev_moveRefresh.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rev_moveRefresh.Location = new System.Drawing.Point(49, 190);
            this.rev_moveRefresh.Name = "rev_moveRefresh";
            this.rev_moveRefresh.Size = new System.Drawing.Size(99, 23);
            this.rev_moveRefresh.TabIndex = 15;
            this.rev_moveRefresh.Text = "Refresh";
            this.rev_moveRefresh.UseVisualStyleBackColor = true;
            this.rev_moveRefresh.Click += new System.EventHandler(this.rev_moveRefresh_Click);
            // 
            // rev_searchResults
            // 
            this.rev_searchResults.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.rev_searchResults.FormattingEnabled = true;
            this.rev_searchResults.Location = new System.Drawing.Point(3, 155);
            this.rev_searchResults.Name = "rev_searchResults";
            this.rev_searchResults.Size = new System.Drawing.Size(187, 21);
            this.rev_searchResults.Sorted = true;
            this.rev_searchResults.TabIndex = 11;
            this.rev_searchResults.SelectedIndexChanged += new System.EventHandler(this.rev_searchResults_SelectedIndexChanged);
            // 
            // label66
            // 
            this.label66.AutoSize = true;
            this.label66.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label66.Location = new System.Drawing.Point(57, 135);
            this.label66.Name = "label66";
            this.label66.Size = new System.Drawing.Size(93, 15);
            this.label66.TabIndex = 6;
            this.label66.Text = "Search Results";
            // 
            // label67
            // 
            this.label67.AutoSize = true;
            this.label67.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label67.Location = new System.Drawing.Point(60, 7);
            this.label67.Name = "label67";
            this.label67.Size = new System.Drawing.Size(80, 15);
            this.label67.TabIndex = 5;
            this.label67.Text = "Move Search";
            // 
            // rev_moveSearch
            // 
            this.rev_moveSearch.Location = new System.Drawing.Point(3, 26);
            this.rev_moveSearch.Name = "rev_moveSearch";
            this.rev_moveSearch.Size = new System.Drawing.Size(187, 20);
            this.rev_moveSearch.TabIndex = 1;
            // 
            // panel22
            // 
            this.panel22.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel22.Controls.Add(this.rev_replacementType);
            this.panel22.Controls.Add(this.rev_removeReplacement);
            this.panel22.Controls.Add(this.rev_addReplacement);
            this.panel22.Controls.Add(this.rev_replacementList);
            this.panel22.Controls.Add(this.label71);
            this.panel22.Controls.Add(this.rev_executor);
            this.panel22.Controls.Add(this.label70);
            this.panel22.Controls.Add(this.label69);
            this.panel22.Controls.Add(this.rev_reversalRemove);
            this.panel22.Controls.Add(this.rev_reversalAdd);
            this.panel22.Controls.Add(this.rev_reversalList);
            this.panel22.Controls.Add(this.label68);
            this.panel22.Location = new System.Drawing.Point(216, 26);
            this.panel22.Name = "panel22";
            this.panel22.Size = new System.Drawing.Size(600, 264);
            this.panel22.TabIndex = 8;
            // 
            // rev_replacementType
            // 
            this.rev_replacementType.Location = new System.Drawing.Point(467, 137);
            this.rev_replacementType.Name = "rev_replacementType";
            this.rev_replacementType.ReadOnly = true;
            this.rev_replacementType.Size = new System.Drawing.Size(102, 20);
            this.rev_replacementType.TabIndex = 18;
            // 
            // rev_removeReplacement
            // 
            this.rev_removeReplacement.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.rev_removeReplacement.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rev_removeReplacement.Location = new System.Drawing.Point(375, 219);
            this.rev_removeReplacement.Name = "rev_removeReplacement";
            this.rev_removeReplacement.Size = new System.Drawing.Size(75, 23);
            this.rev_removeReplacement.TabIndex = 30;
            this.rev_removeReplacement.Text = "Remove";
            this.rev_removeReplacement.UseVisualStyleBackColor = false;
            this.rev_removeReplacement.Click += new System.EventHandler(this.rev_removeReplacement_Click);
            // 
            // rev_addReplacement
            // 
            this.rev_addReplacement.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.rev_addReplacement.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rev_addReplacement.Location = new System.Drawing.Point(252, 219);
            this.rev_addReplacement.Name = "rev_addReplacement";
            this.rev_addReplacement.Size = new System.Drawing.Size(75, 23);
            this.rev_addReplacement.TabIndex = 29;
            this.rev_addReplacement.Text = "Add";
            this.rev_addReplacement.UseVisualStyleBackColor = false;
            this.rev_addReplacement.Click += new System.EventHandler(this.rev_addReplacement_Click);
            // 
            // rev_replacementList
            // 
            this.rev_replacementList.FormattingEnabled = true;
            this.rev_replacementList.Location = new System.Drawing.Point(252, 27);
            this.rev_replacementList.Name = "rev_replacementList";
            this.rev_replacementList.ScrollAlwaysVisible = true;
            this.rev_replacementList.Size = new System.Drawing.Size(198, 186);
            this.rev_replacementList.TabIndex = 28;
            this.rev_replacementList.SelectedIndexChanged += new System.EventHandler(this.rev_replacementList_SelectedIndexChanged);
            // 
            // label71
            // 
            this.label71.AutoSize = true;
            this.label71.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label71.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label71.Location = new System.Drawing.Point(502, 118);
            this.label71.Name = "label71";
            this.label71.Size = new System.Drawing.Size(33, 15);
            this.label71.TabIndex = 27;
            this.label71.Text = "Type";
            // 
            // rev_executor
            // 
            this.rev_executor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.rev_executor.FormattingEnabled = true;
            this.rev_executor.Location = new System.Drawing.Point(467, 56);
            this.rev_executor.Name = "rev_executor";
            this.rev_executor.Size = new System.Drawing.Size(102, 21);
            this.rev_executor.TabIndex = 24;
            this.rev_executor.SelectedIndexChanged += new System.EventHandler(this.rev_executor_SelectedIndexChanged);
            // 
            // label70
            // 
            this.label70.AutoSize = true;
            this.label70.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label70.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label70.Location = new System.Drawing.Point(494, 36);
            this.label70.Name = "label70";
            this.label70.Size = new System.Drawing.Size(57, 15);
            this.label70.TabIndex = 26;
            this.label70.Text = "Executor";
            // 
            // label69
            // 
            this.label69.AutoSize = true;
            this.label69.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label69.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label69.Location = new System.Drawing.Point(309, 7);
            this.label69.Name = "label69";
            this.label69.Size = new System.Drawing.Size(107, 15);
            this.label69.TabIndex = 25;
            this.label69.Text = "Replacement List";
            // 
            // rev_reversalRemove
            // 
            this.rev_reversalRemove.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.rev_reversalRemove.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rev_reversalRemove.Location = new System.Drawing.Point(138, 219);
            this.rev_reversalRemove.Name = "rev_reversalRemove";
            this.rev_reversalRemove.Size = new System.Drawing.Size(75, 23);
            this.rev_reversalRemove.TabIndex = 21;
            this.rev_reversalRemove.Text = "Remove";
            this.rev_reversalRemove.UseVisualStyleBackColor = false;
            this.rev_reversalRemove.Click += new System.EventHandler(this.rev_reversalRemove_Click);
            // 
            // rev_reversalAdd
            // 
            this.rev_reversalAdd.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.rev_reversalAdd.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rev_reversalAdd.Location = new System.Drawing.Point(15, 219);
            this.rev_reversalAdd.Name = "rev_reversalAdd";
            this.rev_reversalAdd.Size = new System.Drawing.Size(75, 23);
            this.rev_reversalAdd.TabIndex = 20;
            this.rev_reversalAdd.Text = "Add";
            this.rev_reversalAdd.UseVisualStyleBackColor = false;
            this.rev_reversalAdd.Click += new System.EventHandler(this.rev_reversalAdd_Click);
            // 
            // rev_reversalList
            // 
            this.rev_reversalList.FormattingEnabled = true;
            this.rev_reversalList.Location = new System.Drawing.Point(15, 27);
            this.rev_reversalList.Name = "rev_reversalList";
            this.rev_reversalList.ScrollAlwaysVisible = true;
            this.rev_reversalList.Size = new System.Drawing.Size(198, 186);
            this.rev_reversalList.Sorted = true;
            this.rev_reversalList.TabIndex = 19;
            this.rev_reversalList.SelectedIndexChanged += new System.EventHandler(this.rev_reversalList_SelectedIndexChanged);
            // 
            // label68
            // 
            this.label68.AutoSize = true;
            this.label68.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label68.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label68.Location = new System.Drawing.Point(80, 7);
            this.label68.Name = "label68";
            this.label68.Size = new System.Drawing.Size(62, 15);
            this.label68.TabIndex = 18;
            this.label68.Text = "Move List";
            // 
            // CustomReversalForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.RoyalBlue;
            this.ClientSize = new System.Drawing.Size(832, 304);
            this.Controls.Add(this.panel21);
            this.Controls.Add(this.panel22);
            this.Name = "CustomReversalForm";
            this.Text = "Custom Reversal Form";
            this.panel21.ResumeLayout(false);
            this.panel21.PerformLayout();
            this.panel22.ResumeLayout(false);
            this.panel22.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel21;
        private System.Windows.Forms.Button rev_save;
        public System.Windows.Forms.ComboBox rev_moveType;
        private System.Windows.Forms.Label label65;
        private System.Windows.Forms.Button rev_moveRefresh;
        public System.Windows.Forms.ComboBox rev_searchResults;
        private System.Windows.Forms.Label label66;
        private System.Windows.Forms.Label label67;
        private System.Windows.Forms.TextBox rev_moveSearch;
        private System.Windows.Forms.Panel panel22;
        private System.Windows.Forms.TextBox rev_replacementType;
        private System.Windows.Forms.Button rev_removeReplacement;
        private System.Windows.Forms.Button rev_addReplacement;
        public System.Windows.Forms.ListBox rev_replacementList;
        private System.Windows.Forms.Label label71;
        public System.Windows.Forms.ComboBox rev_executor;
        private System.Windows.Forms.Label label70;
        private System.Windows.Forms.Label label69;
        private System.Windows.Forms.Button rev_reversalRemove;
        private System.Windows.Forms.Button rev_reversalAdd;
        public System.Windows.Forms.ListBox rev_reversalList;
        private System.Windows.Forms.Label label68;
    }
}