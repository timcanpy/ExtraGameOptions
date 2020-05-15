using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using QoL_Mods;
using QoL_Mods.Private;

namespace Ace.AttireExtension
{
    public class Attire_Select : Form
    {
        public string chosenAttire;

        public static Attire_Select instance = null;

        public FileInfo[] extraCostumes;

        public int pl2;

        public string charType;

        private IContainer components = null;

        private Label label2;

        public Button button2;

        public Button button1;

        private Label label1;

        public ComboBox comboBox1;

        public Attire_Select(FileInfo[] list, int pl, string type)
        {
            this.extraCostumes = list;
            this.pl2 = pl;
            this.charType = type;
            this.InitializeComponent();
        }

        private void Attire_Select_Load(object sender, EventArgs e)
        {
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool flag = this.comboBox1.SelectedIndex != -1;
            if (flag)
            {
                bool flag2 = this.charType == "edit";
                if (flag2)
                {
                    //string oldValue = AttireExtensionForm.RemoveSpecialCharacters(DataBase.GetWrestlerFullName(PrivateOverrides.plObj.WresParam));
                    //this.chosenAttire = this.comboBox1.SelectedItem.ToString().Replace(oldValue, "");
                }
                else
                {
                    bool flag3 = this.charType == "ref";
                    if (flag3)
                    {
                        string oldValue2 = AttireExtensionForm.RemoveSpecialCharacters(GeneralComponents.refObj.RefePrm.name);
                        this.chosenAttire = this.comboBox1.SelectedItem.ToString().Replace(oldValue2, "");
                    }
                }
            }

            ButtonSound(0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            base.Close();
            ButtonSound(1);
        }

        private void Attire_Select_Load_1(object sender, EventArgs e)
        {
            FileInfo[] array = this.extraCostumes;
            for (int i = 0; i < array.Length; i++)
            {
                FileInfo fileInfo = array[i];
                this.comboBox1.Items.Add(fileInfo.Name.Replace(fileInfo.Extension, ""));
            }
            bool flag = this.charType == "edit";
            if (flag)
            {
                //this.label2.Text = DataBase.GetWrestlerFullName(PrivateOverrides.plObj.WresParam) + " - Slot " + this.pl2.ToString();
            }
            else
            {
                bool flag2 = this.charType == "ref";
                if (flag2)
                {
                    this.label2.Text = GeneralComponents.refObj.RefePrm.name;
                }
            }
            this.comboBox1.SelectedIndex = 0;
            base.TopMost = true;
        }

        protected override void Dispose(bool disposing)
        {
            bool flag = disposing && this.components != null;
            if (flag)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.label2 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label2.Location = new System.Drawing.Point(50, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(196, 23);
            this.label2.TabIndex = 9;
            this.label2.Text = " Top Text";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Black;
            this.button2.BackgroundImage = global::QoL_Mods.Properties.Resources.ButtonNotActive;
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(132, 101);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(114, 23);
            this.button2.TabIndex = 8;
            this.button2.Text = "Cancel (Default)";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            this.button2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button2_MouseDown);
            this.button2.MouseEnter += new System.EventHandler(this.button2_MouseEnter);
            this.button2.MouseLeave += new System.EventHandler(this.button2_MouseLeave);
            this.button2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.button2_MouseMove);
            this.button2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.button2_MouseUp);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Black;
            this.button1.BackgroundImage = global::QoL_Mods.Properties.Resources.ButtonNotActive;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(51, 101);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            this.button1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button2_MouseDown);
            this.button1.MouseEnter += new System.EventHandler(this.button2_MouseEnter);
            this.button1.MouseLeave += new System.EventHandler(this.button2_MouseLeave);
            this.button1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.button2_MouseMove);
            this.button1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.button2_MouseUp);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label1.Location = new System.Drawing.Point(51, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(196, 18);
            this.label1.TabIndex = 6;
            this.label1.Text = "Which attire?";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(51, 69);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(195, 21);
            this.comboBox1.TabIndex = 5;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // Attire_Select
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Red;
            this.BackgroundImage = global::QoL_Mods.Properties.Resources.tdialogBackground;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CancelButton = this.button2;
            this.ClientSize = new System.Drawing.Size(287, 152);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Attire_Select";
            this.Text = "Costume Select";
            this.TransparencyKey = System.Drawing.Color.Red;
            this.Load += new System.EventHandler(this.Attire_Select_Load_1);
            this.ResumeLayout(false);

        }

        private void button2_MouseDown(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button;
            btn.BackgroundImage = QoL_Mods.Properties.Resources.ButtonClicked;
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            btn.BackgroundImage = QoL_Mods.Properties.Resources.ButtonNotActive;
        }

        private void button2_MouseMove(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button;
            btn.BackgroundImage = QoL_Mods.Properties.Resources.ButtonActive;
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            btn.BackgroundImage = QoL_Mods.Properties.Resources.ButtonActive;
        }

        private void button2_MouseUp(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button;
            btn.BackgroundImage = QoL_Mods.Properties.Resources.ButtonActive;
        }

        public static void FailSound()
        {
            Menu_SoundManager.Play_SE(Menu_SoundManager.SYSTEM_SOUND.CANCEL, Menu_SoundManager.PLAY_TYPE.ONCE, 1f);
        }


        public static void SuccessSound()
        {
            Menu_SoundManager.Play_SE(Menu_SoundManager.SYSTEM_SOUND.SELECT, Menu_SoundManager.PLAY_TYPE.ONCE, 1f);
        }

        public static void ButtonSound(int id)
        {
            switch (id)
            {
                case 0:
                    SuccessSound();
                    break;
                case 1:
                default:
                    FailSound();
                    break;
            }
        }
    }
}
