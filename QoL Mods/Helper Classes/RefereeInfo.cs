using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QoL_Mods.Helper_Classes
{
    public class RefereeInfo
    {
        private RefereeData data;

        public RefereeData Data
        {
            get { return data; }
            set { data = value; }
        }

        public RefereeInfo(RefereeData refData)
        {
            data = refData;
        }

        public override String ToString()
        {
            return data.Prm.name;
        }
    }
}
