﻿using DG;
using MatchConfig;
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
        public static List<EF_WresIDGroup> wrestlerList = new List<EF_WresIDGroup>();
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
            tos_wrestlerSearch.LostFocus += tos_wrestlerSearch_LostFocus;
            FormClosing += TOSForm_FormClosing;
            LoadMoves();
            LoadSubs();
            LoadStyles();
            LoadWrestlers();
        }

        private void LoadSubs()
        {
            try
            {
                this.tos_wrestlerResults.Items.Clear();
                wrestlerList = new List<EF_WresIDGroup>();

                foreach (EditWrestlerData current in SaveData.inst.editWrestlerData)
                {
                    EF_WresIDGroup wresIDGroup = new EF_WresIDGroup();
                    wresIDGroup.Name = DataBase.GetWrestlerFullName(current.wrestlerParam);
                    wresIDGroup.ID = (Int32)current.editWrestlerID;

                    wrestlerList.Add(wresIDGroup);
                    this.tos_wrestlerResults.Items.Add(wresIDGroup);
                }

                if (tos_wrestlerResults.Items.Count > 0)
                {
                    this.tos_wrestlerResults.SelectedIndex = 0;
                }

            }
            catch (Exception ex)
            {
                L.D("Load Subs Exception: " + ex);
            }

        }

        private void tos_wrestlerSearch_LostFocus(object sender, System.EventArgs e)
        {
            try
            {
                String query = tos_wrestlerSearch.Text;
                tos_wrestlerResults.Items.Clear();

                if (!query.TrimStart().TrimEnd().Equals(""))
                {
                    foreach (EF_WresIDGroup wrestler in wrestlerList)
                    {
                        if (query.ToLower().Equals(wrestler.Name.ToLower()) ||
                            wrestler.Name.ToLower().Contains(query.ToLower()))
                        {
                            tos_wrestlerResults.Items.Add(wrestler);
                        }
                    }
                }

                if (tos_wrestlerResults.Items.Count > 0)
                {
                    tos_wrestlerResults.SelectedIndex = 0;
                }

            }
            catch (Exception ex)
            {
                L.D("Search Error: " + ex.Message);
            }
        }

        private void TOSForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveStyles();
            SaveWrestlers();
        }

        #region Move Search
        private void tos_moveRefresh_Click(object sender, EventArgs e)
        {
            LoadMoves();
        }

        private void tos_moveReload_Click(object sender, EventArgs e)
        {
            LoadStyles();
            LoadWrestlers();
        }
        private void tos_moveSave_Click(object sender, EventArgs e)
        {
            SaveStyles();
            SaveWrestlers();
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

            TOSMoves moves = (TOSMoves)tos_styles.SelectedItem;
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
            try
            {
                if (tos_styles.SelectedItem == null || tos_moveResults.SelectedItem == null)
                {
                    return;
                }

                TOSMoves moves = (TOSMoves)tos_styles.SelectedItem;
                moves.AddMove((Skill)tos_moveResults.SelectedItem);

                tos_styles_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                L.D("tos_addStyleMove_Click: " + ex);
            }
        }

        private void tos_removeStyleMove_Click(object sender, EventArgs e)
        {
            try
            {
                if (tos_styles.SelectedItem == null || tos_styleMoves.SelectedItem == null)
                {
                    return;
                }

                TOSMoves moves = (TOSMoves)tos_styles.SelectedItem;
                Skill skill = (Skill)tos_styleMoves.SelectedItem;
                moves.RemoveMove(skill);

                tos_styles_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                L.D("tos_removeStyleMove_Click: " + ex);
            }
        }

        #endregion

        #region Wrestler Management
        private void LoadWrestlers()
        {
            try
            {
                tos_wrestlers.Items.Clear();

                System.String filePath = Path.Combine(saveFolderName, wrestlerFile);
                if (File.Exists(filePath))
                {
                    var lines = File.ReadAllLines(filePath);
                    foreach (var line in lines)
                    {
                        TOSMoves move = new TOSMoves("");
                        move.LoadMoves(line);
                        tos_wrestlers.Items.Add(move);
                    }
                }
                if (tos_wrestlers.Items.Count > 0)
                {
                    tos_wrestlers.SelectedIndex = 0;
                }
            }
            catch (Exception e)
            {
                L.D("LoadwrestlersError: " + e);
            }
        }

        private void SaveWrestlers()
        {
            try
            {
                System.String filePath = Path.Combine(saveFolderName, wrestlerFile);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                using (StreamWriter sw = File.AppendText(filePath))
                {
                    foreach (TOSMoves move in tos_wrestlers.Items)
                    {
                        sw.WriteLine(move.SaveMoves());
                    }
                }
            }
            catch (Exception e)
            {
                L.D("SavewrestlersError: " + e);
            }

        }

        private void tos_addWrestler_Click(object sender, EventArgs e)
        {
            if (tos_wrestlerResults.SelectedItem == null)
            {
                return;
            }

            try
            {
                String wrestler = ((EF_WresIDGroup)tos_wrestlerResults.SelectedItem).Name;
                TOSMoves moves = new TOSMoves(wrestler);
                tos_wrestlers.Items.Add(moves);

                if (tos_wrestlers.Items.Count > 0)
                {
                    tos_wrestlers.SelectedIndex = tos_wrestlers.Items.Count - 1;
                }

                tos_wrestlers_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                L.D("Add Wrestler Error: " + ex);
            }
        }

        private void tos_removeWrestler_Click(object sender, EventArgs e)
        {
            if (tos_wrestlers.SelectedItem == null)
            {
                return;
            }

            tos_wrestlers.Items.Remove(tos_wrestlers.SelectedItem);

            if (tos_wrestlers.Items.Count > 0)
            {
                tos_wrestlers.SelectedIndex = 0;
                tos_wrestlers_SelectedIndexChanged(null, null);
            }
            else
            {
                ClearWrestlerMoves();
            }
        }

        private void tos_wrestlers_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (tos_wrestlers.Items.Count == 0 || tos_wrestlers.SelectedItem == null)
                {
                    return;
                }

                TOSMoves moves = (TOSMoves)tos_wrestlers.SelectedItem;
                tos_wrestlerMoves.Items.Clear();

                foreach (Skill skill in moves.Moves.Skills)
                {
                    tos_wrestlerMoves.Items.Add(skill);
                }

                if (tos_wrestlerMoves.Items.Count > 0)
                {
                    tos_wrestlerMoves.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                L.D("tos_wrestlers_SelectedIndexChanged: " + ex);
            }

        }

        private void tos_addWrestlerMove_Click(object sender, EventArgs e)
        {
            try
            {
                if (tos_wrestlers.SelectedItem == null || tos_moveResults.SelectedItem == null)
                {
                    return;
                }

                TOSMoves moves = (TOSMoves)tos_wrestlers.SelectedItem;
                moves.AddMove((Skill)tos_moveResults.SelectedItem);

                tos_wrestlers_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                L.D("tos_addWrestlerMove_Click: " + ex);
            }

        }

        private void tos_removeWrestlerMove_Click(object sender, EventArgs e)
        {
            try
            {
                if (tos_wrestlers.SelectedItem == null || tos_wrestlerMoves.SelectedItem == null)
                {
                    return;
                }

                TOSMoves moves = (TOSMoves)tos_wrestlers.SelectedItem;
                Skill skill = (Skill)tos_wrestlerMoves.SelectedItem;
                moves.RemoveMove(skill);

                tos_wrestlers_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                L.D("tos_removeWrestlerMove_Click: " + ex);
            }
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
        private void ClearWrestlerMoves()
        {
            tos_wrestlerMoves.Items.Clear();
        }
        #endregion

    }
}
