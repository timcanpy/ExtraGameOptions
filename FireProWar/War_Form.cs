using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DG;
using MatchConfig;
using Data_Classes;
using System.IO;
using System.Linq;
using UnityEngine;
using System.Drawing;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using FireProWar.Data_Classes;

namespace FireProWar
{
    public partial class War_Form : Form
    {
        #region Variables
        private List<WresIDGroup> wrestlerList = new List<WresIDGroup>();
        private static List<String> promotionList = new List<string>();
        private static String[] promotionStyle = new String[] { "Strong-Style", "King's Road", "Mixed Martial Arts", "Hardcore", "American", "Boxing", "Lucha", "Amateur Wrestling", "Sumo", "Indy" };
        private static Dictionary<String, FightStyleEnum[]> groupFightingStyles;
        private static String[] moraleRank = new String[] { "E", "D", "C", "B", "A", "S" };
        private static String[] saveFolderNames = new String[] { "./EGOData/" };
        private static String[] saveFileNames = new String[] { "FPWData.dat", "FPWConfig.dat" };
        private static HashSet<String> promotionsAdded = new HashSet<String>();
        private static HashSet<String> employeesAdded = new HashSet<String>();
        private static String promotionDivider = "|--Promotion--|";
        public static System.Random rnd = new System.Random();
        public static War_Form form = null;
        public static int promotionFactor = 5;
        public static int demotionFactor = -5;
        public static String csvLocation = "./EGOData/Reports/";
        public static String imageRootFolder = "./EGOData/Images/";
        public static String[] imageFolders = new String[] { "Wrestlers/", "Logos/" };
        public static String defaultWrestlerImage = "Default.png";
        public static String currentVersion = "V2";
        public static char listSeparator = ':';
        public enum FilterType
        {
            All,
            TitleHistory,
            Promotions,
            Demotions,
            Departures,
            UpsetVictories,
            KnockOuts,
            Injuries,
            Term
        };
        public enum ImageTypes
        {
            Wrestler,
            Logo
        };
        #endregion

        #region Form Initialize
        public War_Form()
        {
            form = this;
            InitializeComponent();
            LoadOrgs();
            LoadSubs();
            LoadRegions();
            LoadFightStyles();
            LoadRings();
            LoadMoraleRank();
            LoadGroupFightingStyles();
            LoadWarData();
            LoadFilterOptions();
            FormClosing += WarForm_FormClosing;
            fpw_historyTerm.Leave += new System.EventHandler(fpw_HistoryTerm_LostFocus);
            fpw_matchTerm.Leave += new System.EventHandler(fpw_MatchTerm_LostFocus);
            fpw_rosterFiler.SelectedIndex = 0;
            fpw_detailsView.WordWrap = true;
            fpw_promoHistory.WordWrap = true;
        }
        private void WarForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            WriteToCSV();
            SaveWarData();
        }
        private void LoadOrgs()
        {
            this.ms_groupList.Items.Clear();
            promotionList.Clear();
            foreach (String promotion in MatchConfiguration.LoadPromotions())
            {
                this.ms_groupList.Items.Add(promotion);
                promotionList.Add(promotion);
            }

            ms_groupList.SelectedIndex = 0;
        }
        private void LoadSubs()
        {
            try
            {
                this.ms_searchResults.Items.Clear();
                fpw_historyWrestler.Items.Clear();
                fpw_matchWrestler.Items.Clear();
                wrestlerList.Clear();

                foreach (EditWrestlerData current in SaveData.inst.editWrestlerData)
                {
                    WresIDGroup wresIDGroup = new WresIDGroup();
                    wresIDGroup.Name = DataBase.GetWrestlerFullName(current.wrestlerParam);
                    wresIDGroup.ID = (Int32)current.editWrestlerID;
                    wresIDGroup.Group = current.wrestlerParam.groupID;

                    wrestlerList.Add(wresIDGroup);
                    this.ms_searchResults.Items.Add(wresIDGroup);
                    fpw_historyWrestler.Items.Add(wresIDGroup);
                    fpw_matchWrestler.Items.Add(wresIDGroup);
                }

                this.ms_searchResults.SelectedIndex = 0;
                fpw_historyWrestler.SelectedIndex = 0;
                fpw_matchWrestler.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                L.D("LoadSubsError: " + ex);
            }

        }
        private void LoadRegions()
        {
            fpw_promoRegionList.Items.Clear();
            foreach (var region in MatchConfig.MatchConfiguration.GetRegionList())
            {
                fpw_promoRegionList.Items.Add(CorrectRegionName(region.ToString()));
            }

            fpw_promoRegionList.SelectedIndex = 0;
        }
        private void LoadFightStyles()
        {
            fpw_promoStyleList.Items.Clear();
            foreach (String style in promotionStyle)
            {
                fpw_promoStyleList.Items.Add(style);
            }

            fpw_promoStyleList.SelectedIndex = 0;
        }
        private void LoadRings()
        {
            fpw_ringList.Items.Clear();
            foreach (String ring in MatchConfiguration.LoadRings())
            {
                //Ensure that there's no overlap with our chosen list separator character
                fpw_ringList.Items.Add(ring.Replace(listSeparator, ' '));
            }

            fpw_ringList.SelectedIndex = 0;
        }
        private void LoadMoraleRank()
        {
            ms_moraleRank.Items.Clear();
            foreach (String rank in moraleRank)
            {
                ms_moraleRank.Items.Add(rank);
            }

            ms_moraleRank.SelectedIndex = 0;
        }
        private void LoadGroupFightingStyles()
        {
            groupFightingStyles = new Dictionary<string, FightStyleEnum[]>();
            groupFightingStyles.Add("Strong-Style", new FightStyleEnum[] { FightStyleEnum.Orthodox, FightStyleEnum.Technician, FightStyleEnum.Wrestling, FightStyleEnum.Power, FightStyleEnum.Junior, FightStyleEnum.Mysterious, FightStyleEnum.Shooter, FightStyleEnum.Panther, FightStyleEnum.Devilism });
            groupFightingStyles.Add("King's Road", new FightStyleEnum[] { FightStyleEnum.Orthodox, FightStyleEnum.Technician, FightStyleEnum.Wrestling, FightStyleEnum.Power, FightStyleEnum.Junior, FightStyleEnum.Mysterious, FightStyleEnum.Giant, FightStyleEnum.American });
            groupFightingStyles.Add("Mixed Martial Arts", new FightStyleEnum[] { FightStyleEnum.Grappler, FightStyleEnum.Fighter, FightStyleEnum.Ground, FightStyleEnum.Shooter, FightStyleEnum.Wrestling });
            groupFightingStyles.Add("Hardcore", new FightStyleEnum[] { FightStyleEnum.American, FightStyleEnum.Giant, FightStyleEnum.Heel, FightStyleEnum.Junior, FightStyleEnum.Mysterious });
            groupFightingStyles.Add("American", new FightStyleEnum[] { FightStyleEnum.American, FightStyleEnum.Heel, FightStyleEnum.Junior, FightStyleEnum.Power, FightStyleEnum.Technician, FightStyleEnum.Wrestling, FightStyleEnum.Giant });
            groupFightingStyles.Add("Boxing", new FightStyleEnum[] { FightStyleEnum.Fighter });
            groupFightingStyles.Add("Lucha", new FightStyleEnum[] { FightStyleEnum.Luchador, FightStyleEnum.Junior, FightStyleEnum.Mysterious, FightStyleEnum.Panther, FightStyleEnum.Technician });
            groupFightingStyles.Add("Amateur Wrestling", new FightStyleEnum[] { FightStyleEnum.Wrestling });
            groupFightingStyles.Add("Sumo", new FightStyleEnum[] { FightStyleEnum.Fighter, FightStyleEnum.Grappler, FightStyleEnum.Power });
            groupFightingStyles.Add("Indy", new FightStyleEnum[] { FightStyleEnum.American, FightStyleEnum.Giant, FightStyleEnum.Heel, FightStyleEnum.Mysterious, FightStyleEnum.Junior, FightStyleEnum.Luchador, FightStyleEnum.Power, FightStyleEnum.Technician, FightStyleEnum.Wrestling, FightStyleEnum.Orthodox });
        }
        private void LoadFilterOptions()
        {
            fpw_historyCategory.SelectedIndex = 0;
        }
        #endregion

        #region Data Load
        private void LoadWarData()
        {
            try
            {
                PrepareForLoad();
                String filePath = saveFolderNames[0] + saveFileNames[0];

                #region Loading Promotion Data
                if (File.Exists(filePath))
                {
                    List<String> titleNames = new List<String>();

                    #region Resolve error related to Title Data
                    foreach (var item in SaveData.inst.titleMatch_Data)
                    {
                        titleNames.Add(item.titleName.ToUpper());
                    }

                    //Remove all extra line breaks
                    string text = File.ReadAllText(filePath);
                    foreach (var title in titleNames)
                    {
                        text = text.Replace(title + Environment.NewLine, title + " - ");
                    }

                    //Rewrite file
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }

                    using (StreamWriter sw = File.AppendText(filePath))
                    {
                        sw.Write(text);
                    }

                    #endregion

                    var lines = File.ReadAllLines(filePath);

                    //Determine how we load data
                    if (lines[0].Equals(currentVersion))
                    {
                        int i = 1;
                        while (i < lines.Length)
                        {
                            /*Promotion Data
                                Name, Ring, Type, Region, MatchCount, AverageRating, History, MatchResults, EmployeeData 
                            */
                            if (lines[i].Equals(promotionDivider))
                            {
                                Promotion promotion = new Promotion();
                                promotion.Name = lines[i + 1];
                                promotion.LoadRings(lines[i + 2]);
                                promotion.Type = lines[i + 3];
                                promotion.Region = lines[i + 4];
                                promotion.MatchCount = int.Parse(lines[i + 5]);
                                promotion.AverageRating = float.Parse(lines[i + 6]);
                                promotion.History = lines[i + 7];

                                if (lines[i + 9].Equals(promotionDivider) || i + 9 >= lines.Length)
                                {
                                    promotion.AddEmployeeFromData(lines[i + 8]);
                                    i += 9;
                                }
                                else
                                {
                                    promotion.AddMatchDetailsFromData(lines[i + 8]);
                                    promotion.AddEmployeeFromData(lines[i + 9]);
                                    i += 10;
                                }
                                fpw_promoList.Items.Add(promotion);

                                //Update Promotion and Employee Lists
                                promotionsAdded.Add(promotion.Name);
                                foreach (Employee employee in promotion.EmployeeList)
                                {
                                    employeesAdded.Add(employee.Name);
                                }
                            }

                        }
                    }
                    else
                    {
                        int i = 0;
                        while (i < lines.Length)
                        {
                            /*Promotion Data
                                Name, Ring, Type, Region, MatchCount, AverageRating, History, MatchResults, EmployeeData 
                            */
                            if (lines[i].Equals(promotionDivider))
                            {
                                Promotion promotion = new Promotion();
                                promotion.Name = lines[i + 1];
                                promotion.Rings.Add(lines[i + 2]);
                                promotion.Type = lines[i + 3];
                                promotion.Region = lines[i + 4];
                                promotion.MatchCount = int.Parse(lines[i + 5]);
                                promotion.AverageRating = float.Parse(lines[i + 6]);
                                promotion.History = lines[i + 7];

                                if (lines[i + 9].Equals(promotionDivider) || i + 9 >= lines.Length)
                                {
                                    promotion.AddEmployeeFromData(lines[i + 8]);
                                    i += 9;
                                }
                                else
                                {
                                    promotion.AddMatchDetailsFromData(lines[i + 8]);
                                    promotion.AddEmployeeFromData(lines[i + 9]);
                                    i += 10;
                                }
                                fpw_promoList.Items.Add(promotion);

                                //Update Promotion and Employee Lists
                                promotionsAdded.Add(promotion.Name);
                                foreach (Employee employee in promotion.EmployeeList)
                                {
                                    employeesAdded.Add(employee.Name);
                                }
                            }

                        }
                    }

                    if (fpw_promoList.Items.Count > 0)
                    {
                        fpw_promoList.SelectedIndex = 0;
                        fpw_promoList_SelectedIndexChanged(null, null);
                    }
                }
                #endregion

                #region Loading Config Data

                filePath = saveFolderNames[0] + saveFileNames[1];
                if (File.Exists(filePath))
                {
                    var lines = File.ReadAllLines(filePath);
                    foreach (String line in lines)
                    {
                        String[] elements = line.Split(':');
                        bool value;
                        switch (elements[0])
                        {
                            case "fpw_Enable":
                                Boolean.TryParse(elements[1], out value);
                                fpw_Enable.Checked = value;
                                break;
                            case "fpw_showRecord":
                                Boolean.TryParse(elements[1], out value);
                                fpw_showRecord.Checked = value;
                                break;
                        }

                        elements = line.Split('-');
                        switch (elements[0])
                        {
                            case "fpw_neckInjuries":
                                var neckInjuries = elements[1].Split(listSeparator);
                                fpw_neckInjuries.Items.Clear();
                                foreach (String injury in neckInjuries)
                                {
                                    if (injury.Trim().Equals(String.Empty))
                                    {
                                        continue;
                                    }
                                    fpw_neckInjuries.Items.Add(injury);
                                }
                                break;

                            case "fpw_waistInjuries":
                                var waistInjuries = elements[1].Split(listSeparator);
                                fpw_waistInjuries.Items.Clear();
                                foreach (String injury in waistInjuries)
                                {
                                    if (injury.Trim().Equals(String.Empty))
                                    {
                                        continue;
                                    }
                                    fpw_waistInjuries.Items.Add(injury);
                                }
                                break;

                            case "fpw_armInjuries":
                                var armInjuries = elements[1].Split(listSeparator);
                                fpw_armInjuries.Items.Clear();
                                foreach (String injury in armInjuries)
                                {
                                    if (injury.Trim().Equals(String.Empty))
                                    {
                                        continue;
                                    }
                                    fpw_armInjuries.Items.Add(injury);
                                }
                                break;

                            case "fpw_legInjuries":
                                var legInjuries = elements[1].Split(listSeparator);
                                fpw_legInjuries.Items.Clear();
                                foreach (String injury in legInjuries)
                                {
                                    if (injury.Trim().Equals(String.Empty))
                                    {
                                        continue;
                                    }
                                    fpw_legInjuries.Items.Add(injury);
                                }
                                break;
                        }
                    }
                }

                //Ensure we have at least one injury text
                if (fpw_neckInjuries.Items.Count == 0)
                {
                    fpw_neckInjuries.Items.Add("extreme neck damage");
                }

                if (fpw_waistInjuries.Items.Count == 0)
                {
                    fpw_waistInjuries.Items.Add("extreme waist damage");
                }

                if (fpw_legInjuries.Items.Count == 0)
                {
                    fpw_legInjuries.Items.Add("extreme leg damage");
                }

                if (fpw_armInjuries.Items.Count == 0)
                {
                    fpw_armInjuries.Items.Add("extreme arm damage");
                }

                fpw_neckInjuries.SelectedIndex = 0;
                fpw_armInjuries.SelectedIndex = 0;
                fpw_legInjuries.SelectedIndex = 0;
                fpw_waistInjuries.SelectedIndex = 0;

                #endregion
            }
            catch (Exception e)
            {
                L.D("Load War Data Error: " + e.ToString());
            }
        }
        private void btn_LoadData_Click(object sender, EventArgs e)
        {
            LoadWarData();
        }

        #endregion

        #region Date Save
        private void SaveWarData()
        {
            String folder = saveFolderNames[0];
            CheckForDirectory(folder);

            #region Saving Promotion Data
            String filePath = saveFolderNames[0] + saveFileNames[0];
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            using (StreamWriter sw = File.AppendText(filePath))
            {
                //Label save file for future version control
                sw.WriteLine(currentVersion);

                //Save Promotions
                foreach (Promotion promotion in fpw_promoList.Items)
                {
                    //Promotion Details
                    sw.WriteLine(promotionDivider);
                    sw.WriteLine(promotion.Name);
                    sw.WriteLine(promotion.GetRingList());
                    sw.WriteLine(promotion.Type);
                    sw.WriteLine(promotion.Region);
                    sw.WriteLine(promotion.MatchCount);
                    sw.WriteLine(promotion.AverageRating);
                    sw.WriteLine(promotion.History);

                    //Match Details
                    String matchDetails = "";
                    foreach (String result in promotion.MatchDetails)
                    {
                        matchDetails += result + "|";
                    }

                    sw.WriteLine(matchDetails);

                    //Roster
                    String employeeData = "";
                    foreach (Employee employee in promotion.EmployeeList)
                    {
                        employeeData += employee.CompileEmployeeData();
                    }

                    sw.WriteLine(employeeData);
                }
            }
            #endregion

            try
            {
                #region Saving Config Data

                filePath = saveFolderNames[0] + saveFileNames[1];
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                using (StreamWriter sw = File.AppendText(filePath))
                {
                    sw.WriteLine("fpw_Enable: " + fpw_Enable.Checked);
                    sw.WriteLine("fpw_showRecord: " + fpw_showRecord.Checked);

                    //Save Neck Injuries
                    String injuries = "";
                    foreach (var injury in fpw_neckInjuries.Items)
                    {
                        injuries += (String)injury + listSeparator;
                    }

                    sw.WriteLine("fpw_neckInjuries-" + injuries);

                    //Save Arm Injuries
                    injuries = "";
                    foreach (var injury in fpw_armInjuries.Items)
                    {
                        injuries += (String)injury + listSeparator;
                    }

                    sw.WriteLine("fpw_armInjuries-" + injuries);

                    //Save Leg Injuries
                    injuries = "";
                    foreach (var injury in fpw_legInjuries.Items)
                    {
                        injuries += (String)injury + listSeparator;
                    }

                    sw.WriteLine("fpw_legInjuries-" + injuries);

                    //Save Arm Injuries
                    injuries = "";
                    foreach (var injury in fpw_waistInjuries.Items)
                    {
                        injuries += (String)injury + listSeparator;
                    }

                    sw.WriteLine("fpw_waistInjuries-" + injuries);
                }
                #endregion

            }
            catch (Exception e)
            {
                L.D("Save Config Data Error:" + e);
            }

            //Sending data to Web API
            //SendJSONData();

        }
        private void btn_SaveData_Click(object sender, EventArgs e)
        {
            SaveWarData();
        }

        #endregion

        #region Wrestler Search
        private void WrestlerSearch_LostFocus(object sender, System.EventArgs e)
        {
            SearchWrestler();
        }
        private void GroupList_LostFocus(object sender, System.EventArgs e)
        {
            SearchWrestler();
        }
        private void ms_groupList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchWrestler();
        }
        private void SearchWrestler()
        {
            try
            {
                String query = ms_wrestlerSearch.Text;
                ms_searchResults.Items.Clear();

                if (!query.TrimStart().TrimEnd().Equals(""))
                {
                    foreach (WresIDGroup wrestler in wrestlerList)
                    {
                        if (query.ToLower().Equals(wrestler.Name.ToLower()) || wrestler.Name.ToLower().Contains(query.ToLower()))
                        {
                            ms_searchResults.Items.Add(wrestler);
                        }
                    }
                }

                if (ms_searchResults.Items.Count > 0)
                {
                    ms_searchResults.SelectedIndex = 0;
                    return;
                }

                if (ms_groupList.SelectedItem.ToString().Contains("未登録"))
                {
                    this.LoadSubs();
                    return;
                }

                foreach (WresIDGroup current in wrestlerList)
                {
                    if (current.Group == FindGroup(ms_groupList.SelectedItem.ToString()))
                    {
                        ms_searchResults.Items.Add(current);
                    }
                }

                if (ms_searchResults.Items.Count > 0)
                {
                    ms_searchResults.SelectedIndex = 0;
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
        private void ms_refreshList_Click(object sender, EventArgs e)
        {
            LoadSubs();
            LoadOrgs();
        }
        private void ms_employeeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ms_employeeList.Items.Count == 0)
            {
                return;
            }
            FindImage(ImageTypes.Wrestler, (String)ms_employeeList.SelectedItem);
        }
        private void ms_searchResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ms_searchResults.Items.Count == 0)
            {
                return;
            }

            try
            {
                WresIDGroup wrestler = (WresIDGroup)ms_searchResults.SelectedItem;
                FindImage(ImageTypes.Wrestler, wrestler.Name);

                //Set Championship Data
                var titleData = GetChampionData(wrestler.Name);
                if (titleData == null)
                {
                    rosterTitleInfo.Text = "";
                    rosterTitle.Image = null;
                }
                else
                {
                    int defenseCount = 0;
                    if (titleData.GetLatestMatchRecord() != null)
                    {
                        defenseCount = titleData.GetLatestMatchRecord().DefenseCount;
                    }

                    rosterTitleInfo.Text = titleData.titleName + "\nTitle Type: " + titleData.matchType + " - " +
                                           titleData.playerNum + "\nNumber of Defenses: " + defenseCount;
                    rosterTitle.Image = ModPack.Utility.GetBeltImageFromTex(titleData);
                }
            }
            catch (Exception exception)
            {
                L.D("ms_searchResults_SelectedIndexChangedError: " + exception);
            }
        }
        #endregion

        #region Promotion Management
        private void fpw_addPromotion_Click(object sender, EventArgs e)
        {
            if (fpw_promoName.Text.Trim().Equals(""))
            {
                MessageBox.Show("Promotion Name cannot be empty.");
                return;
            }

            if (promotionsAdded.Contains(fpw_promoName.Text.Trim()))
            {
                MessageBox.Show("Promotion currently exists.");
                return;
            }

            if (RingExists(fpw_ringList.SelectedItem.ToString()))
            {
                MessageBox.Show("Ring is currently used by another promotion.");
                return;
            }

            Promotion promotion = new Promotion();
            promotion.Name = fpw_promoName.Text.Trim();
            promotion.Region = fpw_promoRegionList.SelectedItem.ToString();
            promotion.Rings.Add(fpw_ringList.SelectedItem.ToString());
            promotion.Type = fpw_promoStyleList.SelectedItem.ToString();
            promotionsAdded.Add(promotion.Name);
            fpw_promoList.Items.Add(promotion);
            fpw_promoList.SelectedIndex = fpw_promoList.Items.Count - 1;

        }
        private void fpw_updatePromotion_Click(object sender, EventArgs e)
        {
            if (fpw_promoList.SelectedIndex < 0)
            {
                return;
            }

            Promotion promotion = (Promotion)fpw_promoList.SelectedItem;
            promotion.Region = fpw_promoRegionList.SelectedItem.ToString();
            //promotion.Rings.Add((String)fpw_ringList.SelectedItem);
            promotion.Type = fpw_promoStyleList.SelectedItem.ToString();
            fpw_promoList.SelectedItem = promotion;
        }
        private void fpw_promoList_SelectedIndexChanged(object sender, EventArgs e)
        {
            rosterCount.Text = "";

            if (fpw_promoList.SelectedIndex < 0)
            {
                return;
            }

            Promotion promotion = (Promotion)fpw_promoList.SelectedItem;
            fpw_promoName.Text = promotion.Name;
            fpw_promoRegionList.SelectedIndex = fpw_promoRegionList.Items.IndexOf(promotion.Region);

            fpw_promoRingList.Items.Clear();
            foreach (String item in promotion.Rings)
            {
                fpw_promoRingList.Items.Add(item);
            }

            if (fpw_promoRingList.Items.Count > 0)
            {
                fpw_promoRingList.SelectedIndex = 0;
            }

            fpw_promoStyleList.SelectedIndex = fpw_promoStyleList.Items.IndexOf(promotion.Type);
            fpw_promoMthCnt.Text = promotion.MatchCount.ToString();
            fpw_promoMthRating.Text = Math.Round(Decimal.Parse(promotion.AverageRating.ToString()), 2) + "%";

            //Reset filtering
            fpw_historyCategory.SelectedIndex = 0;
            fpw_historyCategory_SelectedIndexChanged(sender, e);

            fpw_matchTerm.Text = "";

            #region Adding Details
            String details = "";
            foreach (String detail in promotion.MatchDetails)
            {
                details += detail + "~";

            }
            fpw_detailsView.Text = FilterHistory("", FilterType.All, details);
            #endregion

            ms_employeeList.Items.Clear();
            ms_rosterList.Items.Clear();

            //Roster details
            foreach (Employee employee in promotion.EmployeeList)
            {
                ms_employeeList.Items.Add(employee.Name);
                ms_rosterList.Items.Add(employee);
            }

            //Ensure that we filter the roster list appropriately
            fpw_rosterFiler_SelectedIndexChanged(null, null);

            if (ms_rosterList.Items.Count > 0)
            {
                ms_rosterList.SelectedIndex = ms_rosterList.Items.Count - 1;
                ms_rosterList_SelectedIndexChanged(sender, e);
                rosterCount.Text = ms_rosterList.Items.Count.ToString();
            }

            //Update selected roster to reference this promotion
            try
            {
                SelectPromotionRoster(promotion.Name);
            }
            catch (Exception ex)
            {
                L.D("SelectPromotionRosterError: " + ex);
            }

            FindImage(ImageTypes.Logo, promotion.Name);
            FindImage(ImageTypes.Wrestler, ""); //Ensure that we clear out the latest wrestler image
        }
        private void fpw_deletePromotion_Click(object sender, EventArgs e)
        {
            if (fpw_promoList.SelectedIndex < 0)
            {
                return;
            }

            Promotion promotion = (Promotion)fpw_promoList.SelectedItem;
            promotionsAdded.Remove(promotion.Name);
            fpw_promoList.Items.RemoveAt(fpw_promoList.SelectedIndex);

            foreach (Employee employee in promotion.EmployeeList)
            {
                employeesAdded.Remove(employee.Name);
            }

            if (fpw_promoList.Items.Count > 0)
            {
                fpw_promoList.SelectedIndex = 0;
                fpw_promoList_SelectedIndexChanged(sender, e);
            }
            else
            {
                ClearPromoDetails();
            }
        }
        private void fpw_promoClearHistory_Click(object sender, EventArgs e)
        {
            if (fpw_promoList.SelectedIndex < 0)
            {
                return;
            }

            Promotion promotion = ((Promotion)fpw_promoList.SelectedItem);
            if (promotion.History.Equals(""))
            {
                return;
            }

            promotion.ClearHistory();
            fpw_promoList.SelectedItem = promotion;
            fpw_promoList_SelectedIndexChanged(sender, e);
        }
        private void ClearPromoDetails()
        {
            fpw_promoName.Text = "";
            fpw_promoMthCnt.Text = "";
            fpw_promoMthRating.Text = "";
            fpw_promoHistory.Text = "";
            fpw_detailsView.Text = "";
        }
        public void UpdatePromotionData(Promotion promotion)
        {
            foreach (Promotion promo in fpw_promoList.Items)
            {
                if (promo.Name.Equals(promotion.Name))
                {
                    fpw_promoList.SelectedItem = promo;
                    promo.AverageRating = promotion.AverageRating;
                    promo.MatchCount = promotion.MatchCount;
                    promo.MatchDetails = promotion.MatchDetails;
                    promo.History = promotion.History;
                    break;
                }
            }
            fpw_promoList_SelectedIndexChanged(null, null);
        }
        private void fpw_clearDetails_Click(object sender, EventArgs e)
        {
            if (fpw_promoList.SelectedIndex < 0)
            {
                return;
            }

            Promotion promotion = ((Promotion)fpw_promoList.SelectedItem);
            if (promotion.MatchDetails.Count == 0)
            {
                return;
            }

            promotion.ClearEvents();
            fpw_promoList.SelectedItem = promotion;
            fpw_promoList_SelectedIndexChanged(sender, e);
        }
        private void fpw_refreshRings_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LoadRings();
        }
        private void fpw_addRing_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (fpw_promoList.SelectedIndex < 0)
                {
                    return;
                }

                Promotion promotion = (Promotion)fpw_promoList.SelectedItem;
                String ring = (String)fpw_ringList.SelectedItem;

                //Determine if the current ring exists in any other promotions
                foreach (Promotion item in fpw_promoList.Items)
                {
                    if (item.DoesRingExist(ring))
                    {
                        MessageBox.Show(ring + " is currently used by " + item.Name);
                        fpw_promoList.SelectedItem = item;
                        return;
                    }
                }

                promotion.Rings.Add(ring);
                fpw_promoList.SelectedItem = promotion;
                fpw_promoList_SelectedIndexChanged(null, null);
            }
            catch (Exception exception)
            {
                L.D("AddRingException: " + exception);
            }

        }
        private void fpw_removeRing_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (fpw_promoList.SelectedIndex < 0)
                {
                    return;
                }


                Promotion promotion = (Promotion)fpw_promoList.SelectedItem;

                //Promotions must have at least one home ring
                if (promotion.Rings.Count == 1)
                {
                    return;
                }

                String ring = (String)fpw_promoRingList.SelectedItem;

                promotion.Rings.Remove(ring);
                fpw_promoList.SelectedItem = promotion;
                fpw_promoList_SelectedIndexChanged(null, null);
            }
            catch (Exception exception)
            {
                L.D("RemoveRingError: " + exception);
            }
        }
        private void fpw_resetPoints_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (fpw_promoList.SelectedIndex < 0)
            {
                return;
            }

            if (ms_rosterList.SelectedIndex < 0)
            {
                return;
            }

            Employee employee = (Employee)ms_rosterList.SelectedItem;
            employee.ResetMoralePoints();

            Promotion promotion = (Promotion)fpw_promoList.SelectedItem;
            promotion.UpdateEmployeeData(employee);

            ms_rosterList.SelectedItem = employee;
            fpw_promoList.SelectedItem = promotion;
            ms_rosterList_SelectedIndexChanged(sender, e);
        }
        #endregion

        #region Wrestler Management
        private void ms_rosterList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ms_rosterList.Items.Count == 0 || ms_rosterList.SelectedItem == null)
            {
                return;
            }

            Employee employee = (Employee)ms_rosterList.SelectedItem;
            try
            {
                ms_employeeName.Text = employee.Name;
                ms_employeeStyle.Text = employee.Type;
                ms_employeeCountry.Text = employee.Region;
                ms_moraleRank.SelectedIndex = employee.MoraleRank;
                ms_moralePoints.Text = employee.MoralePoints.ToString();
                ms_empMatches.Text = employee.MatchCount.ToString();
                ms_empRating.Text = Math.Round(Decimal.Parse(employee.AverageRating.ToString()), 2) + "%";
                ms_empRecord.Text = employee.Wins + "/" + employee.Losses + "/" + employee.Draws;

                //Set Championship Data
                var titleData = GetChampionData(employee.Name);
                if (titleData == null)
                {
                    championInfo.Text = "";
                    titleBox.Image = null;
                }
                else
                {
                    int defenseCount = 0;
                    if (titleData.GetLatestMatchRecord() != null)
                    {
                        defenseCount = titleData.GetLatestMatchRecord().DefenseCount;
                    }
                    championInfo.Text = titleData.titleName + "\nTitle Type: " + titleData.matchType + " - " +
                                        titleData.playerNum + "\nTotal Title Holders: " +
                                        TitleMatch_Data.GetOrdinalNumberString(titleData.titleMatch_Record_Data.Count) +
                                        "\nNumber of Defenses: " + defenseCount;
                    titleBox.Image = ModPack.Utility.GetBeltImageFromTex(titleData);
                }
            }
            catch (NullReferenceException)
            {
                L.D(employee.Name + " has null data.");
            }

        }
        private void ms_hireWrestler_Click(object sender, EventArgs e)
        {
            try
            {
                if (fpw_promoList.SelectedIndex < 0)
                {
                    return;
                }
                if (ms_searchResults.SelectedIndex < 0 || ms_searchResults.Items.Count == 0)
                {
                    return;
                }

                WresIDGroup wrestler = (WresIDGroup)ms_searchResults.SelectedItem;
                if (employeesAdded.Contains(wrestler.Name))
                {
                    //Find Wrestler's Promotion
                    MessageBox.Show(wrestler.Name + " is working for " + GetEmployeePromotion(wrestler.Name).Name + ".");
                    return;
                }

                Promotion promotion = (Promotion)fpw_promoList.SelectedItem;
                promotion = HireWrestler(wrestler, promotion);

                fpw_promoList.SelectedItem = promotion;
                fpw_promoList_SelectedIndexChanged(sender, e);
            }
            catch (Exception ex)
            {
                L.D("HireWrestlerException: " + ex);
            }

        }
        private void ms_hireGroup_Click(object sender, EventArgs e)
        {
            try
            {
                if (fpw_promoList.SelectedIndex < 0)
                {
                    return;
                }

                if (ms_groupList.SelectedIndex < 0)
                {
                    return;
                }

                ms_wrestlerSearch.Text = "";
                SearchWrestler();
                Promotion promotion = (Promotion)fpw_promoList.SelectedItem;
                foreach (WresIDGroup wrestler in ms_searchResults.Items)
                {
                    if (!employeesAdded.Contains(wrestler.Name))
                    {
                        promotion = HireWrestler(wrestler, promotion);
                    }
                }
                fpw_promoList_SelectedIndexChanged(sender, e);
            }
            catch (Exception ex)
            {
                L.D("HireGroupException: " + ex);
            }

        }
        private void ms_fireOne_Click(object sender, EventArgs e)
        {
            try
            {
                if (fpw_promoList.SelectedIndex < 0)
                {
                    return;
                }

                if (ms_employeeList.SelectedIndex < 0)
                {
                    return;
                }

                Promotion promotion = (Promotion)fpw_promoList.SelectedItem;
                Employee employee = promotion.GetEmployeeData(ms_employeeList.SelectedItem.ToString());
                fpw_promoList.SelectedItem = FireWrestler(employee, promotion);
                fpw_promoList_SelectedIndexChanged(sender, e);
            }
            catch (Exception exception)
            {
                L.D("FireOne Click Error: " + exception.ToString());
            }

        }
        private void ms_fireAll_Click(object sender, EventArgs e)
        {
            try
            {
                if (fpw_promoList.SelectedIndex < 0)
                {
                    return;
                }

                if (ms_employeeList.Items.Count == 0)
                {
                    return;
                }

                Promotion promotion = (Promotion)fpw_promoList.SelectedItem;
                foreach (String employeeName in ms_employeeList.Items)
                {
                    Employee employee = promotion.GetEmployeeData(employeeName);
                    promotion = FireWrestler(employee, promotion);
                }

                fpw_promoList_SelectedIndexChanged(sender, e);
            }
            catch (Exception exception)
            {
                L.D("FireAll Click Error: " + exception.ToString());
            }

        }
        private Promotion FireWrestler(Employee employee, Promotion promotion)
        {
            try
            {
                employeesAdded.Remove(employee.Name);
                promotion.AddHistory(DateTime.Now.ToString("MM/dd/yyyy HH:mm") + "-" + employee.Name + " has left the promotion.~" + employee.GetMatchHistory() +
                ". Final Record: " + employee.GetRecord());

                MessageBox.Show(DateTime.Now.ToString("MM/dd/yyyy HH:mm") + "-" + employee.Name + " has left the promotion.~" + employee.GetMatchHistory() +
                                ". Final Record: " + employee.GetRecord(), "Resignation Notification", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);


                promotion.RemoveEmployee(employee.Name);
                return promotion;
            }
            catch (Exception e)
            {
                L.D("Fire Wrestler Error: " + e.Message);
                return null;
            }

        }
        private Promotion HireWrestler(WresIDGroup wrestler, Promotion promotion)
        {
            //WRS_PROF profile = DataBase.GetWrestlerProfile((WrestlerID)wrestler.ID);
            WrestlerParam param = DataBase.GetWrestlerParam((WrestlerID)wrestler.ID);

            if (param == null)
            {
                L.D("Param is null");
            }
            int moraleBonus = 0;

            Employee employee = new Employee();
            employee.Name = SanitizeWrestlerName(DataBase.GetWrestlerFullName((WrestlerID)wrestler.ID));
            employee.Region = CorrectRegionName(param.country.ToString());
            employee.Type = CorrectStyleName(param.fightStyle);
            employee.QuitRollCeiling = 5 - (int)param.wrestlerRank;

            //Determine wrestler's starting morale rank.
            //Region
            if (!promotion.Region.Equals(CorrectRegionName(param.country.ToString())))
            {
                moraleBonus -= 1;
            }

            //Fighting Style
            if (!CheckStyle(param.fightStyle, promotion.Type))
            {
                moraleBonus -= 1;
            }

            //Charisma
            if (param.charismaRank == RankEnum.A || param.charismaRank == RankEnum.B)
            {
                moraleBonus += 1;
            }
            if (param.charismaRank == RankEnum.S)
            {
                moraleBonus += 2;
            }

            //Determine if employee has a previous record
            employee = GetPreviousRecord(employee, promotion.History);

            employee.MoraleRank = 2 + moraleBonus;
            promotion.EmployeeList.Add(employee);
            employeesAdded.Add(employee.Name);
            return promotion;
        }
        private void ms_moraleRank_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (fpw_promoList.SelectedIndex < 0)
            {
                return;
            }

            if (ms_rosterList.SelectedIndex < 0)
            {
                return;
            }

            Employee employee = (Employee)ms_rosterList.SelectedItem;
            employee.MoraleRank = ms_moraleRank.SelectedIndex;

            Promotion promotion = (Promotion)fpw_promoList.SelectedItem;
            promotion.UpdateEmployeeData(employee);

            ms_rosterList.SelectedItem = employee;
            fpw_promoList.SelectedItem = promotion;
        }
        private void resetPoints_Click(object sender, EventArgs e)
        {
            if (fpw_promoList.SelectedIndex < 0)
            {
                return;
            }

            if (ms_rosterList.SelectedIndex < 0)
            {
                return;
            }

            Employee employee = (Employee)ms_rosterList.SelectedItem;
            employee.ResetMoralePoints();

            Promotion promotion = (Promotion)fpw_promoList.SelectedItem;
            promotion.UpdateEmployeeData(employee);

            ms_rosterList.SelectedItem = employee;
            fpw_promoList.SelectedItem = promotion;
            ms_rosterList_SelectedIndexChanged(sender, e);
        }
        private void fw_refreshRings_Click(object sender, EventArgs e)
        {
            LoadRings();
        }
        private void btn_clean_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < fpw_promoList.Items.Count; i++)
                {
                    Promotion promotion = (Promotion)fpw_promoList.Items[i];
                    var employeeList = promotion.EmployeeList.ToList();

                    foreach (Employee employee in employeeList)
                    {
                        if (!DoesWrestlerExist(employee.Name))
                        {
                            promotion.RemoveEmployee(employee.Name);
                            L.D("Removing " + employee.Name + " from " + promotion.Name);
                        }
                    }

                    fpw_promoList.Items[i] = promotion;
                }

                L.D("Complete");

                //Reset Roster View
                fpw_promoList_SelectedIndexChanged(null, null);
            }
            catch (Exception exception)
            {
                L.D("Cleaning Error:" + exception);
            }

        }

        #endregion

        #region Reporting
        private void rpt_promotions_Click(object sender, EventArgs e)
        {
            PopulatePromotionReport();
        }
        private void rpt_employees_Click(object sender, EventArgs e)
        {
            PopulateEmployeeReport();
        }

        #endregion

        #region  Filter Management
        private void fpw_historyRefresh_Click(object sender, EventArgs e)
        {
            LoadSubs();
        }

        private void fpw_wrestlerRefresh_Click(object sender, EventArgs e)
        {
            LoadSubs();
        }

        private void fpw_historyCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (fpw_promoList.SelectedIndex < 0)
            {
                return;
            }

            Promotion promotion = (Promotion)fpw_promoList.SelectedItem;

            //Determine which category has been selected
            FilterType type = FilterType.All;
            switch (fpw_historyCategory.SelectedIndex)
            {
                case 0:
                    type = FilterType.All;
                    break;
                case 1:
                    type = FilterType.TitleHistory;
                    break;
                case 2:
                    type = FilterType.Promotions;
                    break;
                case 3:
                    type = FilterType.Demotions;
                    break;
                case 4:
                    type = FilterType.Departures;
                    break;
                case 5:
                    type = FilterType.UpsetVictories;
                    break;
                case 6:
                    type = FilterType.KnockOuts;
                    break;
                case 7:
                    type = FilterType.Injuries;
                    break;
            }

            fpw_promoHistory.Text = FilterHistory("", type, promotion.History);
        }

        private void fpw_historyWrestler_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (fpw_promoList.SelectedIndex < 0)
            {
                return;
            }

            Promotion promotion = (Promotion)fpw_promoList.SelectedItem;

            fpw_promoHistory.Text = FilterHistory(((WresIDGroup)fpw_historyWrestler.SelectedItem).Name, FilterType.Term, promotion.History);
        }

        private void fpw_matchWrestler_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (fpw_promoList.SelectedIndex < 0)
            {
                return;
            }

            Promotion promotion = (Promotion)fpw_promoList.SelectedItem;

            fpw_detailsView.Text = FilterHistory(((WresIDGroup)fpw_matchWrestler.SelectedItem).Name, FilterType.Term, String.Join("~", promotion.MatchDetails.ToArray()));
        }

        private void fpw_MatchTerm_LostFocus(object sender, System.EventArgs e)
        {
            if (fpw_promoList.SelectedIndex < 0)
            {
                return;
            }

            Promotion promotion = (Promotion)fpw_promoList.SelectedItem;

            fpw_detailsView.Text = FilterHistory(fpw_matchTerm.Text, FilterType.Term, String.Join("~", promotion.MatchDetails.ToArray()));
        }

        private void fpw_HistoryTerm_LostFocus(object sender, System.EventArgs e)
        {
            if (fpw_promoList.SelectedIndex < 0)
            {
                return;
            }

            Promotion promotion = (Promotion)fpw_promoList.SelectedItem;

            fpw_promoHistory.Text = FilterHistory(fpw_historyTerm.Text, FilterType.Term, promotion.History);
        }

        private void fpw_rosterFiler_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (fpw_promoList.SelectedIndex < 0)
                {
                    return;
                }

                Promotion promotion = (Promotion)fpw_promoList.SelectedItem;

                List<Employee> sortedList = new List<Employee>();

                switch ((String)fpw_rosterFiler.SelectedItem)
                {
                    case "Wins":
                        sortedList = promotion.EmployeeList.OrderByDescending(x => x.Wins).ToList();
                        break;
                    case "Losses":
                        sortedList = promotion.EmployeeList.OrderByDescending(x => x.Losses).ToList();
                        break;
                    case "Total Matches":
                        sortedList = promotion.EmployeeList.OrderByDescending(x => x.MatchCount).ToList();
                        break;
                    case "Average Rating":
                        sortedList = promotion.EmployeeList.OrderByDescending(x => x.AverageRating).ToList();
                        break;
                    case "Morale":
                        sortedList = promotion.EmployeeList.OrderByDescending(x => x.MoraleRank).ToList();
                        break;
                    case "Alphabetical":
                    default:
                        sortedList = promotion.EmployeeList.OrderBy(x => x.Name).ToList();
                        break;
                }

                ms_rosterList.Items.Clear();
                foreach (var employee in sortedList)
                {
                    ms_rosterList.Items.Add(employee);
                }

                if (ms_rosterList.Items.Count > 0)
                {
                    ms_rosterList_SelectedIndexChanged(null, null);
                }
            }
            catch (Exception exception)
            {
                L.D("FilterRosterException: " + exception);
            }

        }

        #endregion

        #region Injuries
        private void fpw_addNeck_Click(object sender, EventArgs e)
        {
            if (fpw_neckText.Text.Equals(String.Empty))
            {
                return;
            }

            try
            {
                fpw_neckInjuries.Items.Add(fpw_neckText.Text);
                fpw_neckInjuries.SelectedIndex = 0;
                fpw_neckInjuries_SelectedIndexChanged(null, null);
            }
            catch (Exception exception)
            {
                L.D("AddNeckException: " + exception);
            }
        }

        private void fpw_removeNeck_Click(object sender, EventArgs e)
        {
            if (fpw_neckInjuries.SelectedIndex < 0 || fpw_neckInjuries.Items.Count == 1)
            {
                return;
            }

            fpw_neckInjuries.Items.RemoveAt(fpw_neckInjuries.SelectedIndex);
            if (fpw_neckInjuries.Items.Count > 0)
            {
                fpw_neckInjuries.SelectedIndex = (fpw_neckInjuries.Items.Count - 1);
                fpw_neckInjuries_SelectedIndexChanged(null, null);

            }
        }

        private void fpw_addWaist_Click(object sender, EventArgs e)
        {
            if (fpw_waistText.Text.Equals(String.Empty))
            {
                return;
            }

            try
            {
                fpw_waistInjuries.Items.Add(fpw_waistText.Text);
                fpw_waistInjuries.SelectedIndex = 0;
                fpw_waistInjuries_SelectedIndexChanged(null, null);
            }
            catch (Exception exception)
            {
                L.D("AddWaistException: " + exception);
            }
        }

        private void fpw_removeWaist_Click(object sender, EventArgs e)
        {
            if (fpw_waistInjuries.SelectedIndex < 0 || fpw_waistInjuries.Items.Count == 1)
            {
                return;
            }

            fpw_waistInjuries.Items.RemoveAt(fpw_waistInjuries.SelectedIndex);
            if (fpw_waistInjuries.Items.Count > 0)
            {
                fpw_waistInjuries.SelectedIndex = (fpw_waistInjuries.Items.Count - 1);
                fpw_waistInjuries_SelectedIndexChanged(null, null);

            }
        }

        private void fpw_addArm_Click(object sender, EventArgs e)
        {
            if (fpw_armText.Text.Equals(String.Empty))
            {
                return;
            }

            try
            {
                fpw_armInjuries.Items.Add(fpw_armText.Text);
                fpw_armInjuries.SelectedIndex = 0;
                fpw_armInjuries_SelectedIndexChanged(null, null);
            }
            catch (Exception exception)
            {
                L.D("AddArmException: " + exception);
            }
        }

        private void fpw_removeArm_Click(object sender, EventArgs e)
        {
            if (fpw_armInjuries.SelectedIndex < 0 || fpw_armInjuries.Items.Count == 1)
            {
                return;
            }

            fpw_armInjuries.Items.RemoveAt(fpw_armInjuries.SelectedIndex);
            if (fpw_armInjuries.Items.Count > 0)
            {
                fpw_armInjuries.SelectedIndex = (fpw_armInjuries.Items.Count - 1);
                fpw_armInjuries_SelectedIndexChanged(null, null);

            }
        }

        private void fpw_addLeg_Click(object sender, EventArgs e)
        {
            if (fpw_legText.Text.Equals(String.Empty))
            {
                return;
            }

            try
            {
                fpw_legInjuries.Items.Add(fpw_legText.Text);
                fpw_legInjuries.SelectedIndex = 0;
                fpw_legInjuries_SelectedIndexChanged(null, null);
            }
            catch (Exception exception)
            {
                L.D("AddLegException: " + exception);
            }
        }

        private void fpw_removeLeg_Click(object sender, EventArgs e)
        {
            if (fpw_legInjuries.SelectedIndex < 0 || fpw_legInjuries.Items.Count == 1)
            {
                return;
            }

            fpw_legInjuries.Items.RemoveAt(fpw_legInjuries.SelectedIndex);
            if (fpw_legInjuries.Items.Count > 0)
            {
                fpw_legInjuries.SelectedIndex = (fpw_legInjuries.Items.Count - 1);
                fpw_legInjuries_SelectedIndexChanged(null, null);

            }
        }

        private void fpw_neckInjuries_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (fpw_neckInjuries.SelectedIndex < 0)
            {
                return;
            }

            String injury = (String)fpw_neckInjuries.SelectedItem;
            fpw_injuryExample.Text = "Wrestler has suffered " + injury + ".";
            fpw_neckText.Text = injury;
        }

        private void fpw_waistInjuries_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (fpw_waistInjuries.SelectedIndex < 0)
            {
                return;
            }

            String injury = (String)fpw_waistInjuries.SelectedItem;
            fpw_injuryExample.Text = "Wrestler has suffered " + injury + ".";
            fpw_waistText.Text = injury;
        }

        private void fpw_armInjuries_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (fpw_armInjuries.SelectedIndex < 0)
            {
                return;
            }

            String injury = (String)fpw_armInjuries.SelectedItem;
            fpw_injuryExample.Text = "Wrestler has suffered " + injury + ".";
            fpw_armText.Text = injury;
        }

        private void fpw_legInjuries_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (fpw_legInjuries.SelectedIndex < 0)
            {
                return;
            }

            String injury = (String)fpw_legInjuries.SelectedItem;
            fpw_injuryExample.Text = "Wrestler has suffered " + injury + ".";
            fpw_legText.Text = injury;
        }
        #endregion

        #region Helper Methods
        private void SelectPromotionRoster(String name)
        {
            for (int i = 0; i < ms_groupList.Items.Count; i++)
            {
                if (ms_groupList.Items[i].ToString().Contains(name))
                {
                    ms_groupList.SelectedIndex = i;
                    SearchWrestler();
                    break;
                }
            }
        }
        private void PrepareForLoad()
        {
            promotionsAdded = new HashSet<String>();
            employeesAdded = new HashSet<String>();
            fpw_promoList.Items.Clear();
            fpw_promoHistory.Clear();
            ms_employeeList.Items.Clear();
            ms_rosterList.Items.Clear();
        }
        private bool CheckStyle(FightStyleEnum fightStyle, String promotionStyle)
        {
            FightStyleEnum[] styles = groupFightingStyles[promotionStyle];
            foreach (FightStyleEnum style in styles)
            {
                if (style.Equals(fightStyle))
                {
                    return true;
                }
            }
            return false;
        }
        private String CorrectStyleName(FightStyleEnum style)
        {
            string styleName = "";
            switch (style)
            {
                case FightStyleEnum.Devilism:
                    styleName = "Inokism";
                    break;
                case FightStyleEnum.Wrestling:
                    styleName = "Amateur Wrestling";
                    break;
                case FightStyleEnum.Ground:
                    styleName = "Ground Style";
                    break;
                case FightStyleEnum.Panther:
                    styleName = "Lucha/Martial Arts";
                    break;
                case FightStyleEnum.Fighter:
                    styleName = "Striking";
                    break;
                case FightStyleEnum.Shooter:
                    styleName = "Shoot Fighting";
                    break;
                case FightStyleEnum.Grappler:
                    styleName = "Grappling";
                    break;
                case FightStyleEnum.Heel:
                    styleName = "Heel Tactics";
                    break;
                case FightStyleEnum.Luchador:
                    styleName = "Lucha";
                    break;
                case FightStyleEnum.American:
                    styleName = "American Wrestling";
                    break;
                case FightStyleEnum.Orthodox:
                    styleName = "Orthodox Wrestling";
                    break;
                default:
                    styleName = style.ToString();
                    break;

            }
            return styleName;
        }
        private String SanitizeWrestlerName(String name)
        {
            return name.Replace("|", " ").Replace(";", " ").Replace("~", " ");
        }
        public Promotion GetRingPromotion(String ringName)
        {
            foreach (Promotion promotion in fpw_promoList.Items)
            {
                foreach (String ring in promotion.Rings)
                {
                    if (ring.Equals(ringName))
                    {
                        return promotion;
                    }
                }

            }

            return null;
        }
        private static String CorrectRegionName(String region)
        {
            //
            switch (region.ToLower())
            {
                case "uitedstates":
                case "america":
                    return "United States";
                case "unitedkingdom":
                case "england":
                    return "United Kingdom";
                case "puertorico":
                    return "Puerto Rico";
                case "southkorea":
                    return "South Korea";
                case "northkorea":
                    return "North Korea";
                case "newzealand":
                    return "New Zealand";
                case "southafrica":
                    return "South Africa";
                case "trinidadandtobago":
                    return "Trinidad And Tobago";
                default:
                    return region;
            }
        }
        public void UpdateEmployeeMorale(Employee employee, bool isSuccess)
        {
            Promotion promotion = GetEmployeePromotion(employee.Name);
            if (promotion == null)
            {
                return;

            }

            if (employee.Name.Equals(String.Empty))
            {
                L.D("Name is empty, unable to process");
            }

            //Determine if the employee should be promoted.
            if (isSuccess)
            {
                if (ProcessEmployeePromotion(employee))
                {
                    employee.MoralePoints = 0;
                    employee.MoraleRank += 1;
                    employee.FailedQuitRolls = 0;
                    if (ValidateRank(employee.MoraleRank, ms_moraleRank.Items.Count))
                    {
                        promotion.AddHistory(DateTime.Now.ToString("MM/dd/yyyy HH:mm") + "-" + employee.Name +
                                             " has been promoted (Rank " +
                                             ms_moraleRank.Items[employee.MoraleRank].ToString() + ").");

                        MessageBox.Show(DateTime.Now.ToString("MM/dd/yyyy HH:mm") + "-" + employee.Name +
                                        " has been promoted (Rank " +
                                        ms_moraleRank.Items[employee.MoraleRank].ToString() + ").", "Promotion Notification", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
            else
            {
                //Determine if employee should be demoted
                if (ProcessEmployeeDemotion(employee))
                {
                    employee.MoralePoints = 0;
                    employee.MoraleRank -= 1;
                    if (ValidateRank(employee.MoraleRank, ms_moraleRank.Items.Count))
                    {
                        promotion.AddHistory(DateTime.Now.ToString("MM/dd/yyyy HH:mm") + "-" + employee.Name +
                                             " has been demoted (Rank " +
                                             ms_moraleRank.Items[employee.MoraleRank].ToString() + ").");

                        MessageBox.Show(DateTime.Now.ToString("MM/dd/yyyy HH:mm") + "-" + employee.Name +
                                        " has been demoted (Rank " +
                                        ms_moraleRank.Items[employee.MoraleRank].ToString() + ").", "Demotion Notification", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else
                {
                    int failedRolls = ProcessEmployeeQuit(employee);
                    if (failedRolls == -1)
                    {
                        promotion = FireWrestler(employee, promotion);
                    }
                    else
                    {
                        employee.FailedQuitRolls = failedRolls;
                    }
                }
            }

            //Ensure Employee Data is Updated
            promotion.UpdateEmployeeData(employee);
            fpw_promoList.SelectedItem = promotion;
            fpw_promoList_SelectedIndexChanged(null, null);
        }
        private int ProcessEmployeeQuit(Employee employee)
        {
            if (employee.MoraleRank != 0)
            {
                return employee.FailedQuitRolls;
            }

            if (employee.MoralePoints < -5)
            {
                int value = rnd.Next(employee.MoralePoints, 1);
                if (value < -5)
                {
                    employee.FailedQuitRolls++;
                }

                if (employee.FailedQuitRolls >= employee.QuitRollCeiling)
                {
                    return -1;
                }

                Promotion promotion = GetEmployeePromotion(employee.Name);
                promotion.UpdateEmployeeData(employee);
                fpw_promoList.SelectedItem = promotion;
            }

            return employee.FailedQuitRolls;
        }
        private bool ProcessEmployeePromotion(Employee employee)
        {
            if (employee.MoraleRank == 5)
            {
                return false;
            }
            if (employee.MoralePoints >= (employee.MoraleRank * promotionFactor))
            {
                L.D("Attempting Promotion For " + employee.Name);
                int value = rnd.Next(1, employee.MoralePoints);
                if (value >= (employee.MoraleRank) * promotionFactor)
                {
                    L.D("Promotion success: " + value + " vs " + employee.MoraleRank * promotionFactor);
                    return true;
                }

                L.D("Promotion failure: " + value + " vs " + employee.MoraleRank * promotionFactor);
            }
            return false;
        }
        private bool ProcessEmployeeDemotion(Employee employee)
        {
            if (employee.MoralePoints < (employee.MoraleRank * demotionFactor))
            {
                L.D("Attempting Demotion For " + employee.Name);
                int value = rnd.Next(employee.MoralePoints, 1);
                if (value < (employee.MoraleRank * demotionFactor))
                {
                    L.D("Demotion success: " + value + " vs " + employee.MoraleRank * demotionFactor);
                    return true;
                }

                L.D("Demotion failure: " + value + " vs " + employee.MoraleRank * demotionFactor);
            }
            return false;
        }
        private Promotion GetEmployeePromotion(String employeeName)
        {
            Promotion promotion = null;
            foreach (Promotion promo in fpw_promoList.Items)
            {
                if (promo.GetEmployeeData(employeeName) != null)
                {
                    promotion = promo;
                    fpw_promoList.SelectedItem = promo;
                }
            }
            return promotion;
        }
        private bool DoesWrestlerExist(String wrestlerName)
        {
            bool isFound = false;
            foreach (EditWrestlerData current in SaveData.inst.editWrestlerData)
            {
                if (wrestlerName.Equals(DataBase.GetWrestlerFullName(current.wrestlerParam)))
                {
                    isFound = true;
                    break;
                }
            }

            return isFound;
        }
        private bool ValidateRank(int rank, int maxValue)
        {
            if (rank >= 0 && rank < maxValue)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void PopulatePromotionReport()
        {

            String savePath = "./EGOData/Reports/PromotionList.csv";
            using (StreamWriter sw = File.AppendText(savePath))
            {
                foreach (Promotion promotion in fpw_promoList.Items)
                {
                    sw.WriteLine(promotion.GetReportFormat());
                }
            }

        }
        private void PopulateEmployeeReport()
        {
            String savePath = csvLocation + "EmployeeList.csv";
            if (File.Exists(savePath))
            {
                File.Delete(savePath);
            }
            using (StreamWriter sw = File.AppendText(savePath))
            {
                //Add headers
                String header = "Promotion,Name,Style,Home Country,Total Matches, Average Rating, Wins, Losses, Draws, Morale Rank, Morale Points";
                sw.WriteLine(header);
                //return new List<String> { name, type, region, matchCount.ToString(), Math.Round(averageRating, 2).ToString(), wins.ToString(), losses.ToString(), draws.ToString() };
                foreach (Promotion promotion in fpw_promoList.Items)
                {
                    foreach (Employee employee in promotion.EmployeeList)
                    {
                        string report = promotion.Name + ", ";
                        var reportData = employee.GetReportFormat();
                        reportData.Add(moraleRank[employee.MoraleRank]);
                        reportData.Add(employee.MoralePoints.ToString());
                        foreach (var data in reportData)
                        {
                            report += data + ",";
                        }

                        sw.WriteLine(report);
                    }
                }
            }
        }
        private void PopulateRosterReport(Promotion promotion)
        {
            String savePath = csvLocation + promotion.Name + ".csv";

            if (File.Exists(savePath))
            {
                File.Delete(savePath);
            }
            using (StreamWriter sw = File.AppendText(savePath))
            {
                foreach (Employee employee in promotion.EmployeeList)
                {
                    var reportData = employee.GetReportFormat();
                    reportData.Add(moraleRank[employee.MoraleRank]);
                    reportData.Add(employee.MoralePoints.ToString());
                    string report = "";
                    foreach (var data in reportData)
                    {
                        report += data + ",";
                    }

                    sw.WriteLine(report);
                }
            }


        }
        private void WriteToCSV()
        {
            try
            {
                PopulatePromotionReport();
                PopulateEmployeeReport();
            }
            catch (Exception e)
            {
                L.D("WriteToCSVError: " + e);
            }

        }
        private void CheckForDirectory(String filePath)
        {
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
        }
        private void FindImage(ImageTypes imageType, String imageName)
        {
            try
            {
                String filePath = imageRootFolder;

                CheckForDirectory(filePath);

                if (imageType == ImageTypes.Wrestler)
                {
                    filePath += imageFolders[0];
                }
                else if (imageType == ImageTypes.Logo)
                {
                    filePath += imageFolders[1];
                }

                CheckForDirectory(filePath);

                //Check each file in the directory
                var files = Directory.GetFiles(filePath);

                String fileName = "";
                imageName = imageName.ToLower(); //Eliminate casing for easier matching

                foreach (String file in files)
                {
                    String name = Path.GetFileNameWithoutExtension(file);
                    if (name.ToLower().Equals(imageName))
                    {
                        fileName = file;
                        break;
                    }
                }

                //Determine whether we can display an image
                if (imageType == ImageTypes.Wrestler)
                {
                    if (fileName.Equals(String.Empty))
                    {
                        //Determine whether the default image is available
                        if (File.Exists(filePath + defaultWrestlerImage))
                        {
                            fpw_wrestlerImage.Image = Image.FromFile(filePath + defaultWrestlerImage);
                        }
                    }
                    else
                    {
                        fpw_wrestlerImage.Image = Image.FromFile(fileName);
                    }
                }
                else if (imageType == ImageTypes.Logo)
                {
                    if (fileName.Equals(String.Empty))
                    {
                        fpw_logoImage.Image = null;
                        fpw_logoImage.Visible = false;
                        lbl_promoDetails.Visible = true;
                        return;
                    }
                    else
                    {
                        fpw_logoImage.Image = Image.FromFile(fileName);
                        fpw_logoImage.Visible = true;
                        lbl_promoDetails.Visible = false;
                    }
                }
            }
            catch (Exception e)
            {
                L.D("FindImageException: " + e);
            }

        }
        private String FilterHistory(String term, FilterType type, String details)
        {
            if (type == FilterType.All)
            {
                var split = details.Split('~');
                Array.Reverse(split);
                var lineSplit = split.ToList();

                String value = "";
                foreach (String line in lineSplit)
                {
                    value += "\n" + line + "\n";
                }

                return value;
            }

            String result = "";

            //Sort by most recent
            var splitArray = details.Split('~');
            Array.Reverse(splitArray);
            var lines = splitArray.ToList();

            foreach (String line in lines)
            {
                String data = "";
                switch (type)
                {
                    case FilterType.TitleHistory:
                        if (line.Contains("defends") || line.Contains("gained") || line.Contains("lost"))
                        {
                            data = line;
                        }
                        break;
                    case FilterType.Demotions:
                        if (line.Contains("demoted"))
                        {
                            data = line;
                        }

                        break;
                    case FilterType.Promotions:
                        if (line.Contains("promoted"))
                        {
                            data = line;
                        }
                        break;
                    case FilterType.Departures:
                        if (line.Contains("left the promotion") || line.Contains("Final Record"))
                        {
                            data = line;
                        }

                        break;
                    case FilterType.UpsetVictories:
                        if (line.Contains("upset victory"))
                        {
                            data = line;
                        }
                        break;
                    case FilterType.KnockOuts:
                        if (line.Contains("knocked out"))
                        {
                            data = line;
                        }
                        break;
                    case FilterType.Injuries:
                        if (line.Contains("suffered"))
                        {
                            data = line;
                        }
                        break;
                    case FilterType.Term:
                        if (line.Contains(term))
                        {
                            data = line;
                        }
                        break;
                }

                if (!data.Equals(String.Empty))
                {
                    result += "\n" + data + "\n";
                }
            }
            return result;
        }
        private bool RingExists(String ring)
        {
            foreach (Promotion item in fpw_promoList.Items)
            {
                if (item.DoesRingExist(ring))
                {
                    return true;
                }
            }

            return false;
        }
        private TitleMatch_Data GetChampionData(String wrestlerName)
        {
            TitleMatch_Data data = null;

            try
            {
                foreach (TitleMatch_Data item in SaveData.GetInst().titleMatch_Data)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (item.wrestlerID[i] == WrestlerID.Invalid)
                        {
                            continue;
                        }
                        if (wrestlerName.Equals(DataBase.GetWrestlerFullName(item.wrestlerID[i])))
                        {
                            data = item;
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                L.D("GetChampionDataError: " + e);
            }

            return data;
        }
        private Employee GetPreviousRecord(Employee employee, String history)
        {
            try
            {
                var details = history.Split('~');
                String lookup = employee.Name + " has left the promotion.";

                for (int i = 0; i < details.Length; i++)
                {
                    if (details[i].Contains(lookup))
                    {
                        if (i + 1 < details.Length)
                        {

                            var record = details[i + 1];

                            L.D("Previous Record Found: " + record);

                            //Total Matches: # Average Rating: #. Final Record: 1/0/0
                            var recordInfo = record.Split(' ');
                            for (int j = 0; j < recordInfo.Length; j++)
                            {
                                if (recordInfo[j].Equals("Matches:"))
                                {
                                    Int32.TryParse(recordInfo[j + 1], out int matchCount);
                                    employee.MatchCount = matchCount;
                                }
                                else if (recordInfo[j].Equals("Rating:"))
                                {
                                    var value = recordInfo[j + 1].Split('.');
                                    float.TryParse(value[0], out float avg);
                                    employee.AverageRating = avg;
                                }
                                else if (recordInfo[j].Equals("Record:"))
                                {
                                    var results = recordInfo[j + 1].Split('/');
                                    Int32.TryParse(results[0], out int wins);
                                    Int32.TryParse(results[1], out int losses);
                                    Int32.TryParse(results[2], out int draws);

                                    employee.Wins = wins;
                                    employee.Losses = losses;
                                    employee.Draws = draws;
                                }
                            }
                        }

                        break;
                    }
                }
            }
            catch (Exception e)
            {
                L.D("GetPreviousRecordError: " + e);
            }

            return employee;

        }

        private void SendJSONData()
        {
            try
            {
                String userID = Steamworks.SteamUser.GetSteamID().ToString();
                if (!userID.Equals("76561198100955117"))
                {
                    return;
                }


                List<String> jsonPromotions = new List<string>();
                List<String> jsonEmployees = new List<string>();
                List<String> jsonTitles = new List<string>();

                foreach (Promotion promotion in fpw_promoList.Items)
                {
                    jsonPromotions.Add(JSONBuilder.CreateJSONPromotion(promotion, userID));

                    foreach (Employee employee in promotion.EmployeeList)
                    {
                        jsonEmployees.Add(JSONBuilder.CreateJSONEmployee(employee, promotion.Name, userID));
                    }

                }

                foreach (TitleMatch_Data item in SaveData.GetInst().titleMatch_Data)
                {
                    jsonTitles.Add(JSONBuilder.CreateJSONTitle(item, userID));
                }


                File.WriteAllLines(saveFolderNames[0] + "Promotions.json", jsonPromotions.ToArray());
                File.WriteAllLines(saveFolderNames[0] + "Employees.json", jsonEmployees.ToArray());
                File.WriteAllLines(saveFolderNames[0] + "Titles.json", jsonTitles.ToArray());

            }
            catch (Exception e)
            {
                L.D("SendJSONData Error: " + e);
            }
        }
        #endregion

        //Required Files - System.Runtime.Serialization, Newton.Json
        private void sendJSONButton_Click(object sender, EventArgs e)
        {
            SendJSONData();
        }
    }
}
