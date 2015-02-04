using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedalService.Models
{
    /// <summary>
    /// Reperesents medal distribution by sport.
    /// </summary>
    public interface ISportMedal
    {
        /// <summary>
        /// Sport being represented.
        /// </summary>
        String Sport { get; }

        /// <summary>
        /// Gold Medals.
        /// </summary>
        int GoldMedals { get; }

        /// <summary>
        /// Silver Medals.
        /// </summary>
        int SilverMedals { get; }

        /// <summary>
        /// Bronze Medals.
        /// </summary>
        int BronzeMedals { get; }

        /// <summary>
        /// Total Medals.
        /// </summary>
        int TotalMedals { get; }
    }
}
