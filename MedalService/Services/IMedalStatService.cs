using MedalService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedalService.Services
{
    /// <summary>
    /// Provides medal count information about Athletes and Countries.
    /// </summary>
    public interface IMedalStatService
    {
        /// <summary>
        /// Get the list of supported years.
        /// </summary>
        /// <returns>A list of supported years.</returns>
        List<int> GetYears();

        /// <summary>
        /// Get the list of supported sports.
        /// </summary>
        /// <returns>A list of supported sports</returns>
        List<string> GetSports();

        /// <summary>
        /// Get Country medal details by Country name.
        /// </summary>
        /// <param name="countryName">The country name.</param>
        /// <returns>Country medal details.</returns>
        ICountry GetCountry(string countryName);

        /// <summary>
        /// Get Country meda details by Country name for a given year.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="countryName">The country name.</param>
        /// <returns>Country medal details.</returns>
        ICountry GetCountry(int year, string countryName);

        /// <summary>
        /// Gets a list of country medal details with optional query parameters. (Sort, Count, Start)
        /// </summary>
        /// <param name="queryParams">Query parametrs for the request.</param>
        /// <returns>List of country medal details by the specified query parameters.</returns>
        List<ICountry> GetCountries(IQueryParams queryParams = null);

        /// <summary>
        /// Gets a list of country medal details for given year with optional query parameters. (Sort, Count, Start)
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="queryParams">Query parameters for the request.</param>
        /// <returns>List of country medals for given year by the specified query parameters.</returns>
        List<ICountry> GetCountries(int year,IQueryParams queryParams = null);
 
        /// <summary>
        /// Gets the medal details for an athlete by their unique id.
        /// </summary>
        /// <param name="id">An unique athelte id provided by the service.</param>
        /// <returns>An athletes medal details.</returns>
        IAthlete GetAlthlete(string id);

        /// <summary>
        /// Gets the medal details for an athlete for a specified year.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="id">An unique athelte id provided by the service.</param>
        /// <returns>An athletes medal details for a specified year.</returns>
        IAthlete GetAlthlete(int year, string id);

        /// <summary>
        /// Gets a list of athletes with optional query parameters. (Sort, Count, Start)
        /// </summary>
        /// <param name="queryParams">Query parameters for the request.</param>
        /// <returns>A list of athlete medal details.</returns>
        List<IAthlete> GetAthletes(IQueryParams queryParams = null);

        /// <summary>
        /// Gets a list of athletes by country with optional query parameters. (Sort, Count, Start)
        /// </summary>
        /// <param name="countryName">The athletes country.</param>
        /// <param name="queryParams">Query parameters for the request.</param>
        /// <returns>A list of athlete medal details by country.</returns>
        List<IAthlete> GetAthletes(string countryName, IQueryParams queryParams = null);

        /// <summary>
        /// Gets a list of athletes by country with optional query parameters. (Sort, Count, Start)
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="queryParams">Query parameters for the request.</param>
        /// <returns>A list of athlete medal details by year.</returns>
        List<IAthlete> GetAthletes(int year, IQueryParams queryParams = null);

        /// <summary>
        /// Gets a list of athletes by country and year with optional query parameters. (Sort, Count, Start)
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="countryName">The athletes country.</param>
        /// <param name="queryParams">Query parameters for the request.</param>
        /// <returns>A list of athlete medal details by country and year.</returns>
        List<IAthlete> GetAthletes(int year, string countryName,IQueryParams queryParams = null);

        /// <summary>
        /// Gets sport medal details by country.
        /// </summary>
        /// <param name="countryName">The country name.</param>
        /// <returns>The sport medal details.</returns>
        List<ISportMedal> GetSportMedals(string countryName);

        /// <summary>
        /// Gets the sport medal details by year. 
        /// </summary>
        /// <param name="year">The year.</param>
        /// <returns>The sport medal details.</returns>
        List<ISportMedal> GetSportMedals(int year);

        /// <summary>
        /// Gets the sport medal details by year and country.
        /// </summary>
        /// <param name="countryName">The country name.</param>
        /// <param name="year">The year.</param>
        /// <returns>The sport medal details.</returns>
        List<ISportMedal> GetSportMedals(string countryName, int year);
    }
}
