using System;
using static Common_Classes.EnumLibrary;

namespace QoL_Mods.Data_Classes.Reversal
{
    public class Move
    {
        protected char fieldDelimeter = ';';
        protected char lineDelimeter = '|';

        protected String name;

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        protected int id;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        protected MoveType type;

        public MoveType Type
        {
            get { return type; }
            set { type = value; }
        }

        private Executor user;

        public Executor User
        {
            get { return user; }
            set { user = value; }
        }


        public override string ToString()
        {
            return Name;
        }

        public string GetMoveData()
        {
            return name + fieldDelimeter + id + fieldDelimeter + type.ToString() + fieldDelimeter + user.ToString();
        }

        public void UpdateName()
        {
            string skillName = DataBase.GetSkillName((SkillID) id);
            if (!skillName.Trim().Equals(String.Empty))
            {
                name = skillName;
            }
        }
    }
}
