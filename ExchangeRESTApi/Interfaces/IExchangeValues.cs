using ExchangeRESTApi.Objects;
using System;
using System.Collections.Generic;

namespace ExchangeRESTApi.Interfaces
{
    interface IExchangeValues
    {
        public void Set(DateTime startDate, DateTime endDate);
        public void SetFromDb(DateTime startDate, DateTime endDate);
        public HashSet<AppCourse> Get(Dictionary<string, string> currencyCodes, DateTime startDate, DateTime endDate, string apiKey);
    }
}
