using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedalService.Models.Impl
{
    public class Athlete : IAthlete
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public String Id { get; set; }

        [BsonElement("Athlete")]
        public string Name { get; set; }

        public Int32 Age { get; set; }

        public String Country { get; set; }

        public int Year { get; set; }

        [BsonElement("Closing Ceremony Date")]
        public String CeremonyDate { get; set; }

        public String Sport { get; set; }

        [BsonElement("Gold Medals")]
        public int GoldMedals { get; set; }

        [BsonElement("Silver Medals")]
        public int SilverMedals { get; set; }

        [BsonElement("Bronze Medals")]
        public int BronzeMedals { get; set; }

        [BsonElement("Total Medals")]
        public int TotalMedals { get; set; }

    }

}
