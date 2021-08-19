using DG;
using MatchConfig;
using System;
using System.Windows.Forms;

namespace QoL_Mods.Private
{
    public partial class EasyResizeForm : Form
    {
        public static EasyResizeForm eeForm = null;
        enum Operation { Addition = 0, Replacement = 1, Reduction = 2 };
        public EasyResizeForm()
        {
            InitializeComponent();
            LoadWrestlers();
        }

        private void LoadWrestlers()
        {
            try
            {
                wrestlerList.Items.Clear();

                foreach (EditWrestlerData current in SaveData.inst.editWrestlerData)
                {
                    WresIDGroup wresIDGroup = new WresIDGroup();
                    wresIDGroup.Name = DataBase.GetWrestlerFullName(current.wrestlerParam);
                    wresIDGroup.ID = (Int32)current.editWrestlerID;
                    wresIDGroup.Group = current.wrestlerParam.groupID;
                    this.wrestlerList.Items.Add(wresIDGroup);
                }

                this.wrestlerList.SelectedIndex = 0;

            }
            catch (Exception e)
            {
                L.D("LoadWrestlersException: " + e);
            }

        }

        private void updateBy_Current_Click(object sender, EventArgs e)
        {
            WresIDGroup wrestler = (WresIDGroup)wrestlerList.SelectedItem;
            UpdateWrestlerPartsByValue(wrestler);
        }

        private void UpdateTo_Current_Click(object sender, EventArgs e)
        {
            WresIDGroup wrestler = (WresIDGroup)wrestlerList.SelectedItem;
            UpdateWrestlerPartsToValue(wrestler);
        }

        private void UpdateBy_All_Click(object sender, EventArgs e)
        {
            foreach (WresIDGroup wrestler in wrestlerList.Items)
            {
                UpdateWrestlerPartsByValue(wrestler);
            }
        }

        private void UpdateTo_All_Click(object sender, EventArgs e)
        {
            foreach (WresIDGroup wrestler in wrestlerList.Items)
            {
                UpdateWrestlerPartsToValue(wrestler);
            }
        }
        private void ReduceBy_Current_Click(object sender, EventArgs e)
        {
            WresIDGroup wrestler = (WresIDGroup)wrestlerList.SelectedItem;
            ReduceWrestlerPartsByValue(wrestler);
        }

        private void ReduceBy_All_Click(object sender, EventArgs e)
        {
            foreach (WresIDGroup wrestler in wrestlerList.Items)
            {
                ReduceWrestlerPartsByValue(wrestler);
            }
        }

        private void UpdateWrestlerPartsByValue(WresIDGroup wrestler)
        {
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    var costumeData = DataBase.GetCostumeData((WrestlerID)wrestler.ID, i);
                    if (costumeData != null)
                    {
                        costumeData = UpdateCostume(Operation.Addition, costumeData);
                    }
                }
                catch (Exception ex)
                {
                    L.D("Error with costume # " + i + ": " + ex);
                }
            }
        }

        private void UpdateWrestlerPartsToValue(WresIDGroup wrestler)
        {
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    var costumeData = DataBase.GetCostumeData((WrestlerID)wrestler.ID, i);
                    if (costumeData != null)
                    {
                        costumeData = UpdateCostume(Operation.Replacement, costumeData);
                    }
                }
                catch (Exception ex)
                {
                    L.D("Error with costume # " + i + ": " + ex);
                }
            }
        }

        private void ReduceWrestlerPartsByValue(WresIDGroup wrestler)
        {
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    var costumeData = DataBase.GetCostumeData((WrestlerID)wrestler.ID, i);
                    if (costumeData != null)
                    {
                        L.D("Costume # " + i);
                        costumeData = UpdateCostume(Operation.Reduction, costumeData);
                    }
                }
                catch (Exception ex)
                {
                    L.D("Error with costume # " + i + ": " + ex);
                }
            }
        }

        private CostumeData UpdateCostume(Operation operand, CostumeData originalCostume)
        {
            if (headCB.Checked)
            {
                float headScale = originalCostume.partsScale[(int)PartsTexEnum.Face];
          
                float newScale = UpdateScale(operand, headScale, (float)headUD.Value / 100);
                originalCostume.partsScale[(int)PartsTexEnum.Face] = newScale;
            }
            if (chestCB.Checked)
            {
                float chestScale = originalCostume.partsScale[(int)PartsTexEnum.Chest];
               
                float newScale = UpdateScale(operand, chestScale, (float)chestUD.Value / 100);
                originalCostume.partsScale[(int)PartsTexEnum.Chest] = newScale;
            }
            if (uarmCB.Checked)
            {
                float uArmScale = originalCostume.partsScale[(int)PartsTexEnum.UpperArm];
                L.D("Original uArm Scale: " + uArmScale);

                float newScale = UpdateScale(operand, uArmScale, (float)uarmUD.Value / 100);
                L.D("New uArm Scale: " + newScale);
                originalCostume.partsScale[(int)PartsTexEnum.UpperArm] = newScale;
            }
            if (larmCB.Checked)
            {
                float lArmScale = originalCostume.partsScale[(int)PartsTexEnum.ForeArm];
                L.D("Original lArm Scale: " + lArmScale);

                float newScale = UpdateScale(operand, lArmScale, (float)larmUD.Value / 100);
                L.D("New lArm Scale: " + newScale);
                originalCostume.partsScale[(int)PartsTexEnum.ForeArm] = newScale;
            }
            if (handsCB.Checked)
            {
                float handsScale = originalCostume.partsScale[(int)PartsTexEnum.Hand];
                L.D("Original hands Scale: " + handsScale);

                float newScale = UpdateScale(operand, handsScale, (float)handsUD.Value / 100);
                L.D("New hands Scale: " + newScale);
                originalCostume.partsScale[(int)PartsTexEnum.Hand] = newScale;
            }
            if (waistCB.Checked)
            {
                float waistScale = originalCostume.partsScale[(int)PartsTexEnum.Body];
                L.D("Original waist Scale: " + waistScale);

                float newScale = UpdateScale(operand, waistScale, (float)waistUD.Value / 100);
                L.D("New waist Scale: " + newScale);
                originalCostume.partsScale[(int)PartsTexEnum.Body] = newScale;
            }
            if (thighCB.Checked)
            {
                float thighScale = originalCostume.partsScale[(int)PartsTexEnum.Thigh];
                L.D("Original thigh Scale: " + thighScale);

                float newScale = UpdateScale(operand, thighScale, (float)thighUD.Value / 100);
                L.D("New thigh Scale: " + newScale);
                originalCostume.partsScale[(int)PartsTexEnum.Thigh] = newScale;
            }
            if (calfCB.Checked)
            {
                float calfScale = originalCostume.partsScale[(int)PartsTexEnum.Shin];
              
                float newScale = UpdateScale(operand, calfScale, (float)calfUD.Value / 100);
                originalCostume.partsScale[(int)PartsTexEnum.Shin] = newScale;
            }
            if (feetCB.Checked)
            {
                float footScale = originalCostume.partsScale[(int)PartsTexEnum.Foot];
                L.D("Original foot Scale: " + footScale);

                float newScale = UpdateScale(operand, footScale, (float)feetUD.Value / 100);
                L.D("New foot Scale: " + newScale);
                originalCostume.partsScale[(int)PartsTexEnum.Foot] = newScale;
            }
            return originalCostume;
        }

        private float UpdateScale(Operation operand, float originalValue, float newValue)
        {
            if (operand == Operation.Addition)
            {
                var result = originalValue + newValue;
                if (result > 2)
                {
                    return 2;
                }
                else
                {
                    return result;
                }
            }
            else if (operand == Operation.Replacement)
            {
                return newValue;
            }
            else if (operand == Operation.Reduction)
            {
                var result = originalValue - newValue;
                if (result < 0)
                {
                    return 0;
                }
                else
                {
                    return result;
                }
            }
            //We should never reach this point.
            else
            {
                return originalValue;
            }
        }
        private void wrestlerRefresh_Click(object sender, EventArgs e)
        {
            LoadWrestlers();
        }

    }
}
