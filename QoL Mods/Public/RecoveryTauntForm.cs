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
        private static Modversion modVersion = Modversion.v2;
        #endregion

        #region Wake Up Taunts
        private void LoadRecoveryTaunts(HashSet<Skill> validTaunts = null)
        {
            String filePath = CheckSaveFile("StyleWT");
            if (File.Exists(filePath))
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    var lines = File.ReadAllLines(filePath);
                    List<string> existingGroups = new List<string>();
                    foreach (String tauntData in lines)
                    {
                        try
                        {
                            WakeUpGroup newGroup = new WakeUpGroup("");
                            String groupName = newGroup.GetTauntDataName(tauntData);

                            if (existingGroups.Contains(groupName))
                            {
                                L.D(groupName + " already exists!");
                                foreach (WakeUpGroup styleGroup in wu_styles.Items)
                                {
                                    if (styleGroup.Name.Equals(groupName))
                                    {
                                        styleGroup.LoadWakeUpData(tauntData, validTaunts, modVersion);
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                L.D(groupName + " is a new WakeUpGroup!");
                                newGroup.LoadWakeUpData(tauntData, validTaunts, modVersion);
                                wu_styles.Items.Add(newGroup);
                                existingGroups.Add(groupName);
                            }
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
                var lines = File.ReadAllLines(filePath);
                List<string> existingGroups = new List<string>();
                foreach (String tauntData in lines)
                {
                    try
                    {
                        WakeUpGroup newGroup = new WakeUpGroup("");
                        String groupName = newGroup.GetTauntDataName(tauntData);

                        if (existingGroups.Contains(groupName))
                        {
                            L.D(groupName + " already exists!");
                            foreach (WakeUpGroup wrestlerGroup in wu_wrestlers.Items)
                            {
                                if (wrestlerGroup.Name.Equals(groupName))
                                {
                                    wrestlerGroup.LoadWakeUpData(tauntData, validTaunts, modVersion);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            L.D(groupName + " is a new WakeUpGroup!");
                            newGroup.LoadWakeUpData(tauntData, validTaunts, modVersion);
                            wu_wrestlers.Items.Add(newGroup);
                            existingGroups.Add(groupName);
                        }
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
                foreach (WakeUpGroup group in wu_styles.Items)
                {
                    foreach (WakeUpTaunt taunt in group.WakeUpTaunts)
                    {
                        sw.WriteLine(taunt.SaveWakeUpData());
                    }
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
                foreach (WakeUpGroup group in wu_wrestlers.Items)
                {
                    foreach (WakeUpTaunt taunt in group.WakeUpTaunts)
                    {
                        sw.WriteLine(taunt.SaveWakeUpData());
                    }
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
        private void RefreshStyleView()
        {
            wu_styles_SelectedIndexChanged(null, null);
        }
        private void RefreshWrestlerView()
        {
            wu_wrestlers_SelectedIndexChanged(null, null);
        }
        private void wu_moveRefresh_Click_1(object sender, EventArgs e)
        {
            SetValidMoves();
        }

        private void wu_styles_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (wu_styles.Items.Count == 0)
                {
                    return;
                }

                ClearStyleMoves();
                WakeUpGroup currentGroup = (WakeUpGroup)wu_styles.SelectedItem;
                foreach (WakeUpTaunt taunt in currentGroup.WakeUpTaunts)
                {
                    Skill lowSkill = taunt.GetSkill(DamageState.Low);
                    Skill mediumSkill = taunt.GetSkill(DamageState.Medium);
                    Skill heavySkill = taunt.GetSkill(DamageState.Heavy);
                    Skill criticalSkill = taunt.GetSkill(DamageState.Critical);

                    if (lowSkill != null)
                    {
                        wu_styleLight.Items.Add(lowSkill);
                    }
                    if (mediumSkill != null)
                    {
                        wu_styleMiddle.Items.Add(mediumSkill);
                    }
                    if (heavySkill != null)
                    {
                        wu_styleHeavy.Items.Add(heavySkill);
                    }
                    if (criticalSkill != null)
                    {
                        wu_styleCritical.Items.Add(criticalSkill);
                    }

                }

                if (wu_styleLight.Items.Count > 0)
                {
                    wu_styleLight.SelectedIndex = 0;
                }
                if (wu_styleMiddle.Items.Count > 0)
                {
                    wu_styleMiddle.SelectedIndex = 0;
                }
                if (wu_styleHeavy.Items.Count > 0)
                {
                    wu_styleHeavy.SelectedIndex = 0;
                }
                if (wu_styleCritical.Items.Count > 0)
                {
                    wu_styleCritical.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                L.D("wu_styles_SelectedIndexChanged Error:" + ex);
            }

        }

        private void wu_wrestlerResults_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void wu_wrestlers_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (wu_wrestlers.SelectedItem == null)
                {
                    return;
                }
                if (wu_wrestlers.Items.Count == 0)
                {
                    return;
                }

                ClearWrestlerMoves();
                WakeUpGroup currentGroup = (WakeUpGroup)wu_wrestlers.SelectedItem;
                L.D("Checking " + currentGroup.Name);

                foreach (WakeUpTaunt taunt in currentGroup.WakeUpTaunts)
                {
                    Skill lowSkill = taunt.GetSkill(DamageState.Low);
                    Skill mediumSkill = taunt.GetSkill(DamageState.Medium);
                    Skill heavySkill = taunt.GetSkill(DamageState.Heavy);
                    Skill criticalSkill = taunt.GetSkill(DamageState.Critical);

                    if (lowSkill != null)
                    {
                        wu_wrestlerLight.Items.Add(lowSkill);
                    }
                    if (mediumSkill != null)
                    {
                        wu_wrestlerMedium.Items.Add(mediumSkill);
                    }
                    if (heavySkill != null)
                    {
                        wu_wrestlerHeavy.Items.Add(heavySkill);
                    }
                    if (criticalSkill != null)
                    {
                        wu_wrestlerCritical.Items.Add(criticalSkill);
                    }
                }

                if (wu_wrestlerLight.Items.Count > 0)
                {
                    wu_wrestlerLight.SelectedIndex = 0;
                }
                if (wu_wrestlerMedium.Items.Count > 0)
                {
                    wu_wrestlerMedium.SelectedIndex = 0;
                }
                if (wu_wrestlerHeavy.Items.Count > 0)
                {
                    wu_wrestlerHeavy.SelectedIndex = 0;
                }
                if (wu_wrestlerCritical.Items.Count > 0)
                {
                    wu_wrestlerCritical.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                L.D("wu_wrestlers_SelectedIndexChanged error: " + ex);
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
            try
            {
                if (wu_moveResults.Items.Count == 0)
                {
                    return;
                }

                Skill skill = (Skill)wu_moveResults.SelectedItem;
                WakeUpGroup currentGroup = (WakeUpGroup)wu_styles.SelectedItem;
                currentGroup.AddWakeUpMove(skill, DamageState.Low);
                wu_styles.SelectedItem = currentGroup;
                RefreshStyleView();
            }
            catch (Exception ex)
            {
                L.D("Adding Error: " + ex);
            }

        }

        private void wu_heavyAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (wu_moveResults.Items.Count == 0)
                {
                    return;
                }

                Skill skill = (Skill)wu_moveResults.SelectedItem;
                WakeUpGroup currentGroup = (WakeUpGroup)wu_styles.SelectedItem;
                currentGroup.AddWakeUpMove(skill, DamageState.Heavy);
                wu_styles.SelectedItem = currentGroup;
                RefreshStyleView();
            }
            catch (Exception ex)
            {
                L.D("Adding Error: " + ex);
            } 
        }

        private void wu_mediumAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (wu_moveResults.Items.Count == 0)
                {
                    return;
                }

                Skill skill = (Skill)wu_moveResults.SelectedItem;
                WakeUpGroup currentGroup = (WakeUpGroup)wu_styles.SelectedItem;
                currentGroup.AddWakeUpMove(skill, DamageState.Medium);
                wu_styles.SelectedItem = currentGroup;
                RefreshStyleView();
            }
            catch (Exception ex)
            {
                L.D("Adding Error: " + ex);
            }     
        }

        private void wu_criticalAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (wu_moveResults.Items.Count == 0)
                {
                    return;
                }

                Skill skill = (Skill)wu_moveResults.SelectedItem;
                WakeUpGroup currentGroup = (WakeUpGroup)wu_styles.SelectedItem;
                currentGroup.AddWakeUpMove(skill, DamageState.Critical);
                wu_styles.SelectedItem = currentGroup;
                RefreshStyleView();
            }
            catch (Exception ex)
            {
                L.D("Adding Error: " + ex);
            }
        }

        private void wu_lightRemove_Click(object sender, EventArgs e)
        {
            try
            {
                WakeUpGroup currentGroup = (WakeUpGroup)wu_styles.SelectedItem;
                Skill skill = (Skill)wu_styleLight.SelectedItem;
                currentGroup.RemoveWakeUpMove(skill, DamageState.Low);
                wu_styles.SelectedItem = currentGroup;
                RefreshStyleView();
            }
            catch (Exception ex)
            {
                L.D("Removing Error: " + ex);
            }
        }

        private void wu_heavyRemove_Click(object sender, EventArgs e)
        {
            try
            {
                WakeUpGroup currentGroup = (WakeUpGroup)wu_styles.SelectedItem;
                Skill skill = (Skill)wu_styleHeavy.SelectedItem;
                currentGroup.RemoveWakeUpMove(skill, DamageState.Heavy);
                wu_styles.SelectedItem = currentGroup;
                RefreshStyleView();
            }
            catch (Exception ex)
            {
                L.D("Removing Error: " + ex);
            }
        }

        private void wu_mediumRemove_Click(object sender, EventArgs e)
        {
            try
            {
                WakeUpGroup currentGroup = (WakeUpGroup)wu_styles.SelectedItem;
                Skill skill = (Skill)wu_styleMiddle.SelectedItem;
                currentGroup.RemoveWakeUpMove(skill, DamageState.Medium);
                wu_styles.SelectedItem = currentGroup;
                RefreshStyleView();
            }
            catch (Exception ex)
            {
                L.D("Removing Error: " + ex);
            }
        }

        private void wu_criticalRemove_Click(object sender, EventArgs e)
        {
            try
            {
                WakeUpGroup currentGroup = (WakeUpGroup)wu_styles.SelectedItem;
                Skill skill = (Skill)wu_styleCritical.SelectedItem;
                currentGroup.RemoveWakeUpMove(skill, DamageState.Critical);
                wu_styles.SelectedItem = currentGroup;
                RefreshStyleView();
            }
            catch (Exception ex)
            {
                L.D("Removing Error: " + ex);
            } 
        }
        #endregion

        #region Wrestlers
        private void wu_wrestlerLightAdd_Click(object sender, EventArgs e)
        {
            try
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
                WakeUpGroup currentGroup = (WakeUpGroup)wu_wrestlers.SelectedItem;
                currentGroup.AddWakeUpMove(skill, DamageState.Low);
                wu_wrestlers.SelectedItem = currentGroup;
                RefreshWrestlerView();
            }
            catch (Exception ex)
            {
                L.D("Adding Error: " + ex);
            }

        }

        private void wu_wrestlerHeavyAdd_Click(object sender, EventArgs e)
        {
            try
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
                WakeUpGroup currentGroup = (WakeUpGroup)wu_wrestlers.SelectedItem;
                currentGroup.AddWakeUpMove(skill, DamageState.Heavy);
                wu_wrestlers.SelectedItem = currentGroup;
                RefreshWrestlerView();
            }
            catch (Exception ex)
            {
                L.D("Adding Error: " + ex);
            }

        }

        private void wu_wrestlerMediumAdd_Click(object sender, EventArgs e)
        {
            try
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
                WakeUpGroup currentGroup = (WakeUpGroup)wu_wrestlers.SelectedItem;
                currentGroup.AddWakeUpMove(skill, DamageState.Medium);
                wu_wrestlers.SelectedItem = currentGroup;
                RefreshWrestlerView();
            }
            catch (Exception ex)
            {
                L.D("Adding Error: " + ex);
            }

        }

        private void wu_wrestlerCriticalAdd_Click(object sender, EventArgs e)
        {
            try
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
                WakeUpGroup currentGroup = (WakeUpGroup)wu_wrestlers.SelectedItem;
                currentGroup.AddWakeUpMove(skill, DamageState.Critical);
                wu_wrestlers.SelectedItem = currentGroup;
                RefreshWrestlerView();
            }
            catch (Exception ex)
            {
                L.D("Adding Error: " + ex);
            }

        }

        private void wu_wrestlerLightRemove_Click(object sender, EventArgs e)
        {
            try
            {
                WakeUpGroup currentGroup = (WakeUpGroup)wu_wrestlers.SelectedItem;

                Skill skill = (Skill)wu_wrestlerLight.SelectedItem;
                currentGroup.RemoveWakeUpMove(skill, DamageState.Low);
                wu_wrestlers.SelectedItem = currentGroup;
                RefreshWrestlerView();
            }
            catch (Exception ex)
            {
                L.D("Removing Error: " + ex);
            }

        }

        private void wu_wrestlerHeavyRemove_Click(object sender, EventArgs e)
        {
            try
            {
                WakeUpGroup currentGroup = (WakeUpGroup)wu_wrestlers.SelectedItem;

                Skill skill = (Skill)wu_wrestlerHeavy.SelectedItem;
                currentGroup.RemoveWakeUpMove(skill, DamageState.Heavy);
                wu_wrestlers.SelectedItem = currentGroup;
                RefreshWrestlerView();
            }
            catch (Exception ex)
            {
                L.D("Removing Error: " + ex);
            }

        }

        private void wu_wrestlerMediumRemove_Click(object sender, EventArgs e)
        {
            try
            {
                WakeUpGroup currentGroup = (WakeUpGroup)wu_wrestlers.SelectedItem;

                Skill skill = (Skill)wu_wrestlerMedium.SelectedItem;
                currentGroup.RemoveWakeUpMove(skill, DamageState.Medium);
                wu_wrestlers.SelectedItem = currentGroup;
                RefreshWrestlerView();
            }
            catch (Exception ex)
            {
                L.D("Removing Error: " + ex);
            }

        }

        private void wu_wrestlerCriticalRemove_Click(object sender, EventArgs e)
        {
            try
            {
                WakeUpGroup currentGroup = (WakeUpGroup)wu_wrestlers.SelectedItem;

                Skill skill = (Skill)wu_wrestlerCritical.SelectedItem;
                currentGroup.RemoveWakeUpMove(skill, DamageState.Critical);
                wu_wrestlers.SelectedItem = currentGroup;
                RefreshWrestlerView();
            }
            catch (Exception ex)
            {
                L.D("Removing Error: " + ex);
            }
        }
        #endregion

        #endregion

        #region Helper Methods

        private void ClearStyleMoves()
        {
            wu_styleLight.Items.Clear();
            wu_styleMiddle.Items.Clear();
            wu_styleHeavy.Items.Clear();
            wu_styleCritical.Items.Clear();
        }
        private void ClearWrestlerMoves()
        {
            wu_wrestlerLight.Items.Clear();
            wu_wrestlerMedium.Items.Clear();
            wu_wrestlerHeavy.Items.Clear();
            wu_wrestlerCritical.Items.Clear();
        }
        private bool IsValidTaunt(Skill taunt)
        {
            return (taunt != null);
        }
        private HashSet<Skill> GetValidTaunts()
        {
            HashSet<Skill> wakeUpSkills = new HashSet<Skill>();
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
                            L.D("Added move " + DataBase.GetSkillName(current.Key) + " with " + (Int32)current.Key);
                        }
                    }
                }
                catch
                {

                }
            }
            return wakeUpSkills;
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
                WakeUpGroup group = new WakeUpGroup(wrestler);
                wu_wrestlers.Items.Add(group);
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

        private void wu_moveReload_Click(object sender, EventArgs e)
        {
            wu_styles.Items.Clear();
            wu_wrestlers.Items.Clear();
            LoadRecoveryTaunts(GetValidTaunts());
        }
    }
}
