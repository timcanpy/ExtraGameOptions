using DG;
using QoL_Mods.Data_Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QoL_Mods.Private
{
    public partial class TOSForm : Form
    {
        #region Variables
        private static List<Skill> moves = new List<Skill>();
        private static String saveFolderName = "./EGOData/";
        private static String wrestlerFile = "WrestlerTOS.dat";
        private static String styleFile = "StyleTOS.dat";
        public static TOSForm form = null;
        #endregion
        public TOSForm()
        {
            InitializeComponent();
            form = this;
            tos_moveSearch.LostFocus += tos_moveSearch_LostFocus;
            FormClosing += TOSForm_FormClosing;
            LoadStyles();
        }

        private void TOSForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveStyles();
        }

        #region Move Search
        private void tos_moveRefresh_Click(object sender, EventArgs e)
        {
            LoadMoves();
        }

        private void tos_moveReload_Click(object sender, EventArgs e)
        {
            LoadStyles();
        }
        private void tos_moveSave_Click(object sender, EventArgs e)
        {
            SaveStyles();
        }

        private void tos_moveSearch_LostFocus(object sender, System.EventArgs e)
        {
            if (tos_moveSearch.Text.Trim().Equals(String.Empty))
            {
                return;
            }

            tos_moveResults.Items.Clear();
            foreach (Skill move in moves)
            {
                if (move.SkillName.ToLower().Contains(tos_moveSearch.Text.ToLower()))
                {
                    tos_moveResults.Items.Add(move);
                }
            }

            if (tos_moveResults.Items.Count > 0)
            {
                tos_moveResults.SelectedIndex = 0;
            }
        }
        #endregion

        #region Style Management

        private void LoadStyles()
        {
            try
            {
                tos_styles.Items.Clear();

                System.String filePath = Path.Combine(saveFolderName, styleFile);
                if (File.Exists(filePath))
                {
                    var lines = File.ReadAllLines(filePath);
                    foreach (var line in lines)
                    {
                        TOSMoves move = new TOSMoves("");
                        move.LoadMoves(line);
                        tos_styles.Items.Add(move);
                    }
                }
                else
                {
                    tos_styles.Items.Add(new TOSMoves("Orthodox"));
                    tos_styles.Items.Add(new TOSMoves("Technician"));
                    tos_styles.Items.Add(new TOSMoves("Wrestling"));
                    tos_styles.Items.Add(new TOSMoves("Ground"));
                    tos_styles.Items.Add(new TOSMoves("Power"));
                    tos_styles.Items.Add(new TOSMoves("American"));
                    tos_styles.Items.Add(new TOSMoves("Junior"));
                    tos_styles.Items.Add(new TOSMoves("Luchador"));
                    tos_styles.Items.Add(new TOSMoves("Heel"));
                    tos_styles.Items.Add(new TOSMoves("Mysterious"));
                    tos_styles.Items.Add(new TOSMoves("Shooter"));
                    tos_styles.Items.Add(new TOSMoves("Fighter"));
                    tos_styles.Items.Add(new TOSMoves("Grappler"));
                    tos_styles.Items.Add(new TOSMoves("Panther"));
                    tos_styles.Items.Add(new TOSMoves("Giant"));
                    tos_styles.Items.Add(new TOSMoves("Devilism"));

                }

                if (tos_styles.Items.Count > 0)
                {
                    tos_styles.SelectedIndex = 0;
                }
            }
            catch (Exception e)
            {
                L.D("LoadStylesError: " + e);
            }
        }

        private void SaveStyles()
        {
            try
            {
                System.String filePath = Path.Combine(saveFolderName, styleFile);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                using (StreamWriter sw = File.AppendText(filePath))
                {
                    foreach (TOSMoves move in tos_styles.Items)
                    {
                        sw.WriteLine(move.SaveMoves());
                    }
                }
            }
            catch (Exception e)
            {
                L.D("SaveStylesError: " + e);
            }

        }

        private void tos_styles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tos_styles.Items.Count == 0 || tos_styles.SelectedItem == null)
            {
                return;
            }

            TOSMoves moves = (TOSMoves) tos_styles.SelectedItem;
            tos_styleMoves.Items.Clear();

            foreach (Skill skill in moves.Moves.Skills)
            {
                tos_styleMoves.Items.Add(skill);
            }

            if (tos_styleMoves.Items.Count > 0)
            {
                tos_styleMoves.SelectedIndex = 0;
            }
        }

        private void tos_addStyleMove_Click(object sender, EventArgs e)
        {
            if (tos_styles.SelectedItem == null || tos_moveResults.SelectedItem == null)
            {
                return;
            }

            TOSMoves moves = (TOSMoves)tos_styles.SelectedItem;
            moves.AddMove((Skill) tos_moveResults.SelectedItem);
            tos_styles.SelectedItem = moves;
            tos_styles_SelectedIndexChanged(null, null);
        }

        private void tos_removeStyleMove_Click(object sender, EventArgs e)
        {

            if (tos_styles.SelectedItem == null || tos_styleMoves.SelectedItem == null)
            {
                return;
            }

            TOSMoves moves = (TOSMoves)tos_styles.SelectedItem;
            Skill skill = (Skill) tos_styleMoves.SelectedItem;
            moves.RemoveMove(skill);
            tos_styles.SelectedItem = moves;
            tos_styles_SelectedIndexChanged(null, null);
        }

        #endregion

        #region Wrestler Management
        private void tos_addWrestler_Click(object sender, EventArgs e)
        {

        }

        private void tos_removeWrestler_Click(object sender, EventArgs e)
        {

        }

        private void tos_wrestlers_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tos_addWrestlerMove_Click(object sender, EventArgs e)
        {

        }

        private void tos_removeWrestlerMove_Click(object sender, EventArgs e)
        {

        }
        #endregion


        #region Helper Methods
        private void LoadMoves()
        {
            try
            {
                moves.Clear();

                moves.Add(new Skill("Clinch", -1));

                foreach (KeyValuePair<SkillID, SkillInfo> current in SkillInfoManager.inst.skillInfoList)
                {
                    //Grapples
                    if (current.Value.anmBankType.ToString().Contains("Grapple_L") ||
                        current.Value.anmBankType.ToString().Contains("Grapple_M") ||
                        current.Value.anmBankType.ToString().Contains("Grapple_H"))
                    {
                        moves.Add(new Skill(DataBase.GetSkillName(current.Key), (int)current.Key));
                    }
                }

                tos_moveResults.Items.Clear();
                foreach (Skill move in moves)
                {
                    tos_moveResults.Items.Add(move);
                }


                tos_moveResults.SelectedIndex = 0;
            }
            catch (Exception e)
            {
                L.D("LoadMovesError: " + e);
            }
        }
        #endregion
        
    }
}
