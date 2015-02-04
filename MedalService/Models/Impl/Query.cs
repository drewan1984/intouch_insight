using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedalService.Models.Impl
{
    public class QueryParams : IQueryParams
    {
        public Sort? Sort
        {
            get;
            set;
        }

        public string SortProperty
        {
            get;
            set;
        }

        public int? Start
        {
            get;
            set;
        }

        public int? Count
        {
            get;
            set;
        }
    }

}
