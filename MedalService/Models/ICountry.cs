using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedalService.Models
{
    /// <summary>
    /// Country Medal Details.
    /// </summary>
    public interface ICountry
    {

        /// <summary>
        /// Coutry Medal Name
        /// </summary>
        String Name { get; }

        /// <summary>
        /// Gold medals.
        /// </summary>
        int GoldMedals { get; }

        /// <summary>
        /// Silver medals.
        /// </summary>
        int SilverMedals { get; }

        /// <summary>
        /// Bronze medals.
        /// </summary>
        int BronzeMedals { get; }

        /// <summary>
        /// Total medals.
        /// </summary>
        int TotalMedals { get; }
    }
}
