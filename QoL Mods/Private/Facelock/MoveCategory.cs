using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QoL_Mods.Data_Classes
{
    public class MoveCategory
    {
        public String Name { get; set; }
        public HashSet<Skill> Skills { get; set; }

        public void AddSkill(Skill skill)
        {
            Skills.Add(skill);
        }

        public MoveCategory(String name, HashSet<Skill> skills)
        {
            Name = name;
            Skills = skills;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
