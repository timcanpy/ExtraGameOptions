using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QoL_Mods.Data_Classes
{
    public class TOSMoves
    {
        private MoveCategory moves;
        private char moveSeparator = ';';
        private char dataSeparator = ':';
        
        public String Name
        {
            get { return moves.Name; }
        }

        public List<Skill> Skills
        {
            get { return moves.Skills.ToList(); }
        }


        public MoveCategory Moves
        {
            get { return moves; }
        }

        public String SaveMoves()
        {
            String data = moves.Name + dataSeparator;

            foreach (Skill skill in moves.Skills)
            {
                data += skill.ExportSkill() + moveSeparator;
            }

            return data;
        }

        public void LoadMoves(String data)
        {
            var dataSplit = data.Split(dataSeparator);
            moves.Name = dataSplit[0];

            foreach (String skillData in dataSplit[1].Split(moveSeparator))
            {
                if (skillData.Trim().Equals(String.Empty))
                {
                    continue;
                }

                moves.Skills.Add(new Skill(skillData));
            }
        }

        public TOSMoves(String name)
        {
            moves = new MoveCategory(name, new HashSet<Skill>());
        }

        public void SetName(String name)
        {
            if (!name.Equals(String.Empty))
            {
                moves.Name = name;
            }
        }

        public void AddMove(Skill skill)
        {
            moves.Skills.Add(skill);
        }

        public void RemoveMove(Skill skill)
        {
            moves.Skills.Remove(skill);
        }

        public override string ToString()
        {
            return this.moves.Name;
        }

    }
}
