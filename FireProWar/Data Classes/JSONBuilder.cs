using Data_Classes;
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
            foreach (String item in promotion.History.Split('~'))
            {
                writer.WriteValue(item);
            }
            writer.WriteEndArray();

            writer.WritePropertyName("details");
            writer.WriteStartArray();
            foreach (String detail in promotion.MatchDetails)
            {
                writer.WriteValue(detail);
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
            writer.WriteValue(title.GetCurrentTitleHolderName());

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
            foreach(var record in title.titleMatch_Record_Data)
            {
                writer.WriteValue(record.Champion + ":" + record.DefenseCount);
            }
            writer.WriteEndArray();

            writer.WriteEndObject();

            return stringWriter.ToString();
        }
    }
}
