using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DG;
using MatchConfig;
using Data_Classes;
using System.IO;
using System.Linq;

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
        private static String[] saveFileNames = new String[] { "FPWData.dat" };
        private static HashSet<String> promotionsAdded = new HashSet<String>();
        private static HashSet<String> employeesAdded = new HashSet<String>();
        private static String promotionDivider = "|--Promotion--|";
        public static System.Random rnd = new System.Random();
        public static War_Form form = null;
        public static int promotionFactor = 5;
        public static int demotionFactor = -5;
        public static String csvLocation = "./EGOData/Reports";
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
            FormClosing += WarForm_FormClosing;
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
                wrestlerList.Clear();

                foreach (EditWrestlerData current in SaveData.inst.editWrestlerData)
                {
                    WresIDGroup wresIDGroup = new WresIDGroup();
                    wresIDGroup.Name = DataBase.GetWrestlerFullName(current.wrestlerParam);
                    wresIDGroup.ID = (Int32)WrestlerID.EditWrestlerIDTop + SaveData.inst.editWrestlerData.IndexOf(current);
                    wresIDGroup.Group = current.wrestlerParam.groupID;

                    wrestlerList.Add(wresIDGroup);
                    this.ms_searchResults.Items.Add(wresIDGroup);
                }

                this.ms_searchResults.SelectedIndex = 0;

            }
            catch
            {

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
            fpw_promoRingList.Items.Clear();
            foreach (String ring in MatchConfiguration.LoadRings())
            {
                fpw_promoRingList.Items.Add(ring);
            }

            fpw_promoRingList.SelectedIndex = 0;
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

        #endregion

        #region Data Load
        private void LoadWarData()
        {
            try
            {
                PrepareForLoad();
                String filePath = saveFolderNames[0] + saveFileNames[0];
                if (File.Exists(filePath))
                {
                    var lines = File.ReadAllLines(filePath);
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
                            promotion.Ring = lines[i + 2];
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

                    if (fpw_promoList.Items.Count > 0)
                    {
                        fpw_promoList.SelectedIndex = 0;
                        fpw_promoList_SelectedIndexChanged(null, null);
                    }
                }
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
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            String filePath = saveFolderNames[0] + saveFileNames[0];
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            using (StreamWriter sw = File.AppendText(filePath))
            {
                //Save Promotions
                foreach (Promotion promotion in fpw_promoList.Items)
                {
                    //Promotion Details
                    sw.WriteLine(promotionDivider);
                    sw.WriteLine(promotion.Name);
                    sw.WriteLine(promotion.Ring);
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
        #endregion

        #region Promotion Management
        private void fpw_refreshRings_Click(object sender, EventArgs e)
        {
            LoadRings();
        }
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

            Promotion promotion = new Promotion();
            promotion.Name = fpw_promoName.Text.Trim();
            promotion.Region = fpw_promoRegionList.SelectedItem.ToString();
            promotion.Ring = fpw_promoRingList.SelectedItem.ToString();
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
            promotion.Ring = fpw_promoRingList.SelectedItem.ToString();
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
            fpw_promoRingList.SelectedIndex = fpw_promoRingList.Items.IndexOf(promotion.Ring);
            fpw_promoStyleList.SelectedIndex = fpw_promoStyleList.Items.IndexOf(promotion.Type);
            fpw_promoMthCnt.Text = promotion.MatchCount.ToString();
            fpw_promoMthRating.Text = Math.Round(Decimal.Parse(promotion.AverageRating.ToString()), 2) + "%";
            fpw_promoHistory.Text = promotion.History.Replace("~", "\n");

            #region Adding Details
            String details = "";
            foreach (String detail in promotion.MatchDetails)
            {
                details += "\n" + detail;
            }
            fpw_detailsView.Text = details;
            #endregion

            ms_employeeList.Items.Clear();
            ms_rosterList.Items.Clear();

            //Roster details
            foreach (Employee employee in promotion.EmployeeList)
            {
                ms_employeeList.Items.Add(employee.Name);
                ms_rosterList.Items.Add(employee);
            }

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
        #endregion

        #region Wrestler Management
        private void ms_rosterList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ms_rosterList.Items.Count == 0)
            {
                return;
            }

            Employee employee = (Employee)ms_rosterList.SelectedItem;
            ms_employeeName.Text = employee.Name;
            ms_employeeStyle.Text = employee.Type;
            ms_employeeCountry.Text = employee.Region;
            ms_moraleRank.SelectedIndex = employee.MoraleRank;
            ms_moralePoints.Text = employee.MoralePoints.ToString();
            ms_empMatches.Text = employee.MatchCount.ToString();
            ms_empRating.Text = Math.Round(Decimal.Parse(employee.AverageRating.ToString()), 2) + "%";
            ms_empRecord.Text = employee.Wins + "/" + employee.Losses + "/" + employee.Draws;
        }
        private void ms_hireWrestler_Click(object sender, EventArgs e)
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
        private void ms_hireGroup_Click(object sender, EventArgs e)
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
            WRS_PROF profile = DataBase.GetWrestlerProfile((WrestlerID)wrestler.ID);
            WrestlerParam param = DataBase.GetWrestlerParam((WrestlerID)wrestler.ID);
            int moraleBonus = 0;

            Employee employee = new Employee();
            employee.Name = SanitizeWrestlerName(DataBase.GetWrestlerFullName((WrestlerID)wrestler.ID));
            employee.Region = CorrectRegionName(profile.country);
            employee.Type = CorrectStyleName(param.fightStyle);
            employee.QuitRollCeiling = 5 - (int)param.wrestlerRank;

            //Determine wrestler's starting morale rank.
            //Region
            if (!promotion.Region.Equals(CorrectRegionName(profile.country)))
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
                if (promotion.Ring.Equals(ringName))
                {
                    return promotion;
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
        public void UpdateEmployeeMorale(Employee employee, bool isWinner)
        {
            Promotion promotion = GetEmployeePromotion(employee.Name);
            if (promotion == null)
            {
                return;

            }

            //Determine if the employee should be promoted.
            if (isWinner)
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
            String savePath = "./EGOData/Reports/EmployeeList.csv";
            if (File.Exists(savePath))
            {
                File.Delete(savePath);
            }
            using (StreamWriter sw = File.AppendText(savePath))
            {
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
            String savePath = "./EGOData/Reports/" + promotion.Name + ".csv";

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

        #endregion
    }
}
