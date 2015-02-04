using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedalService.Models.Impl
{
    public class SportMedal : ISportMedal
    {
        [BsonElement("_id")]
        public String Sport
        {
            get;
            set;
        }

        [BsonElement("G")]
        public int GoldMedals
        {
            get;
            set;
        }

        [BsonElement("S")]
        public int SilverMedals
        {
            get;
            set;
        }

        [BsonElement("B")]
        public int BronzeMedals
        {
            get;
            set;
        }

        [BsonElement("T")]
        public int TotalMedals
        {
            get;
            set;
        }
    }
}
