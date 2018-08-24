using System;
using System.Windows.Forms;
using UnityEngine;
using System.Collections.Generic;
using DG;
using MatchConfig;
using QoL_Mods.Data_Classes;

namespace QoL_Mods
{
    public partial class QoL_Form : Form
    {
        #region Variables
        private List<WresIDGroup> wrestlerList = new List<WresIDGroup>();
        public static List<String> promotionList = new List<string>();
        public static List<RefereeData> refereeList = new List<RefereeData>();
        public static String[] injuryTypes = new String[] { "Healthy", "Bruised", "Sprained", "Hurt", "Injured", "Broken" };
        public static QoL_Form form = null;
        public static SteamWorkshop_UGC workShop = null;
        public static List<WrestlerHealth> healthList = new List<WrestlerHealth>();
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

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
