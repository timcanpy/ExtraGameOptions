using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace FirePro_API.Models
{
    public class Promotion
    {
        #region Properties
        public string ID { get; set; }
        public string OwnerID { get; set; }
        public string Name { get; set; }
        public string Style { get; set; }
        public string Region { get; set; }
        public List<string> History { get; set; }
        public List<string> Details { get; set; }
        public int MatchCount { get; set; }
        public decimal AverageRating { get; set; }
        public int RosterCount { get; set; }
        #endregion
    }
}
