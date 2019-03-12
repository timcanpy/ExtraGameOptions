using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QoL_Mods.Data_Classes.Facelock
{
    public class SlotStorage
    {
        public SkillID weakSlot { get; set; }
        public SkillID mediumSlot { get; set; }
        public SkillID heavySlot { get; set; }
        public SkillID criticalSlot { get; set; }
    }
}
