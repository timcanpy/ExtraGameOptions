using DG;
using QoL_Mods.Data_Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QoL_Mods.Private
{
    public partial class TOSForm : Form
    {
        #region Variables
        private static List<Skill> moves = new List<Skill>();
        private static String saveFolderName = "./EGOData/";
        public static TOSForm form = null;
        #endregion
        public TOSForm()
        {
            InitializeComponent();
            form = this;
            tos_moveSearch.LostFocus += tos_moveSearch_LostFocus;
        }

        #region Move Search
        private void tos_moveRefresh_Click(object sender, EventArgs e)
        {
            LoadMoves();
        }

        private void tos_moveReload_Click(object sender, EventArgs e)
        {

        }

        private void tos_moveSearch_LostFocus(object sender, System.EventArgs e)
        {
            if (tos_moveSearch.Text.Trim().Equals(String.Empty))
            {
                return;
            }

            tos_moveResults.Items.Clear();
            foreach (Skill move in moves)
            {
                if (move.SkillName.ToLower().Contains(tos_moveSearch.Text.ToLower()))
                {
                    tos_moveResults.Items.Add(move);
                }
            }

            if (tos_moveResults.Items.Count > 0)
            {
                tos_moveResults.SelectedIndex = 0;
            }
        }
        #endregion

        #region Style Management
        #endregion
        private void tos_styles_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tos_addStyleMove_Click(object sender, EventArgs e)
        {

        }

        private void tos_removeStyleMove_Click(object sender, EventArgs e)
        {

        }

        private void tos_addWrestler_Click(object sender, EventArgs e)
        {

        }

        private void tos_removeWrestler_Click(object sender, EventArgs e)
        {

        }

        private void tos_wrestlers_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tos_addWrestlerMove_Click(object sender, EventArgs e)
        {

        }

        private void tos_removeWrestlerMove_Click(object sender, EventArgs e)
        {

        }

        #region Helper Methods
        private void LoadMoves()
        {
            try
            {
                moves.Clear();

                foreach (KeyValuePair<SkillID, SkillInfo> current in SkillInfoManager.inst.skillInfoList)
                {
                    //Grapples
                    if (current.Value.anmBankType.ToString().Contains("Grapple_L") ||
                        current.Value.anmBankType.ToString().Contains("Grapple_M") ||
                        current.Value.anmBankType.ToString().Contains("Grapple_H"))
                    {
                        moves.Add(new Skill(DataBase.GetSkillName(current.Key), (int)current.Key));
                    }
                }

                tos_moveResults.Items.Clear();
                foreach (Skill move in moves)
                {
                    tos_moveResults.Items.Add(move);
                }


                tos_moveResults.SelectedIndex = 0;
            }
            catch (Exception e)
            {
                L.D("LoadMovesError: " + e);
            }
        }
        #endregion
    }
}
