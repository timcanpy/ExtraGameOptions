using DG;
using QoL_Mods.Data_Classes.Reversal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static Common_Classes.EnumLibrary;

namespace QoL_Mods.Private
{
    public partial class CustomReversalForm : Form
    {
        public CustomReversalForm()
        {
            reversalForm = this;
            InitializeComponent();
            FormClosing += ReversalFormClosing;
            rev_moveSearch.LostFocus += rev_moveSearch_LostFocus;
            try
            {
                LoadReversalMoves();
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                throw;
            }
        }

        #region Variables
        private static String[] moveTypes = Enum.GetNames(typeof(MoveType));
        private static String[] executorTypes = Enum.GetNames(typeof(Executor));
        private static List<Move> moves = new List<Move>();
        public static List<ReversalMove> fgrappleMoves = new List<ReversalMove>();
        public static List<ReversalMove> cornerMoves = new List<ReversalMove>();
        private static String saveFileName = "Reversal_FGrapple.dat";
        private static String saveFolderName = "./EGOData/";
        public static CustomReversalForm reversalForm = null;
        #endregion

        #region Data Load
        private void LoadReversalMoves()
        {
            LoadExecutors();
            LoadTypes();
            LoadMoves();
            LoadReversalData();
        }
        private void LoadMoves()
        {
            try
            {
                moves.Clear();

                foreach (KeyValuePair<SkillID, SkillInfo> current in SkillInfoManager.inst.skillInfoList)
                {
                    //Grapples
                    if ((String)rev_moveType.SelectedItem == MoveType.FGrapple.ToString())
                    {
                        if (current.Value.anmBankType.ToString().Contains("Grapple_L") ||
                            current.Value.anmBankType.ToString().Contains("Grapple_M") ||
                            current.Value.anmBankType.ToString().Contains("Grapple_H"))
                        {
                            moves.Add(
                                new Move
                                {
                                    ID = (int)current.Key,
                                    Name = DataBase.GetSkillName(current.Key),
                                    Type = MoveType.FGrapple
                                });
                        }
                    }
                    //Corner Grapples
                    else if ((String)rev_moveType.SelectedItem == MoveType.Corner.ToString())
                    {
                        if (current.Value.anmBankType == AnmBankType.CornerGrapple ||
                            current.Value.anmBankType == AnmBankType.CornerGrapple_Move ||
                            current.Value.anmBankType == AnmBankType.CornerGrapple_Switch)
                        {
                            moves.Add(
                                new Move
                                {
                                    ID = (int)current.Key,
                                    Name = DataBase.GetSkillName(current.Key),
                                    Type = MoveType.Corner
                                });
                        }
                    }
                }

                rev_searchResults.Items.Clear();
                foreach (Move move in moves)
                {
                    rev_searchResults.Items.Add(move);
                }

                rev_searchResults.SelectedIndex = 0;
            }
            catch (Exception e)
            {
                L.D("LoadMovesError: " + e);
            }
        }
        private void LoadTypes()
        {
            foreach (String type in moveTypes)
            {
                rev_moveType.Items.Add(type);
            }

            rev_moveType.SelectedIndex = 0;

        }
        private void LoadExecutors()
        {
            foreach (String executor in executorTypes)
            {
                rev_executor.Items.Add(executor);
            }

            rev_executor.SelectedIndex = 0;
        }
        private void LoadReversalData()
        {
            try
            {
                String filePath = Path.Combine(saveFolderName, saveFileName);
                if (File.Exists(filePath))
                {
                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        var lines = File.ReadAllLines(filePath);
                        foreach (String line in lines)
                        {
                            ReversalMove move = new ReversalMove();
                            move.LoadReversalData(line);
                            fgrappleMoves.Add(move);
                        }
                    }
                }

                if (fgrappleMoves.Count > 0)
                {
                    rev_moveType_SelectedIndexChanged(null, null);
                }
            }
            catch (Exception e)
            {
                L.D("LoadReversalException: " + e);
            }

        }
        #endregion

        #region Data Save
        private void ReversalFormClosing(object sender, FormClosingEventArgs e)
        {
            SaveReversalData();
        }
        private void rev_save_Click(object sender, EventArgs e)
        {
            SaveReversalData();
        }
        private void SaveReversalData()
        {
            try
            {
                //Save Front Grapple Reversals
                String filePath = Path.Combine(saveFolderName, saveFileName);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                using (StreamWriter sw = File.AppendText(filePath))
                {
                    foreach (ReversalMove move in fgrappleMoves)
                    {
                        sw.WriteLine(move.SaveReversalData());
                    }
                }
            }
            catch (Exception e)
            {
                L.D("SaveReversalException: " + e);
            }

        }
        #endregion

        #region Move Search
        private void rev_moveSearch_LostFocus(object sender, System.EventArgs e)
        {
            if (rev_moveSearch.Text.Trim().Equals(String.Empty))
            {
                return;
            }

            rev_searchResults.Items.Clear();
            foreach (Move move in moves)
            {
                if (move.Name.ToLower().Contains(rev_moveSearch.Text.ToLower()))
                {
                    rev_searchResults.Items.Add(move);
                }
            }

            if (rev_searchResults.Items.Count > 0)
            {
                rev_searchResults.SelectedIndex = 0;
            }
        }

        private void rev_searchResults_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void rev_moveRefresh_Click(object sender, EventArgs e)
        {
            LoadMoves();
        }

        #endregion

        #region Move List Actions
        private void rev_reversalAdd_Click(object sender, EventArgs e)
        {
            if (rev_searchResults.SelectedIndex < 0)
            {
                return;
            }

            try
            {
                Move move = (Move)rev_searchResults.SelectedItem;
                String type = (String)rev_moveType.SelectedItem;
                if (type == MoveType.FGrapple.ToString())
                {
                    //Determine if move already exists
                    if (!CheckMoveUsed(MoveType.FGrapple, move.Name))
                    {
                        ReversalMove reversal = new ReversalMove
                        {
                            ID = move.ID,
                            Name = move.Name,
                            Type = move.Type,
                            ReplacementsList = new List<Move>(),
                        };
                        fgrappleMoves.Add(reversal);

                        //Update control
                        rev_reversalList.Items.Add(reversal);
                    }

                }
                else if (type == MoveType.Corner.ToString())
                {
                    //Determine if move already exists
                    if (!CheckMoveUsed(MoveType.Corner, move.Name))
                    {
                        ReversalMove reversal = new ReversalMove
                        {
                            ID = move.ID,
                            Name = move.Name,
                            Type = move.Type,
                            ReplacementsList = new List<Move>(),
                        };
                        cornerMoves.Add(reversal);

                        //Update control
                        rev_reversalList.Items.Add(reversal);
                    }
                }

                rev_reversalList.SelectedIndex = rev_reversalList.Items.Count - 1;
                rev_reversalList_SelectedIndexChanged(null, null);

            }
            catch (Exception exception)
            {
                L.D("ReversalAddException: " + exception);
            }

        }

        private void rev_reversalRemove_Click(object sender, EventArgs e)
        {
            if (rev_reversalList.SelectedIndex < 0)
            {
                return;
            }

            try
            {
                String type = (String)rev_moveType.SelectedItem;
                ReversalMove reversal = (ReversalMove)rev_reversalList.SelectedItem;

                int index;
                if (type == MoveType.FGrapple.ToString())
                {
                    index = FindMoveIndex(reversal.Name, fgrappleMoves);
                    if (index >= 0)
                    {
                        fgrappleMoves.RemoveAt(index);
                    }

                }
                else if (type == MoveType.Corner.ToString())
                {
                    index = FindMoveIndex(reversal.Name, fgrappleMoves);
                    if (index >= 0)
                    {
                        cornerMoves.RemoveAt(index);
                    }
                }

                rev_reversalList.Items.RemoveAt(rev_reversalList.SelectedIndex);

                if (rev_reversalList.Items.Count > 0)
                {
                    rev_reversalList.SelectedIndex = rev_reversalList.Items.Count - 1;
                    rev_reversalList_SelectedIndexChanged(null, null);
                }

            }
            catch (Exception exception)
            {
                L.D("ReversalRemoveException: " + exception);
            }
        }

        private void rev_reversalList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (rev_reversalList.SelectedIndex < 0)
                {
                    return;
                }

                ReversalMove reversal = (ReversalMove)rev_reversalList.SelectedItem;

                rev_replacementList.Items.Clear();

                //Populate Replacement List
                foreach (Move move in reversal.ReplacementsList)
                {
                    rev_replacementList.Items.Add(move);
                }

                if (rev_replacementList.Items.Count > 0)
                {
                    rev_replacementList.SelectedIndex = 0;
                    rev_replacementList_SelectedIndexChanged(null, null);
                }

            }
            catch (Exception exception)
            {
                L.D("ReversalListIndexChangedError: " + exception);
            }
        }

        private void rev_moveType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMoves();

            String type = (String)rev_moveType.SelectedItem;
            rev_reversalList.Items.Clear();
            rev_replacementList.Items.Clear();

            if (type == MoveType.FGrapple.ToString())
            {
                foreach (var move in fgrappleMoves)
                {
                    rev_reversalList.Items.Add(move);
                }
            }
            else if (type == MoveType.Corner.ToString())
            {
                foreach (var move in cornerMoves)
                {
                    rev_reversalList.Items.Add(move);
                }
            }

            if (rev_reversalList.Items.Count > 0)
            {
                rev_reversalList.SelectedIndex = 0;
                rev_reversalList_SelectedIndexChanged(null, null);
            }
        }

        #endregion

        #region Replacement List Actions
        private void rev_replacementList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rev_replacementList.SelectedIndex < 0)
            {
                return;
            }

            try
            {
                Move move = (Move)rev_replacementList.SelectedItem;
                rev_replacementType.Text = move.Type.ToString();
                rev_executor.SelectedIndex = (int)move.User;
            }
            catch (Exception exception)
            {
                L.D("RevReplacementError: " + exception);
            }
        }

        private void rev_addReplacement_Click(object sender, EventArgs e)
        {
            if (rev_reversalList.SelectedIndex < 0 || rev_searchResults.SelectedIndex < 0)
            {
                return;
            }

            try
            {
                ReversalMove reversal = (ReversalMove)rev_reversalList.SelectedItem;
                Move move = (Move)rev_searchResults.SelectedItem;

                //Only add moves if they're the same type as the parent
                if (reversal.Type != move.Type)
                {
                    return;
                }

                if (!CheckReversalUsed(move.Name, reversal.ReplacementsList))
                {
                    int index;
                    if (reversal.Type == MoveType.Corner)
                    {
                        index = FindMoveIndex(reversal.Name, cornerMoves);
                        if (index == -1)
                        {
                            return;
                        }

                        cornerMoves[index].AddReplacement(move);
                    }
                    else if (reversal.Type == MoveType.FGrapple)
                    {
                        index = FindMoveIndex(reversal.Name, fgrappleMoves);
                        if (index == -1)
                        {
                            return;
                        }

                        fgrappleMoves[index].AddReplacement(move);
                    }
                }

                rev_reversalList_SelectedIndexChanged(null, null);
            }
            catch (Exception exception)
            {
                L.D("ReplacementAddException: " + exception);
            }
        }

        private void rev_removeReplacement_Click(object sender, EventArgs e)
        {
            if (rev_replacementList.SelectedIndex < 0 || rev_reversalList.SelectedIndex < 0)
            {
                return;
            }

            try
            {
                ReversalMove reversal = (ReversalMove)rev_reversalList.SelectedItem;
                Move replacement = (Move)rev_replacementList.SelectedItem;
                List<Move> moves = reversal.ReplacementsList;
                for (int i = 0; i < moves.Count; i++)
                {
                    if (replacement.Name.Equals(moves[i].Name))
                    {
                        moves.RemoveAt(i);
                        break;
                    }
                }

                //Determine which list to update
                int index;
                if (reversal.Type == MoveType.Corner)
                {
                    index = FindMoveIndex(reversal.Name, cornerMoves);
                    if (index >= 0)
                    {
                        cornerMoves[index].ReplacementsList = moves;
                    }
                }
                else if (reversal.Type == MoveType.FGrapple)
                {
                    index = FindMoveIndex(reversal.Name, fgrappleMoves);
                    if (index >= 0)
                    {
                        fgrappleMoves[index].ReplacementsList = moves;
                    }
                }

                rev_replacementList.Items.RemoveAt(rev_replacementList.SelectedIndex);

                if (rev_replacementList.Items.Count > 0)
                {
                    rev_replacementList.SelectedIndex = rev_replacementList.Items.Count - 1;
                    rev_replacementList_SelectedIndexChanged(null, null);
                }
            }
            catch (Exception exception)
            {
                L.D("RemoveReplacementError: " + exception);
            }
        }

        private void rev_executor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rev_reversalList.SelectedIndex < 0 || rev_replacementList.SelectedIndex < 0)
            {
                return;
            }

            try
            {
                //Save current replacement data
                int index;
                Move move = (Move)rev_replacementList.SelectedItem;
                ReversalMove reversal = (ReversalMove)rev_reversalList.SelectedItem;
                String type = (String)rev_executor.SelectedItem;
                move.User = (Executor)Enum.Parse(typeof(Executor), type);

                if (reversal.Type == MoveType.Corner)
                {
                    index = FindMoveIndex(reversal.Name, cornerMoves);
                    if (index < 0)
                    {
                        return;
                    }

                    cornerMoves[index].ReplacementsList[rev_replacementList.SelectedIndex] = move;
                }
                else if (reversal.Type == MoveType.FGrapple)
                {
                    index = FindMoveIndex(reversal.Name, fgrappleMoves);
                    if (index < 0)
                    {
                        return;
                    }

                    fgrappleMoves[index].ReplacementsList[rev_replacementList.SelectedIndex] = move;
                }

                rev_replacementList_SelectedIndexChanged(null, null);
            }
            catch (Exception exception)
            {
                L.D("ExecutorChangedError: " + exception);
            }
        }
        #endregion

        #region Helper Methods
        private bool CheckMoveUsed(MoveType type, String name)
        {
            bool isUsed = false;

            if (type == MoveType.Corner)
            {
                foreach (ReversalMove move in cornerMoves)
                {
                    if (move.Name.Equals(name))
                    {
                        isUsed = true;
                        break;
                    }
                }
            }
            else if (type == MoveType.FGrapple)
            {
                foreach (ReversalMove move in fgrappleMoves)
                {
                    if (move.Name.Equals(name))
                    {
                        isUsed = true;
                        break;
                    }
                }
            }

            return isUsed;
        }
        private bool CheckReversalUsed(String name, List<Move> moves)
        {
            bool isUsed = false;
            foreach (Move move in moves)
            {
                if (name.Equals(move.Name))
                {
                    isUsed = true;
                    break;
                }
            }

            return isUsed;
        }
        public static int FindMoveIndex(String name, List<ReversalMove> moves)
        {
            int index = -1;

            for (int i = 0; i < moves.Count; i++)
            {
                if (name.Equals(moves[i].Name))
                {
                    index = i;
                    break;
                }
            }

            return index;
        }
        #endregion

    }
}
