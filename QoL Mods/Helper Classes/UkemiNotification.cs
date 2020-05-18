using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common_Classes;
using DG;

namespace QoL_Mods.Helper_Classes
{
    public class UkemiNotification
    {
        private const char dataSeparator = ':';
        private const char listSeparator = ';';

        public EnumLibrary.NotificationType Type { get => type; set => type = value; }
        private EnumLibrary.NotificationType type;

        public string Name { get => name; set => name = value; }
        private String name;

        public List<CheerVoiceEnum> CheerList { get => cheerList; set => cheerList = value; }
        private List<CheerVoiceEnum> cheerList;

        public String SaveNotificationData()
        {
            String saveData = "";
            String cheerData = "";

            try
            {
                saveData = name + dataSeparator + type + dataSeparator;

                foreach (CheerVoiceEnum cheer in cheerList)
                {
                    cheerData += cheer.ToString() + listSeparator;
                }

                //Remove last separator
                //if (cheerData[cheerData.Length - 1].Equals(':'))
                //{
                //    cheerData = cheerData.Remove(cheerData.Length - 1);
                //}
                saveData += cheerData;

            }
            catch (Exception e)
            {
                L.D("SaveNotificationDataError: " + e);
            }

            return saveData;
        }

        public void LoadNotificationData(String data)
        {
            try
            {
                //Split data into different categories
                var splitData = data.Split(dataSeparator);
                name = splitData[0];
                type = (EnumLibrary.NotificationType)Enum.Parse(typeof(EnumLibrary.NotificationType), splitData[1]);

                cheerList = new List<CheerVoiceEnum>();
                foreach (String cheer in splitData[2].Split(listSeparator))
                {
                    if (cheer.Trim().Equals(String.Empty))
                    {
                        continue;
                    }
                    cheerList.Add((CheerVoiceEnum)Enum.Parse(typeof(CheerVoiceEnum), cheer));
                }
            }
            catch (Exception e)
            {
                L.D("LoadNotificationDataError: " + e);
            }

        }

        public override String ToString()
        {
            return name;
        }
    }
}
