namespace MedalService
{
    using System;
    using System.Text;
    using System.Linq;
    using System.Collections.Generic;
    using Nancy;
    using MongoDB.Bson;
    using MongoDB.Driver.Linq;
    using MongoDB.Driver;
    using MedalService.Services;
    using MedalService.Util;

    public class Controller : NancyModule
    {
        private IMedalStatService medalStats;


        public Controller(IMedalStatService medalStats)
        {
            this.medalStats = medalStats;

            //Serves up the charts page.
            Get["/"] = x =>
            {
                return View["Charts"];
            };

            //Get All Countries
            Get["/Countries"] = x => 
            { 
                return Response.AsJson(medalStats.GetCountries(Request.GetQueryParams())); 
            };

            //Get Country by name.
            Get["/Countries/{country}/"] = x => 
            {
                dynamic countryName = x.country;
                if (countryName != null)
                    return Response.AsJson(medalStats.GetCountry((string)countryName));
                return new NotFoundResponse();
            };

            //Get Country by name.
            Get["/Countries/{year}/"] = x =>
            {
                dynamic yearValue = x.year;
                if (yearValue != null)
                    return Response.AsJson(medalStats.GetCountries((int)yearValue, Request.GetQueryParams()));
                return new NotFoundResponse();
            };

            //Get all athletes from specified country.
            Get["/Countries/{country}/Athletes"] = x =>
            {
                dynamic countryName = x.country;
                if (countryName != null)
                    return Response.AsJson(medalStats.GetAthletes((string)countryName, Request.GetQueryParams()));
                return new NotFoundResponse();
            };


            //Get all athletes.
            Get["/Athletes"] = x => 
            { 
                return Response.AsJson(medalStats.GetAthletes(Request.GetQueryParams())); 
            };

            //Get all athletes by year.
            Get["/Athletes/{year}/"] = x =>
            {
                dynamic yearValue = x.year;
                if (yearValue != null)
                    return Response.AsJson(medalStats.GetAthletes((int)yearValue, Request.GetQueryParams()));
                return new NotFoundResponse();
            };

            //Get all athletes by country and year.
            Get["/Athletes/{country}/{year}/"] = x =>
            {
                dynamic yearValue = x.year;
                dynamic countryName = x.country;
                if (yearValue != null && countryName != null)
                    return Response.AsJson(medalStats.GetAthletes((int)yearValue, (string)countryName, Request.GetQueryParams()));
                return new NotFoundResponse();
            };



            Get["/Athletes/{id}"] = x => 
            {
                dynamic athletedId = x.Id;
                if (athletedId != null)
                    return Response.AsJson(medalStats.GetAlthlete((string)athletedId));
                return new NotFoundResponse();
            };

            Get["/Sports/{country}/{year}/"] = x => 
            {
                dynamic year = x.year;
                dynamic country = x.country;
                if (year != null && country != null)
                    return Response.AsJson(medalStats.GetSportMedals((string)country, (int)year));

                return new NotFoundResponse();
            };
        }
    
    }
}