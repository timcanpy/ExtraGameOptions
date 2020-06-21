using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace QoL_Mods.Data_Classes
{
    public class Skill
    {
        public String SkillName { get; set; }
        public int SkillID { get; set; }
        private char dataSeparator = '-';

        public Skill(String name, int id)
        {
            SkillName = name.Replace(dataSeparator, ' ');
            SkillID = id;
        }

        public Skill(String data)
        {
            var dataSplit = data.Split(dataSeparator);
            SkillName = dataSplit[0];
        
            Int32.TryParse(dataSplit[1], out int id);

            if (id == 0)
            {
                SkillID = -1;
            }
            else
            {
                SkillID = id;
            }
        }

        public override string ToString()
        {
            return this.SkillName;
        }

        public String ExportSkill()
        {
            return SkillName + dataSeparator + SkillID;
        }

    }
}
