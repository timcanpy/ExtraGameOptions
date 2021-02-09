using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirePro_API.Models
{
    public class Title
    {
        #region Properties
        public string ID { get; set; }
        public string OwnerID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string CurrentChamp { get; set; }
        public int CurrentDefenses { get; set; }
        public List<string> PreviousChamps { get; set; }

        #endregion
    }
}
