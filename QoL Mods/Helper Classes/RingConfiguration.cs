using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG;
using MatchConfig;

namespace QoL_Mods.Helper_Classes
{
    public class RingConfiguration
    {
        private char dataSeparator = ';';
        private char listSeparator = ':';

        private string ringName;
        public string RingName { get => ringName; set => ringName = value; }

        private List<RefereeInfo> referees;
        public List<RefereeInfo> Referees { get => referees; set => referees = value; }

        private List<String> bgms;
        public List<string> Bgms { get => bgms; set => bgms = value; }

        private GrappleSetting grappleSetting;
        public GrappleSetting GrappleSetting { get => grappleSetting; set => grappleSetting = value; }

        private int singlesSpeed;
        public int SinglesSpeed { get => singlesSpeed; set => singlesSpeed = value; }

        private int multiSpeed;
        public int MultiSpeed { get => multiSpeed; set => multiSpeed = value; }

        public RingConfiguration()
        {
            bgms = new List<String>();
            referees = new List<RefereeInfo>();
            grappleSetting = new GrappleSetting();
        }

        public override String ToString()
        {
            return ringName;
        }

        public string SaveConfiguration()
        {
            String data = "";

            //L.D("Saving " + ringName);
            //L.D("Referee Number: " + referees.Count);
            data += ringName + dataSeparator;
            data += grappleSetting.SaveSettings() + dataSeparator;
            data += singlesSpeed + "" + dataSeparator;
            data += multiSpeed + "" + dataSeparator;

            //Referees
            string listData = "";
            foreach (var referee in referees)
            {
                if (referee == null)
                {
                    continue;
                }
                listData += referee.Data.Prm.name + listSeparator;
            }
            data += listData + dataSeparator;

            //BGMs
            listData = "";
            foreach (var bgm in bgms)
            {
                listData += bgm + listSeparator;
            }

            data += listData;

            return data;
        }

        public void LoadConfiguration(string data)
        {
            try
            {
                var dataLines = data.Split(dataSeparator);

                ringName = dataLines[0];
                grappleSetting.LoadSettings(dataLines[1]);
                Int32.TryParse(dataLines[2], out singlesSpeed);
                Int32.TryParse(dataLines[3], out multiSpeed);

                //Referees
                referees = new List<RefereeInfo>();
                var listData = dataLines[4].Split(listSeparator);
                foreach (var item in listData)
                {
                    if (item.Trim().Equals(String.Empty))
                    {
                        continue;
                    }

                    RefereeInfo referee = EF_MatchConfiguration.GetRefereeInfo(item);
                    
                    if (referee == null)
                    {
                        continue;
                    }

                    referees.Add(referee);
                }

                //BGMs
                bgms = new List<String>();
                listData = dataLines[5].Split(listSeparator);
                foreach (var item in listData)
                {
                    if (item.Trim().Equals(String.Empty))
                    {
                        continue;
                    }

                    bgms.Add(item);
                }
            }
            catch (Exception e)
            {
                L.D("LoadRingConfigurationError: " + e);
            }

        }
    }
}
