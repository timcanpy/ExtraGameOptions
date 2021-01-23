using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG;

namespace QoL_Mods.Data_Classes
{
    public class WakeUpTaunt
    {
        #region Variables
        public Style StyleItem { get; set; }
        public Skill[] WakeupMoves { get; set; }
        public TauntStartPosition[] StartPositions { get; set; }
        public TauntEndPosition[] EndPositions { get; set; }

        private readonly String nullValue = "--null--";
        #endregion

        public WakeUpTaunt(Style item)
        {
            StyleItem = item;
            WakeupMoves = new Skill[4];
            StartPositions = new TauntStartPosition[4];
            EndPositions = new TauntEndPosition[4];

            for (int i = 0; i < 4; i++)
            {
                WakeupMoves[i] = null;
                StartPositions[i] = TauntStartPosition.FaceDown;
                EndPositions[i] = TauntEndPosition.Standing;
            }
        }

        public void AddWakeUpMove(Skill skill, DamageState damageState)
        {
            int index = (int)damageState;

            WakeupMoves[index] = skill;
            StartPositions[index] = GetStartPosition((SkillID)skill.SkillID);
            EndPositions[index] = GetEndPosition((SkillID)skill.SkillID);
        }

        public void RemoveWakeUpMove(DamageState damageState)
        {
            int index = (int)damageState;

            WakeupMoves[index] = null;
            StartPositions[index] = TauntStartPosition.FaceDown;
            EndPositions[index] = TauntEndPosition.Standing;
        }

        public String SaveWakeUpData()
        {
            String data = "";
            data += StyleItem.Name + ";" + StyleItem.StyleType + "|";
            foreach (Skill skill in WakeupMoves)
            {
                if (skill == null)
                {
                    data += nullValue + ";|";
                }
                else
                {
                    data += skill.SkillName + ";" + skill.SkillID + "|";
                }
            }

            foreach (TauntStartPosition position in StartPositions)
            {
                data += position + "|";
            }

            foreach (TauntEndPosition position in EndPositions)
            {
                data += position + "|";
            }
            return data;
        }

        public string LoadWakeUpData(String data, HashSet<Skill> validSkills = null)
        {
            String[] information = data.Split('|');
            StyleItem.Name = information[0].Split(';')[0];
            StyleItem.StyleType = (FightStyleEnum)Enum.Parse(typeof(FightStyleEnum), information[0].Split(';')[1]);
            for (int i = 0; i < 4; i++)
            {
                if (information[i + 1].Split(';')[0].Equals(nullValue))
                {
                    WakeupMoves[i] = null;
                }
                else
                {
                    WakeupMoves[i] = new Skill("", 0)
                    {
                        SkillName = information[i + 1].Split(';')[0],
                        SkillID = int.Parse(information[i + 1].Split(';')[1])
                    };

                    //Verify that the skillID stored matches the Move Data entry
                    //If it does not, attempt to find a valid replacement
                    if (validSkills != null)
                    {
                        if (!IsValidTaunt(WakeupMoves[i], validSkills))
                        {
                            L.D(WakeupMoves[i].SkillName + " has an invalid ID " + WakeupMoves[i].SkillID + ". Attempting to replace with a valid ID.");
                            foreach (var skill in validSkills)
                            {
                                if (skill.SkillName == WakeupMoves[i].SkillName)
                                {
                                    L.D("Replacing " + WakeupMoves[i].SkillName + " ID from " + WakeupMoves[i].SkillID + " to " + skill.SkillID);
                                    WakeupMoves[i].SkillID = skill.SkillID;
                                    break;
                                }
                            }
                        }
                    }

                }
                StartPositions[i] = (TauntStartPosition)Enum.Parse(typeof(TauntStartPosition), information[i + 5]);
                EndPositions[i] = (TauntEndPosition)Enum.Parse(typeof(TauntEndPosition), information[i + 9]);
            }

            return StyleItem.Name;
        }

        #region Helper Methods
        public Skill LocateSkill(Skill skill, DamageState damageState)
        {
            if (WakeupMoves[(int)damageState] == null)
            {
                return null;
            }

            if (WakeupMoves[(int)damageState].Equals(skill))
            {
                return skill;
            }
            else
            {
                return null;
            }
        }

        public Skill GetSkill(DamageState damageState)
        {
            try
            {
                return WakeupMoves[(int)damageState];
            }
            catch(ArgumentOutOfRangeException)
            {
                return null;
            }
        }

        public bool IsGroupEmpty()
        {
            foreach (Skill skill in WakeupMoves)
            {
                if (skill != null)
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsValidTaunt(Skill skill, HashSet<Skill> validSkills)
        {
            bool isValid = false;

            //Ignore the value of Rolling, which is a default action
            if (skill.SkillID == -100)
            {
                return true;
            }

            foreach (var item in validSkills)
            {
                if (item.SkillID == skill.SkillID)
                {
                    isValid = true;
                    break;
                }
            }

            return isValid;
        }

        private TauntStartPosition GetStartPosition(SkillID skillID)
        {
            if (skillID < 0)
            {
                return TauntStartPosition.FaceDown;
            }
            var anmData = SkillDataMan.inst.GetSkillData(skillID)[0].anmData[0];
            if (anmData.formDispList[0].formIdx == 101)
            {
                return TauntStartPosition.FaceDown;
            }
            else
            {
                return TauntStartPosition.FaceUp;
            }
        }

        private TauntEndPosition GetEndPosition(SkillID skillID)
        {
            if (skillID < 0)
            {
                return TauntEndPosition.Grounded;
            }
            var anmData = SkillDataMan.inst.GetSkillData(skillID)[0].anmData[0];
            if (anmData.formDispList[anmData.formNum - 1].formIdx != 101 && anmData.formDispList[anmData.formNum - 1].formIdx != 100)
            {
                return TauntEndPosition.Standing;
            }
            else
            {
                return TauntEndPosition.Grounded;
            }
        }
        #endregion

    }
}
