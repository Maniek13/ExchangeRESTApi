using ExchangeRESTApi.Interfaces;
using System;

namespace ExchangeRESTApi.Objects
{
    public class AppCourse : ICourse
    {
        public DateTime Date { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public double Rate { get; set; }
    }
}
