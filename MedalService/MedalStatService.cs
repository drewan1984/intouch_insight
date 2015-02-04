using MongoDB.Driver;
using MongoTest2.Models;
using MongoTest2.Models.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver.Builders;

namespace MongoTest2.Services.Impl
{
    public class MedalStatService : IMedalStatService
    {
        private MongoClient client;
        private MongoServer server;
        private MongoDatabase database;
        private MongoCollection<Athlete> athletes;

        private BsonDocument YearGroup = new BsonDocument
        {
            { "$group", new BsonDocument
                {
                    { "_id", "$Year" }
                }
            }
        };

        private BsonDocument YearSort = new BsonDocument 
        {
            { "$sort", new BsonDocument
                {
                    { "_id", 1 }
                }
            }
        };

        private BsonDocument CountryGroup = new BsonDocument
        {
            { "$group", new BsonDocument
                {
                    { "_id", "$Country" },
                }
            }

        };
        private BsonDocument SportGroup = new BsonDocument 
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
        private BsonDocument CountryMedalGroup = new BsonDocument
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

        private BsonDocument GetAthleteCountyGroupMatch(string country)
        {
            return new BsonDocument
            {
                { "$match", new BsonDocument
                    {
                        { "Country", country }
                    }
                }
            };
        }
        private BsonDocument GetAthleteYearGroupMatch(int year)
        {
            return new BsonDocument
            {
                { "$match", new BsonDocument
                    {
                        { "Year", year }
                    }
                }
            };
        }
        private BsonDocument GetSortDocument(string field, Sort sort)
        {
            return new BsonDocument
            {
                { "$sort", new BsonDocument
                    {
                        { field, sort == Sort.Ascending ? 1 : -1 }
                    }
                }
            };
        }
        
        private string CountryFieldToBsonColumn(String countryField)
        {

            if (countryField == "GoldMedals") return "G";
            if (countryField == "SilverMedals") return "S";
            if (countryField == "BronzeMedals") return "B";
            if (countryField == "TotalMedals") return "T";

            return "_id";
        }

        public MedalStatService()
        {
            client = new MongoClient("mongodb://localhost");
            server = client.GetServer();
            database = server.GetDatabase("oymdb");
            athletes = database.GetCollection<Athlete>("medal_winners");
        }

        public List<int> GetYears()
        {
            var result = default(List<int>);
            var aggr = athletes.Aggregate(new AggregateArgs 
            { 
                Pipeline = new [] 
                { 
                    YearGroup,
                    YearSort
                } 
            });


            result = aggr.Select(a => (int)a.First().Value).ToList();
            return result;
        }
        public List<string> GetSports()
        {
            var result = default(List<string>);
            var aggr = athletes.Aggregate(new AggregateArgs
            {
                Pipeline = new[] 
                { 
                    SportGroup,
                }
            });
            result = aggr.Select(a => (string)a.First().Value).ToList();
            return result;
        }

        #region Country Group Helpers

        private List<ICountry> _getCountryGroups(IEnumerable<BsonDocument> pipeline, IQueryParams queryParams)
        {
            var result = default(List<ICountry>);

            var groupAggregate = athletes.Aggregate(new AggregateArgs { Pipeline = pipeline });

            if (queryParams != null && queryParams.Count != null)
            {
                if (queryParams.Start != null)
                {
                    result = groupAggregate
                        .Skip(queryParams.Start.Value)
                        .Take(queryParams.Count.Value)
                        .Select(BsonSerializer.Deserialize<Country>)
                        .ToList<ICountry>();
                }
                else
                {
                    result = groupAggregate
                    .Take(queryParams.Count.Value)
                    .Select(BsonSerializer.Deserialize<Country>)
                    .ToList<ICountry>();
                }
            }
            else
            {
                result = groupAggregate
                    .Select(BsonSerializer.Deserialize<Country>)
                    .ToList<ICountry>();
            }

            return result;
        }
        private void _applyQueryParams(List<BsonDocument> pipeline, IQueryParams queryParams)
        {
            if (queryParams != null && queryParams.Sort != null)
                pipeline.Add(
                    GetSortDocument
                    (
                        CountryFieldToBsonColumn(queryParams.SortProperty),
                        queryParams.Sort.Value)
                    );
        }
        
        #endregion

        #region Country Implementations

        public List<ICountry> GetCountries(IQueryParams queryParams = null)
        {
            var pipeline = new List<BsonDocument>();
            pipeline.Add(CountryMedalGroup);
            _applyQueryParams(pipeline,queryParams);
            return _getCountryGroups(pipeline, queryParams);
        }
        public List<ICountry> GetCountries(int year, IQueryParams queryParams = null)
        {
            var pipeline = new List<BsonDocument>();
            pipeline.Add(GetAthleteYearGroupMatch(year));
            pipeline.Add(CountryMedalGroup);
            _applyQueryParams(pipeline, queryParams);
            return _getCountryGroups(pipeline, queryParams);
        }
        public ICountry GetCountry(string countryName)
        {
            var pipeline = new List<BsonDocument>();
            pipeline.Add(GetAthleteCountyGroupMatch(countryName));
            pipeline.Add(CountryMedalGroup);
            return _getCountryGroups(pipeline,null)
                 .FirstOrDefault<ICountry>();
        }
        public ICountry GetCountry(int year, string countryName)
        {
            var pipeline = new List<BsonDocument>();
            pipeline.Add(GetAthleteYearGroupMatch(year));
            pipeline.Add(GetAthleteCountyGroupMatch(countryName));
            pipeline.Add(CountryMedalGroup);
            return _getCountryGroups(pipeline, null)
                 .FirstOrDefault<ICountry>();
        }

        #endregion

        #region Athlete Helpers

        private List<IAthlete> _getAthletes(IEnumerable<BsonDocument> pipeline, IQueryParams queryParams)
        {
            var result = default(List<IAthlete>);
            IEnumerable<BsonDocument> groupAggregate;

            if (pipeline != null)
                groupAggregate = athletes.Aggregate(new AggregateArgs { Pipeline = pipeline });
            else
                groupAggregate = athletes.AsQueryable<BsonDocument>();


            if (queryParams != null && queryParams.Count != null)
            {
                if (queryParams.Start != null)
                {
                    result = groupAggregate
                        .Skip(queryParams.Start.Value)
                        .Take(queryParams.Count.Value)
                        .Select(BsonSerializer.Deserialize<Athlete>)
                        .ToList<IAthlete>();
                }
                else
                {
                    result = groupAggregate
                    .Take(queryParams.Count.Value)
                    .Select(BsonSerializer.Deserialize<Athlete>)
                    .ToList<IAthlete>();
                }
            }
            else
            {
                result = groupAggregate
                    .Select(BsonSerializer.Deserialize<Athlete>)
                    .ToList<IAthlete>();
            }

            return result;
        }
        /*
        private IQueryable<Athlete> _getAthletes(Func<Athlete, Boolean> condition = null, IQueryParams queryParams = null)
        {
            var result = default(IQueryable<Athlete>);

            if (queryParams != null && queryParams.Count != null)
            {
                if (queryParams.Start != null)
                {
                    result = athletes
                    .AsQueryable<Athlete>()
                    .Where(a => condition(a))
                    .Skip(queryParams.Start.Value)
                    .Take(queryParams.Count.Value);
                }
                else
                {
                    result = athletes
                    .AsQueryable<Athlete>()
                    .Where(a => condition(a))
                    .Take(queryParams.Count.Value);
                }
            }

            if (condition != null)
            {
                result = athletes
                    .AsQueryable<Athlete>()
                    .Where(a => condition(a))
                    .Select(a => a);
            }
            else
                result = athletes.AsQueryable<Athlete>();

            return result;
        }*/
        private IAthlete _getAthlete(Func<Athlete,Boolean> condition)
        {
            return athletes
            .AsQueryable<Athlete>()
            .FirstOrDefault(condition);
        }
        private bool _matchAthleteYear(Athlete a, int year)
        {
            return (a.Year.Equals(year));
        }
        private bool _matchAthleteName(Athlete a, string name)
        {
            return (a.Name.Equals(name));
        }
        private bool _matchAthleteCountry(Athlete a, string country)
        {
            return (a.Country.Equals(country));
        }
        private bool _matchAthleteId(Athlete a, string id)
        {
            return (a.Id.Equals(id));
        }

        #endregion

        #region Athlete Implementations

        public IAthlete GetAlthlete(string id)
        {
            var result = _getAthlete((a => _matchAthleteId(a, id)));
            return result;
        }
        public IAthlete GetAlthlete(int year, string id)
        {
            var result = _getAthlete(
                (a =>
                    _matchAthleteId(a, id) &&
                    _matchAthleteYear(a, year))
                );
            return result;
        }

        public List<IAthlete> GetAthletes(string countryName, IQueryParams queryParams = null)
        {
            var pipeline = new List<BsonDocument>();
            pipeline.Add(GetAthleteCountyGroupMatch(countryName));
            var result = _getAthletes(pipeline, queryParams);
            return result;
        }
        public List<IAthlete> GetAthletes(int year, IQueryParams queryParams = null)
        {
            var pipeline = new List<BsonDocument>();
            pipeline.Add(GetAthleteYearGroupMatch(year));
            var result = _getAthletes(pipeline, queryParams);;
            return result;
        }
        public List<IAthlete> GetAthletes(int year, string countryName, IQueryParams queryParams = null)
        {
            var pipeline = new List<BsonDocument>();
            pipeline.Add(GetAthleteYearGroupMatch(year));
            pipeline.Add(GetAthleteCountyGroupMatch(countryName));
            var result = _getAthletes(pipeline, queryParams); ;
            return result;
            return result;
        }
        public List<IAthlete> GetAthletes(Models.IQueryParams queryParams = null)
        {
            var result = _getAthletes(null, queryParams).ToList<IAthlete>();;
            return result;
        }


        #endregion

        #region Sport Helpers
        private List<ISportMedal> _getSportGroups(IEnumerable<BsonDocument> pipeline)
        {
            var sportModels = default(List<ISportMedal>);

            var groupAggregate = athletes.Aggregate(new AggregateArgs { Pipeline = pipeline });

            sportModels = groupAggregate.Select(BsonSerializer.Deserialize<SportMedal>).ToList<ISportMedal>();



            return sportModels;
        }

        #endregion

        #region SportMedal Implementation

        public List<ISportMedal> GetSportMedals(string countryName)
        {
            var pipeline = new List<BsonDocument>();
            pipeline.Add(GetAthleteCountyGroupMatch(countryName));
            pipeline.Add(SportGroup);
            return _getSportGroups(pipeline);
        }

        public List<ISportMedal> GetSportMedals(int year)
        {
            var pipeline = new List<BsonDocument>();
            pipeline.Add(GetAthleteYearGroupMatch(year));
            pipeline.Add(SportGroup);
            return _getSportGroups(pipeline);
        }

        public List<ISportMedal> GetSportMedals(string countryName, int year)
        {
            var pipeline = new List<BsonDocument>();
            pipeline.Add(GetAthleteCountyGroupMatch(countryName));
            pipeline.Add(GetAthleteYearGroupMatch(year));
            pipeline.Add(SportGroup);
            return _getSportGroups(pipeline);
        }

        #endregion
    }


}
