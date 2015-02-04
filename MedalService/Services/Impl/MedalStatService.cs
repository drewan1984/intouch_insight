
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Driver.Builders;
using MedalService.Util;
using MedalService.Models;
using MedalService.Models.Impl;


namespace MedalService.Services.Impl
{
    /// <summary>
    /// IMedalStatService MongoDBImplementation.
    /// Retrieves data from a mongo database source.
    /// </summary>
    public class MedalStatServiceMongoDB : IMedalStatService
    {
        private MongoClient client;
        private MongoServer server;
        private MongoDatabase database;
        private MongoCollection<Athlete> athletes;

        public MedalStatServiceMongoDB()
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
                    AthleteBsonUtil.Groups.Year,
                    AthleteBsonUtil.Aggregates.Sort(AthleteBsonUtil.Fields.Id, Sort.Ascending)
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
                Pipeline = new[] { AthleteBsonUtil.Groups.Sport }
            });
            result = aggr.Select(a => (string)a.First().Value).ToList();
            return result;
        }

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
        public List<ICountry> GetCountries(IQueryParams queryParams = null)
        {
            var pipeline = new List<BsonDocument>();
            pipeline.Add(AthleteBsonUtil.Groups.Country);
            _applyQueryParams(pipeline,queryParams);
            return _getCountryGroups(pipeline, queryParams);
        }
        public List<ICountry> GetCountries(int year, IQueryParams queryParams = null)
        {
            var pipeline = new List<BsonDocument>();
            pipeline.Add(AthleteBsonUtil.Aggregates.Match(AthleteBsonUtil.Fields.Year,year));
            pipeline.Add(AthleteBsonUtil.Groups.Country);
            _applyQueryParams(pipeline, queryParams);
            return _getCountryGroups(pipeline, queryParams);
        }
        public ICountry GetCountry(string countryName)
        {
            var pipeline = new List<BsonDocument>();
            pipeline.Add(AthleteBsonUtil.Aggregates.Match(AthleteBsonUtil.Fields.Country,countryName));
            pipeline.Add(AthleteBsonUtil.Groups.Country);
            return _getCountryGroups(pipeline,null)
                 .FirstOrDefault<ICountry>();
        }
        public ICountry GetCountry(int year, string countryName)
        {
            var pipeline = new List<BsonDocument>();
            pipeline.Add(AthleteBsonUtil.Aggregates.Match(AthleteBsonUtil.Fields.Year,year));
            pipeline.Add(AthleteBsonUtil.Aggregates.Match(AthleteBsonUtil.Fields.Country,countryName));
            pipeline.Add(AthleteBsonUtil.Groups.Country);
            return _getCountryGroups(pipeline, null)
                 .FirstOrDefault<ICountry>();
        }

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
            pipeline.Add(AthleteBsonUtil.Aggregates.Match(AthleteBsonUtil.Fields.Country,countryName));
            var result = _getAthletes(pipeline, queryParams);
            return result;
        }
        public List<IAthlete> GetAthletes(int year, IQueryParams queryParams = null)
        {
            var pipeline = new List<BsonDocument>();
            pipeline.Add(AthleteBsonUtil.Aggregates.Match(AthleteBsonUtil.Fields.Year,year));
            var result = _getAthletes(pipeline, queryParams);;
            return result;
        }
        public List<IAthlete> GetAthletes(int year, string countryName, IQueryParams queryParams = null)
        {
            var pipeline = new List<BsonDocument>();
            pipeline.Add(AthleteBsonUtil.Aggregates.Match(AthleteBsonUtil.Fields.Year, year));
            pipeline.Add(AthleteBsonUtil.Aggregates.Match(AthleteBsonUtil.Fields.Country, countryName));
            var result = _getAthletes(pipeline, queryParams); ;
            return result;
        }
        public List<IAthlete> GetAthletes(Models.IQueryParams queryParams = null)
        {
            var result = _getAthletes(null, queryParams).ToList<IAthlete>();;
            return result;
        }

        private List<ISportMedal> _getSportGroups(IEnumerable<BsonDocument> pipeline)
        {
            var sportModels = default(List<ISportMedal>);
            var groupAggregate = athletes.Aggregate(new AggregateArgs { Pipeline = pipeline });
            sportModels = groupAggregate.Select(BsonSerializer.Deserialize<SportMedal>).ToList<ISportMedal>();
            return sportModels;
        }
        public List<ISportMedal> GetSportMedals(string countryName)
        {
            var pipeline = new List<BsonDocument>();
            pipeline.Add(AthleteBsonUtil.Aggregates.Match(AthleteBsonUtil.Fields.Country, countryName));
            pipeline.Add(AthleteBsonUtil.Groups.Sport);
            return _getSportGroups(pipeline);
        }
        public List<ISportMedal> GetSportMedals(int year)
        {
            var pipeline = new List<BsonDocument>();
            pipeline.Add(AthleteBsonUtil.Aggregates.Match(AthleteBsonUtil.Fields.Year, year));
            pipeline.Add(AthleteBsonUtil.Groups.Sport);
            return _getSportGroups(pipeline);
        }
        public List<ISportMedal> GetSportMedals(string countryName, int year)
        {
            var pipeline = new List<BsonDocument>();
            pipeline.Add(AthleteBsonUtil.Aggregates.Match(AthleteBsonUtil.Fields.Country, countryName));
            pipeline.Add(AthleteBsonUtil.Aggregates.Match(AthleteBsonUtil.Fields.Year, year));
            pipeline.Add(AthleteBsonUtil.Groups.Sport);
            return _getSportGroups(pipeline);
        }

        private void _applyQueryParams(List<BsonDocument> pipeline, IQueryParams queryParams)
        {
            if (queryParams != null && queryParams.Sort != null)
                pipeline.Add(
                    AthleteBsonUtil.Aggregates.Sort
                    (
                        AthleteBsonUtil.CountryFieldtoBsonColumn(queryParams.SortProperty),
                        queryParams.Sort.Value)
                    );
        }

    }
}
