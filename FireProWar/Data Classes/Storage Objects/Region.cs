using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FireProWar.Data_Classes.Storage_Objects
{
    [Serializable]
    [DynamoDBTable("Region")]
    class Region
    {
        #region Parameters
        [DynamoDBHashKey]
        private int id;

        public int ID
        {
            get { return id; }
        }

        [DynamoDBProperty]
        private String name;

        public String Name
        {
            get { return name; }
        }
        #endregion

        #region Methods
        public Region (int RegionID, string RegionName)
        {
            id = RegionID;
            name = RegionName;
        }
        #endregion
    }
}
