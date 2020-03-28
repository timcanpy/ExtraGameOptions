using DG;
using MatchConfig;
using QoL_Mods.Helper_Classes;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Common_Classes;
using System.IO;

namespace QoL_Mods.Public
{
    public partial class UkemiNotificationForm : Form
    {
        #region Variables

        public static UkemiNotificationForm form = null;
        private static List<WresIDGroup> wrestlerList = new List<WresIDGroup>();
        private static String saveFileName = "Notifications.dat";
        private static String saveFolderName = "./EGOData/";
        #endregion

        public UkemiNotificationForm()
        {
            form = this;
            InitializeComponent();
            PopulateNotificationControls();
            uk_wrestlerSearch.LostFocus += FindWrestler;
            uk_ringSearch.LostFocus += FindRing;
            FormClosing += SaveNotificationData;

            try
            {

                PopulateRings();
                PopulateWrestlers();
                PopulateNotificationControls();
                LoadNotificationData();
            }
            catch (Exception e)
            {
                L.D("UkemiNotificationLoadError: " + e);
            }
        }

        private void LoadNotificationData()
        {
            String filePath = Path.Combine(saveFolderName, saveFileName);
            if (File.Exists(filePath))
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    List<UkemiNotification> wrestlers = new List<UkemiNotification>();
                    List<UkemiNotification> rings = new List<UkemiNotification>();

                    var lines = File.ReadAllLines(filePath);
                    foreach (String line in lines)
                    {
                        UkemiNotification notification = new UkemiNotification();
                        notification.LoadNotificationData(line);
                        if (notification.Type == EnumLibrary.NotificationType.Wrestler)
                        {
                            wrestlers.Add(notification);
                        }
                        else
                        {
                            rings.Add(notification);
                        }
                    }

                    uk_wrestlers.Items.Clear();
                    foreach (UkemiNotification item in wrestlers)
                    {
                        uk_wrestlers.Items.Add(item);
                    }

                    uk_ringsList.Items.Clear();
                    foreach (UkemiNotification item in rings)
                    {
                        uk_ringsList.Items.Add(item);
                    }
                }

                if (uk_wrestlers.Items.Count > 0)
                {
                    uk_wrestlers.SelectedIndex = 0;
                    uk_wrestlers_SelectedIndexChanged(null, null);
                }

                if (uk_ringsList.Items.Count > 0)
                {
                    uk_ringsList.SelectedIndex = 0;
                    uk_rings_SelectedIndexChanged(null, null);
                }
            }
        }

        private void SaveNotificationData(object sender, FormClosingEventArgs e)
        {
            try
            {
                String filePath = Path.Combine(saveFolderName, saveFileName);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                using (StreamWriter sw = File.AppendText(filePath))
                {
                    foreach (UkemiNotification item in uk_wrestlers.Items)
                    {
                        sw.WriteLine(item.SaveNotificationData());
                    }

                    foreach (UkemiNotification item in uk_ringsList.Items)
                    {
                        sw.WriteLine(item.SaveNotificationData());
                    }
                }
            }
            catch (Exception exception)
            {
                L.D("SaveNotificationDataError: " + exception);
            }

        }

        private void PopulateNotificationControls()
        {
            var cheerList = Enum.GetValues(typeof(CheerVoiceEnum));

            uk_notificationList.Items.Clear();

            foreach (CheerVoiceEnum cheer in cheerList)
            {
                if (cheer == CheerVoiceEnum.Invalid || cheer == CheerVoiceEnum.Num)
                {
                    continue;
                }

                uk_notificationList.Items.Add(cheer);
            }

            uk_notificationList.SelectedIndex = 0;
        }

        private void PopulateWrestlers()
        {
            try

            {
                uk_wrestlerResults.Items.Clear();
                wrestlerList.Clear();

                foreach (EditWrestlerData current in SaveData.inst.editWrestlerData)
                {
                    WresIDGroup wresIDGroup = new WresIDGroup();
                    wresIDGroup.Name = DataBase.GetWrestlerFullName(current.wrestlerParam);
                    wresIDGroup.ID = (Int32)current.editWrestlerID;
                    wresIDGroup.Group = current.wrestlerParam.groupID;
                    uk_wrestlerResults.Items.Add(wresIDGroup);
                    wrestlerList.Add(wresIDGroup);
                }

                uk_wrestlerResults.SelectedIndex = 0;

            }
            catch

            {

            }
        }

        private void PopulateRings()
        {
            uk_ringResults.Items.Clear();

            foreach (var ring in MatchConfiguration.LoadRings())
            {
                uk_ringResults.Items.Add(new RingInfo { editRingID = ring.editRingID, name = ring.name });
            }

            if (uk_ringResults.Items.Count > 0)
            {
                uk_ringResults.SelectedIndex = 0;
            }
        }

        private void FindWrestler(object sender, System.EventArgs e)
        {
            try
            {
                String query = uk_wrestlerSearch.Text;
                uk_wrestlerResults.Items.Clear();

                if (!query.TrimStart().TrimEnd().Equals(""))
                {
                    foreach (WresIDGroup wrestler in wrestlerList)
                    {
                        if (query.ToLower().Equals(wrestler.Name.ToLower()) || wrestler.Name.ToLower().Contains(query.ToLower()))
                        {
                            uk_wrestlerResults.Items.Add(wrestler);
                        }
                    }
                }

                if (uk_wrestlerResults.Items.Count > 0)
                {
                    uk_wrestlerResults.SelectedIndex = 0;
                    return;
                }
                else
                {
                    foreach (WresIDGroup wrestler in wrestlerList)
                    {
                        uk_wrestlerResults.Items.Add(wrestler);
                    }
                }
            }
            catch (Exception ex)
            {
                L.D("FindWrestlerError: " + ex.Message);
            }
        }

        private void FindRing(object sender, System.EventArgs e)
        {
            try
            {
                String query = uk_ringSearch.Text;
                uk_ringResults.Items.Clear();

                if (!query.TrimStart().TrimEnd().Equals(""))
                {
                    foreach (var ring in MatchConfiguration.LoadRings())
                    {
                        if (query.ToLower().Equals(ring.name.ToLower()) || ring.name.ToLower().Contains(query.ToLower()))
                        {
                            uk_ringResults.Items.Add(new RingInfo { editRingID = ring.editRingID, name = ring.name });
                        }
                    }
                }

                if (uk_ringResults.Items.Count > 0)
                {
                    uk_ringResults.SelectedIndex = 0;
                    return;
                }
            }
            catch (Exception ex)
            {
                L.D("FindRingError: " + ex);
            }
        }

        #region Ring Notifications
        private void uk_addRing_Click(object sender, EventArgs e)
        {
            if (uk_ringResults.Items.Count == 0)
            {
                return;
            }

            try
            {
                RingInfo ring = (RingInfo)uk_ringResults.SelectedItem;

                foreach (UkemiNotification item in uk_ringsList.Items)
                {
                    if (item.Name.Equals(ring.name))
                    {
                        return;
                    }
                }

                UkemiNotification notification = new UkemiNotification
                {
                    Name = ring.name,
                    Type = EnumLibrary.NotificationType.Promotion,
                    CheerList = new List<CheerVoiceEnum>()
                };
                uk_ringsList.Items.Add(notification);
                uk_ringsList.SelectedIndex = 0;
                uk_rings_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                L.D("AddRingError: " + ex);
            }
        }

        private void uk_removeRing_Click(object sender, EventArgs e)
        {
            if (uk_ringsList.Items.Count == 0)
            {
                return;
            }

            try
            {
                var items = RemoveItem(uk_ringsList.Items, ((UkemiNotification)uk_ringsList.SelectedItem).Name);
                uk_ringsList.Items.Clear();
                foreach (var item in items)
                {
                    uk_ringsList.Items.Add(item);
                }

                if (uk_ringsList.Items.Count > 0)
                {
                    uk_ringsList.SelectedIndex = uk_ringsList.Items.Count - 1;
                    uk_rings_SelectedIndexChanged(null, null);
                }
                else
                {
                    uk_ringNotifications.Items.Clear();
                }
            }
            catch (Exception exception)
            {
                L.D("RemoveRingException: " + exception);
            }
        }

        private void uk_rings_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (uk_ringsList.SelectedItem == null)
            {
                return;
            }
            UkemiNotification notification = (UkemiNotification)uk_ringsList.SelectedItem;

            uk_ringNotifications.Items.Clear();
            foreach (var cheer in notification.CheerList)
            {
                uk_ringNotifications.Items.Add(cheer);
            }

            if (uk_ringNotifications.Items.Count > 0)
            {
                uk_ringNotifications.SelectedIndex = 0;
            }
        }

        private void uk_addRingNotification_Click(object sender, EventArgs e)
        {
            if (uk_ringsList.Items.Count == 0)
            {
                return;
            }

            try
            {
                UkemiNotification notification = (UkemiNotification)uk_ringsList.SelectedItem;
                foreach (var cheer in notification.CheerList)
                {
                    if ((CheerVoiceEnum)uk_notificationList.SelectedItem == cheer)
                    {
                        return;
                    }
                }

                notification.CheerList.Add((CheerVoiceEnum)uk_notificationList.SelectedItem);
                int index = uk_ringsList.SelectedIndex;
                uk_ringsList.Items[index] = notification;

                uk_rings_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                L.D("AddRingNotificationError: " + ex);
            }
        }

        private void uk_removeRingNotification_Click(object sender, EventArgs e)
        {
            if (uk_ringsList.Items.Count == 0 || uk_ringNotifications.Items.Count == 0)
            {
                return;
            }

            try
            {
                UkemiNotification notification = (UkemiNotification)uk_ringsList.SelectedItem;
                notification.CheerList.Remove((CheerVoiceEnum)uk_ringNotifications.SelectedItem);
                int index = uk_ringsList.SelectedIndex;
                uk_ringsList.Items[index] = notification;

                uk_rings_SelectedIndexChanged(null, null);
            }
            catch (Exception exception)
            {
                L.D("RemoveNotificationException: " + exception);
            }

        }
        #endregion

        #region Wrestler Notifications
        private void uk_wrestlers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (uk_wrestlers.SelectedItem == null)
            {
                return;
            }

            UkemiNotification notification = (UkemiNotification)uk_wrestlers.SelectedItem;

            uk_wrestlerNotification.Items.Clear();
            foreach (var cheer in notification.CheerList)
            {
                uk_wrestlerNotification.Items.Add(cheer);
            }

            if (uk_wrestlerNotification.Items.Count > 0)
            {
                uk_wrestlerNotification.SelectedIndex = 0;
            }
        }

        private void uk_addWrestlerNoticiation_Click(object sender, EventArgs e)
        {
            if (uk_wrestlers.Items.Count == 0)
            {
                return;
            }

            try
            {
                UkemiNotification notification = (UkemiNotification)uk_wrestlers.SelectedItem;
                foreach (var cheer in notification.CheerList)
                {
                    if ((CheerVoiceEnum)uk_notificationList.SelectedItem == cheer)
                    {
                        return;
                    }
                }

                notification.CheerList.Add((CheerVoiceEnum)uk_notificationList.SelectedItem);
                int index = uk_wrestlers.SelectedIndex;
                uk_wrestlers.Items[index] = notification;

                uk_wrestlers_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                L.D("AddWrestlerNotificationError: " + ex);
            }
        }

        private void uk_removeWrestlerNotification_Click(object sender, EventArgs e)
        {
            if (uk_wrestlers.Items.Count == 0 || uk_wrestlerNotification.Items.Count == 0)
            {
                return;
            }

            try
            {
                UkemiNotification notification = (UkemiNotification)uk_wrestlers.SelectedItem;
                notification.CheerList.Remove((CheerVoiceEnum)uk_wrestlerNotification.SelectedItem);
                int index = uk_wrestlers.SelectedIndex;
                uk_wrestlers.Items[index] = notification;

                uk_wrestlers_SelectedIndexChanged(null, null);
            }
            catch (Exception exception)
            {
                L.D("RemoveNotificationError: " + exception);
            }
        }

        private void uk_addWrestler_Click(object sender, EventArgs e)
        {
            if (uk_wrestlerResults.Items.Count == 0)
            {
                return;
            }

            try
            {
                WresIDGroup wrestler = (WresIDGroup)uk_wrestlerResults.SelectedItem;

                foreach (UkemiNotification item in uk_wrestlers.Items)
                {
                    if (item.Name == wrestler.Name)
                    {
                        return;
                    }
                }

                UkemiNotification notification = new UkemiNotification { Name = wrestler.Name, Type = EnumLibrary.NotificationType.Wrestler, CheerList = new List<CheerVoiceEnum>() };
                uk_wrestlers.Items.Add(notification);
                uk_wrestlers.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                L.D("AddWrestlerError: " + ex);
            }
        }

        private void uk_removeWrestler_Click(object sender, EventArgs e)
        {
            try
            {
                if (uk_wrestlers.Items.Count == 0)
                {
                    return;
                }

                var items = RemoveItem(uk_wrestlers.Items, ((UkemiNotification)uk_wrestlers.SelectedItem).Name);
                uk_wrestlers.Items.Clear();
                foreach (var item in items)
                {
                    uk_wrestlers.Items.Add(item);
                }

                if (uk_wrestlers.Items.Count > 0)
                {
                    uk_wrestlers.SelectedIndex = uk_wrestlers.Items.Count - 1;
                    uk_wrestlers_SelectedIndexChanged(null, null);
                }
                else
                {
                    uk_wrestlerNotification.Items.Clear();
                }

            }
            catch (Exception exception)
            {
                L.D("RemoveWrestlerError: " + exception);
            }
        }
        #endregion

        private List<UkemiNotification> RemoveItem(ComboBox.ObjectCollection list, String itemName)
        {
            List<UkemiNotification> newList = new List<UkemiNotification>();

            foreach (var item in list)
            {
                if (!((UkemiNotification)item).Name.Equals(itemName))
                {
                    newList.Add((UkemiNotification)item);
                }
            }

            return newList;
        }

        private void uk_save_Click(object sender, EventArgs e)
        {
            SaveNotificationData(null, null);
        }

        private void uk_load_Click(object sender, EventArgs e)
        {
            LoadNotificationData();
        }
    }
}
