using System;
using System.Windows.Forms;
using UnityEngine;
using System.Collections.Generic;
using DG;
using MatchConfig;
using System.IO;
using QoL_Mods.Helper_Classes;

namespace QoL_Mods
{
    public partial class SearchForm : Form
    {
        #region Variables
        private List<EF_WresIDGroup> wrestlerList = new List<EF_WresIDGroup>();
        private static List<String> promotionList = new List<string>();
        public static SearchForm form = null;
        #endregion

        #region Init Methods
        public void QoL_Form_Load(object sender, EventArgs e)
        {
        }
        public SearchForm()
        {
            form = this;
            InitializeComponent();
            LoadOrgs();
            LoadSubs();
            LoadReferees();
            we_searchBox.LostFocus += we_SearchBox_LostFocus;
        }
        #endregion

        #region Data Load
        public void LoadOrgs()
        {
            promotionList.Clear();
            this.we_promotionBox.Items.Clear();

            foreach (String promotion in EF_MatchConfiguration.LoadPromotions())
            {
                this.we_promotionBox.Items.Add(promotion);
                promotionList.Add(promotion);
            }

            if (this.we_promotionBox.Items.Count > 0)
            {
                this.we_promotionBox.SelectedIndex = 0;
            }
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
                    EF_WresIDGroup wresIDGroup = new EF_WresIDGroup();
                    wresIDGroup.Name = DataBase.GetWrestlerFullName(current.wrestlerParam);
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
                    this.we_resultList.Items.Add(wresIDGroup);
                }

                this.we_resultList.SelectedIndex = 0;

            }
            catch
            {

            }

        }
        private void LoadReferees()
        {
            we_refList.Items.Clear();

            foreach (RefereeInfo referee in EF_MatchConfiguration.LoadReferees())
            {
                we_refList.Items.Add(referee);
            }

            if (we_refList.Items.Count > 0)
            {
                we_refList.SelectedIndex = 0;
            }
        }
        #endregion

        #region Edit Management
        #region Wrestlers
        private void we_search_Click(object sender, EventArgs e)
        {
            try
            {
                String query = we_searchBox.Text;
                we_resultList.Items.Clear();

                if (!query.TrimStart().TrimEnd().Equals(""))
                {
                    foreach (EF_WresIDGroup wrestler in wrestlerList)
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

                foreach (EF_WresIDGroup current in wrestlerList)
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
                else
                {
                    this.LoadSubs();
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
                EF_WresIDGroup wrestler = (EF_WresIDGroup)we_resultList.SelectedItem;
                WrestlerID wID = (WrestlerID)wrestler.ID;
                L.D("Loading " + wrestler.Name + " at " + wrestler.ID);
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
                global::CreateMenu_SceneManager.editWrestlerID = wID;
                global::CreateMenu_SceneManager.edit_data = global::SaveData.GetInst().GetEditWrestlerData(wID).Clone();
                global::CreateMenu_SceneManager.flg_Overwrite = true;
                global::CreateMenu_SceneManager.user_edit_point = 380;
                global::CreateMenu_SceneManager.presetEditMode = false;
                MyMusic.Init();
                UnityEngine.SceneManagement.SceneManager.LoadScene("Scene_WrestlerEditMenu");
            }
            catch (Exception ex)
            {
                L.D("Edit Click Error: " + ex.StackTrace);
            }

        }
        private void searchWiki_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (we_resultList.SelectedItem == null)
            {
                return;
            }

            var wrestler = (EF_WresIDGroup)we_resultList.SelectedItem;
            System.Diagnostics.Process.Start("https://en.wikipedia.org/wiki/" + wrestler.Name);

        }
        private void btn_mainMenu_Click(object sender, EventArgs e)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Scene_MainMenu");
        }
        private void we_refresh_Click(object sender, EventArgs e)
        {
            LoadSubs();
            LoadOrgs();
        }
        private void we_SearchBox_LostFocus(object sender, System.EventArgs e)
        {
            SearchEdit();
        }
        private void we_promotionBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            we_search_Click(sender, e);
        }
        private void we_resultList_SelectedIndexChanged(object sender, EventArgs e)
        {
            EF_WresIDGroup edit = (EF_WresIDGroup)we_resultList.SelectedItem;
            ws_promotionLbl.Text = promotionList[edit.Group] + " (" + (DataBase.GetWrestlerParam((WrestlerID)edit.ID).fightStyle + ")"); ;
        }
        private void SearchEdit()
        {
            try
            {
                String query = we_searchBox.Text;
                we_resultList.Items.Clear();

                if (!query.TrimStart().TrimEnd().Equals(""))
                {
                    foreach (EF_WresIDGroup wrestler in wrestlerList)
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

                foreach (EF_WresIDGroup current in wrestlerList)
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
                else
                {
                    this.LoadSubs();
                }
            }
            catch (Exception ex)
            {
                L.D("Search Error: " + ex.Message);
            }
        }
        private int FindGroup(String groupName)
        {
            return promotionList.IndexOf(groupName);
        }
        #endregion
        #region Referees
        private void refEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (we_refList.Items.Count == 0)
                {
                    return;
                }

                RefereeInfo referee = (RefereeInfo)we_refList.SelectedItem;
                global::Menu_RefereeCreate.referee_data = referee.Data;
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
        #endregion
        #endregion

    }
}


