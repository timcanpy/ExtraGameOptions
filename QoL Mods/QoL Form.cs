using System;
using System.Windows.Forms;
using UnityEngine;
using System.Collections.Generic;
using DG;
using MatchConfig;
using QoL_Mods.Data_Classes;
using System.IO;

namespace QoL_Mods
{
    public partial class QoL_Form : Form
    {
        #region Variables
        private List<WresIDGroup> wrestlerList = new List<WresIDGroup>();
        private static List<String> promotionList = new List<string>();
        private static List<RefereeData> refereeList = new List<RefereeData>();
        private static String[] injuryTypes = new String[] { "Healthy", "Bruised", "Sprained", "Hurt", "Injured", "Broken" };
        private static int injuryFloor = 3;
        private static int injuryCeiling = injuryTypes.Length;
        public static QoL_Form form = null;
        private static SteamWorkshop_UGC workShop = null;
        private static List<WrestlerHealth> healthList = new List<WrestlerHealth>();
        private static String[] saveFileNames = new String[] { "InjuryData.dat" };
        private static String[] saveFolderNames = new String[] { "./EGOData/" };
        #endregion

        public void QoL_Form_Load(object sender, EventArgs e)
        {

        }
        public QoL_Form()
        {
            form = this;
            InitializeComponent();
            LoadOrgs();
            LoadSubs();
            LoadReferees();
            LoadLogicThreshold();
            LoadRings();
            LoadVenues();
            LoadHealth();
            LoadInjuryData();
            FormClosing += QoLForm_FormClosing;
            ////
            //ego_MainTabs.TabPages.RemoveAt(1);
            //overrideTabs.TabPages.RemoveAt(0);
        }
        public void LoadOrgs()
        {
            promotionList.Clear();
            this.we_promotionBox.Items.Clear();

            foreach (String promotion in MatchConfiguration.LoadPromotions())
            {
                //string longName = SaveData.GetInst().organizationList[current.organizationID].longName;
                //this.we_promotionBox.Items.Add(longName + " : " + current.longName);
                //promotionList.Add(longName + " : " + current.longName);
                this.we_promotionBox.Items.Add(promotion);
                promotionList.Add(promotion);
            }
        }
        public void LoadRings()
        {
            or_ringList.Items.Clear();
            foreach (String ring in MatchConfiguration.LoadRings())
            {
                or_ringList.Items.Add(ring);
            }

            or_ringList.SelectedIndex = 0;
        }
        public void LoadVenues()
        {
            or_venueList.Items.Clear();
            foreach (String venue in MatchConfiguration.LoadVenue())
            {
                or_venueList.Items.Add(venue);
            }

            or_venueList.SelectedIndex = 0;
        }
        private void LoadSubs()
        {
            this.we_resultList.Items.Clear();
            wrestlerList.Clear();
            int index = 0;

            foreach (EditWrestlerData current in SaveData.inst.editWrestlerData)
            {
                WresIDGroup wresIDGroup = new WresIDGroup();
                wresIDGroup.Name = DataBase.GetWrestlerFullName(current.wrestlerParam);
                wresIDGroup.ID = (Int32)WrestlerID.EditWrestlerIDTop + SaveData.inst.editWrestlerData.IndexOf(current);
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
                this.we_resultList.Items.Add(wresIDGroup);
            }

            this.we_resultList.SelectedIndex = 0;
        }
        private void LoadReferees()
        {
            refereeList.Clear();
            we_refList.Items.Clear();
            foreach (RefereeData referee in SaveData.GetInst().editRefereeData)
            {
                we_refList.Items.Add(referee.Prm.name);
                refereeList.Add(referee);
            }

            we_refList.SelectedIndex = 0;
        }
        private void LoadHealth()
        {
            ij_neckHP.Items.Clear();
            ij_bodyHP.Items.Clear();
            ij_armHP.Items.Clear();
            ij_legHP.Items.Clear();

            foreach (String injury in injuryTypes)
            {
                ij_neckHP.Items.Add(injury);
                ij_bodyHP.Items.Add(injury);
                ij_armHP.Items.Add(injury);
                ij_legHP.Items.Add(injury);
            }
        }

        #region Wrestler Management
        private void we_search_Click(object sender, EventArgs e)
        {
            try
            {
                String query = we_searchBox.Text;
                we_resultList.Items.Clear();

                if (!query.TrimStart().TrimEnd().Equals(""))
                {
                    foreach (WresIDGroup wrestler in wrestlerList)
                    {
                        if (query.ToLower().Equals(wrestler.Name.ToLower()) || wrestler.Name.ToLower().Contains(query.ToLower()))
                        {
                            we_resultList.Items.Add(wrestler);
                        }
                    }
                }

                if (we_resultList.Items.Count > 0)
                {
                    we_resultList.SelectedIndex = 0;
                    return;
                }

                if (we_promotionBox.SelectedItem.ToString().Contains("未登録"))
                {
                    this.LoadSubs();
                    return;
                }

                foreach (WresIDGroup current in wrestlerList)
                {
                    if (current.Group == FindGroup(we_promotionBox.SelectedItem.ToString()))
                    {
                        L.D("Adding " + current.Name);
                        we_resultList.Items.Add(current);
                    }
                }

                if (we_resultList.Items.Count > 0)
                {
                    we_resultList.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                L.D("Search Error: " + ex.Message);
            }
        }
        private void we_edit_Click(object sender, EventArgs e)
        {
            try
            {
                //Menu_SceneManager manager = new Menu_SceneManager();
                CreateMenu_SceneManager manager = new CreateMenu_SceneManager();
                WresIDGroup wrestler = (WresIDGroup)we_resultList.SelectedItem;
                WrestlerID wID = (WrestlerID)wrestler.ID;

                if (global::CreateMenu_SceneManager.edit_data == null)
                {
                    global::CreateMenu_SceneManager.edit_data = new EditWrestlerData();
                }
                if (global::CreateMenu_SceneManager.edit_data.criticalMoveName == null)
                {
                    global::CreateMenu_SceneManager.edit_data.criticalMoveName = string.Empty;
                }

                if (global::CreateMenu_SceneManager.edit_data.ThemeMusic_Filename == null)
                {
                    global::CreateMenu_SceneManager.edit_data.ThemeMusic_Filename = string.Empty;
                }
                global::CreateMenu_SceneManager.edit_data.appearanceData.Set(global::DataBase.GetAppearanceData(wID));
                global::CreateMenu_SceneManager.data_num = wID;
                global::CreateMenu_SceneManager.edit_data = global::SaveData.GetInst().GetEditWrestlerData(wID).Clone();
                global::CreateMenu_SceneManager.flg_Overwrite = true;
                global::CreateMenu_SceneManager.user_edit_point = 380;
                global::CreateMenu_SceneManager.presetEditMode = false;
                MyMusic.Init();
                UnityEngine.SceneManagement.SceneManager.LoadScene("Scene_WrestlerEditMenu");

                if (this.we_unsubscribe.Checked)
                {
                    Unsubcribe(wID);
                }
            }
            catch (Exception ex)
            {
                L.D("Edit Click Error: " + ex.StackTrace);
            }

        }
        private void btn_mainMenu_Click(object sender, EventArgs e)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Scene_MainMenu");
        }
        private void Unsubcribe(WrestlerID wrestlerID)
        {
            //Get current wrestler from list
            WresIDGroup wrestler = null;
            bool workshopAdded = false;
            foreach (WresIDGroup edit in wrestlerList)
            {
                if (wrestlerID == (WrestlerID)edit.ID)
                {
                    wrestler = edit;
                    break;
                }
            }

            //Remove subscription if it exists
            if (wrestler.Info == null)
            {
                L.D("No subscription exists for " + wrestler.Name);
                return;
            }

            //Load the Steam Workshop item
            GameObject[] array = UnityEngine.Object.FindObjectsOfType<GameObject>();
            if (array.Length != 0)
            {
                try
                {
                    workShop = array[0].GetComponent<SteamWorkshop_UGC>();
                    if (workShop == null)
                    {
                        workShop = array[0].AddComponent<SteamWorkshop_UGC>();
                        workshopAdded = true;
                    }

                    if (workShop != null)
                    {
                        workShop.UnsubscribeItem(wrestler.Info.GetPublishFileId());
                        SaveData.inst.subscribeItemInfoData.Remove(wrestler.Info);
                        wrestler.Info = null;
                        L.D("Successfully unsubscribed " + wrestler.Name);
                    }
                    else
                    {
                        L.D("Unable to unsubscribe for " + wrestler.Name);
                    }
                }
                catch (Exception ex)
                {
                    L.D("Unsubcribe Error: " + ex.Message);
                }
                finally
                {
                    if (workshopAdded)
                    {
                        UnityEngine.Object.Destroy(workShop);
                        workShop = null;
                    }
                }
            }
        }
        private void we_refresh_Click(object sender, EventArgs e)
        {
            LoadSubs();
            LoadOrgs();
            LoadReferees();
        }
        private void we_SearchBox_LostFocus(object sender, System.EventArgs e)
        {
            we_search_Click(sender, e);
        }
        private void we_promotionBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            we_search_Click(sender, e);
        }
        private void ij_wrestlerSearch_LostFocus(object sender, System.EventArgs e)
        {
            ij_Search();
        }
        private void ij_recoveryRate_LostFocus(object sender, System.EventArgs e)
        {
            float recoveryRate = 1;
            if (healthList.Count == 0)
            {
                return;
            }
            float.TryParse(ij_recoveryRate.Text, out recoveryRate);
            ij_recoveryRate.Text = recoveryRate.ToString();
            healthList[ij_wrestlerList.SelectedIndex].RecoveryRate = recoveryRate;
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
        private int FindGroup(String groupName)
        {
            return promotionList.IndexOf(groupName);
        }
        #endregion

        #region AI Behaviour
        #endregion

        #region Overrides

        private void refEdit_Click(object sender, EventArgs e)
        {
            try
            {

                RefereeData referee = refereeList[we_refList.SelectedIndex];
                global::Menu_RefereeCreate.referee_data = referee;
                global::CreateMenu_SceneManager.flg_Overwrite = true;
                UnityEngine.SceneManagement.SceneManager.LoadScene("Scene_RefereeEdit");
            }
            catch (Exception ex)
            {
                L.D("Create Ref Error: " + ex.Message);
            }
        }

        private void re_refresh_Click(object sender, EventArgs e)
        {
            LoadReferees();
        }

        private void btn_bumpRef_Click(object sender, EventArgs e)
        {
            Referee mref = RefereeMan.inst.GetRefereeObj();
            mref.mRefereeClash();
        }

        private void LoadLogicThreshold()
        {
            for (int i = 100; i > 0; i--)
            {
                refBum_minLogic.Items.Add(i);
            }
            refBum_minLogic.SelectedIndex = 0;
        }
        #endregion

        private void ij_add_Click(object sender, EventArgs e)
        {
            WrestlerHealth wrestler = new WrestlerHealth(ij_result.SelectedItem.ToString(), "Healthy", "Healthy", "Healthy", "Healthy", 1, 0);
            ij_wrestlerList.Items.Add(wrestler);
            healthList.Add(wrestler);
        }

        private void ij_removeOne_Click(object sender, EventArgs e)
        {
            WrestlerHealth wrestler = (WrestlerHealth)ij_wrestlerList.SelectedItem;
            RemoveWrestler(wrestler.Name);

        }

        private void ij_removeAll_Click(object sender, EventArgs e)
        {
            ij_wrestlerList.Items.Clear();
            healthList.Clear();
        }

        private void ij_wrestlerList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Update interface values to show current wrestler data
            WrestlerHealth wrestler = healthList[ij_wrestlerList.SelectedIndex];
            ij_neckHP.SelectedItem = wrestler.NeckHealth;
            ij_bodyHP.SelectedItem = wrestler.BodyHealth;
            ij_armHP.SelectedItem = wrestler.ArmHealth;
            ij_legHP.SelectedItem = wrestler.LegHealth;
            ij_recoveryRate.Text = wrestler.RecoveryRate.ToString();
            ij_matches.Text = wrestler.MatchCount.ToString();
            ij_recoveryDate.Text = wrestler.ReturnDate.ToShortDateString();
        }

        private void ij_neckHP_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (healthList.Count == 0)
            {
                return;
            }
            
            healthList[ij_wrestlerList.SelectedIndex].NeckHealth = ij_neckHP.SelectedItem.ToString();
            healthList[ij_wrestlerList.SelectedIndex] = SetRecoveryTime(healthList[ij_wrestlerList.SelectedIndex], healthList[ij_wrestlerList.SelectedIndex].NeckHealth, GetWrestlerEndurance("Neck",healthList[ij_wrestlerList.SelectedIndex].Name));
            ij_wrestlerList_SelectedIndexChanged(null, null);
        }

        private void ij_armHP_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (healthList.Count == 0)
            {
                return;
            }
            healthList[ij_wrestlerList.SelectedIndex].ArmHealth = ij_armHP.SelectedItem.ToString();
            healthList[ij_wrestlerList.SelectedIndex] = SetRecoveryTime(healthList[ij_wrestlerList.SelectedIndex], healthList[ij_wrestlerList.SelectedIndex].ArmHealth, GetWrestlerEndurance("Arm", healthList[ij_wrestlerList.SelectedIndex].Name));
            ij_wrestlerList_SelectedIndexChanged(null, null);
        }

        private void ij_bodyHP_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (healthList.Count == 0)
            {
                return;
            }
            healthList[ij_wrestlerList.SelectedIndex].BodyHealth = ij_bodyHP.SelectedItem.ToString();
            healthList[ij_wrestlerList.SelectedIndex] = SetRecoveryTime(healthList[ij_wrestlerList.SelectedIndex], healthList[ij_wrestlerList.SelectedIndex].BodyHealth, GetWrestlerEndurance("Body", healthList[ij_wrestlerList.SelectedIndex].Name));
            ij_wrestlerList_SelectedIndexChanged(null, null);
        }

        private void ij_legHP_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (healthList.Count == 0)
            {
                return;
            }
            healthList[ij_wrestlerList.SelectedIndex].LegHealth = ij_legHP.SelectedItem.ToString();
            healthList[ij_wrestlerList.SelectedIndex] = SetRecoveryTime(healthList[ij_wrestlerList.SelectedIndex], healthList[ij_wrestlerList.SelectedIndex].LegHealth, GetWrestlerEndurance("Leg", healthList[ij_wrestlerList.SelectedIndex].Name));
            ij_wrestlerList_SelectedIndexChanged(null, null);
        }

        public void RemoveWrestler(String wrestlerName)
        {
            if (healthList.Count == 0)
            {
                return;
            }

            try
            {
                for (int i = 0; i < healthList.Count; i++)
                {
                    if (healthList[i].Name.Equals(wrestlerName))
                    {
                        healthList.RemoveAt(i);
                    }
                    WrestlerHealth health = (WrestlerHealth)ij_wrestlerList.Items[i];
                    if (health.Name.Equals(wrestlerName))
                    {
                        ij_wrestlerList.Items.RemoveAt(i);
                    }
                }
            }
            catch (Exception ex)
            {
                L.D("Injury Remove Error: " + ex.Message);
            }
        }

        public void UpdateWrestlerHealthInfo(WrestlerHealth healthInfo)
        {
            //Determine if wrestler exists
            WrestlerHealth wrestler = GetWrestlerHealthInfo(healthInfo.Name);
            if (wrestler != null)
            {
                int index = healthList.IndexOf(wrestler);
                healthList.RemoveAt(index);
                ij_wrestlerList.Items.RemoveAt(index);
            }

            //Add new injury information
            healthList.Add(healthInfo);
            ij_wrestlerList.Items.Add(healthInfo);
        }

        public WrestlerHealth GetWrestlerHealthInfo(String wrestlerName)
        {
            WrestlerHealth healthInfo = null;

            foreach (WrestlerHealth wrestlerHealth in healthList)
            {
                if (wrestlerHealth.Name.Equals(wrestlerName))
                {
                    healthInfo = wrestlerHealth;
                    break;
                }
            }
            return healthInfo;
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
                foreach (WrestlerHealth health in healthList)
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

        public void LoadInjuryData()
        {
            String filePath = CheckSaveFile("Injury");

            if (File.Exists(filePath))
            {
                ij_wrestlerList.Items.Clear();
                healthList.Clear();

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
                        if(recoveryDate > DateTime.Now)
                        {
                            healthList.Add(new WrestlerHealth(name, neckHP, bodyHP, armHP, legHP, recoveryRate, matchCount, recoveryDate));
                        }
                    }
                }

                if (healthList.Count == 0)
                {
                    return;
                }
                //Update the GUI
                foreach (WrestlerHealth health in healthList)
                {
                    ij_wrestlerList.Items.Add(health);
                }

                //Update controls for the initial index
                ij_wrestlerList.SelectedIndex = 0;
                ij_wrestlerList_SelectedIndexChanged(null, null);
            }
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
            if(rngValue < 0)
            {
                rngValue = 1;
            }
            healthInfo.ReturnDate = healthInfo.ReturnDate.AddDays(rngValue);
            return healthInfo;
        }
        
        private static WrestlerHealth ResetReturnDate(WrestlerHealth healthInfo)
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

        private String CheckSaveFile(String dataType)
        {
            String path = CheckSaveFolder(dataType);

            switch (dataType)
            {
                case "Injury":
                    path = path + saveFileNames[0];
                    break;
                default:
                    path = path + saveFileNames[0];
                    break;
            }

            return path;

        }

        private String CheckSaveFolder(String dataType)
        {
            String folder = "";
            switch (dataType)
            {
                case "Injury":
                    folder = saveFolderNames[0];
                    break;
                default:
                    folder = saveFolderNames[0];
                    break;
            }

            //folder = System.IO.Directory.GetCurrentDirectory() + folder;

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            return folder;

        }

        private void ij_save_Click(object sender, EventArgs e)
        {
            SaveInjuryData();
        }

        private void ij_load_Click(object sender, EventArgs e)
        {
            LoadInjuryData();
        }

        private void QoLForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveInjuryData();
        }

        private int GetWrestlerEndurance(String part, String wrestlerName)
        {
            WrestlerID id = 0;
            foreach(WresIDGroup wrestler in wrestlerList)
            {
                if(wrestler.Name.Equals(wrestlerName))
                {
                    id = (WrestlerID)wrestler.ID;
                }
            }

            if(id == 0)
            {
                return 0;
            }
            else
            {
                int endurance = 0;

                switch(part)
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
    }
}
