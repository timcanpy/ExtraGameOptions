using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirePro_API.Models
{
    public class SRData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Promotion { get; set; }
        public string OwnerId { get; set; }
        public String Date { get; set; }
        public string Record { get; set; }
        public int MaxWinStreak { get; set; }
        public double MaxRating { get; set; }
        public List<string> Details { get; set; }
    }
}
