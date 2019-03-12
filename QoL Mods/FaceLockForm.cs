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
    public partial class FaceLockForm : Form
    {
        public FaceLockForm()
        {
            InitializeComponent();
            LoadFaceLocks();
        }

        public static FaceLockForm form = null;
        private List<WresIDGroup> wrestlerList = new List<WresIDGroup>();
        private static String[] saveFileNames = new String[] { "StyleFL.dat", "WrestlerFL.dat" };
        private static String[] saveFolderNames = new String[] { "./EGOData/" };

        private void LoadFaceLocks()
        {
            //Load Styles
            String filePath = CheckSaveFile("StyleFL");
            try
            {
                if (File.Exists(filePath))
                {
                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        styleMoves.Clear();
                        var lines = File.ReadAllLines(filePath);
                        foreach (String style in lines)
                        {
                            FaceLockMoves moves = new FaceLockMoves(new QoL_Mods.Data_Classes.Style("", FightStyleEnum.American));
                            moves.LoadFaceLockData(style);
                            styleMoves.Add(moves);
                        }
                    }

                    foreach (FaceLockMoves moves in styleMoves)
                    {
                        nl_styleBox.Items.Add(moves);
                    }

                    if (nl_styleBox.Items.Count > 0)
                    {
                        nl_styleBox.SelectedIndex = 0;
                        nl_styleBox_SelectedIndexChanged(null, null);
                    }
                }
                else
                {
                    SetStyles();
                }
            }
            catch (Exception e)
            {
                L.D("Load Face Lock Error (Style): " + e);
            }

            //Load Wrestlers
            filePath = CheckSaveFile("WrestlerFL");
            try
            {
                if (File.Exists(filePath))
                {
                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        var lines = File.ReadAllLines(filePath);
                        foreach (String wrestler in lines)
                        {
                            FaceLockMoves moves = new FaceLockMoves(new QoL_Mods.Data_Classes.Style("", FightStyleEnum.American));
                            moves.LoadFaceLockData(wrestler);
                            wrestlerMoves.Add(moves);
                        }
                    }

                    foreach (FaceLockMoves moves in wrestlerMoves)
                    {
                        nl_wresterList.Items.Add(moves);
                    }

                    if (nl_wresterList.Items.Count > 0)
                    {
                        nl_wresterList.SelectedIndex = 0;
                        nl_wresterList_SelectedIndexChanged(null, null);
                    }
                }
            }
            catch (Exception e)
            {
                L.D("Load Face Lock Error (Wrestlers): " + e);
            }
        }

        private void SaveFaceLocks()
        {
            //Save Style Face Lock Moves
            String filePath = CheckSaveFile("StyleFL");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            using (StreamWriter sw = File.AppendText(filePath))
            {
                foreach (FaceLockMoves move in styleMoves)
                {
                    L.D("Saving " + move.StyleItem.Name);
                    sw.WriteLine(move.SaveFaceLockData());
                }
            }

            //Save Wrestler Face Lock Moves
            filePath = CheckSaveFile("WrestlerFL");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            if (wrestlerMoves.Count == 0)
            {
                return;
            }
            using (StreamWriter sw = File.AppendText(filePath))
            {
                foreach (FaceLockMoves move in wrestlerMoves)
                {
                    sw.WriteLine(move.SaveFaceLockData());
                }
            }
        }

        #region Front Neck Locks
        #region Variables
        public static List<FaceLockMoves> styleMoves = new List<FaceLockMoves>();
        public static List<FaceLockMoves> wrestlerMoves = new List<FaceLockMoves>();
        #endregion

        #region Setup
        private void SetStyles()
        {
            nl_styleBox.Items.Add(new FaceLockMoves(new QoL_Mods.Data_Classes.Style("Orthodox", FightStyleEnum.Orthodox)));
            nl_styleBox.Items.Add(new FaceLockMoves(new Data_Classes.Style("Technician", FightStyleEnum.Technician)));
            nl_styleBox.Items.Add(new FaceLockMoves(new QoL_Mods.Data_Classes.Style("Wrestling", FightStyleEnum.Wrestling)));
            nl_styleBox.Items.Add(new FaceLockMoves(new QoL_Mods.Data_Classes.Style("Ground", FightStyleEnum.Ground)));
            nl_styleBox.Items.Add(new FaceLockMoves(new QoL_Mods.Data_Classes.Style("Power", FightStyleEnum.Power)));
            nl_styleBox.Items.Add(new FaceLockMoves(new QoL_Mods.Data_Classes.Style("American", FightStyleEnum.American)));
            nl_styleBox.Items.Add(new FaceLockMoves(new Data_Classes.Style("Junior", FightStyleEnum.Junior)));
            nl_styleBox.Items.Add(new FaceLockMoves(new QoL_Mods.Data_Classes.Style("Luchador", FightStyleEnum.Luchador)));
            nl_styleBox.Items.Add(new FaceLockMoves(new QoL_Mods.Data_Classes.Style("Heel", FightStyleEnum.Heel)));
            nl_styleBox.Items.Add(new FaceLockMoves(new QoL_Mods.Data_Classes.Style("Mysterious", FightStyleEnum.Mysterious)));
            nl_styleBox.Items.Add(new FaceLockMoves(new QoL_Mods.Data_Classes.Style("Shooter", FightStyleEnum.Shooter)));
            nl_styleBox.Items.Add(new FaceLockMoves(new QoL_Mods.Data_Classes.Style("Fighter", FightStyleEnum.Fighter)));
            nl_styleBox.Items.Add(new FaceLockMoves(new QoL_Mods.Data_Classes.Style("Grappler", FightStyleEnum.Grappler)));
            nl_styleBox.Items.Add(new FaceLockMoves(new QoL_Mods.Data_Classes.Style("Panther", FightStyleEnum.Panther)));
            nl_styleBox.Items.Add(new FaceLockMoves(new QoL_Mods.Data_Classes.Style("Giant", FightStyleEnum.Giant)));
            nl_styleBox.Items.Add(new FaceLockMoves(new Data_Classes.Style("Devilism", FightStyleEnum.Devilism)));

            nl_styleBox.SelectedIndex = 0;

            //Setting up style moves
            if (styleMoves.Count == 0)
            {
                foreach (FaceLockMoves moves in nl_styleBox.Items)
                {
                    styleMoves.Add(moves);
                }
            }
        }
        private void SetMoveCategories()
        {
            nl_Categories.Items.Clear();
            nl_moveResult.Items.Clear();

            //Basic Moves
            nl_Categories.Items.Add(new MoveCategory("Basic Moves", new HashSet<Skill>
            {
                new Skill("Knee Lift", (int)BasicSkillEnum.PowerCompetitionWin_BodyKneeLift),
                new Skill("Elbow Butt", (int)BasicSkillEnum.PowerCompetitionWin_ElbowBat),
                new Skill("Drop Toe Hold", (int)BasicSkillEnum.PowerCompetitionWin_LegScissors),
                new Skill("Double Wrist Suplex", (int)BasicSkillEnum.PowerCompetitionWin_DoubleWristArmSault),
                new Skill("Headbutt", (int)BasicSkillEnum.PowerCompetitionWin_HeadBat),
                new Skill("Hammer Blow", (int)BasicSkillEnum.PowerCompetitionWin_HammerBlow),
                new Skill("Arm Drag", (int)BasicSkillEnum.PowerCompetitionWin_CycloneWhip),
                new Skill("Oosotogari", (int)BasicSkillEnum.PowerCompetitionWin_Oosotogari),
                new Skill("Leg Tackle", (int)BasicSkillEnum.PowerCompetitionWin_TuckleSingleLeg),
                new Skill("Knuckle Arrow", (int)BasicSkillEnum.PowerCompetitionWin_KnackleArrow),
                new Skill ("Irish Whip (Horizontal)", (int)BasicSkillEnum.HammerThrough_U),
                new Skill ("Irish Whip (Corner)", (int)BasicSkillEnum.HammerThrough_S),
                new Skill ("Irish Whip (Vertical)", (int)BasicSkillEnum.HammerThrough_D)
            }));

            //Custom Moves
            HashSet<Skill> smallSkills = new HashSet<Skill>();
            HashSet<Skill> medSkills = new HashSet<Skill>();
            HashSet<Skill> bigSkills = new HashSet<Skill>();

            foreach (KeyValuePair<SkillID, SkillInfo> current in SkillInfoManager.inst.skillInfoList)
            {
                if (current.Value.sortingOrder[(int)SkillSlotEnum.Grapple_X] > 0)
                {
                    smallSkills.Add(new Skill(DataBase.GetSkillName(current.Key), (Int32)current.Key));
                }
            }

            nl_Categories.Items.Add(new MoveCategory("Small Moves", smallSkills));

            foreach (KeyValuePair<SkillID, SkillInfo> current in SkillInfoManager.inst.skillInfoList)
            {
                if (current.Value.sortingOrder[(int)SkillSlotEnum.Grapple_A] > 0 && current.Value.sortingOrder[(int)SkillSlotEnum.Grapple_X] == 0)
                {
                    medSkills.Add(new Skill(DataBase.GetSkillName(current.Key), (Int32)current.Key));
                }
            }

            nl_Categories.Items.Add(new MoveCategory("Medium Moves", medSkills));

            foreach (KeyValuePair<SkillID, SkillInfo> current in SkillInfoManager.inst.skillInfoList)
            {
                if (current.Value.sortingOrder[(int)SkillSlotEnum.Grapple_B] > 0 && current.Value.sortingOrder[(int)SkillSlotEnum.Grapple_A] == 0 && current.Value.sortingOrder[(int)SkillSlotEnum.Grapple_X] == 0)
                {
                    bigSkills.Add(new Skill(DataBase.GetSkillName(current.Key), (Int32)current.Key));
                }
            }

            nl_Categories.Items.Add(new MoveCategory("Big Moves", bigSkills));

            nl_Categories.SelectedIndex = 0;
        }
        #endregion

        #region Adding Moves
        #region Style Moves
        private void style_addLight_Click(object sender, EventArgs e)
        {
            if (CheckSelectedMove())
            {
                try
                {
                    FaceLockMoves move = (FaceLockMoves)nl_styleBox.SelectedItem;
                    MoveCategory category = ((MoveCategory)nl_Categories.SelectedItem);
                    Skill skill = (Skill)nl_moveResult.SelectedItem;
                    move = UpdateMove(move, skill, category, 0);

                    //Update Style List
                    Data_Classes.Style fightingStyle = ((FaceLockMoves)nl_styleBox.SelectedItem).StyleItem;
                    UpdateStyleList(move, fightingStyle.Name);
                }
                catch (Exception exception)
                {
                    L.D("AddLightStyle Error:" + exception.Message);
                }
            }
        }

        private void style_addMedium_Click(object sender, EventArgs e)
        {
            if (CheckSelectedMove())
            {
                FaceLockMoves move = (FaceLockMoves)nl_styleBox.SelectedItem;
                MoveCategory category = ((MoveCategory)nl_Categories.SelectedItem);
                Skill skill = (Skill)nl_moveResult.SelectedItem;
                move = UpdateMove(move, skill, category, 1);

                //Update Style List
                Data_Classes.Style fightingStyle = ((FaceLockMoves)nl_styleBox.SelectedItem).StyleItem;
                UpdateStyleList(move, fightingStyle.Name);
            }
        }

        private void style_addHeavy_Click(object sender, EventArgs e)
        {
            if (CheckSelectedMove())
            {
                FaceLockMoves move = (FaceLockMoves)nl_styleBox.SelectedItem;
                MoveCategory category = ((MoveCategory)nl_Categories.SelectedItem);
                Skill skill = (Skill)nl_moveResult.SelectedItem;
                move = UpdateMove(move, skill, category, 2);

                //Update Style List
                Data_Classes.Style fightingStyle = ((FaceLockMoves)nl_styleBox.SelectedItem).StyleItem;
                UpdateStyleList(move, fightingStyle.Name);
            }
        }

        private void style_addCritical_Click(object sender, EventArgs e)
        {
            if (CheckSelectedMove())
            {
                FaceLockMoves move = (FaceLockMoves)nl_styleBox.SelectedItem;
                MoveCategory category = ((MoveCategory)nl_Categories.SelectedItem);
                Skill skill = (Skill)nl_moveResult.SelectedItem;
                move = UpdateMove(move, skill, category, 3);

                //Update Style List
                Data_Classes.Style fightingStyle = ((FaceLockMoves)nl_styleBox.SelectedItem).StyleItem;
                UpdateStyleList(move, fightingStyle.Name);
            }
        }
        #endregion

        #region Wrestler Moves
        private void wrestler_addLight_Click(object sender, EventArgs e)
        {
            if (CheckSelectedMove())
            {
                if (nl_wresterList.SelectedIndex < 0)
                {
                    L.D("No Wrestler Selected");
                    return;
                }
                FaceLockMoves move = (FaceLockMoves)nl_wresterList.SelectedItem;
                int index = wrestlerMoves.IndexOf(move);
                MoveCategory category = ((MoveCategory)nl_Categories.SelectedItem);
                Skill skill = (Skill)nl_moveResult.SelectedItem;
                move = UpdateMove(move, skill, category, 0);
                nl_wresterList.SelectedItem = move;
                wrestlerMoves[index] = move;
                nl_wresterList_SelectedIndexChanged(null, null);

            }
        }

        private void wrestler_addMedium_Click(object sender, EventArgs e)
        {
            if (CheckSelectedMove())
            {
                if (nl_wresterList.SelectedIndex < 0)
                {
                    return;
                }
                FaceLockMoves move = (FaceLockMoves)nl_wresterList.SelectedItem;
                int index = wrestlerMoves.IndexOf(move);
                MoveCategory category = ((MoveCategory)nl_Categories.SelectedItem);
                Skill skill = (Skill)nl_moveResult.SelectedItem;
                move = UpdateMove(move, skill, category, 1);
                nl_wresterList.SelectedItem = move;
                wrestlerMoves[index] = move;
                nl_wresterList_SelectedIndexChanged(null, null);


            }
        }

        private void wrestler_addHeavy_Click(object sender, EventArgs e)
        {
            if (CheckSelectedMove())
            {
                if (nl_wresterList.SelectedIndex < 0)
                {
                    return;
                }
                FaceLockMoves move = (FaceLockMoves)nl_wresterList.SelectedItem;
                int index = wrestlerMoves.IndexOf(move);
                MoveCategory category = ((MoveCategory)nl_Categories.SelectedItem);
                Skill skill = (Skill)nl_moveResult.SelectedItem;
                move = UpdateMove(move, skill, category, 2);
                nl_wresterList.SelectedItem = move;
                wrestlerMoves[index] = move;
                nl_wresterList_SelectedIndexChanged(null, null);

            }
        }

        private void wrestler_addCritical_Click(object sender, EventArgs e)
        {
            if (CheckSelectedMove())
            {
                if (nl_wresterList.SelectedIndex < 0)
                {
                    return;
                }
                FaceLockMoves move = (FaceLockMoves)nl_wresterList.SelectedItem;
                int index = wrestlerMoves.IndexOf(move);
                MoveCategory category = ((MoveCategory)nl_Categories.SelectedItem);
                Skill skill = (Skill)nl_moveResult.SelectedItem;
                move = UpdateMove(move, skill, category, 3);
                nl_wresterList.SelectedItem = move;
                wrestlerMoves[index] = move;
                nl_wresterList_SelectedIndexChanged(null, null);

            }
        }


        #endregion

        #endregion

        #region Update Display
        private void nl_Categories_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (nl_Categories.SelectedIndex < 0)
                {
                    return;
                }
                MoveCategory category = (MoveCategory)nl_Categories.SelectedItem;
                nl_moveResult.Items.Clear();

                foreach (Skill skill in category.Skills)
                {
                    nl_moveResult.Items.Add(skill);
                }

                nl_moveResult.SelectedIndex = 0;
            }
            catch (Exception exception)
            {
                L.D("CategoryDisplay Error: " + exception.Message);
            }

        }
        private void nl_refreshMoves_Click(object sender, EventArgs e)
        {
            SetMoveCategories();
        }
        private void nl_styleBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FaceLockMoves moves = (FaceLockMoves)nl_styleBox.SelectedItem;
                if (moves.Type[0] == SkillType.BasicMove || moves.Type[0] == SkillType.IrishWhip)
                {
                    style_lightDmg.Text = moves.BasicSkills[0].SkillName;
                }
                else
                {
                    style_lightDmg.Text = moves.CustomSkills[0].SkillName;
                }

                if (moves.Type[1] == SkillType.BasicMove || moves.Type[1] == SkillType.IrishWhip)
                {
                    style_mediumDmg.Text = moves.BasicSkills[1].SkillName;
                }
                else
                {
                    style_mediumDmg.Text = moves.CustomSkills[1].SkillName;
                }

                if (moves.Type[2] == SkillType.BasicMove || moves.Type[2] == SkillType.IrishWhip)
                {
                    style_heavyDmg.Text = moves.BasicSkills[2].SkillName;
                }
                else
                {
                    style_heavyDmg.Text = moves.CustomSkills[2].SkillName;
                }

                if (moves.Type[3] == SkillType.BasicMove || moves.Type[3] == SkillType.IrishWhip)
                {
                    style_critDmg.Text = moves.BasicSkills[3].SkillName;
                }
                else
                {
                    style_critDmg.Text = moves.CustomSkills[3].SkillName;
                }
            }
            catch (Exception exception)
            {
                L.D("Change Style Error: " + exception.Message);
            }
        }
        private void nl_moveResult_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void nl_wresterList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FaceLockMoves moves = (FaceLockMoves)nl_wresterList.SelectedItem;
                if (moves.Type[0] == SkillType.BasicMove || moves.Type[0] == SkillType.IrishWhip)
                {
                    wrestler_lightDmg.Text = moves.BasicSkills[0].SkillName;
                }
                else
                {
                    wrestler_lightDmg.Text = moves.CustomSkills[0].SkillName;
                }

                if (moves.Type[1] == SkillType.BasicMove || moves.Type[1] == SkillType.IrishWhip)
                {
                    wrestler_mediumDmg.Text = moves.BasicSkills[1].SkillName;
                }
                else
                {
                    wrestler_mediumDmg.Text = moves.CustomSkills[1].SkillName;
                }

                if (moves.Type[2] == SkillType.BasicMove || moves.Type[2] == SkillType.IrishWhip)
                {
                    wrestler_heavyDmg.Text = moves.BasicSkills[2].SkillName;
                }
                else
                {
                    wrestler_heavyDmg.Text = moves.CustomSkills[2].SkillName;
                }

                if (moves.Type[3] == SkillType.BasicMove || moves.Type[3] == SkillType.IrishWhip)
                {
                    wrestler_criticalDmg.Text = moves.BasicSkills[3].SkillName;
                }
                else
                {
                    wrestler_criticalDmg.Text = moves.CustomSkills[3].SkillName;
                }
            }
            catch (Exception exception)
            {
                L.D("Change Wrestler Error: " + exception.Message);
            }
        }

        #endregion

        #region Helper Methods
        private bool CheckSelectedMove()
        {
            try
            {
                if (nl_moveResult.SelectedIndex < 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                L.D("CheckSelected Error: " + e.Message);
                return false;

            }

        }
        private bool CheckSelectedWrestler()
        {
            return false;
        }
        private FaceLockMoves UpdateMove(FaceLockMoves move, Skill skill, MoveCategory category, int damageLevel)
        {
            L.D("Adding Skill - " + skill.SkillName);
            //Ensure the correct category is selected
            for (int i = 0; i < nl_Categories.Items.Count; i++)
            {
                foreach (Skill catSkill in ((MoveCategory)nl_Categories.Items[i]).Skills)
                {
                    if (skill.Equals(catSkill))
                    {
                        category = (MoveCategory)nl_Categories.Items[i];
                        break;
                    }
                }
            }

            try
            {
                if (category.Name.Equals("Basic Moves"))
                {
                    move.BasicSkills[damageLevel] = skill;

                    if (skill.SkillName.Contains("Irish Whip"))
                    {
                        move.Type[damageLevel] = SkillType.IrishWhip;
                        L.D("Added Irish Whip");
                    }
                    else
                    {
                        move.Type[damageLevel] = SkillType.BasicMove;
                        L.D("Added Basic Move");
                    }
                }
                else
                {
                    move.Type[damageLevel] = SkillType.CustomMove;
                    move.CustomSkills[damageLevel] = skill;
                    L.D("Added Custom Move");
                }

            }
            catch (Exception ex)
            {
                L.D("UpdateMove Error: " + ex.Message);
            }

            return move;

        }
        private void UpdateStyleList(FaceLockMoves move, String styleName)
        {
            try
            {
                for (int i = 0; i < styleMoves.Count; i++)
                {
                    if (styleMoves[i].StyleItem.Name.Equals(styleName))
                    {
                        styleMoves[i] = move;
                        nl_styleBox.SelectedItem = move;
                        break;
                    }
                }

                nl_styleBox_SelectedIndexChanged(null, null);
            }
            catch (Exception e)
            {
                L.D("UpdateStyle Error: " + e.Message);
            }

        }
        #endregion

        private void nl_moveSearch_LostFocus(object sender, System.EventArgs e)
        {
            if (nl_moveSearch.Text.Trim().Equals(""))
            {
                return;
            }

            nl_moveResult.Items.Clear();

            foreach (MoveCategory category in nl_Categories.Items)
            {
                foreach (Skill skill in category.Skills)
                {
                    if (skill.SkillName.Contains(nl_moveSearch.Text))
                    {
                        nl_moveResult.Items.Add(skill);
                    }
                }
            }

            if (nl_moveResult.Items.Count > 0)
            {
                nl_moveResult.SelectedIndex = 0;
            }
        }

        private void nl_wrestlerSearch_LostFocus(object sender, System.EventArgs e)
        {
            try
            {
                String query = nl_wrestlerSearch.Text;
                nl_wrestlerResults.Items.Clear();

                if (!query.TrimStart().TrimEnd().Equals(""))
                {
                    foreach (WresIDGroup wrestler in wrestlerList)
                    {
                        if (query.ToLower().Equals(wrestler.Name.ToLower()) || wrestler.Name.ToLower().Contains(query.ToLower()))
                        {
                            nl_wrestlerResults.Items.Add(wrestler.Name);
                        }
                    }
                }

                if (nl_wrestlerResults.Items.Count > 0)
                {
                    nl_wrestlerResults.SelectedIndex = 0;
                    return;
                }
                else
                {
                    foreach (WresIDGroup wrestler in wrestlerList)
                    {
                        nl_wrestlerResults.Items.Add(wrestler.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                L.D("Search Error: " + ex.Message);
            }
        }

        private void nl_addWrestler_Click(object sender, EventArgs e)
        {
            if (nl_wrestlerResults.SelectedIndex < 0)
            {
                return;
            }

            String wrestler = (String)nl_wrestlerResults.SelectedItem;
            FaceLockMoves move = new FaceLockMoves(new QoL_Mods.Data_Classes.Style(wrestler, FightStyleEnum.American));
            nl_wresterList.Items.Add(move);
            wrestlerMoves.Add(move);
            L.D("Added move for " + wrestler);

            if (nl_wresterList.Items.Count == 0)
            {
                L.D("Wrestler List has no items");
                return;
            }
            nl_wresterList.SelectedIndex = 0;
            nl_wresterList_SelectedIndexChanged(null, null);
        }

        private void nl_removeWrestler_Click(object sender, EventArgs e)
        {
            if (nl_wresterList.SelectedIndex < 0)
            {
                return;
            }

            wrestlerMoves.Remove((FaceLockMoves)nl_wresterList.SelectedItem);
            nl_wresterList.Items.RemoveAt(nl_wresterList.SelectedIndex);

            if (nl_wresterList.Items.Count > 0)
            {
                nl_wresterList.SelectedIndex = 0;
                nl_wresterList_SelectedIndexChanged(null, null);
            }
        }
        #endregion
        private String CheckSaveFile(String dataType)
        {
            String path = CheckSaveFolder(dataType);

            switch (dataType)
            {

                case "StyleFL":
                    path = path + saveFileNames[0];
                    break;
                case "WrestlerFL":
                    path = path + saveFileNames[1];
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
    }
}
