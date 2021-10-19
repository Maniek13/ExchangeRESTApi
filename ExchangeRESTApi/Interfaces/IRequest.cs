using System;
using System.Collections.Generic;

namespace ExchangeRESTApi.Interfaces
{
    interface IRequest
    {
        public Dictionary<string, string> CurrencyCodes { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ApiKey { get; set; }
    }
}
