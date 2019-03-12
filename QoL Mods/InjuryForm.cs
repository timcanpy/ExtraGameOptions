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
    public partial class InjuryForm : Form
    {
        public InjuryForm()
        {
            InitializeComponent();
            LoadSubs();
            LoadInjuryData();
        }

        #region Variables
        private static String[] injuryTypes = new String[] { "Healthy", "Bruised", "Sprained", "Hurt", "Injured", "Broken" };
        private static int injuryFloor = 3;
        private static int injuryCeiling = injuryTypes.Length;
        private static String[] saveFileNames = new String[] { "InjuryData.dat", "Options.dat", "RatingsData.dat", "RingData.dat", "StyleFL.dat", "WrestlerFL.dat" };
        private static String[] saveFolderNames = new String[] { "./EGOData/" };
        private List<WresIDGroup> wrestlerList = new List<WresIDGroup>();
#endregion

        #region Data Load
        private void LoadSubs()
        {
            try
            {

                wrestlerList.Clear();
                int index = 0;

                foreach (EditWrestlerData current in SaveData.inst.editWrestlerData)
                {
                    WresIDGroup wresIDGroup = new WresIDGroup();
                    wresIDGroup.Name = DataBase.GetWrestlerFullName(current.wrestlerParam);
                    //wresIDGroup.ID = (Int32)WrestlerID.EditWrestlerIDTop + SaveData.inst.editWrestlerData.IndexOf(current);
                    wresIDGroup.ID = (Int32)current.editWrestlerID;
                    wresIDGroup.Group = current.wrestlerParam.groupID;
                    index = SaveData.inst.editWrestlerData.IndexOf(current);

                    //Set the subscription ID for the wrestler
                    foreach (SubscribeItemInfo sub in SaveData.inst.subscribeItemInfoData)
                    {
                        if (sub.GetItemType() == Workshop_Item.Wrestler)
                        {
                            if (sub.GetItemId() == index)
                            {
                                wresIDGroup.Info = sub;
                            }
                        }
                    }

                    wrestlerList.Add(wresIDGroup);
                    this.ij_result.Items.Add(wresIDGroup);
                }
            }
            catch
            {

            }

        }
        public void LoadInjuryData()
        {
            String filePath = CheckSaveFile("Injury");

            if (File.Exists(filePath))
            {
                ij_wrestlerList.Items.Clear();

                using (StreamReader sr = new StreamReader(filePath))
                {
                    var lines = File.ReadAllLines(filePath);

                    int i = 0, matchCount = 0;
                    float recoveryRate = 1;
                    String name, matches, recovery, neckHP, bodyHP, armHP, legHP;
                    DateTime recoveryDate = DateTime.Now;

                    while (i < lines.Length)
                    {
                        name = lines[i];
                        matches = lines[i + 1];
                        recovery = lines[i + 2];
                        neckHP = ValidateInjuryState(lines[i + 3]);
                        bodyHP = ValidateInjuryState(lines[i + 4]);
                        armHP = ValidateInjuryState(lines[i + 5]);
                        legHP = ValidateInjuryState(lines[i + 6]);
                        recoveryDate = DateTime.Parse(lines[i + 7]);

                        //Validate Match Count and Recovery Rate
                        float.TryParse(recovery, out recoveryRate);
                        int.TryParse(matches, out matchCount);

                        i += 8;

                        //Determine if this wrestler has healed
                        if (recoveryDate > DateTime.Now)
                        {
                            ij_wrestlerList.Items.Add(new WrestlerHealth(name, neckHP, bodyHP, armHP, legHP, recoveryRate, matchCount, recoveryDate));
                        }
                    }
                }

                //Update controls for the initial index
                if (ij_wrestlerList.Items.Count == 0)
                {
                    return;
                }
                ij_wrestlerList.SelectedIndex = 0;
                SetUIElements();
            }
        }
        #endregion

        #region Injury Methods
        private string CheckSaveFile(string v)
        {
            throw new NotImplementedException();
        }

        public void SaveInjuryData()
        {
            String filePath = CheckSaveFile("Injury");

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            using (StreamWriter sw = File.AppendText(filePath))
            {
                foreach (WrestlerHealth health in ij_wrestlerList.Items)
                {
                    sw.WriteLine(health.Name);
                    sw.WriteLine(health.MatchCount);
                    sw.WriteLine(health.RecoveryRate);
                    sw.WriteLine(health.NeckHealth);
                    sw.WriteLine(health.BodyHealth);
                    sw.WriteLine(health.ArmHealth);
                    sw.WriteLine(health.LegHealth);
                    sw.WriteLine(health.ReturnDate);
                }
            }
        }

        private void ij_highRisk_CheckedChanged(object sender, EventArgs e)
        {
            if (ij_highRisk.Checked)
            {
                em_injuries.Checked = true;
            }
        }
        private void em_injuries_CheckedChanged(object sender, EventArgs e)
        {
            if (!em_injuries.Checked)
            {
                ij_highRisk.Checked = false;
            }
        }
        private void ij_wrestlerSearch_LostFocus(object sender, System.EventArgs e)
        {
            ij_Search();
        }
        private void ij_recoveryRate_LostFocus(object sender, System.EventArgs e)
        {
            float recoveryRate = 1;
            float.TryParse(ij_recoveryRate.Text, out recoveryRate);
            ij_recoveryRate.Text = recoveryRate.ToString();
            WrestlerHealth health = (WrestlerHealth)ij_wrestlerList.SelectedItem;
            health.RecoveryRate = recoveryRate;
            ij_wrestlerList.SelectedItem = health;
        }
        private void ij_Search()
        {
            String wrestlerName = ij_wrestlerSearch.Text.ToLower();
            if (wrestlerName.Trim().Equals(""))
            {
                return;
            }

            ij_result.Items.Clear();
            foreach (WresIDGroup wrestler in wrestlerList)
            {
                if (wrestler.Name.ToLower().Contains(wrestlerName))
                {
                    ij_result.Items.Add(wrestler.Name);
                }
            }

            if (ij_result.Items.Count > 0)
            {
                ij_result.SelectedIndex = 0;
            }
        }
        private void ij_add_Click(object sender, EventArgs e)
        {
            WrestlerHealth wrestler = new WrestlerHealth(ij_result.SelectedItem.ToString(), "Healthy", "Healthy", "Healthy", "Healthy", 1, 0, DateTime.Today);
            ij_wrestlerList.Items.Add(wrestler);
            if (ij_wrestlerList.Items.Count == 1)
            {
                ij_wrestlerList.SelectedIndex = 0;
                ij_wrestlerList_SelectedIndexChanged(sender, e);
            }
        }
        private void ij_removeOne_Click(object sender, EventArgs e)
        {
            if (ij_wrestlerList.Items.Count == 0)
            {
                return;
            }
            try
            {
                WrestlerHealth health = (WrestlerHealth)ij_wrestlerList.SelectedItem;
                WrestlerHealth[] items = new WrestlerHealth[ij_wrestlerList.Items.Count];
                ij_wrestlerList.Items.CopyTo(items, 0);
                ij_wrestlerList.Items.Clear();
                foreach (WrestlerHealth item in items)
                {
                    if (item != health)
                    {
                        ij_wrestlerList.Items.Add(item);
                    }
                }

                if (ij_wrestlerList.Items.Count > 0)
                {
                    ij_wrestlerList.SelectedIndex = 0;
                    ij_wrestlerList_SelectedIndexChanged(sender, e);
                }
            }
            catch (Exception ex)
            {
                L.D("Error: " + ex.Message + ": " + ij_wrestlerList.SelectedIndex);
            }
        }
        private void ij_removeAll_Click(object sender, EventArgs e)
        {
            ij_wrestlerList.Items.Clear();
        }
        private void ij_wrestlerList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Update interface values to show current wrestler data
            SetUIElements();
        }
        private void ij_neckHP_LostFocus(object sender, EventArgs e)
        {
            WrestlerHealth health = (WrestlerHealth)ij_wrestlerList.SelectedItem;
            health.NeckHealth = ij_neckHP.SelectedItem.ToString();
            health = SetRecoveryTime(health, health.NeckHealth, GetWrestlerEndurance("Neck", health.Name));
            ij_wrestlerList.SelectedItem = health;
            ij_recoveryDate.Text = health.ReturnDate.ToShortDateString();
        }
        private void ij_armHP_LostFocus(object sender, EventArgs e)
        {
            WrestlerHealth health = (WrestlerHealth)ij_wrestlerList.SelectedItem;
            health.ArmHealth = ij_armHP.SelectedItem.ToString();
            health = SetRecoveryTime(health, health.ArmHealth, GetWrestlerEndurance("Arm", health.Name));
            ij_wrestlerList.SelectedItem = health;
            ij_recoveryDate.Text = health.ReturnDate.ToShortDateString();
        }
        private void ij_bodyHP_LostFocus(object sender, EventArgs e)
        {
            WrestlerHealth health = (WrestlerHealth)ij_wrestlerList.SelectedItem;
            health.BodyHealth = ij_bodyHP.SelectedItem.ToString();
            health = SetRecoveryTime(health, health.BodyHealth, GetWrestlerEndurance("Body", health.Name));
            ij_wrestlerList.SelectedItem = health;
            ij_recoveryDate.Text = health.ReturnDate.ToShortDateString();
        }
        private void ij_legHP_LostFocus(object sender, EventArgs e)
        {
            WrestlerHealth health = (WrestlerHealth)ij_wrestlerList.SelectedItem;
            health.LegHealth = ij_legHP.SelectedItem.ToString();
            health = SetRecoveryTime(health, health.LegHealth, GetWrestlerEndurance("Leg", health.Name));
            ij_wrestlerList.SelectedItem = health;
            ij_recoveryDate.Text = health.ReturnDate.ToShortDateString();
        }
        private void RemoveWrestler(WrestlerHealth healthInfo)
        {
            if (ij_wrestlerList.Items.Count == 0)
            {
                return;
            }
            ij_wrestlerList.Items.Remove(healthInfo);
        }
        public void UpdateWrestlerHealthInfo(WrestlerHealth healthInfo)
        {
            //Determine if wrestler exists
            WrestlerHealth wrestler = GetWrestlerHealthInfo(healthInfo.Name);
            if (wrestler != null)
            {
                int index = ij_wrestlerList.Items.IndexOf(healthInfo);
                ij_wrestlerList.Items[index] = healthInfo;
            }
            else
            {
                ij_wrestlerList.Items.Add(healthInfo);
            }
        }
        private void SetUIElements()
        {
            WrestlerHealth wrestler = (WrestlerHealth)ij_wrestlerList.SelectedItem;
            ij_neckHP.SelectedItem = wrestler.NeckHealth;
            ij_bodyHP.SelectedItem = wrestler.BodyHealth;
            ij_armHP.SelectedItem = wrestler.ArmHealth;
            ij_legHP.SelectedItem = wrestler.LegHealth;
            ij_recoveryRate.Text = wrestler.RecoveryRate.ToString();
            ij_matches.Text = wrestler.MatchCount.ToString();
            ij_recoveryDate.Text = wrestler.ReturnDate.ToShortDateString();
        }
        public WrestlerHealth GetWrestlerHealthInfo(String wrestlerName)
        {
            WrestlerHealth healthInfo = null;
            try
            {
                foreach (WrestlerHealth wrestlerHealth in ij_wrestlerList.Items)
                {
                    if (wrestlerHealth.Name.Equals(wrestlerName))
                    {
                        healthInfo = wrestlerHealth;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                L.D("GetWrestlerHealthInfo:" + ex.ToString());
            }

            return healthInfo;
        }
        public WrestlerHealth SetRecoveryTime(WrestlerHealth healthInfo, String injuryType, int partEndurance)
        {
            //Determine if we should reset the return date
            if (injuryType.Equals(injuryTypes[0]))
            {
                if (healthInfo.NeckHealth.Equals(injuryTypes[0]) && healthInfo.BodyHealth.Equals(injuryTypes[0]) && healthInfo.ArmHealth.Equals(injuryTypes[0]) && healthInfo.LegHealth.Equals(injuryTypes[0]))
                {
                    healthInfo = ResetReturnDate(healthInfo);
                    return healthInfo;
                }
            }

            float modifier = healthInfo.RecoveryRate;
            int index = Array.FindIndex(injuryTypes, row => row.Contains(injuryType));
            float rngValue = UnityEngine.Random.Range((injuryFloor) * 2 - (partEndurance * modifier), (injuryCeiling * 2) - (partEndurance * modifier));
            if (rngValue < 0)
            {
                rngValue = 1;
            }
            healthInfo.ReturnDate = healthInfo.ReturnDate.AddDays(rngValue);
            return healthInfo;
        }
        private WrestlerHealth ResetReturnDate(WrestlerHealth healthInfo)
        {
            healthInfo.ReturnDate = DateTime.Now;
            return healthInfo;
        }
        private String ValidateInjuryState(String state)
        {
            foreach (String injury in injuryTypes)
            {
                if (injury.Equals(state))
                {
                    return state;
                }
            }
            return injuryTypes[0];
        }
        private void ij_save_Click(object sender, EventArgs e)
        {
            SaveInjuryData();
        }
        private void ij_load_Click(object sender, EventArgs e)
        {
            LoadInjuryData();
        }
        private int GetWrestlerEndurance(String part, String wrestlerName)
        {
            WrestlerID id = 0;
            foreach (WresIDGroup wrestler in wrestlerList)
            {
                if (wrestler.Name.Equals(wrestlerName))
                {
                    id = (WrestlerID)wrestler.ID;
                }
            }

            if (id == 0)
            {
                return 0;
            }
            else
            {
                int endurance = 0;

                switch (part)
                {
                    case "Neck":
                        endurance = DataBase.GetWrestlerParam(id).defNeck;
                        break;
                    case "Body":
                        endurance = DataBase.GetWrestlerParam(id).defWaist;
                        break;
                    case "Arm":
                        endurance = DataBase.GetWrestlerParam(id).defArm;
                        break;
                    case "Leg":
                        endurance = DataBase.GetWrestlerParam(id).defLeg;
                        break;
                }
                return endurance;
            }
        }
        #endregion
    }
}
