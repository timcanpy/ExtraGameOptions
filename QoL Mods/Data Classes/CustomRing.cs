using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QoL_Mods.Data_Classes
{
    class CustomRing
    {

        public CustomRing()
        {
            refereeList = new HashSet<string>();
            themeList = new HashSet<string>();
        }
        private String name;

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        private HashSet<String> refereeList;

        public HashSet<String> RefereeList
        {
            get { return refereeList; }
            set { refereeList = value; }
        }

        private HashSet<String> themeList;

        public HashSet<String> ThemeList
        {
            get { return themeList; }
            set { themeList = value; }
        }
        
        public override string ToString()
        {
            return this.Name;
        }
    }
}
