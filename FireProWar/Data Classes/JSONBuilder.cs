using Data_Classes;
using DG;
using ModPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FireProWar.Data_Classes
{
    public static class JSONBuilder
    {
        public static String CreateJSONPromotion(Promotion promotion, String userID)
        {
            StringWriter stringWriter = new StringWriter();
            JsonTextWriter writer = new JsonTextWriter(stringWriter);

            writer.WriteStartObject();

            writer.WritePropertyName("id");
            writer.WriteValue(userID + promotion.Name);

            writer.WritePropertyName("ownerId");
            writer.WriteValue(userID);

            writer.WritePropertyName("name");
            writer.WriteValue(promotion.Name);

            writer.WritePropertyName("style");
            writer.WriteValue(promotion.Type);

            writer.WritePropertyName("region");
            writer.WriteValue(promotion.Region);

            writer.WritePropertyName("history");
            writer.WriteStartArray();

            //Ensure that at least one item is written
            var historyItems = promotion.History.Split('~');
            if (historyItems.Length > 0)
            {
                foreach (String item in promotion.History.Split('~'))
                {
                    writer.WriteValue(item);
                }
            }
            else
            {
                writer.WriteValue(String.Empty);
            }
            writer.WriteEndArray();

            writer.WritePropertyName("details");
            writer.WriteStartArray();

            //Ensure that at least one item is written
            if (promotion.MatchDetails.Count > 0)
            {
                foreach (String detail in promotion.MatchDetails)
                {
                    writer.WriteValue(detail);
                }
            }
            else
            {
                writer.WriteValue(String.Empty);
            }
            writer.WriteEndArray();

            writer.WritePropertyName("matchCount");
            writer.WriteValue(promotion.MatchCount);

            writer.WritePropertyName("averageRating");
            writer.WriteValue(promotion.AverageRating);

            writer.WritePropertyName("rosterCount");
            writer.WriteValue(promotion.EmployeeList.Count);

            writer.WriteEndObject();

            return stringWriter.ToString();
        }

        public static String CreateJSONEmployee(Employee employee, String currentPromotion, String userID)
        {
            StringWriter stringWriter = new StringWriter();
            JsonTextWriter writer = new JsonTextWriter(stringWriter);

            writer.WriteStartObject();

            writer.WritePropertyName("id");
            writer.WriteValue(userID + employee.Name);

            writer.WritePropertyName("ownerId");
            writer.WriteValue(userID);

            writer.WritePropertyName("name");
            writer.WriteValue(employee.Name);

            writer.WritePropertyName("style");
            writer.WriteValue(employee.Type);

            writer.WritePropertyName("country");
            writer.WriteValue(employee.Region);

            writer.WritePropertyName("moraleRank");
            writer.WriteValue(employee.MoraleRank);

            writer.WritePropertyName("moralePoints");
            writer.WriteValue(employee.MoralePoints);

            writer.WritePropertyName("matchCount");
            writer.WriteValue(employee.MatchCount);

            writer.WritePropertyName("averageRating");
            writer.WriteValue(employee.AverageRating);

            writer.WritePropertyName("currentWins");
            writer.WriteValue(employee.Wins);

            writer.WritePropertyName("currentLosses");
            writer.WriteValue(employee.Losses);

            writer.WritePropertyName("currentDraws");
            writer.WriteValue(employee.Draws);

            writer.WritePropertyName("currentPromotion");
            writer.WriteValue(currentPromotion);

            writer.WriteEndObject();

            return stringWriter.ToString();
        }

        public static String CreateJSONTitle(TitleMatch_Data title, String userID)
        {
            StringWriter stringWriter = new StringWriter();
            JsonTextWriter writer = new JsonTextWriter(stringWriter);

            writer.WriteStartObject();

            writer.WritePropertyName("id");
            writer.WriteValue(userID + title.titleName);

            writer.WritePropertyName("ownerID");
            writer.WriteValue(userID);

            writer.WritePropertyName("name");
            writer.WriteValue(title.titleName);

            writer.WritePropertyName("type");
            writer.WriteValue(Enum.GetName(typeof(MatchType), title.matchType));

            writer.WritePropertyName("currentChamp");
            writer.WriteValue(CheckName(title.GetCurrentTitleHolderName(), title));

            writer.WritePropertyName("currentDefenses");
            global::TitleMatch_Record_Data latestMatchRecord = title.GetLatestMatchRecord();
            if (latestMatchRecord == null)
            {
                writer.WriteValue("0");
            }
            else
            {
                writer.WriteValue(latestMatchRecord.DefenseCount);
            }

            writer.WritePropertyName("previousChamps");
            writer.WriteStartArray();
            foreach (var record in title.titleMatch_Record_Data)
            {
                writer.WriteValue(CheckName(record.Champion, title) + ":" + record.DefenseCount);
            }
            if (title.titleMatch_Record_Data.Count == 0)
            {
                writer.WriteValue(":0");
            }
            writer.WriteEndArray();

            writer.WriteEndObject();

            return stringWriter.ToString();
        }

        private static String CheckName(String name, TitleMatch_Data title)
        {
            if (title.playerNum == TagType.Single || name == String.Empty)
            {
                return name;
            }

            String members = String.Empty;
            Team champTeam = null;
            foreach (Team team in ModPack.ModPack.Teams)
            {
                if (team.Name.Equals(name) || team.Nickname.Equals(name))
                {
                    champTeam = team;
                    break;
                }
            }

            if(champTeam == null)
            {
                return name;
            }

            foreach (string item in champTeam.Members)
            {
                if (members == String.Empty)
                {
                    members = item;
                }
                else
                {
                    members = members + ", " + item;
                }
            }

            name = name + " (" + members + ")";

            return name;
        }
    }
}
