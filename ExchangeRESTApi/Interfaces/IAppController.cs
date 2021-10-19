using ExchangeRESTApi.Objects;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRESTApi.Interfaces
{
    interface IAppController
    {
        public string Index();

        public JsonResult GetNewAppKey();

        [HttpPost]
        public JsonResult GetExchangeValue([Bind("currencyCodes, startDate, endDate, apiKey")] Request request);
    }
}
