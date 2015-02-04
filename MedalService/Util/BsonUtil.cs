using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedalService.Util
{

    public static class AthleteBsonUtil
    {

        public static class Fields
        {
            public const string Id = "_id";
            public const string Year = "Year";
            public const string Country = "Country";
        }
        public static string CountryFieldtoBsonColumn(string countryField)
        {
            if (countryField == "GoldMedals")
                return "G";
            if (countryField == "SilverMedals")
                return "S";
            if (countryField == "BronzeMedals")
                return "B";
            if (countryField == "TotalMedals")
                return "T";

            return "_id";
        }
        public static class Groups
        {
            public static BsonDocument Year
            {
                get
                {
                    return new BsonDocument
                    {
                        { "$group", new BsonDocument
                            {
                                { "_id", "$Year" }
                            }
                        }
                    };
                }
            }
            public static BsonDocument CountrySlim
            {
                get
                {
                    return new BsonDocument
                        {
                            { "$group", new BsonDocument
                                {
                                    { "_id", "$Country" },
                                }
                            }
                        };
                }
            }
            public static BsonDocument Sport
            {
                get
                {
                    return new BsonDocument
                        {
                            { "$group", new BsonDocument
                                {
                                    { "_id", "$Sport" },
                                    { "G",   new BsonDocument {{ "$sum","$Gold Medals"  }} },
                                    { "S",   new BsonDocument {{ "$sum","$Silver Medals"  }} },
                                    { "B",   new BsonDocument {{ "$sum","$Bronze Medals"  }} },
                                    { "T",   new BsonDocument {{ "$sum","$Total Medals" }} }
                                }
                            }
                        };
                }
            }
            public static BsonDocument Country
            {
                get
                {
                    return new BsonDocument
                        {
                            { "$group", new BsonDocument
                                {
                                    { "_id", "$Country" },
                                    { "G",   new BsonDocument {{ "$sum","$Gold Medals"  }} },
                                    { "S",   new BsonDocument {{ "$sum","$Silver Medals"  }} },
                                    { "B",   new BsonDocument {{ "$sum","$Bronze Medals"  }} },
                                    { "T",   new BsonDocument {{ "$sum","$Total Medals" }} }
                                }
                            }
                        };
                }
            }
        }
        public static class Aggregates
        {

            public static BsonDocument Sort(string field, MedalService.Models.Sort sort)
            {
                return new BsonDocument
                    {
                        { "$sort", new BsonDocument
                            {
                                { field, sort == MedalService.Models.Sort.Ascending ? 1 : -1 }
                            }
                        }
                    };
            }
            public static BsonDocument Match(string field, int value)
            {
                return new BsonDocument
                    {
                        { "$match", new BsonDocument
                            {
                                { field, value }
                            }
                        }
                    };
            }
            public static BsonDocument Match(string field, string value)
            {
                return new BsonDocument
                    {
                        { "$match", new BsonDocument
                            {
                                { field, value }
                            }
                        }
                    };
            }
        }
    }
}
