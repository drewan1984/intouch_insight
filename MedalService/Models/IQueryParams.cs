using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedalService.Models
{
    public enum Sort { Ascending ,Descending }

    /// <summary>
    /// Query parameters for data query.
    /// </summary>
    public interface IQueryParams
    {
        /// <summary>
        /// Sort order of property being sorted. (Ascending,Descending)
        /// </summary>
        Sort? Sort { get; set; }

        /// <summary>
        /// Property to sort.
        /// </summary>
        String SortProperty { get; set; }

        /// <summary>
        /// Index offset of data.
        /// </summary>
        Int32? Start { get; set; }

        /// <summary>
        /// Amount of results returned.
        /// </summary>
        Int32? Count { get; set; }
        
    }
}
