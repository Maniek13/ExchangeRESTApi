using ExchangeRESTApi.Classes;
using ExchangeRESTApi.Interfaces;
using ExchangeRESTApi.Objects;
using ExchangeRESTApi.Tests;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace ExchangeRESTApi.Controllers
{
    [Produces("application/json")]
    public class AppController : Controller, IAppController
    {
        public AppController()
        {
        }

        public string Index()
        {
            return "Welcome to exchange rest api";
        }

        public JsonResult GetNewAppKey()
        {
            return AppConfig.AppKey switch
            {
                "1" => Json(new { AppKey = AppKey(new AppKey()) }),
                _ => Json(new { AppKey = "Set AppKey" }),
            };
        }

        [HttpPost]
        public JsonResult GetExchangeValue([Bind("CurrencyCodes, StartDate, EndDate, ApiKey")] Request request)
        {
            JsonResult result;

            // set test
            SpeedTests speedTests = new SpeedTests("GetExchangeValue");
            speedTests.SetTime();

            try
            {
                result = AppConfig.ExchangeValues switch
                {
                    "1" => Json(RequestData(new ExchangeValues(), request)),
                    "2" => Json(RequestData(new ExchangeElasticSearch(), request)),
                    _ => Json(new { ExchangeValues = "Set one" }),
                };
            }
            catch(ArgumentOutOfRangeException e)
            {
                result = Json(new { error = e.ParamName });
            }

            //get data
            speedTests.GetResult();

            return result;
        }

        private string AppKey(IAppKey appKey)
        {
            appKey.SetNewKey();
            return appKey.GetKey();
        }

        private HashSet<AppCourse> RequestData(IExchangeValues exchangeValues, Request request)
        {
            try
            {
                return exchangeValues.Get(request.CurrencyCodes, request.StartDate, request.EndDate, request.ApiKey);
            }
            catch (ArgumentOutOfRangeException e)
            {
                throw new ArgumentOutOfRangeException(e.ParamName);
            }
        }
    }
}
