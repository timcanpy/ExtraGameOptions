using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace Data_Classes
{
    public class Employee
    {
        public Employee()
        {
            wins = 0;
            losses = 0;
            draws = 0;
            matchCount = 0;
            averageRating = 0;
        }

        #region Properties
        private String name;

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        private String type;

        public String Type
        {
            get { return type; }
            set { type = value; }
        }

        private String region;

        public String Region
        {
            get { return region; }
            set { region = value; }
        }

        private int matchCount;

        public int MatchCount
        {
            get { return matchCount; }
            set { matchCount = value; }
        }

        private float averageRating;

        public float AverageRating
        {
            get { return averageRating; }
            set { averageRating = value; }
        }

        private int wins;

        public int Wins
        {
            get { return wins; }
            set { wins = value; }
        }

        private int losses;

        public int Losses
        {
            get { return losses; }
            set { losses = value; }
        }

        private int draws;

        public int Draws
        {
            get { return draws; }
            set { draws = value; }
        }
        
        private int moraleRank;

        public int MoraleRank
        {
            get { return moraleRank; }
            set { moraleRank = value; }
        }

        private int moralePoints;

        public int MoralePoints
        {
            get { return moralePoints; }
            set { moralePoints = value; }
        }

        private int failedQuitRolls;

        public int FailedQuitRolls
        {
            get { return failedQuitRolls; }
            set { failedQuitRolls = value; }
        }

        private int quitRollCeiling;
        #endregion

        #region  Methods
        public int QuitRollCeiling 
        {
            get { return quitRollCeiling; }
            set { quitRollCeiling = value; }
        }

        public String CompileEmployeeData()
        {
            return name + ":" + type+":" + region+":"+matchCount+":"+averageRating+":"+wins+":"+losses+":"+draws+":"+moraleRank+":"+moralePoints+":"+failedQuitRolls+":"+quitRollCeiling+"|";
        }

        public override string ToString()
        {
            return this.Name;
        }

        public String GetRecord()
        {
            return wins + "/" + losses + "/" + draws;
        }

        public String GetMatchHistory()
        {
            return "Total Matches: " + matchCount + " Average Rating: " + averageRating;
        }

        public void ResetMoralePoints()
        {
            moralePoints = 0;
        }

        public String GetMoraleInfo()
        {
            return "Morale Rank: " + moraleRank + " Morale Points: " + moralePoints;
        }
        #endregion

    }


}
