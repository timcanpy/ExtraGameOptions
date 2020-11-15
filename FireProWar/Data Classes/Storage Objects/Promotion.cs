using Amazon.DynamoDBv2.DataModel;
using System;

namespace FireProWar.Data_Classes.Storage_Objects
{
    [Serializable]
    [DynamoDBTable("Promotion")]
    class Promotion
    {
        #region Parameters
        [DynamoDBHashKey]
        private String promotionID;

        public String PromotionID
        {
            get { return promotionID; }
        }

        [DynamoDBProperty]
        private String ownerID;

        public String OwnerID
        {
            get { return ownerID; }
        }

        [DynamoDBProperty]
        private String name;

        public String Name
        {
            get { return name; }
        }

        [DynamoDBProperty]
        private PromotionStyles style;

        public PromotionStyles Style
        {
            get { return style; }
        }

        [DynamoDBProperty]
        private CountryEnum region;

        public CountryEnum Region
        {
            get { return region; }
        }
        #endregion

        public Promotion(String Promotion, String Owner, String PromotionName, PromotionStyles PromotionStyle, CountryEnum PromotionRegion)
        {
            promotionID = Promotion;
            ownerID = Owner;
            name = PromotionName;
            style = PromotionStyle;
            region = PromotionRegion;
        }
    }
}
