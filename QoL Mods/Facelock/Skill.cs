using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QoL_Mods.Data_Classes
{
    public class Skill
    {
        public String SkillName { get; set; }
        public int SkillID { get; set; }

        public Skill(String name, int id)
        {
            SkillName = name;
            SkillID = id;
        }

        public override string ToString()
        {
            return this.SkillName;
        }

    }
}
