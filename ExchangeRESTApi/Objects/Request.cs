using ExchangeRESTApi.Interfaces;
using System;
using System.Collections.Generic;

namespace ExchangeRESTApi.Objects
{
    public class Request : IRequest
    {
        public Dictionary<string, string> CurrencyCodes { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ApiKey { get; set; }
    }
}
