using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QoL_Mods.Data_Classes
{
    public class Style
    {
        public String Name { get; set; }
        public FightStyleEnum StyleType { get; set; }

        public Style(String name, FightStyleEnum style)
        {
            Name = name;
            StyleType = style;
        }

        public override string ToString()
        {
            return this.Name;
        }

    }
}
