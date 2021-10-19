using ExchangeRESTApi.BasesClasses;
using ExchangeRESTApi.Interfaces;

namespace ExchangeRESTApi.Classes
{
    public class AppKey : BaseAppKey, IAppKey
    {
        public string GetKey()
        {
            return AppConfig.SecretKey;
        }

        public void SetNewKey()
        {
            AppConfig.SecretKey = GenerateKey();
        }
    }
}
