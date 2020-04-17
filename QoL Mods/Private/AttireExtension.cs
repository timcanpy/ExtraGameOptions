using DG;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using MatchConfig;
using QoL_Mods.Helper_Classes;

namespace Ace.AttireExtension
{
    public class AttireExtensionForm : Form
    {
        #region Variables
        public static string name;

        public static int id;

        public static int[] test = new int[0];

        public static int[] numVal = new int[8];

        public static SaveData saveData = SaveData.inst;

        public static bool textChanged = false;

        public static int selID;

        private List<int> searchID = new List<int>();

        public static bool RtextChanged = false;

        public static int RselID;

        private List<int> RsearchID = new List<int>();

        public static int cos;

        public static int txtToImport;

        public static List<EditWrestlerData> wresDatList = new List<EditWrestlerData>();

        public static List<int> wresIDList = new List<int>();

        public static List<RefereeInfo> refDatList = new List<RefereeInfo>();

        public static List<int> refIDList = new List<int>();

        public static AttireExtensionForm instance = null;

        private IContainer components = null;

        private ListBox refereeList;

        private GroupBox refereeBox;

        private Button refImportButton;

        private Button refExportButton;

        private Button button13;

        private TextBox textBox2;

        private Button button14;

        private Panel panel2;

        private Label label10;

        private Label label11;

        private Panel panel1;

        private Button defRefSave;

        private ComboBox defRefComboBox;

        private Label label9;
        #endregion

        private void AttireExtensionForm_Load(object sender, EventArgs e)
        {
            this.LoadRefs();
        }

        public static void SetupFolders()
        {
            if (!Directory.Exists("./EGOData/RefereeCostumes"))
            {
                Directory.CreateDirectory("./EGOData/RefereeCostumes");
            }



            if (!File.Exists("./EGOData/RefereeCostumes/Defaults.txt"))
            {
                File.Create("./EGOData/RefereeCostumes/Defaults.txt");
            }
        }

        public AttireExtensionForm()
        {
            AttireExtensionForm.SetupFolders();
            AttireExtensionForm.instance = this;
            this.InitializeComponent();
        }

        public static bool IsValidFilename(string filename)
        {
            bool result;
            try
            {
                File.OpenRead(filename).Close();
            }
            catch (ArgumentException)
            {
                result = false;
                return result;
            }
            catch (Exception)
            {
            }
            result = true;
            return result;
        }

        public static string RemoveSpecialCharacters(string str)
        {
            return string.Join("", str.Split(Path.GetInvalidFileNameChars()));
        }

        public static bool IsNumeric(object Expression)
        {
            double num;
            return double.TryParse(Convert.ToString(Expression), NumberStyles.Any, NumberFormatInfo.InvariantInfo, out num);
        }


        private void LoadRefs()
        {
            this.refereeList.Items.Clear();
            AttireExtensionForm.refDatList.Clear();
            AttireExtensionForm.refIDList.Clear();
            foreach (RefereeInfo current in MatchConfiguration.LoadReferees())
            {
                AttireExtensionForm.refDatList.Add(current);
                AttireExtensionForm.refIDList.Add((int)current.Data.editRefereeID);
                this.refereeList.Items.Add(current);
            }
        }
        private void ImportRefAttire()
        {
            GlobalWork inst = GlobalWork.inst;
            RefereeMan inst2 = RefereeMan.inst;
            AttireExtensionForm.saveData = SaveData.inst;
            int num = AttireExtensionForm.refIDList[this.refereeList.SelectedIndex];
            string text = AttireExtensionForm.saveData.GetEditRefereeData((RefereeID)num).Prm.name;
            CostumeData costumeData = AttireExtensionForm.saveData.GetEditRefereeData((RefereeID)num).appearanceData.costumeData[0];
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "COSTUME Files (*.cos)|*.cos|MCD Files (*.mcd)|*.mcd)";
            openFileDialog.InitialDirectory = "./EGOData/RefereeCostumes";
            bool flag = openFileDialog.ShowDialog() == DialogResult.OK;
            if (flag)
            {
                DialogResult dialogResult = MessageBox.Show("Overwrite in-game referee attire with " + openFileDialog.FileName + "? Are you certain? There is no undoing this.", "Please confirm your action.", MessageBoxButtons.YesNo);
                bool flag2 = dialogResult == DialogResult.Yes;
                if (flag2)
                {
                    StreamReader streamReader = new StreamReader(openFileDialog.FileName);
                    CostumeData costumeData2 = new CostumeData();
                    while (streamReader.Peek() != -1)
                    {
                        costumeData2.valid = true;
                        for (int i = 0; i < 9; i++)
                        {
                            for (int j = 0; j < 16; j++)
                            {
                                costumeData2.layerTex[i, j] = streamReader.ReadLine();
                                costumeData2.color[i, j].r = float.Parse(streamReader.ReadLine());
                                costumeData2.color[i, j].g = float.Parse(streamReader.ReadLine());
                                costumeData2.color[i, j].b = float.Parse(streamReader.ReadLine());
                                costumeData2.color[i, j].a = float.Parse(streamReader.ReadLine());
                                costumeData2.highlightIntensity[i, j] = float.Parse(streamReader.ReadLine());
                            }
                            costumeData2.partsScale[i] = float.Parse(streamReader.ReadLine());
                        }
                        string text2 = streamReader.ReadLine();
                    }
                    try
                    {
                        costumeData.valid = true;
                        for (int k = 0; k < 9; k++)
                        {
                            for (int l = 0; l < 16; l++)
                            {
                                costumeData.layerTex[k, l] = costumeData2.layerTex[k, l];
                                costumeData.color[k, l].r = costumeData2.color[k, l].r;
                                costumeData.color[k, l].g = costumeData2.color[k, l].g;
                                costumeData.color[k, l].b = costumeData2.color[k, l].b;
                                costumeData.color[k, l].a = costumeData2.color[k, l].a;
                                costumeData.highlightIntensity[k, l] = costumeData2.highlightIntensity[k, l];
                            }
                            costumeData.partsScale[k] = costumeData2.partsScale[k];
                        }
                        L.D("ATTIRE EXTENSION: REFEREE ATTIRE IMPORTED", new object[0]);
                    }
                    catch
                    {
                        L.D("ATTIRE EXTENSION: REFEREE ATTIRE NOT IMPORTED", new object[0]);
                    }
                    streamReader.Dispose();
                    streamReader.Close();
                }
            }
        }

        private void SaveIndividualRefAttires()
        {
            string str = AttireExtensionForm.RemoveSpecialCharacters(AttireExtensionForm.refDatList[this.refereeList.SelectedIndex].Data.Prm.name);
            int num = AttireExtensionForm.refIDList[this.refereeList.SelectedIndex];
            AttireExtensionForm.saveData = SaveData.inst;
            RefereeData editRefereeData = AttireExtensionForm.saveData.GetEditRefereeData((RefereeID)num);
            CostumeData costumeData = editRefereeData.appearanceData.costumeData[0];
            bool valid = costumeData.valid;
            if (valid)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.InitialDirectory = "./EGOData/RefereeCostumes";
                saveFileDialog.Filter = "COSTUME Files (*.cos)|*.cos";
                saveFileDialog.FileName = str + "_";
                bool flag = saveFileDialog.ShowDialog() == DialogResult.OK;
                if (flag)
                {
                    try
                    {
                        using (StreamWriter streamWriter = new StreamWriter(saveFileDialog.FileName))
                        {
                            for (int i = 0; i < 9; i++)
                            {
                                for (int j = 0; j < 16; j++)
                                {
                                    streamWriter.WriteLine(costumeData.layerTex[i, j]);
                                    streamWriter.WriteLine(costumeData.color[i, j].r);
                                    streamWriter.WriteLine(costumeData.color[i, j].g);
                                    streamWriter.WriteLine(costumeData.color[i, j].b);
                                    streamWriter.WriteLine(costumeData.color[i, j].a);
                                    streamWriter.WriteLine(costumeData.highlightIntensity[i, j]);
                                }
                                streamWriter.WriteLine(costumeData.partsScale[i]);
                            }
                            streamWriter.Dispose();
                            streamWriter.Close();
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Couldn't save attire to './EGOData/RefereeCostumes" + AttireExtensionForm.name + ".cos'");
                    }
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            AttireExtensionForm.textChanged = true;
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                bool flag = this.refereeList.SelectedIndex == -1;
                if (!flag)
                {
                    string str = AttireExtensionForm.RemoveSpecialCharacters(this.refereeList.SelectedItem.ToString());
                    DirectoryInfo directoryInfo = new DirectoryInfo("./EGOData/RefereeCostumes");
                    FileInfo[] files = directoryInfo.GetFiles(str + "*.cos");
                    this.defRefComboBox.Items.Clear();
                    this.defRefComboBox.Items.Add("None");
                    FileInfo[] array = files;
                    for (int i = 0; i < array.Length; i++)
                    {
                        FileInfo fileInfo = array[i];
                        this.defRefComboBox.Items.Add(fileInfo.Name.Replace(fileInfo.Extension, ""));
                    }
                    List<string> list = new List<string>();
                    string[] array2 = File.ReadAllLines("./EGOData/RefereeCostumes/Defaults.txt");
                    for (int j = 0; j < array2.Length; j++)
                    {
                        string item = array2[j];
                        list.Add(item);
                    }
                    bool flag2 = list.Contains(this.refereeList.SelectedItem.ToString());
                    if (flag2)
                    {
                        this.defRefComboBox.SelectedItem = list[list.IndexOf(this.refereeList.SelectedItem.ToString()) + 1].Replace("/Referees/", "");
                    }
                    else
                    {
                        this.defRefComboBox.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception exception)
            {
                L.D(exception.ToString());
            }
        }

        private void refExportButton_Click(object sender, EventArgs e)
        {
            bool flag = this.refereeList.SelectedIndex != -1;
            if (flag)
            {
                this.SaveIndividualRefAttires();
            }
        }

        private void refImportButton_Click(object sender, EventArgs e)
        {
            bool flag = this.refereeList.SelectedIndex != -1;
            if (flag)
            {
                this.ImportRefAttire();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            AttireExtensionForm.RtextChanged = true;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            try
            {
                bool rtextChanged = AttireExtensionForm.RtextChanged;
                if (rtextChanged)
                {
                    this.RsearchID.Clear();
                    AttireExtensionForm.RselID = 0;
                    bool flag = this.textBox2.Text == "" || this.textBox2.Text == " " || this.textBox2.Text == "  ";
                    if (!flag)
                    {
                        for (int i = 0; i < this.refereeList.Items.Count; i++)
                        {
                            bool flag2 = this.refereeList.Items[i].ToString().ToLower().Contains(this.textBox2.Text.ToLower());
                            if (flag2)
                            {
                                this.RsearchID.Add(i);
                            }
                        }
                        this.refereeList.SetSelected(this.RsearchID[0], true);
                        AttireExtensionForm.RtextChanged = false;
                    }
                }
                else
                {
                    this.refereeList.ClearSelected();
                    try
                    {
                        AttireExtensionForm.RselID++;
                        this.refereeList.SetSelected(this.RsearchID[AttireExtensionForm.RselID], true);
                        this.refereeList.Refresh();
                    }
                    catch
                    {
                        AttireExtensionForm.RselID = 0;
                        this.refereeList.SetSelected(this.RsearchID[AttireExtensionForm.RselID], true);
                        this.refereeList.Refresh();
                    }
                }
            }
            catch
            {
            }
        }

        private void CheckFile()
        {
            bool flag = !File.Exists("./EGOData/RefereeCostumes/Defaults.txt");
            if (flag)
            {
                File.Create("./EGOData/RefereeCostumes/Defaults.txt");
            }
        }

        private void DefRefSave_Click(object sender, EventArgs e)
        {
            this.CheckFile();
            bool flag = this.defRefComboBox.SelectedIndex == -1;
            if (!flag)
            {
                List<string> list = new List<string>();
                string[] array = File.ReadAllLines("./EGOData/RefereeCostumes/Defaults.txt");
                for (int i = 0; i < array.Length; i++)
                {
                    string item = array[i];
                    list.Add(item);
                }
                bool flag2 = this.defRefComboBox.SelectedIndex != 0;
                if (flag2)
                {
                    bool flag3 = list.Contains(this.refereeList.SelectedItem.ToString());
                    if (flag3)
                    {
                        list[list.IndexOf(this.refereeList.SelectedItem.ToString()) + 1] = "/Referees/" + this.defRefComboBox.SelectedItem.ToString();
                    }
                    else
                    {
                        list.Add(this.refereeList.SelectedItem.ToString());
                        list.Add("/Referees/" + this.defRefComboBox.SelectedItem.ToString());
                    }
                }
                else
                {
                    bool flag4 = list.Contains(this.refereeList.SelectedItem.ToString());
                    if (flag4)
                    {
                        list.RemoveAt(list.IndexOf(this.refereeList.SelectedItem.ToString()) + 1);
                        list.RemoveAt(list.IndexOf(this.refereeList.SelectedItem.ToString()));
                    }
                }
                string[] contents = list.ToArray();
                File.WriteAllLines("./EGOData/RefereeCostumes/Defaults.txt", contents);
            }
        }

        private void DefRefComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void button13_Click(object sender, EventArgs e)
        {
            this.LoadRefs();
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
            this.refereeList = new System.Windows.Forms.ListBox();
            this.refereeBox = new System.Windows.Forms.GroupBox();
            this.defRefSave = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.refImportButton = new System.Windows.Forms.Button();
            this.defRefComboBox = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.refExportButton = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.button13 = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button14 = new System.Windows.Forms.Button();
            this.refereeBox.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // refereeList
            // 
            this.refereeList.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.refereeList.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.refereeList.FormattingEnabled = true;
            this.refereeList.ItemHeight = 15;
            this.refereeList.Location = new System.Drawing.Point(6, 19);
            this.refereeList.Name = "refereeList";
            this.refereeList.Size = new System.Drawing.Size(201, 274);
            this.refereeList.TabIndex = 46;
            this.refereeList.SelectedIndexChanged += new System.EventHandler(this.listBox2_SelectedIndexChanged);
            // 
            // refereeBox
            // 
            this.refereeBox.Controls.Add(this.defRefSave);
            this.refereeBox.Controls.Add(this.panel2);
            this.refereeBox.Controls.Add(this.defRefComboBox);
            this.refereeBox.Controls.Add(this.panel1);
            this.refereeBox.Controls.Add(this.label9);
            this.refereeBox.Controls.Add(this.button13);
            this.refereeBox.Controls.Add(this.refereeList);
            this.refereeBox.Controls.Add(this.textBox2);
            this.refereeBox.Controls.Add(this.button14);
            this.refereeBox.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.refereeBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.refereeBox.Location = new System.Drawing.Point(22, 12);
            this.refereeBox.Name = "refereeBox";
            this.refereeBox.Size = new System.Drawing.Size(213, 567);
            this.refereeBox.TabIndex = 47;
            this.refereeBox.TabStop = false;
            this.refereeBox.Text = "Referees";
            // 
            // defRefSave
            // 
            this.defRefSave.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.defRefSave.ForeColor = System.Drawing.SystemColors.ControlText;
            this.defRefSave.Location = new System.Drawing.Point(6, 408);
            this.defRefSave.Name = "defRefSave";
            this.defRefSave.Size = new System.Drawing.Size(201, 23);
            this.defRefSave.TabIndex = 47;
            this.defRefSave.Text = "Save";
            this.defRefSave.UseVisualStyleBackColor = false;
            this.defRefSave.Click += new System.EventHandler(this.DefRefSave_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.label11);
            this.panel2.Controls.Add(this.refImportButton);
            this.panel2.ForeColor = System.Drawing.Color.Black;
            this.panel2.Location = new System.Drawing.Point(6, 475);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(201, 84);
            this.panel2.TabIndex = 42;
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Franklin Gothic Medium", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Red;
            this.label10.Location = new System.Drawing.Point(3, 44);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(193, 38);
            this.label10.TabIndex = 38;
            this.label10.Text = "IT WILL OVERWRITE IN-GAME COSTUME/ATTIRE SAVE DATA";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("Franklin Gothic Medium", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.Red;
            this.label11.Location = new System.Drawing.Point(3, 32);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(193, 15);
            this.label11.TabIndex = 37;
            this.label11.Text = "BE CAREFUL WITH THIS!";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // refImportButton
            // 
            this.refImportButton.BackColor = System.Drawing.SystemColors.Window;
            this.refImportButton.Location = new System.Drawing.Point(3, 3);
            this.refImportButton.Name = "refImportButton";
            this.refImportButton.Size = new System.Drawing.Size(192, 23);
            this.refImportButton.TabIndex = 49;
            this.refImportButton.Text = "Import Referee Attire";
            this.refImportButton.UseVisualStyleBackColor = false;
            this.refImportButton.Click += new System.EventHandler(this.refImportButton_Click);
            // 
            // defRefComboBox
            // 
            this.defRefComboBox.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.defRefComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.defRefComboBox.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.defRefComboBox.FormattingEnabled = true;
            this.defRefComboBox.Location = new System.Drawing.Point(6, 381);
            this.defRefComboBox.Name = "defRefComboBox";
            this.defRefComboBox.Size = new System.Drawing.Size(201, 23);
            this.defRefComboBox.TabIndex = 48;
            this.defRefComboBox.SelectedIndexChanged += new System.EventHandler(this.DefRefComboBox_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.refExportButton);
            this.panel1.Location = new System.Drawing.Point(6, 437);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(201, 32);
            this.panel1.TabIndex = 41;
            // 
            // refExportButton
            // 
            this.refExportButton.BackColor = System.Drawing.SystemColors.Window;
            this.refExportButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.refExportButton.Location = new System.Drawing.Point(3, 3);
            this.refExportButton.Name = "refExportButton";
            this.refExportButton.Size = new System.Drawing.Size(192, 23);
            this.refExportButton.TabIndex = 30;
            this.refExportButton.Text = "Export Referee Attire";
            this.refExportButton.UseVisualStyleBackColor = false;
            this.refExportButton.Click += new System.EventHandler(this.refExportButton_Click);
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label9.Location = new System.Drawing.Point(6, 355);
            this.label9.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(201, 23);
            this.label9.TabIndex = 49;
            this.label9.Text = "Default Attire";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button13
            // 
            this.button13.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.button13.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button13.Location = new System.Drawing.Point(6, 327);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(201, 23);
            this.button13.TabIndex = 46;
            this.button13.Text = "Refresh";
            this.button13.UseVisualStyleBackColor = false;
            this.button13.Click += new System.EventHandler(this.button13_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(6, 302);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(137, 20);
            this.textBox2.TabIndex = 47;
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // button14
            // 
            this.button14.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.button14.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button14.Location = new System.Drawing.Point(149, 302);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(58, 20);
            this.button14.TabIndex = 48;
            this.button14.Text = "Search";
            this.button14.UseVisualStyleBackColor = false;
            this.button14.Click += new System.EventHandler(this.button14_Click);
            // 
            // AttireExtensionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.RoyalBlue;
            this.ClientSize = new System.Drawing.Size(254, 588);
            this.Controls.Add(this.refereeBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "AttireExtensionForm";
            this.Text = "Referee Costume Extension";
            this.Load += new System.EventHandler(this.AttireExtensionForm_Load);
            this.refereeBox.ResumeLayout(false);
            this.refereeBox.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
    }
}
