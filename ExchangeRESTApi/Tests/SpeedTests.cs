using ExchangeRESTApi.BaseClasses;
using ExchangeRESTApi.Interfaces;
using System;

namespace ExchangeRESTApi.Tests
{
    public class SpeedTests : BaseSpeedTests, ISpeedTests
    {
        public SpeedTests(string functionName)
        {
            _functionName = functionName;
        }
        public void SetTime()
        {
            start = DateTime.Now.ToFileTime();
        }

        public void GetResult()
        {
            end = DateTime.Now.ToFileTime();
            double time = (end - start) / Math.Pow(10, 6);
            Console.WriteLine("Time to execute " + _functionName + " is: " + time + "ms");
        }
    }
}
