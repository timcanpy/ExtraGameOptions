﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DG;
using QoL_Mods.Helper_Classes;

namespace MatchConfig
{
    public static class EF_MatchConfiguration
    {
    
        public static WrestlerID GetWrestlerNo(EF_WresIDGroup wrestler)
        {
            return (WrestlerID)wrestler.ID;
        }

        public static EF_WresIDGroup GetWrestlerData(int id, List<EF_WresIDGroup> wrestlerList)
        {
            EF_WresIDGroup wrestlerData = null;

            foreach (EF_WresIDGroup wrestler in wrestlerList)
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

        public static List<RingData> LoadRings()
        {
            List<RingData> rings = new List<RingData>();
            foreach (RingData ring in SaveData.GetInst().editRingData)
            {
                rings.Add(ring);
            }
            return rings;
        }

        public static List<RefereeInfo> LoadReferees()
        {
            List<RefereeInfo> referees = new List<RefereeInfo>();
            foreach (RefereeData referee in SaveData.GetInst().editRefereeData)
            {
                referees.Add(new RefereeInfo(referee));
            }
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

        public static List<EF_WresIDGroup> LoadWrestlers()
        {
            List<EF_WresIDGroup> wrestlers = new List<EF_WresIDGroup>();

            foreach (EditWrestlerData current in SaveData.inst.editWrestlerData)
            {
                EF_WresIDGroup wresIDGroup = new EF_WresIDGroup
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

        public static List<EF_WresIDGroup> LoadWrestlersFromPromotion(List<EF_WresIDGroup> wrestlerList, String promotionName, List<String> promotionList)
        {
            List<EF_WresIDGroup> wrestlers = new List<EF_WresIDGroup>();
            return wrestlers;
        }

        public static RefereeInfo GetRefereeInfo(int refereeID)
        {
            foreach (RefereeData referee in SaveData.GetInst().editRefereeData)
            {
                if ((int)referee.editRefereeID == refereeID)
                {
                    return new RefereeInfo(referee);
                }
            }

            return null;
        }

        public static int GetPlayerCount()
        {
            int players = 0;
            MatchSetting matchSetting = global::GlobalWork.inst.MatchSetting;
            for (int i = 0; i < 8; i++)
            {
                Player plObj = PlayerMan.inst.GetPlObj(i);

                if (!plObj)
                {
                    continue;
                }

                if (matchSetting.matchWrestlerInfo[plObj.PlIdx].isSecond || matchSetting.matchWrestlerInfo[plObj.PlIdx].isIntruder)
                {
                    continue;
                }

                players++;
            }

            return players;
        }
        public static RefereeInfo GetRefereeInfo(String name)
        {
            foreach (RefereeData referee in SaveData.GetInst().editRefereeData)
            {
                if (referee.Prm.name.Equals(name))
                {
                    return new RefereeInfo(referee);
                }
            }

            return null;
        }
    }
}
