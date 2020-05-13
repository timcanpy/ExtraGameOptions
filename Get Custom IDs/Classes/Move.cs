using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoveFileCleanup.Classes
{
    public class Move
    {
        private String name;

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        private String[] properties;

        public String[] Properties
        {
            get { return properties; }
            set { properties = value; }
        }


    }
}
