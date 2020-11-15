using Amazon.DynamoDBv2.DataModel;
using System;

namespace FireProWar.Data_Classes.Storage_Objects
{
    [Serializable]
    [DynamoDBTable("Owner")]
    public class Owner
    {
        #region Parameters
        [DynamoDBHashKey]
        private String id;

        public String ID
        {
            get { return id; }
        }

        [DynamoDBProperty]
        private String name;

        public String Name
        {
            get { return name; }
        }

        [DynamoDBProperty]
        private DateTime lastUpdate;

        public DateTime LastUpdate
        {
            get { return lastUpdate; }
        }

        #endregion

        public Owner(String OwnerID, String OwnerName, DateTime EntryDate)
        {
            id = OwnerID;
            name = OwnerName;
            lastUpdate = EntryDate; 
        }
    }
}
