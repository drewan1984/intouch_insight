using MedalService.Models;
using MedalService.Models.Impl;
using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedalService.Util
{
    public static class NancyRequestExtensionMethods
    {

        public static IQueryParams GetQueryParams(this Request request)
        {
            dynamic count = request.Query["count"];
            dynamic start = request.Query["start"];
            dynamic sort = request.Query["sort"];
            dynamic sortProperty = request.Query["sortIndex"];

            Sort? _sort = null;
            if (sort != null)
                _sort = (sort == "AS") ? Sort.Ascending : Sort.Descending;

            return new QueryParams 
            {
                Count = count != null ? count : null,
                Sort = (_sort != null) ? _sort : null,
                SortProperty = sortProperty != null ? sortProperty : null,
                Start = start != null ? start : null
            };
        }
    }
}
