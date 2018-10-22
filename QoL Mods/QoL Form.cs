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
        private static String[] saveFileNames = new String[] { "InjuryData.dat", "Options.dat", "RatingsData.dat", "RingData.dat" };
        private static String[] saveFolderNames = new String[] { "./EGOData/" };
        private static HashSet<CustomRing> customRings = new HashSet<CustomRing>();
        #endregion

        #region Init Methods
        public void QoL_Form_Load(object sender, EventArgs e)
        {
            ego_MainTabs.TabPages.RemoveAt(1);
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
            LoadThemes();
            LoadOptions();
            LoadCustomRings();
            LoadRatedWrestlers();
            FormClosing += QoLForm_FormClosing;
        }

        #endregion

        #region Data Load
        public void LoadOrgs()
        {
            promotionList.Clear();
            this.we_promotionBox.Items.Clear();

            foreach (String promotion in MatchConfiguration.LoadPromotions())
            {
                this.we_promotionBox.Items.Add(promotion);
                promotionList.Add(promotion);
            }
        }
        private void LoadThemes()
        {
            or_bgmList.Items.Clear();
            rs_themeList.Items.Clear();
            foreach (String theme in MatchConfiguration.LoadBGMs())
            {
                or_bgmList.Items.Add(theme);
                rs_themeList.Items.Add(theme);
            }
            or_bgmList.SelectedIndex = 0;
            rs_themeList.SelectedIndex = 0;
        }
        public void LoadRings()
        {
            or_ringList.Items.Clear();
            rs_ringList.Items.Clear();
            foreach (String ring in MatchConfiguration.LoadRings())
            {
                or_ringList.Items.Add(ring);
                rs_ringList.Items.Add(ring);
            }

            rs_ringList.SelectedIndex = 0;
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
            try
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

                foreach (WresIDGroup wrestler in wrestlerList)
                {
                    ij_result.Items.Add(wrestler.Name);
                    sr_result.Items.Add(wrestler.Name);
                }
                ij_result.SelectedIndex = 0;
                sr_result.SelectedIndex = 0;
            }
            catch
            {

            }

        }
        private void LoadReferees()
        {
            refereeList.Clear();
            we_refList.Items.Clear();
            rs_refereeList.Items.Clear();
            foreach (String referee in MatchConfiguration.LoadReferees())
            {
                if (referee.Equals("Mr Judgement"))
                {
                    continue;
                }
                rs_refereeList.Items.Add(referee);
                we_refList.Items.Add(referee);
                or_refList.Items.Add(referee);
            }
            foreach (RefereeData referee in SaveData.GetInst().editRefereeData)
            {
                refereeList.Add(referee);
            }
            rs_refereeList.SelectedIndex = 0;
            or_refList.SelectedIndex = 0;
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
        private void LoadOptions()
        {
            String filePath = CheckSaveFile("Options");

            if (File.Exists(filePath))
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    var lines = File.ReadAllLines(filePath);
                    foreach (String line in lines)
                    {
                        try
                        {

                            String control = line.Split(':')[0];
                            String value = line.Split(':')[1];

                            switch (control)
                            {
                                case "refBum_minLogic":
                                    var controls = this.Controls.Find(control, true);
                                    foreach (System.Windows.Forms.Control item in controls)
                                    {
                                        if (item is DomainUpDown)
                                        {
                                            ((DomainUpDown)item).Text = value;
                                        }
                                    }
                                    break;
                                default:
                                    var controlList = this.Controls.Find(control, true);
                                    foreach (System.Windows.Forms.Control item in controlList)
                                    {
                                        if (item is CheckBox)
                                        {
                                            try
                                            {
                                                ((CheckBox)item).Checked = Convert.ToBoolean(value);
                                            }
                                            catch
                                            {
                                                ((CheckBox)item).Checked = false;
                                            }
                                        }
                                    }
                                    break;
                            }

                        }
                        catch (Exception ex)
                        {
                            L.D("Load Options Exception: " + ex);
                        }
                    }
                }

            }
        }
        private void LoadRatedWrestlers()
        {
            String filePath = CheckSaveFile("Ratings");

            if (File.Exists(filePath))
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    var lines = File.ReadAllLines(filePath);
                    foreach (String line in lines)
                    {
                        String wrestler = line.Split(':')[0];
                        String group = line.Split(':')[1];

                        if (group.ToLower().Trim().Equals("liked"))
                        {
                            sr_likedList.Items.Add(wrestler);
                        }
                        else
                        {
                            sr_dislikedList.Items.Add(wrestler);
                        }
                    }
                }
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
        private void LoadCustomRings()
        {
            String filePath = CheckSaveFile("CustomRings");

            try
            {
                if (File.Exists(filePath))
                {
                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        var lines = File.ReadAllLines(filePath);
                        int i = 0;
                        while (i < lines.Length)
                        {
                            CustomRing ring = new CustomRing();
                            ring.Name = lines[i];

                            int.TryParse(lines[i + 1].Split(':')[1], out int refCount);
                            int.TryParse(lines[i + 2].Split(':')[1], out int themeCount);
                            i += 3;

                            for (int refPointer = 0; refPointer < refCount; refPointer++)
                            {
                                ring.RefereeList.Add(lines[i + refPointer]);
                            }
                            i += refCount;

                            for (int themePointer = 0; themePointer < themeCount; themePointer++)
                            {
                                ring.ThemeList.Add(lines[i + themePointer]);
                            }
                            i += themeCount;

                            customRings.Add(ring);
                            rs_customRings.Items.Add(ring);
                        }
                    }

                    if (customRings.Count > 0)
                    {
                        rs_customRings.SelectedIndex = 0;
                        rs_customRings_SelectedIndexChanged(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                L.D("Load Custom Ring Error:" + ex);
            }
        }

        #endregion

        #region Data Save
        private void QoLForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveInjuryData();
            SaveOptions();
            SaveRatedWrestlers();
            SaveCustomRings();
        }
        private void SaveOptions()
        {
            String filePath = CheckSaveFile("Options");

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            using (StreamWriter sw = File.AppendText(filePath))
            {
                //Edit Management
                sw.WriteLine("we_unsubscribe:" + we_unsubscribe.Checked);
                sw.WriteLine("em_injuries:" + em_injuries.Checked);
                sw.WriteLine("ij_highRisk:" + ij_highRisk.Checked);
                sw.WriteLine("ij_notifications:" + ij_notifications.Checked);
                sw.WriteLine("ed_ringSettings:" + ed_ringSettings.Checked);

                //Overrides
                sw.WriteLine("or_ring:" + or_ring.Checked);
                sw.WriteLine("or_venue:" + or_venue.Checked);
                sw.WriteLine("or_bgm:" + or_bgm.Checked);
                sw.WriteLine("or_default:" + or_default.Checked);
                sw.WriteLine("ma_forceTag:" + ma_forceTag.Checked);
                sw.WriteLine("ma_throwOut:" + ma_throwOut.Checked);
                sw.WriteLine("ma_aiCall:" + ma_aiCall.Checked);
                sw.WriteLine("ma_headbutt:" + ma_headbutt.Checked);
                sw.WriteLine("ma_convertSeconds:" + ma_convertSeconds.Checked);
                sw.WriteLine("refBum_minLogic:" + refBum_minLogic.Text);
                sw.WriteLine("sr_usage:" + sr_usage.Checked);
                sw.WriteLine("ma_downtime:" + ma_downtime.Checked);
                sw.WriteLine("or_lowRecover:" + or_lowRecover.Checked);

            }
        }
        private void SaveRatedWrestlers()
        {
            String filePath = CheckSaveFile("Ratings");

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            using (StreamWriter sw = File.AppendText(filePath))
            {
                //Add Liked Wrestlers
                foreach (String wrestler in sr_likedList.Items)
                {
                    sw.WriteLine(wrestler + ":liked");
                }

                //Add Disliked Wrestlers
                foreach (String wrestler in sr_dislikedList.Items)
                {
                    sw.WriteLine(wrestler + ":disliked");
                }
            }
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
        private String CheckSaveFile(String dataType)
        {
            String path = CheckSaveFolder(dataType);

            switch (dataType)
            {
                case "Injury":
                    path = path + saveFileNames[0];
                    break;
                case "Options":
                    path = path + saveFileNames[1];
                    break;
                case "Ratings":
                    path = path + saveFileNames[2];
                    break;
                case "CustomRings":
                    path = path + saveFileNames[3];
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

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            return folder;

        }
        private void SaveCustomRings()
        {
            String filePath = CheckSaveFile("CustomRings");

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            using (StreamWriter sw = File.AppendText(filePath))
            {
                foreach (CustomRing ring in customRings)
                {
                    sw.WriteLine(ring.Name);
                    sw.WriteLine("RefereeCount:" + ring.RefereeList.Count);
                    sw.WriteLine("ThemeCount:" + ring.ThemeList.Count);
                    foreach (String referee in ring.RefereeList)
                    {
                        sw.WriteLine(referee);
                    }
                    foreach (String theme in ring.ThemeList)
                    {
                        sw.WriteLine(theme);
                    }
                }
            }

        }

        #endregion

        #region Wrestler Management
        #region Wrestlers
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
        #endregion
        #region Injuries
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

            foreach (WrestlerHealth wrestlerHealth in ij_wrestlerList.Items)
            {
                if (wrestlerHealth.Name.Equals(wrestlerName))
                {
                    healthInfo = wrestlerHealth;
                    break;
                }
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
        #endregion

        #region AI Behaviour
        #endregion

        #region Overrides
        private void refEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (refereeList.Count == 0)
                {
                    return;
                }
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
        private void or_ring_CheckedChanged(object sender, EventArgs e)
        {
            ed_ringSettings.Checked = false;
        }
        private void or_referee_CheckedChanged(object sender, EventArgs e)
        {
            ed_ringSettings.Checked = false;
        }
        private void or_bgm_CheckedChanged(object sender, EventArgs e)
        {
            ed_ringSettings.Checked = false;
        }
        #endregion

        #region Star Ratings
        private void sr_search_LostFocus(object sender, System.EventArgs e)
        {
            sr_Search();
        }
        private void sr_Search()
        {
            String wrestlerName = sr_search.Text.ToLower();
            if (wrestlerName.Trim().Equals(""))
            {
                return;
            }

            sr_result.Items.Clear();
            foreach (WresIDGroup wrestler in wrestlerList)
            {
                if (wrestler.Name.ToLower().Contains(wrestlerName))
                {
                    sr_result.Items.Add(wrestler.Name);
                }
            }

            if (sr_result.Items.Count > 0)
            {
                sr_result.SelectedIndex = 0;
            }
        }
        private void sr_addWrestler_Click(object sender, EventArgs e)
        {
            if (sr_addLiked.Checked)
            {
                sr_likedList.Items.Add(sr_result.SelectedItem);
            }
            else
            {
                sr_dislikedList.Items.Add(sr_result.SelectedItem);
            }
        }
        private void sr_removeAllLiked_Click(object sender, EventArgs e)
        {
            sr_likedList.Items.Clear();
        }
        private void sr_removeOneLiked_Click(object sender, EventArgs e)
        {
            sr_likedList.Items.Remove(sr_likedList.SelectedItem);
        }
        private void sr_removeOneDisliked_Click(object sender, EventArgs e)
        {
            sr_dislikedList.Items.Remove(sr_dislikedList.SelectedItem);
        }
        private void sr_removeAllDisliked_Click(object sender, EventArgs e)
        {
            sr_dislikedList.Items.Clear();
        }
        private void sr_refresh_Click(object sender, EventArgs e)
        {
            LoadSubs();
        }
        #endregion

        #region Custom Ring Settings
        private void rs_addRing_Click(object sender, EventArgs e)
        {
            //Determine if ring settings exist
            String ringName = (String)rs_ringList.SelectedItem;
            CustomRing customRing = null;
            foreach (CustomRing ring in rs_customRings.Items)
            {
                if (ring.Name.Equals(ringName))
                {
                    customRing = ring;
                    break;
                }
            }

            if (customRing == null)
            {
                customRing = new CustomRing { Name = ringName, ThemeList = new HashSet<string> { (String)rs_themeList.SelectedItem }, RefereeList = new HashSet<string> { (String)rs_refereeList.SelectedItem } };
            }
            else
            {
                rs_customRings.Items.Remove(customRing);
                customRing.RefereeList.Add((String)rs_refereeList.SelectedItem);
                customRing.ThemeList.Add((String)rs_themeList.SelectedItem);
            }

            rs_customRings.Items.Add(customRing);
            customRings.Add(customRing);
            rs_customRings.SelectedIndex = (rs_customRings.Items.Count - 1);
            rs_customRings_SelectedIndexChanged(sender, e);
        }
        private void rs_customRings_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rs_customRings.SelectedIndex == -1 && rs_customRings.Items.Count > 0)
            {
                rs_customRings.SelectedIndex = 0;
            }
            else if (rs_customRings.Items.Count == 0)
            {
                return;
            }

            CustomRing ring = PrepareRingRemoval();
            if (ring == null)
            {
                return;
            }

            foreach (String referee in ring.RefereeList)
            {
                rs_customReferees.Items.Add(referee);
            }
            rs_customReferees.SelectedIndex = 0;

            foreach (String theme in ring.ThemeList)
            {
                rs_customThemes.Items.Add(theme);
            }

            rs_customThemes.SelectedIndex = 0;
        }
        private void rs_removeReferee_Click(object sender, EventArgs e)
        {
            String referee = ((String)rs_customReferees.SelectedItem);
            L.D(referee);
            CustomRing ring = PrepareRingRemoval();
            if (ring == null)
            {
                return;
            }
            if (ring.RefereeList.Count == 1)
            {
                rs_customRings_SelectedIndexChanged(sender, e);
                return;
            }
            customRings.Remove(ring);
            ring.RefereeList.Remove(referee);
            UpdateCustomRings(ring);
            rs_customRings_SelectedIndexChanged(sender, e);
        }
        private void rs_removeRing_Click(object sender, EventArgs e)
        {
            try
            {
                CustomRing ring = PrepareRingRemoval();
                if (ring == null)
                {
                    return;
                }
                customRings.Remove(ring);
                UpdateCustomRings(null);
                if (rs_customRings.Items.Count == 0)
                {
                    return;
                }
                else
                {
                    rs_customRings.SelectedIndex = 0;
                }
                rs_customRings_SelectedIndexChanged(sender, e);

            }
            catch (Exception ex)
            {
                L.D("Ring Removal Error: " + ex);
            }
        }
        private void rs_removeTheme_Click(object sender, EventArgs e)
        {
            String theme = ((String)rs_customThemes.SelectedItem);
            L.D(theme);
            CustomRing ring = PrepareRingRemoval();
            if (ring == null)
            {
                return;
            }
            if (ring.ThemeList.Count == 1)
            {
                rs_customRings_SelectedIndexChanged(sender, e);
                return;
            }
            customRings.Remove(ring);
            ring.ThemeList.Remove(theme);
            UpdateCustomRings(ring);
            rs_customRings_SelectedIndexChanged(sender, e);
        }
        private CustomRing PrepareRingRemoval()
        {
            if (rs_customRings.Items.Count == 0)
            {
                return null;
            }
            CustomRing ring = (CustomRing)rs_customRings.SelectedItem;
            rs_customThemes.Items.Clear();
            rs_customReferees.Items.Clear();

            return ring;
        }
        private void UpdateCustomRings(CustomRing ring)
        {
            rs_customRings.Items.Clear();
            if (ring != null)
            {
                customRings.Add(ring);
            }
            foreach (CustomRing customRing in customRings)
            {
                rs_customRings.Items.Add(customRing);
            }
            rs_customRings.SelectedIndex = rs_customRings.Items.Count - 1;
        }
        private void ed_ringSettings_CheckedChanged(object sender, EventArgs e)
        {
            or_ring.Checked = false;
            or_bgm.Checked = false;
            or_referee.Checked = false;
        }
        private void rs_saveData_Click(object sender, EventArgs e)
        {
            SaveCustomRings();
        }
        private void rs_loadData_Click(object sender, EventArgs e)
        {
            LoadCustomRings();
        }
        private int FindGroup(String groupName)
        {
            return promotionList.IndexOf(groupName);
        }

        #endregion

    }
}
