using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FireProWar.Data_Classes.Storage_Objects
{
    [Serializable]
    class Region
    {
        #region Parameters
        private int id;

        public int ID
        {
            get { return id; }
        }
        private String name;

        public String Name
        {
            get { return name; }
        }
        #endregion
    }
}
