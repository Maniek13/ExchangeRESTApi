using ExchangeRESTApi.Classes;
using System;

namespace ExchangeRESTApi
{
    public class AppConfig
    {
        internal static string SecretKey = "EXCV$%SDAS!@$#@SDFSDGSDR@#@#FDS";
        internal static DateTime StartDate = DateTime.Now.Date;
        internal static readonly DateTime EndData = DateTime.Now.Date;
        internal static readonly string AppKey = "1";
        internal static readonly string ExchangeValues = "1";
        internal static readonly string Index = "exchangecourses";
    }
}
