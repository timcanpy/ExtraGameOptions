using DG;
using System;

namespace QoL_Mods.Data_Classes
{
    public class WakeUpTaunt
    {
        public Style StyleItem { get; set; }
        public Skill[] WakeupMoves { get; set; }
        public TauntStartPosition[] StartPositions { get; set; }
        public TauntEndPosition[] EndPositions { get; set; }
        private String nullValue = "--null--";

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

        public void AddWakeUpMove(Skill skill, int index)
        {
                WakeupMoves[index] = skill;
                StartPositions[index] = GetStartPosition((SkillID)skill.SkillID);
                EndPositions[index] = GetEndPosition((SkillID) skill.SkillID);
        }

        public void RemoveWakeUpMove(int index)
        {
            WakeupMoves[index] = null;
            StartPositions[index] = TauntStartPosition.FaceDown;
            EndPositions[index] = TauntEndPosition.Standing;
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

        public void LoadWakeUpData(String data)
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
                }
                StartPositions[i] = (TauntStartPosition)Enum.Parse(typeof(TauntStartPosition), information[i + 5]);
                EndPositions[i] = (TauntEndPosition)Enum.Parse(typeof(TauntEndPosition), information[i + 9]);
            }
        }

        public override string ToString()
        {
            return this.StyleItem.Name;
        }
        
    }
}