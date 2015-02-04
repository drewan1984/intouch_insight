using MedalService.Models;
using MedalService.Services;
using MedalService.Services.Impl;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedelServiceTester
{
    [TestFixture]
    class MedalStatServiceTest
    {
        IMedalStatService service;
        const int expectedYearMin = 2000;
        const int expectedYearMax = 2012;
        const int expectedYearStep = 2;
        const string expectedCountry = "United States";

        const string country_name = "Name";
        const string country_goldMedals = "GoldMedals";
        const string country_silverMedals = "SilverMedals";
        const string country_bronzeMedals = "BronzeMedals";
        const string country_totalMedals = "TotalMedals";

        const string sport_sport = "Sport";
        const string sport_goldMedals = "GoldMedals";
        const string sport_silverMedals = "SilverMedals";
        const string sport_bronzeMedals = "BronzeMedals";
        const string sport_totalMedals = "TotalMedals";

        const string athlete_id = "Id";
        const string athlete_name = "Name";
        const string athlete_age = "Age";
        const string athlete_country = "Country";
        const string athlete_year = "Year";
        const string athlete_CeremonyData = "CeremonyDate";
        const string athlete_Sport = "Sport";
        const string athlete_goldMedals = "GoldMedals";
        const string athlete_silverMedals = "SilverMedals";
        const string athlete_bronzeMedals = "BronzeMedals";
        const string athlete_totalMedals = "TotalMedals";

        string IsNullMessage(string field) 
        { 
            return field + " is null."; 
        }
        string QueryNoResultsMessage(string query) 
        {
            return query + " has produced no results.";
        }
        
        private class QueryParams : IQueryParams
        {
            public Sort? Sort { get; set; }
            public string SortProperty { get; set;}
            public int? Start { get; set; }
            public int? Count { get; set;}
        }

        private void ValidateCountry(ICountry country) 
        {
            Assert.IsNotNull(country.Name, IsNullMessage(country_name));
            Assert.IsNotNull(country.GoldMedals, IsNullMessage(country_goldMedals));
            Assert.IsNotNull(country.SilverMedals, IsNullMessage(country_silverMedals));
            Assert.IsNotNull(country.BronzeMedals, IsNullMessage(country_bronzeMedals));
            Assert.IsNotNull(country.TotalMedals, IsNullMessage(country_totalMedals));
        }
        private void ValidateAthlete(IAthlete athlete)
        {
            Assert.IsNotNull(athlete.Id, IsNullMessage(athlete_id));
            Assert.IsNotNull(athlete.Name, IsNullMessage(athlete_name));
            Assert.IsNotNull(athlete.Age, IsNullMessage(athlete_age));
            Assert.IsNotNull(athlete.Country, IsNullMessage(athlete_country));
            Assert.IsNotNull(athlete.Year, IsNullMessage(athlete_year));
            Assert.IsNotNull(athlete.CeremonyDate, IsNullMessage(athlete_CeremonyData));
            Assert.IsNotNull(athlete.Sport, IsNullMessage(athlete_Sport));
            Assert.IsNotNull(athlete.GoldMedals, IsNullMessage(athlete_goldMedals));
            Assert.IsNotNull(athlete.SilverMedals, IsNullMessage(athlete_silverMedals));
            Assert.IsNotNull(athlete.BronzeMedals, IsNullMessage(athlete_bronzeMedals));
            Assert.IsNotNull(athlete.TotalMedals, IsNullMessage(athlete_totalMedals));
        }

        private bool TypeContainsProperty(Type type,String properyName) 
        {
            return ((type
                .GetProperties()
                .Where(p => p.Name.Equals(properyName))
                .FirstOrDefault()) != null);
        }
        private bool TypeContainsOnlyProperties(Type type,IEnumerable<string> properyNames) 
        {
            return ((type
                .GetProperties()
                .Where(p=>!properyNames.Contains(p.Name))) 
                != null);
        }


        [SetUp]
        public void Setup()
        {
            service = new MedalStatServiceMongoDB();
        }

        [TestCase, 
        Description("Validates whether the GetYears method returns a result.")]
        public void GetYears()
        {
            var years = service.GetYears();
            Assert.IsNotEmpty(years, QueryNoResultsMessage(query: "GetYears"));
        }

        [TestCase,
        Description("Validates the result of the GetYears() method.")]
        public void YearRange()
        {
            var years = service.GetYears();
            var invalidYearFound = years.Where(y => !(
                y >= expectedYearMin &&
                y <= expectedYearMax &&
                y % expectedYearStep == 0
                )).FirstOrDefault();

            Assert.IsTrue(invalidYearFound == 0,"Year "+invalidYearFound+" is invalid.");
        }

        [TestCase,
        Description("Validates whether the GetSports method returns a result.")]
        public void GetSports()
        {
            var sports = service.GetSports();
            Assert.IsNotEmpty(sports, QueryNoResultsMessage(query: "GetSports"));
        }

        #region Country Tests

        [TestCase]
        public void ValidateAssumedCountryFields() 
        {
            var countryType = typeof(ICountry);
            
            var fields = new[] 
            { 
                country_name, 
                country_goldMedals, 
                country_silverMedals,
                country_bronzeMedals, 
                country_totalMedals  
            };

            Assert.IsTrue(TypeContainsOnlyProperties(countryType, fields));
        }

        [TestCase]
        public void ValidateCountryField(
            [Values(
                country_name,
                country_goldMedals,
                country_silverMedals,
                country_bronzeMedals,
                country_totalMedals)
            ] string field
            )
        {
            
            Assert.IsTrue(field == null || TypeContainsProperty(typeof(ICountry), field));
        }

        [TestCase]
        public void GetCountries_NoQueryParams()
        {
            var countries = service.GetCountries();
            Assert.IsNotEmpty(countries, QueryNoResultsMessage(query: "GetCountries"));
        }

        [TestCase]
        public void GetCountries_NoQueryParams_Result()
        {
            var result = service.GetCountries().First();
            ValidateCountry(result);
        }

        [TestCase]
        public void GetCountries_QueryParams_Empty()
        {
            var countries = service.GetCountries(new QueryParams());
            Assert.IsNotEmpty(countries, QueryNoResultsMessage(query: "GetCountries"));
        }

        [TestCase]
        public void GetCountries_QueryParamRangeValdiation(
            [Values(null,Sort.Ascending,Sort.Descending)] Sort sort,
            [Values(null,country_name,country_goldMedals,country_silverMedals,country_bronzeMedals,country_totalMedals)] String sortProperty,
            [Values(null,-1,0,1,Int32.MinValue,Int32.MaxValue)] int start,
            [Values(null,-1,0,1,Int32.MinValue,Int32.MaxValue)] int count
            )
        {
            var countries = service.GetCountries(new QueryParams 
            {
                Count = count,
                Sort = sort,
                SortProperty = sortProperty,
                Start = start
            });

            Assert.IsNotNull(countries,"Result should always return at least an empty list.");
        }

        [TestCase]
        public void GetCountries_Year_NoQueryParams(
            [Range(expectedYearMin,expectedYearMax,expectedYearStep)] int year)
        {
            var countries = service.GetCountries(year);
            Assert.IsNotEmpty(countries, "Result should always return at least one result.");
        }

        [TestCase]
        public void GetCountry_ValidateResultType()
        {
            var countryName = service.GetCountries().Select(c => c.Name).FirstOrDefault();
            var country = service.GetCountry(countryName);
            ValidateCountry(country);
        }

        [TestCase]
        public void GetCountry_ValidateAllCountries()
        {
            var countryNames = service.GetCountries().Select(c => c.Name);
            foreach(var countryName in countryNames)
                Assert.IsNotNull(service.GetCountry(countryName), IsNullMessage(countryName));
        }

        [TestCase]
        public void GetCountry_ValidateResultType(
            [Range(expectedYearMin,expectedYearMax,expectedYearStep)] int year)
        {
            var country = service.GetCountry(year, expectedCountry);
            ValidateCountry(country);
        }


        #endregion

        #region Athlete Tests

        [TestCase]
        public void ValidateAssumedAtheleteFields()
        {
            var athleteType = typeof(IAthlete);

            var fields = new[] 
            { 
               athlete_id,
                athlete_name,
                athlete_age,
                athlete_country,
                athlete_year,
                athlete_CeremonyData,
                athlete_Sport,
                athlete_goldMedals,
                athlete_silverMedals,
                athlete_bronzeMedals,
                athlete_totalMedals
            };
            Assert.IsTrue(TypeContainsOnlyProperties(athleteType, fields));
        }

        [TestCase]
        public void ValidateAthletesField(
            [Values(
                athlete_id,
                athlete_name,
                athlete_age,
                athlete_country,
                athlete_year,
                athlete_CeremonyData,
                athlete_Sport,
                athlete_goldMedals,
                athlete_silverMedals,
                athlete_bronzeMedals,
                athlete_totalMedals)
            ] string field
            )
        {

            Assert.IsTrue(field == null || TypeContainsProperty(typeof(IAthlete), field));
        }

        [TestCase]
        public void GetAthletes_NoQueryParams()
        {
            var athletes = service.GetAthletes();
            Assert.IsNotEmpty(athletes, QueryNoResultsMessage(query: "GetAthletes"));
        }

        [TestCase]
        public void GetAthletes_NoQueryParams_Result()
        {
            var result = service.GetAthletes().First();
            ValidateAthlete(result);
        }

        [TestCase]
        public void GetAthletes_QueryParams_Empty()
        {
            var countries = service.GetAthletes(new QueryParams());
            Assert.IsNotEmpty(countries, QueryNoResultsMessage(query: "GetAthletes"));
        }

        [TestCase]
        public void GetAthletes_QueryParamRangeValdiation(
            [Values(null, Sort.Ascending, Sort.Descending)] Sort sort,
            [Values(null,
                athlete_id,
                athlete_name,
                athlete_age,
                athlete_country,
                athlete_year,
                athlete_CeremonyData,
                athlete_Sport,
                athlete_goldMedals,
                athlete_silverMedals,
                athlete_bronzeMedals,
                athlete_totalMedals)] String sortProperty,
            [Values(null, -1, 0, 1, Int32.MinValue, Int32.MaxValue)] int start,
            [Values(null, -1, 0, 1, Int32.MinValue, Int32.MaxValue)] int count
            )
        {
            var athletes = service.GetAthletes(new QueryParams
            {
                Count = count,
                Sort = sort,
                SortProperty = sortProperty,
                Start = start
            });

            Assert.IsNotNull(athletes, "Result should always return at least an empty list.");
        }

        [TestCase]
        public void GetAthletes_Year_NoQueryParams(
            [Range(expectedYearMin, expectedYearMax, expectedYearStep)] int year)
        {
            var countries = service.GetAthletes(year);
            Assert.IsNotEmpty(countries, "Result should always return at least one result.");
        }

        [TestCase]
        public void GetAthletes_CountryName_NoQueryParams()
        {
            var countryNames = service.GetCountries().Select(c => c.Name);
            var athletes = default(List<IAthlete>);
            foreach (var countryName in countryNames)
            {
                athletes = service.GetAthletes(countryName);
                Assert.IsNotEmpty(athletes, "Result should always return at least one result.");
            }
            Assert.IsNotEmpty(athletes, "Result should always return at least one result.");
        }

        [TestCase]
        public void GetAthletes_Year_CountryName_NoQueryParams(
            [Range(expectedYearMin, expectedYearMax, expectedYearStep)] int year)
        {
            var countryNames = service.GetCountries().Select(c => c.Name);
            var athletes = default(List<IAthlete>);
             athletes = service.GetAthletes(year,expectedCountry);
            Assert.IsNotEmpty(athletes, "Result should always return at least one result.");
        }


        [TestCase]
        public void GetAthletes_Year_QueryParams(
            [Range(expectedYearMin, expectedYearMax, expectedYearStep)] int year)
        {
            var countries = service.GetAthletes(year, 
                new QueryParams 
                { 
                    Count = 1, 
                    Start = 1, 
                    SortProperty = athlete_name,
                    Sort = Sort.Ascending
                });
            Assert.IsNotEmpty(countries, "Result should always return at least one result.");
        }

        [TestCase]
        public void GetCountries_CountryName_QueryParams()
        {
            var countryNames = service.GetCountries().Select(c => c.Name);
            var athletes = default(List<IAthlete>);
            foreach (var countryName in countryNames)
            {
                athletes = service.GetAthletes(countryName,
                new QueryParams
                {
                    Count = 1,
                    Start = 0,
                    SortProperty = athlete_name,
                    Sort = Sort.Ascending
                });
                Assert.IsNotEmpty(athletes, "Result should always return at least one result.");
            }
            Assert.IsNotEmpty(athletes, "Result should always return at least one result.");
        }

        [TestCase]
        public void GetCountries_Year_CountryName_QueryParams(
            [Range(expectedYearMin, expectedYearMax, expectedYearStep)] int year)
        {
            var countryNames = service.GetCountries().Select(c => c.Name);
            var athletes = default(List<IAthlete>);
            athletes = service.GetAthletes(year, expectedCountry,
                new QueryParams
                {
                    Count = 1,
                    Start = 1,
                    SortProperty = athlete_name,
                    Sort = Sort.Ascending
                });
            Assert.IsNotEmpty(athletes, "Result should always return at least one result.");
        }


        [TestCase]
        public void GetAthlete_ValidateResultType()
        {
            var athletesIds = service.GetAthletes().Select(a => a.Id).FirstOrDefault();
            var athlete = service.GetAlthlete(athletesIds);
            ValidateAthlete(athlete);
        }

        [TestCase]
        public void GetAthlete_ValidateAllCountries()
        {
            var athletesId = service.GetAthletes().Select(a => a.Id).First();
            Assert.IsNotNull(service.GetAlthlete(athletesId), IsNullMessage(athletesId));
        }

        [TestCase]
        public void GetAthlete_ValidateResultType(
            [Range(expectedYearMin, expectedYearMax, expectedYearStep)] int year)
        {
            var foundAthlete = service.GetAthletes().First();
            var athlete = service.GetAlthlete(foundAthlete.Year, foundAthlete.Id);
            ValidateAthlete(athlete);
        }



        #endregion

        #region Sport Tests

        [TestCase]
        public void ValidateAssumedSportFields()
        {
            var sportType = typeof(ISportMedal);

            var fields = new[] 
            { 
                sport_sport,
                sport_goldMedals,
                sport_silverMedals,
                sport_bronzeMedals,
                sport_totalMedals
            };
            Assert.IsTrue(TypeContainsOnlyProperties(sportType, fields));
        }

        [TestCase]
        public void ValidateSportField(
            [Values(
                sport_sport,
                sport_goldMedals,
                sport_silverMedals,
                sport_bronzeMedals,
                sport_totalMedals)
            ] string field
            )
        {

            Assert.IsTrue(field == null || TypeContainsProperty(typeof(ISportMedal), field));
        }


        [TestCase]
        public void GetSportMedals_Year(
            [Range(expectedYearMin, expectedYearMax, expectedYearStep)] int year,
            object o
            )
        {
            var sportMedals = service.GetSportMedals(year);
            Assert.IsNotEmpty(sportMedals, "Result should always return at least one result.");
        }

        [TestCase]
        public void GetSportMedals_CountryName()
        {
            var countryNames = service.GetCountries().Select(c => c.Name);
            var sportMedals = default(List<ISportMedal>);
            foreach (var countryName in countryNames)
            {
                sportMedals = service.GetSportMedals(countryName);
                Assert.IsNotEmpty(sportMedals, "Result should always return at least one result.");
            }
            Assert.IsNotEmpty(sportMedals, "Result should always return at least one result.");
        }

        [TestCase]
        public void GetSportMedals_Year_CountryName(
            [Range(expectedYearMin, expectedYearMax, expectedYearStep)] int year)
        {
            var sportMedals = default(List<ISportMedal>);
            sportMedals = service.GetSportMedals(expectedCountry, year);
            Assert.IsNotEmpty(sportMedals, "Result should always return at least one result.");
        }


        #endregion
    }


}
