using System;
using System.Windows.Forms;
using UnityEngine;
using System.Collections.Generic;
using DG;
using MatchConfig;
using System.IO;

namespace QoL_Mods
{
    public partial class SearchForm : Form
    {
        #region Variables
        private List<WresIDGroup> wrestlerList = new List<WresIDGroup>();
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
                    WresIDGroup wresIDGroup = new WresIDGroup();
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
                WresIDGroup wrestler = (WresIDGroup)we_resultList.SelectedItem;
                WrestlerID wID = (WrestlerID)wrestler.ID;
                L.D("Loading " +wrestler.Name +" at " + wrestler.ID);
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

                //if (this.we_unsubscribe.Checked)
                //{
                //    Unsubcribe(wID);
                //}
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
        //private void Unsubcribe(WrestlerID wrestlerID)
        //{
        //    //Get current wrestler from list
        //    WresIDGroup wrestler = null;
        //    bool workshopAdded = false;
        //    foreach (WresIDGroup edit in wrestlerList)
        //    {
        //        if (wrestlerID == (WrestlerID)edit.ID)
        //        {
        //            wrestler = edit;
        //            break;
        //        }
        //    }

        //    //Remove subscription if it exists
        //    if (wrestler.Info == null)
        //    {
        //        return;
        //    }

        //    //Load the Steam Workshop item
        //    GameObject[] array = UnityEngine.Object.FindObjectsOfType<GameObject>();
        //    if (array.Length != 0)
        //    {
        //        try
        //        {
        //            workShop = array[0].GetComponent<SteamWorkshop_UGC>();
        //            if (workShop == null)
        //            {
        //                workShop = array[0].AddComponent<SteamWorkshop_UGC>();
        //                workshopAdded = true;
        //            }

        //            if (workShop != null)
        //            {
        //                workShop.UnsubscribeItem(wrestler.Info.GetPublishFileId());
        //                SaveData.inst.subscribeItemInfoData.Remove(wrestler.Info);
        //                wrestler.Info = null;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            L.D("Unsubcribe Error: " + ex.Message);
        //        }
        //        finally
        //        {
        //            if (workshopAdded)
        //            {
        //                UnityEngine.Object.Destroy(workShop);
        //                workShop = null;
        //            }
        //        }
        //    }
        //}
        private void we_refresh_Click(object sender, EventArgs e)
        {
            LoadSubs();
            LoadOrgs();
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
        #endregion

        private int FindGroup(String groupName)
        {
            return promotionList.IndexOf(groupName);
        }

        private void we_resultList_SelectedIndexChanged(object sender, EventArgs e)
        {
            WresIDGroup edit = (WresIDGroup)we_resultList.SelectedItem;
            ws_promotionLbl.Text = promotionList[edit.Group];
        }
    }
}


