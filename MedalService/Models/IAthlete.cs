using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedalService.Models
{
    public interface IAthlete
    {
        /// <summary>
        /// Unique athlete id which represents athletes medal victory.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Athlete name (first and last)
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The athletes age.
        /// </summary>
        Int32 Age { get; }

        /// <summary>
        /// The athletes home country.
        /// </summary>
        String Country { get; }

        /// <summary>
        /// The year the athlete won the medal.
        /// </summary>
        int Year { get; }

        /// <summary>
        /// The date of the ceremony.
        /// </summary>
        String CeremonyDate { get; }

        /// <summary>
        /// The ceremony the medal was won.
        /// </summary>
        String Sport { get; }

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
