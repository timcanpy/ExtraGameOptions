using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.VisualStyles;
using DG;

namespace QoL_Mods.Data_Classes
{
    public class FaceLockMoves
    {
        public Style StyleItem { get; set; }
        public Skill[] BasicSkills { get; set; }
        public SkillType[] Type { get; set; }
        public Skill[] CustomSkills { get; set; }

        public FaceLockMoves(Style item)
        {
            StyleItem = item;
            BasicSkills = new Skill[4];
            Type = new SkillType[4];
            CustomSkills = new Skill[4];
            
            //Set initial moves
            for (int i = 0; i < Type.Length; i++)
            {
                BasicSkills[i] = new Skill("Elbow Butt", (int)BasicSkillEnum.PowerCompetitionWin_ElbowBat);
                Type[i] = SkillType.BasicMove;
                CustomSkills[i] = new Skill("Elbow Butt", (int)BasicSkillEnum.PowerCompetitionWin_ElbowBat);
            }
       }

        public override string ToString()
        {
            return this.StyleItem.Name;
        }

        public String SaveFaceLockData()
        {
            String data = "";
            data += StyleItem.Name +";" + StyleItem.StyleType + "|";
            foreach (Skill skill in BasicSkills)
            {
                data += skill.SkillName + ";" + skill.SkillID + "|";
            }

            foreach (SkillType type in Type)
            {
                data += type + "|";
            }

            foreach (Skill skill in CustomSkills)
            {
                data += skill.SkillName + ";" + skill.SkillID + "|";
            }
            return data;
        }

        public void LoadFaceLockData(String data)
        {
            String[] information = data.Split('|');
            StyleItem.Name = information[0].Split(';')[0];
            StyleItem.StyleType = (FightStyleEnum)Enum.Parse(typeof(FightStyleEnum), information[0].Split(';')[1]);
            for (int i = 0; i < 4; i++)
            {
                BasicSkills[i].SkillName = information[i + 1].Split(';')[0];
                BasicSkills[i].SkillID = int.Parse(information[i + 1].Split(';')[1]);
                Type[i] = (SkillType)Enum.Parse(typeof(SkillType), information[i + 5]);
                CustomSkills[i].SkillName = information[i + 9].Split(';')[0];
                CustomSkills[i].SkillID = int.Parse(information[i + 9].Split(';')[1]);
            }
        }
    }
}
