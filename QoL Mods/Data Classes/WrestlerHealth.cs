using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QoL_Mods.Data_Classes
{
    public class WrestlerHealth
    {
        #region Properties
        private String name;

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        private String neckHealth;

        public String NeckHealth
        {
            get { return neckHealth; }
            set { neckHealth = value; }
        }

        private String bodyHealth;

        public String BodyHealth
        {
            get { return bodyHealth; }
            set { bodyHealth = value; }
        }

        private String armHealth;

        public String ArmHealth
        {
            get { return armHealth; }
            set { armHealth = value; }
        }

        private String legHealth;

        public String LegHealth
        {
            get { return legHealth; }
            set { legHealth = value; }
        }

        private float recoveryRate;

        public float RecoveryRate
        {
            get { return recoveryRate; }
            set { recoveryRate = value; }
        }

        private int matchCount;

        public int MatchCount
        {
            get { return matchCount; }
            set { matchCount = value; }
        }
        #endregion

        public WrestlerHealth(String wrestlerName, String neckHP, String bodyHP, String armHP, String legHP, float recovery, int matches)
        {
            name = wrestlerName;
            neckHealth = neckHP;
            bodyHealth = bodyHP;
            armHealth = armHP;
            legHealth = legHP;
            RecoveryRate = recovery;
            matchCount = matches;
        }




    }
}
