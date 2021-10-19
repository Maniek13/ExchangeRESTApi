using System;

namespace ExchangeRESTApi.Interfaces
{
    interface ICourse
    {
        public DateTime Date { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public double Rate { get; set; }
    }
}
