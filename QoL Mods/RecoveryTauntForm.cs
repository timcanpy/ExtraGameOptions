using DG;
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

namespace QoL_Mods
{
    public partial class RecoveryTauntForm : Form
    {
        #region General Form Methods
        public void QoL_Form_Load(object sender, EventArgs e)
        {

        }

        public RecoveryTauntForm()
        {
            form = this;
            InitializeComponent();
            LoadRecoveryTaunts();
            LoadSubs();
            FormClosing += RecoveryForm_FormClosing;
            wu_wrestlerSearch.LostFocus += wu_wrestlerSearch_LostFocus;
            wu_moveSearch.LostFocus += wu_wrestlerSearch_LostFocus;
        }
        
        private void RecoveryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveRecoveryTaunts();
        }

        #endregion

        #region Variables
        public static RecoveryTauntForm form = null;
        public static Skill rollSkill = new Skill("Rolling", -100);
        public static HashSet<Skill> wakeUpSkills = new HashSet<Skill>();
        public static List<WresIDGroup> wrestlerList = new List<WresIDGroup>();
        private static String[] saveFileNames = new String[] { "StyleWT.dat", "WrestlerWT.dat" };
        private static String[] saveFolderNames = new String[] { "./EGOData/" };
        #endregion

        #region Wake Up Taunts
        private void LoadRecoveryTaunts()
        {
            String filePath = CheckSaveFile("StyleWT");
            if (File.Exists(filePath))
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    var lines = File.ReadAllLines(filePath);
                    foreach (String style in lines)
                    {
                        WakeUpTaunt taunt = new WakeUpTaunt(new QoL_Mods.Data_Classes.Style("", FightStyleEnum.American));
                        try
                        {
                            taunt.LoadWakeUpData(style);
                            wu_styles.Items.Add(taunt);
                        }
                        catch (Exception ex)
                        {
                            L.D("WakeUpTaunt Load Error:" + ex);
                        }
                    }

                    if (wu_styles.Items.Count > 0)
                    {
                        wu_styles.SelectedIndex = 0;
                    }
                }
            }
            else
            {
                LoadWakeUp();
            }

            filePath = CheckSaveFile("WrestlerWT");
            if (File.Exists(filePath))
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    var lines = File.ReadAllLines(filePath);
                    foreach (String style in lines)
                    {
                        WakeUpTaunt taunt = new WakeUpTaunt(new QoL_Mods.Data_Classes.Style("", FightStyleEnum.American));

                        try
                        {
                            taunt.LoadWakeUpData(style);
                            wu_wrestlers.Items.Add(taunt);
                        }
                        catch (Exception ex)
                        {
                            L.D("WakeUpTaunt Load Error:" + ex);
                        }
                    }

                    if (wu_wrestlers.Items.Count > 0)
                    {
                        wu_wrestlers.SelectedIndex = 0;
                    }
                }
            }

            SetValidMoves();
        }

        private void SaveRecoveryTaunts()
        {
            //Save Style WakeUpTaunts
            String filePath = CheckSaveFile("StyleWT");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            using (StreamWriter sw = File.AppendText(filePath))
            {
                foreach (WakeUpTaunt taunt in wu_styles.Items)
                {
                    sw.WriteLine(taunt.SaveWakeUpData());
                }
            }

            //Save Wrestler WakeUpTaunts
            filePath = CheckSaveFile("WrestlerWT");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            using (StreamWriter sw = File.AppendText(filePath))
            {
                foreach (WakeUpTaunt taunt in wu_wrestlers.Items)
                {
                    sw.WriteLine(taunt.SaveWakeUpData());
                }
            }
        }
        #region Variables
        #endregion

        #region Setup
        private void LoadWakeUp()
        {
            wu_styles.Items.Add(new WakeUpTaunt(new QoL_Mods.Data_Classes.Style("Orthodox", FightStyleEnum.Orthodox)));
            wu_styles.Items.Add(new WakeUpTaunt(new QoL_Mods.Data_Classes.Style("Technician", FightStyleEnum.Technician)));
            wu_styles.Items.Add(new WakeUpTaunt(new QoL_Mods.Data_Classes.Style("Wrestling", FightStyleEnum.Wrestling)));
            wu_styles.Items.Add(new WakeUpTaunt(new QoL_Mods.Data_Classes.Style("Ground", FightStyleEnum.Ground)));
            wu_styles.Items.Add(new WakeUpTaunt(new QoL_Mods.Data_Classes.Style("Power", FightStyleEnum.Power)));
            wu_styles.Items.Add(new WakeUpTaunt(new QoL_Mods.Data_Classes.Style("American", FightStyleEnum.American)));
            wu_styles.Items.Add(new WakeUpTaunt(new QoL_Mods.Data_Classes.Style("Junior", FightStyleEnum.Junior)));
            wu_styles.Items.Add(new WakeUpTaunt(new QoL_Mods.Data_Classes.Style("Luchador", FightStyleEnum.Luchador)));
            wu_styles.Items.Add(new WakeUpTaunt(new QoL_Mods.Data_Classes.Style("Heel", FightStyleEnum.Heel)));
            wu_styles.Items.Add(new WakeUpTaunt(new QoL_Mods.Data_Classes.Style("Mysterious", FightStyleEnum.Mysterious)));
            wu_styles.Items.Add(new WakeUpTaunt(new QoL_Mods.Data_Classes.Style("Shooter", FightStyleEnum.Shooter)));
            wu_styles.Items.Add(new WakeUpTaunt(new QoL_Mods.Data_Classes.Style("Fighter", FightStyleEnum.Fighter)));
            wu_styles.Items.Add(new WakeUpTaunt(new QoL_Mods.Data_Classes.Style("Grappler", FightStyleEnum.Grappler)));
            wu_styles.Items.Add(new WakeUpTaunt(new QoL_Mods.Data_Classes.Style("Panther", FightStyleEnum.Panther)));
            wu_styles.Items.Add(new WakeUpTaunt(new QoL_Mods.Data_Classes.Style("Giant", FightStyleEnum.Giant)));
            wu_styles.Items.Add(new WakeUpTaunt(new QoL_Mods.Data_Classes.Style("Devilism", FightStyleEnum.Devilism)));

            wu_styles.SelectedIndex = 0;
        }
        private void SetValidMoves()
        {
            wakeUpSkills.Clear();
            wu_moveResults.Items.Clear();
            try
            {
                wakeUpSkills.Add(rollSkill);
                wu_moveResults.Items.Add(rollSkill);
            }
            catch (Exception e)
            {
                L.D("SetDefaultMoveException: " + e);
            }

            foreach (KeyValuePair<SkillID, SkillInfo> current in SkillInfoManager.inst.skillInfoList)
            {
                try
                {
                    //Ensure that we only add performances beginning with either face up or face down positions.
                    if (current.Value.filteringType == SkillFilteringType.Performance)
                    {
                        var anmData = SkillDataMan.inst.GetSkillData(current.Key)[0].anmData[0];
                        if (anmData.formDispList[0].formIdx != 101 && anmData.formDispList[0].formIdx != 100)
                        {
                            continue;
                        }
                        else
                        {
                            wakeUpSkills.Add(new Skill(DataBase.GetSkillName(current.Key), (Int32)current.Key));
                            wu_moveResults.Items.Add(new Skill(DataBase.GetSkillName(current.Key), (Int32)current.Key));
                        }
                    }
                }
                catch (Exception e)
                {
                    L.D("SetValidMoveException: " + e);
                }
            }

            if (wakeUpSkills.Count > 0)
            {
                wu_moveResults.SelectedIndex = 0;
            }
        }

        #endregion

        #region Update Display
        private void wu_moveRefresh_Click_1(object sender, EventArgs e)
        {
            SetValidMoves();
        }

        private void wu_styles_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ClearStyleMoves();
                WakeUpTaunt styleTaunt = (WakeUpTaunt)wu_styles.SelectedItem;
                if (IsValidTaunt(styleTaunt.WakeupMoves[0]))
                {
                    wu_styleLight.Text = styleTaunt.WakeupMoves[0].SkillName;
                }

                if (IsValidTaunt(styleTaunt.WakeupMoves[1]))
                {
                    wu_styleMiddle.Text = styleTaunt.WakeupMoves[1].SkillName;
                }

                if (IsValidTaunt(styleTaunt.WakeupMoves[2]))
                {
                    wu_styleHeavy.Text = styleTaunt.WakeupMoves[2].SkillName;
                }

                if (IsValidTaunt(styleTaunt.WakeupMoves[3]))
                {
                    wu_styleCritical.Text = styleTaunt.WakeupMoves[3].SkillName;
                }
            }
            catch (Exception ex)
            {
                L.D("TESTEXCEPTION:" + ex);
            }
          
        }

        private void wu_wrestlerResults_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void wu_wrestlers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (wu_wrestlers.SelectedItem == null)
            {
                return;
            }
            ClearWrestlerMoves();
            WakeUpTaunt styleTaunt = (WakeUpTaunt)wu_wrestlers.SelectedItem;

            if (IsValidTaunt(styleTaunt.WakeupMoves[0]))
            {
                wu_wrestlerLight.Text = styleTaunt.WakeupMoves[0].SkillName;
            }

            if (IsValidTaunt(styleTaunt.WakeupMoves[1]))
            {
                wu_wrestlerMedium.Text = styleTaunt.WakeupMoves[1].SkillName;
            }

            if (IsValidTaunt(styleTaunt.WakeupMoves[2]))
            {
                wu_wrestlerHeavy.Text = styleTaunt.WakeupMoves[2].SkillName;
            }

            if (IsValidTaunt(styleTaunt.WakeupMoves[3]))
            {
                wu_wrestlerCritical.Text = styleTaunt.WakeupMoves[3].SkillName;
            }
        }

        private void wu_moveRefresh_Click(object sender, EventArgs e)
        {
            SetValidMoves();
        }
        #endregion

        #region Add Moves
        #region Style
        private void wu_lightAdd_Click(object sender, EventArgs e)
        {
            if (wu_moveResults.Items.Count == 0)
            {
                return;
            }

            Skill skill = (Skill)wu_moveResults.SelectedItem;
            WakeUpTaunt styleTaunt = (WakeUpTaunt)wu_styles.SelectedItem;
            wu_styleLight.Text = skill.SkillName;
            styleTaunt.AddWakeUpMove(skill, 0);
            wu_styles.SelectedItem = styleTaunt;
        }

        private void wu_heavyAdd_Click(object sender, EventArgs e)
        {
            if (wu_moveResults.Items.Count == 0)
            {
                return;
            }

            Skill skill = (Skill)wu_moveResults.SelectedItem;
            WakeUpTaunt styleTaunt = (WakeUpTaunt)wu_styles.SelectedItem;

            wu_styleHeavy.Text = skill.SkillName;
            styleTaunt.AddWakeUpMove(skill, 2);
            wu_styles.SelectedItem = styleTaunt;
        }

        private void wu_mediumAdd_Click(object sender, EventArgs e)
        {
            if (wu_moveResults.Items.Count == 0)
            {
                return;
            }

            Skill skill = (Skill)wu_moveResults.SelectedItem;
            WakeUpTaunt styleTaunt = (WakeUpTaunt)wu_styles.SelectedItem;

            wu_styleMiddle.Text = skill.SkillName;
            styleTaunt.AddWakeUpMove(skill, 1);
            wu_styles.SelectedItem = styleTaunt;
        }

        private void wu_criticalAdd_Click(object sender, EventArgs e)
        {
            if (wu_moveResults.Items.Count == 0)
            {
                return;
            }

            Skill skill = (Skill)wu_moveResults.SelectedItem;
            WakeUpTaunt styleTaunt = (WakeUpTaunt)wu_styles.SelectedItem;

            wu_styleCritical.Text = skill.SkillName;
            styleTaunt.AddWakeUpMove(skill, 3);
            wu_styles.SelectedItem = styleTaunt;
        }

        private void wu_lightRemove_Click(object sender, EventArgs e)
        {
            WakeUpTaunt styleTaunt = (WakeUpTaunt)wu_styles.SelectedItem;
            styleTaunt.RemoveWakeUpMove(0);
            wu_styleLight.Clear();
            wu_styles.SelectedItem = styleTaunt;
        }

        private void wu_heavyRemove_Click(object sender, EventArgs e)
        {
            WakeUpTaunt styleTaunt = (WakeUpTaunt)wu_styles.SelectedItem;
            styleTaunt.RemoveWakeUpMove(2);
            wu_styleHeavy.Clear();
            wu_styles.SelectedItem = styleTaunt;
        }

        private void wu_mediumRemove_Click(object sender, EventArgs e)
        {
            WakeUpTaunt styleTaunt = (WakeUpTaunt)wu_styles.SelectedItem;
            styleTaunt.RemoveWakeUpMove(1);
            wu_styleMiddle.Clear();
            wu_styles.SelectedItem = styleTaunt;
        }

        private void wu_criticalRemove_Click(object sender, EventArgs e)
        {
            WakeUpTaunt styleTaunt = (WakeUpTaunt)wu_styles.SelectedItem;
            styleTaunt.RemoveWakeUpMove(3);
            wu_styleCritical.Clear();
            wu_styles.SelectedItem = styleTaunt;
        }
        #endregion

        #region Wrestlers
        private void wu_wrestlerLightAdd_Click(object sender, EventArgs e)
        {
            if (wu_moveResults.Items.Count == 0)
            {
                return;
            }

            if (wu_wrestlers.SelectedItem == null)
            {
                return;
            }

            Skill skill = (Skill)wu_moveResults.SelectedItem;
            WakeUpTaunt styleTaunt = (WakeUpTaunt)wu_wrestlers.SelectedItem;

            wu_wrestlerLight.Text = skill.SkillName;
            styleTaunt.AddWakeUpMove(skill, 0);
            wu_wrestlers.SelectedItem = styleTaunt;
        }

        private void wu_wrestlerHeavyAdd_Click(object sender, EventArgs e)
        {
            if (wu_moveResults.Items.Count == 0)
            {
                return;
            }

            if (wu_wrestlers.SelectedItem == null)
            {
                return;
            }

            Skill skill = (Skill)wu_moveResults.SelectedItem;
            WakeUpTaunt styleTaunt = (WakeUpTaunt)wu_wrestlers.SelectedItem;

            wu_wrestlerHeavy.Text = skill.SkillName;
            styleTaunt.AddWakeUpMove(skill, 2);
            wu_wrestlers.SelectedItem = styleTaunt;
        }

        private void wu_wrestlerMediumAdd_Click(object sender, EventArgs e)
        {
            if (wu_moveResults.Items.Count == 0)
            {
                return;
            }

            if (wu_wrestlers.SelectedItem == null)
            {
                return;
            }

            Skill skill = (Skill)wu_moveResults.SelectedItem;
            WakeUpTaunt styleTaunt = (WakeUpTaunt)wu_wrestlers.SelectedItem;

            //wu_styleMedium.Text = skill.SkillName;
            styleTaunt.AddWakeUpMove(skill, 1);
            wu_wrestlerMedium.Text = skill.SkillName;
            wu_wrestlers.SelectedItem = styleTaunt;
        }

        private void wu_wrestlerCriticalAdd_Click(object sender, EventArgs e)
        {
            if (wu_moveResults.Items.Count == 0)
            {
                return;
            }

            if (wu_wrestlers.SelectedItem == null)
            {
                return;
            }

            Skill skill = (Skill)wu_moveResults.SelectedItem;
            WakeUpTaunt styleTaunt = (WakeUpTaunt)wu_wrestlers.SelectedItem;

            //wu_styleMedium.Text = skill.SkillName;
            styleTaunt.AddWakeUpMove(skill, 3);
            wu_wrestlerCritical.Text = skill.SkillName;
            wu_wrestlers.SelectedItem = styleTaunt;
        }

        private void wu_wrestlerLightRemove_Click(object sender, EventArgs e)
        {
            WakeUpTaunt styleTaunt = (WakeUpTaunt)wu_wrestlers.SelectedItem;
            styleTaunt.RemoveWakeUpMove(0);
            wu_wrestlerLight.Clear();
        }

        private void wu_wrestlerHeavyRemove_Click(object sender, EventArgs e)
        {
            WakeUpTaunt styleTaunt = (WakeUpTaunt)wu_wrestlers.SelectedItem;
            styleTaunt.RemoveWakeUpMove(2);
            wu_wrestlerHeavy.Clear();
        }

        private void wu_wrestlerMediumRemove_Click(object sender, EventArgs e)
        {
            WakeUpTaunt styleTaunt = (WakeUpTaunt)wu_wrestlers.SelectedItem;
            styleTaunt.RemoveWakeUpMove(1);
            wu_wrestlerMedium.Clear();
        }

        private void wu_wrestlerCriticalRemove_Click(object sender, EventArgs e)
        {
            WakeUpTaunt styleTaunt = (WakeUpTaunt)wu_wrestlers.SelectedItem;
            styleTaunt.RemoveWakeUpMove(3);
            wu_wrestlerCritical.Clear();
        }
        #endregion

        #endregion

        #region Helper Methods

        private void ClearStyleMoves()
        {
            wu_styleLight.Clear();
            wu_styleMiddle.Clear();
            wu_styleHeavy.Clear();
            wu_styleCritical.Clear();
        }
        private void ClearWrestlerMoves()
        {
            wu_wrestlerLight.Clear();
            wu_wrestlerMedium.Clear();
            wu_wrestlerHeavy.Clear();
            wu_wrestlerCritical.Clear();
        }
        private bool IsValidTaunt(Skill taunt)
        {
            return (taunt != null);
        }

        private void wu_moveSearch_LostFocus(object sender, System.EventArgs e)
        {
            if (wu_moveSearch.Text.Trim().Equals(""))
            {
                return;
            }

            wu_moveResults.Items.Clear();
            foreach (Skill skill in wakeUpSkills)
            {
                if (skill.SkillName.Contains(wu_moveResults.Text))
                {
                    wu_moveResults.Items.Add(skill);
                }
            }

            if (wu_moveResults.Items.Count > 0)
            {
                wu_moveResults.SelectedIndex = 0;
            }
        }

        private void wu_addWrestler_Click(object sender, EventArgs e)
        {
            if (wu_wrestlerResults.SelectedItem == null)
            {
                return;
            }

            try
            {
                String wrestler = ((WresIDGroup)wu_wrestlerResults.SelectedItem).Name;
                WakeUpTaunt taunt = new WakeUpTaunt(new QoL_Mods.Data_Classes.Style(wrestler, FightStyleEnum.American));
                wu_wrestlers.Items.Add(taunt);
                if (wu_wrestlers.Items.Count > 0)
                {
                    wu_wrestlers.SelectedIndex = wu_wrestlers.Items.Count - 1;
                }

                wu_wrestlers_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                L.D("Add Wake Up Taunt Wrestler Error: " + ex);
            }


        }

        private void wu_removeWrestler_Click(object sender, EventArgs e)
        {
            if (wu_wrestlers.SelectedItem == null)
            {
                return;
            }

            wu_wrestlers.Items.Remove(wu_wrestlers.SelectedItem);

            if (wu_wrestlers.Items.Count > 0)
            {
                wu_wrestlers.SelectedIndex = 0;
                wu_wrestlerResults_SelectedIndexChanged(null, null);
            }
            else
            {
                ClearWrestlerMoves();
            }
        }

        private void wu_wrestlerSearch_LostFocus(object sender, System.EventArgs e)
        {
            try
            {
                String query = wu_wrestlerSearch.Text;
                wu_wrestlerResults.Items.Clear();

                if (!query.TrimStart().TrimEnd().Equals(""))
                {
                    foreach (WresIDGroup wrestler in wrestlerList)
                    {
                        if (query.ToLower().Equals(wrestler.Name.ToLower()) ||
                            wrestler.Name.ToLower().Contains(query.ToLower()))
                        {
                            wu_wrestlerResults.Items.Add(wrestler);
                        }
                    }
                }

                if (wu_wrestlerResults.Items.Count > 0)
                {
                    wu_wrestlerResults.SelectedIndex = 0;
                }

            }
            catch (Exception ex)
            {
                L.D("Search Error: " + ex.Message);
            }
        }

        #endregion

        #endregion

        #region Setup Methods

        private void LoadSubs()
        {
            try
            {
                this.wu_wrestlerResults.Items.Clear();
                wrestlerList = new List<WresIDGroup>();

                foreach (EditWrestlerData current in SaveData.inst.editWrestlerData)
                {
                    WresIDGroup wresIDGroup = new WresIDGroup();
                    wresIDGroup.Name = DataBase.GetWrestlerFullName(current.wrestlerParam);
                    wresIDGroup.ID = (Int32)current.editWrestlerID;

                    wrestlerList.Add(wresIDGroup);
                    this.wu_wrestlerResults.Items.Add(wresIDGroup);
                }

                if (wu_wrestlerResults.Items.Count > 0)
                {
                    this.wu_wrestlerResults.SelectedIndex = 0;
                }

            }
            catch (Exception ex)
            {
                L.D("Load Subs Exception: " + ex);
            }

        }

        private String CheckSaveFile(String dataType)
        {
            String path = CheckSaveFolder(dataType);

            switch (dataType)
            {

                case "StyleWT":
                    path = path + saveFileNames[0];
                    break;
                case "WrestlerWT":
                    path = path + saveFileNames[1];
                    break;
                default:
                    path = "";
                    break;
            }

            return path;

        }
        private String CheckSaveFolder(String dataType)
        {
            String folder = "";
            switch (dataType)
            {
                default:
                    folder = saveFolderNames[0];
                    break;
            }

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            return folder;

        }
        #endregion
    }
}
