using System;
using System.Linq;

namespace ExchangeRESTApi.BasesClasses
{
    public class BaseAppKey
    {
        internal static void SetBaseAppKey(string Key)
        {
            try
            {
                AppConfig.SecretKey = Key;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private protected string GenerateKey()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()?><|[]{}|;:";
            return new string(Enumerable.Repeat(chars, 31)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
