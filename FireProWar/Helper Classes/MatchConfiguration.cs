﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MatchConfig
{
    public static class MatchConfiguration
    {

        public static WrestlerID GetWrestlerNo(FPT_WresIDGroup wrestler)
        {
            return (WrestlerID)wrestler.ID;
        }

        public static FPT_WresIDGroup GetWrestlerData(int id, List<FPT_WresIDGroup> wrestlerList)
        {
            FPT_WresIDGroup wrestlerData = null;

            foreach (FPT_WresIDGroup wrestler in wrestlerList)
            {
                if (wrestler.ID == id)
                {
                    wrestlerData = wrestler;
                    break;
                }
            }
            return wrestlerData;
        }

        public static String GetWrestlerName(String wrestlerData)
        {
            String[] wrestlerName = wrestlerData.Split(':');
            return wrestlerName[0];
        }

        public static uint[] LoadSpeed()
        {
            uint[] speeds = new uint[9];
            speeds[0] = 100;
            speeds[1] = 125;
            speeds[2] = 150;
            speeds[3] = 175;
            speeds[4] = 200;
            speeds[5] = 300;
            speeds[6] = 400;
            speeds[7] = 800;
            speeds[8] = 1000;
            return speeds;
        }

        public static String[] LoadDifficulty()
        {
            String[] levels = new String[10];

            for (int i = 1; i <= 10; i++)
            {
                levels[i - 1] = i.ToString();
            }

            return levels;
        }

        public static String[] LoadVenue()
        {
            String[] venues = new String[6];
            venues[0] = "Big Garden Arena";
            venues[1] = "SCS Stadium";
            venues[2] = "Arena De Universo";
            venues[3] = "Spike Dome";
            venues[4] = "Yurakuen Hall";
            venues[5] = "Dojo";

            return venues;
        }

        public static List<String> LoadRings()
        {
            List<String> rings = new List<String>{"SWA", "NJPW"};
            foreach (RingData ring in SaveData.GetInst().editRingData)
            {
                rings.Add(ring.name);
            }
            return rings;
        }

        public static List<String> LoadReferees()
        {
            List<String> referees = new List<String>
            {
                "Mr Judgement"
            };
            //foreach (RefereeData referee in SaveData.GetInst().editRefereeData)
            //{
            //    referees.Add(referee.Prm.name);
            //}
            return referees;
        }

        public static List<String> LoadBGMs()
        {
            List<String> bgms = new List<String>
            {
                "Fire Pro Wrestling 2017",
                "Spinning Panther 2017",
                "Lonely Stage 2017"
            };

            string currentPath = System.IO.Directory.GetCurrentDirectory();

            IEnumerable<String> themes;
            themes = Directory.GetFiles(currentPath + @"\BGM");
            foreach (String theme in themes)
            {
                bgms.Add(theme.Replace(currentPath + @"\BGM", "").Replace(@"\", ""));
            }
            return bgms;
        }

        public static List<FPT_WresIDGroup> LoadWrestlers()
        {
            List<FPT_WresIDGroup> wrestlers = new List<FPT_WresIDGroup>();

            foreach (EditWrestlerData current in SaveData.inst.editWrestlerData)
            {
                FPT_WresIDGroup wresIDGroup = new FPT_WresIDGroup
                {
                    Name = DataBase.GetWrestlerFullName(current.wrestlerParam),
                    ID = (Int32)current.editWrestlerID,
                    Group = current.wrestlerParam.groupID
                };
                wrestlers.Add(wresIDGroup);
            }
            return wrestlers;
        }

        public static List<String> LoadPromotions()
        {
            List<String> promotions = new List<String>();
            foreach (GroupInfo current in SaveData.GetInst().groupList)
            {
                string longName = SaveData.GetInst().organizationList[current.organizationID].longName;
                promotions.Add(longName + " : " + current.longName);
            }
            return promotions;
        }

        public static List<FPT_WresIDGroup> LoadWrestlersFromPromotion(List<FPT_WresIDGroup> wrestlerList, String promotionName, List<String> promotionList)
        {
            List<FPT_WresIDGroup> wrestlers = new List<FPT_WresIDGroup>();
            return wrestlers;
        }

        public static List<CountryEnum> GetRegionList()
        {
            var regionList = Enum.GetValues(typeof(CountryEnum)).Cast<CountryEnum>().ToList();
            return regionList;
        }


    }
}
