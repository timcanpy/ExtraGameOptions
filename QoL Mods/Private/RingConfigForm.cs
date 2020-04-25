using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DG;
using MatchConfig;
using QoL_Mods.Helper_Classes;

namespace QoL_Mods.Private
{
    public partial class RingConfigForm : Form
    {
        public static RingConfigForm ringForm = null;
        private static System.String saveFileName = "RignConfig.dat";
        private static System.String saveFolderName = "./EGOData/";

        public RingConfigForm()
        {
            InitializeComponent();
            ringForm = this;
            LoadReferees();
            LoadBGMs();
            LoadRings();
            LoadConfiguration();
            rc_bgmSearch.LostFocus += FindBGM;
            rc_refSearch.LostFocus += FindReferee;
            rc_ringSearch.LostFocus += FindRing;
            FormClosing += SaveConfiguration;
        }

        #region Initialize Methods
        private void LoadReferees()
        {
            rc_refResults.Items.Clear();

            foreach (var item in MatchConfiguration.LoadReferees())
            {
                rc_refResults.Items.Add(item);
            }

            if (rc_refResults.Items.Count > 0)
            {
                rc_refResults.SelectedIndex = 0;
            }
        }
        private void LoadBGMs()
        {
            rc_bgmResult.Items.Clear();

            foreach (var item in MatchConfiguration.LoadBGMs())
            {
                rc_bgmResult.Items.Add(item);
            }

            if (rc_bgmResult.Items.Count > 0)
            {
                rc_bgmResult.SelectedIndex = 0;
            }
        }
        private void LoadRings()
        {
            rc_ringResults.Items.Clear();

            foreach (var item in MatchConfiguration.LoadRings())
            {
                rc_ringResults.Items.Add(item.name);
            }

            if (rc_ringResults.Items.Count > 0)
            {
                rc_ringResults.SelectedIndex = 0;
            }
        }

        #endregion

        private void LoadConfiguration()
        {
            try
            {
                System.String filePath = Path.Combine(saveFolderName, saveFileName);
                if (File.Exists(filePath))
                {
                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        rc_ringList.Items.Clear();

                        var lines = File.ReadAllLines(filePath);
                        foreach (System.String line in lines)
                        {
                            RingConfiguration config = new RingConfiguration();
                            config.LoadConfiguration(line);
                            rc_ringList.Items.Add(config);
                        }

                        if(rc_ringList.Items.Count > 0)
                        {
                            rc_ringList.SelectedIndex = 0;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                L.D("LoadConfigurationError: " + e);
            }
        }

        private void SaveConfiguration(object sender, FormClosingEventArgs e)
        {
            try
            {
                System.String filePath = Path.Combine(saveFolderName, saveFileName);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                using (StreamWriter sw = File.AppendText(filePath))
                {
                    foreach (RingConfiguration item in rc_ringList.Items)
                    {
                        if (item == null)
                        {
                            continue;
                        }
                        
                        sw.WriteLine(item.SaveConfiguration());
                    }
                }
            }
            catch (Exception exception)
            {
                L.D("SaveConfigDataError: " + exception);
            }
        }

        private void FindBGM(object sender, System.EventArgs e)
        {
            try
            {
                System.String query = rc_bgmSearch.Text;
                rc_bgmResult.Items.Clear();

                if (!query.TrimStart().TrimEnd().Equals(""))
                {
                    foreach (var bgm in MatchConfiguration.LoadBGMs())
                    {
                        if (query.ToLower().Equals(bgm.ToLower()) || bgm.ToLower().Contains(query.ToLower()))
                        {
                            rc_bgmResult.Items.Add(bgm);
                        }
                    }
                }

                if (rc_bgmResult.Items.Count > 0)
                {
                    rc_bgmResult.SelectedIndex = 0;
                    return;
                }
            }
            catch (Exception ex)
            {
                L.D("FindBGMError: " + ex);
            }
        }

        private void FindReferee(object sender, System.EventArgs e)
        {
            try
            {
                System.String query = rc_refSearch.Text;
                rc_refResults.Items.Clear();

                if (!query.TrimStart().TrimEnd().Equals(""))
                {
                    foreach (var referee in MatchConfiguration.LoadReferees())
                    {
                        if (query.ToLower().Equals(referee.Data.Prm.name.ToLower()) || referee.Data.Prm.name.ToLower().Contains(query.ToLower()))
                        {
                            rc_refResults.Items.Add(referee);
                        }
                    }
                }

                if (rc_refResults.Items.Count > 0)
                {
                    rc_refResults.SelectedIndex = 0;
                    return;
                }
            }
            catch (Exception ex)
            {
                L.D("FindRefError: " + ex);
            }
        }

        private void FindRing(object sender, System.EventArgs e)
        {
            try
            {
                System.String query = rc_ringSearch.Text;
                rc_ringResults.Items.Clear();

                if (!query.TrimStart().TrimEnd().Equals(""))
                {
                    foreach (var ring in MatchConfiguration.LoadRings())
                    {
                        if (query.ToLower().Equals(ring.name.ToLower()) || ring.name.ToLower().Contains(query.ToLower()))
                        {
                            rc_ringResults.Items.Add(ring.name);
                        }
                    }
                }

                if (rc_ringResults.Items.Count > 0)
                {
                    rc_ringResults.SelectedIndex = 0;
                    return;
                }
            }
            catch (Exception ex)
            {
                L.D("FindRingError: " + ex);
            }
        }

        private void uc_addRing_Click(object sender, EventArgs e)
        {
            try
            {
                if (rc_ringResults.Items.Count == 0)
                {
                    return;
                }

                String selectedRing = (String)rc_ringResults.SelectedItem;

                foreach (RingConfiguration ring in rc_ringList.Items)
                {
                    if (ring.RingName.Equals(selectedRing))
                    {
                        return;
                    }
                }

                //Get ring data
                rc_ringList.Items.Add(new RingConfiguration { RingName = selectedRing });
                rc_ringList.SelectedIndex = (rc_ringList.Items.Count - 1);
            }
            catch (Exception ex)
            {
                L.D("AddRingConfigError: " + ex);
            }
        }

        private void uc_removeRing_Click(object sender, EventArgs e)
        {
            try
            {
                if (rc_ringList.SelectedIndex < 0)
                {
                    return;
                }

                rc_ringList.Items.RemoveAt(rc_ringList.SelectedIndex);

                if (rc_ringList.Items.Count > 0)
                {
                    rc_ringList.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                L.D("RemoveRingConfigError: " + ex);
            }
        }

        private void rc_addRef_Click(object sender, EventArgs e)
        {
            try
            {
                if (rc_ringList.SelectedIndex < 0 || rc_refResults.SelectedIndex < 0)
                {
                    return;
                }

                RingConfiguration config = (RingConfiguration)rc_ringList.SelectedItem;
                RefereeInfo referee = (RefereeInfo)rc_refResults.SelectedItem;

                foreach (var item in config.Referees)
                {
                    if (item.Data.editRefereeID == referee.Data.editRefereeID)
                    {
                        return;
                    }
                }

                config.Referees.Add(referee);
                rc_ringList.SelectedItem = config;
                rc_ringList_SelectedIndexChanged(null, null);

            }
            catch (Exception ex)
            {
                L.D("AddRefereeConfigError: " + ex);
            }

        }

        private void rc_removeRef_Click(object sender, EventArgs e)
        {
            try
            {
                if (rc_ringList.SelectedIndex < 0 || rc_refereeList.SelectedIndex < 0)
                {
                    return;
                }

                RingConfiguration config = (RingConfiguration)rc_ringList.SelectedItem;
                RefereeInfo referee = (RefereeInfo)rc_refereeList.SelectedItem;

                for (int i = 0; i < config.Referees.Count; i++)
                {
                    var item = config.Referees[i];
                    if (item.Data.editRefereeID == referee.Data.editRefereeID)
                    {
                        config.Referees.RemoveAt(i);
                        break;
                    }
                }

                rc_ringList.SelectedItem = config;
                rc_ringList_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                L.D("RemoveRefereeConfigurationError: " + ex);
            }

        }

        private void rc_addBgm_Click(object sender, EventArgs e)
        {
            try
            {
                if (rc_ringList.SelectedIndex < 0 || rc_bgmResult.SelectedIndex < 0)
                {
                    return;
                }

                RingConfiguration config = (RingConfiguration)rc_ringList.SelectedItem;
                String bgm = (String)rc_bgmResult.SelectedItem;

                foreach (var item in config.Bgms)
                {
                    if (item.Equals(bgm))
                    {
                        return;
                    }
                }

                config.Bgms.Add(bgm);
                rc_ringList.SelectedItem = config;
                rc_ringList_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                L.D("AddBGMConfigurationError: " + ex);
            }
        }

        private void rc_removeBgm_Click(object sender, EventArgs e)
        {
            try
            {
                if (rc_ringList.SelectedIndex < 0 || rc_bgmList.SelectedIndex < 0)
                {
                    return;
                }

                RingConfiguration config = (RingConfiguration)rc_ringList.SelectedItem;
                String bgm = (String)rc_bgmList.SelectedItem;

                for (int i = 0; i < config.Bgms.Count; i++)
                {
                    var item = config.Bgms[i];
                    if (item.Equals(bgm))
                    {
                        config.Bgms.RemoveAt(i);
                        break;
                    }
                }

                rc_ringList.SelectedItem = config;
                rc_ringList_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                L.D("RemoveBGMConfigurationError: " + ex);
            }
        }

        private void rc_ringList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (rc_ringList.SelectedIndex < 0)
                {
                    return;
                }

                RingConfiguration config = (RingConfiguration)rc_ringList.SelectedItem;

                rc_bgmList.Items.Clear();
                rc_refereeList.Items.Clear();

                rc_singleSpeed.SelectedIndex = config.SinglesSpeed;
                rc_multiSpeed.SelectedIndex = config.MultiSpeed;
                rc_small.Value = config.GrappleSetting.Low;
                rc_medium.Value = config.GrappleSetting.Medium;
                rc_large.Value = config.GrappleSetting.High;

                //Referees
                foreach (var item in config.Referees)
                {
                    rc_refereeList.Items.Add(item);
                }

                if (config.Referees.Count > 0)
                {
                    rc_refereeList.SelectedIndex = 0;
                }

                foreach (var item in config.Bgms)
                {
                    rc_bgmList.Items.Add(item);
                }

                if (config.Bgms.Count > 0)
                {
                    rc_bgmList.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                L.D("RingConfigChangedError: " + ex);
            }
        }

        private void rc_small_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (rc_ringList.SelectedIndex < 0)
                {
                    return;
                }

                RingConfiguration config = (RingConfiguration)rc_ringList.SelectedItem;
                config.GrappleSetting.Low = int.Parse(rc_small.Value.ToString());
                rc_ringList.SelectedItem = config;
            }
            catch (Exception exception)
            {
                L.D("ChangeGrappleError: " + exception);
            }
        }

        private void rc_medium_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (rc_ringList.SelectedIndex < 0)
                {
                    return;
                }

                RingConfiguration config = (RingConfiguration)rc_ringList.SelectedItem;
                config.GrappleSetting.Medium = int.Parse(rc_medium.Value.ToString());
                rc_ringList.SelectedItem = config;
            }
            catch (Exception exception)
            {
                L.D("ChangeGrappleError: " + exception);
            }
        }

        private void rc_large_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (rc_ringList.SelectedIndex < 0)
                {
                    return;
                }

                RingConfiguration config = (RingConfiguration)rc_ringList.SelectedItem;
                config.GrappleSetting.High = int.Parse(rc_large.Value.ToString());
                rc_ringList.SelectedItem = config;
            }
            catch (Exception exception)
            {
                L.D("ChangeGrappleError: " + exception);
            }
        }

        private void rc_singleSpeed_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (rc_ringList.SelectedIndex < 0)
                {
                    return;
                }

                RingConfiguration config = (RingConfiguration)rc_ringList.SelectedItem;
                config.SinglesSpeed = rc_singleSpeed.SelectedIndex;
                rc_ringList.SelectedItem = config;
            }
            catch (Exception exception)
            {
                L.D("ChangeSpeedError: " + exception);
            }
        }

        private void rc_multiSpeed_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (rc_ringList.SelectedIndex < 0)
                {
                    return;
                }

                RingConfiguration config = (RingConfiguration)rc_ringList.SelectedItem;
                config.MultiSpeed = rc_multiSpeed.SelectedIndex;
                rc_ringList.SelectedItem = config;
            }
            catch (Exception exception)
            {
                L.D("ChangeGrappleError: " + exception);
            }
        }
    }
}
