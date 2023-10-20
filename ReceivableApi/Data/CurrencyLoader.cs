using Newtonsoft.Json.Linq;
using ReceivableApi.Models;

namespace ReceivableApi.Data
{
    public class CurrencyLoader
    {
        private const string CurrencyFileLocation = "Data\\currencies.json";
        private readonly ILogger<CurrencyLoader> logger;
        private readonly IJsonFileLoader fileLoader;

        public CurrencyLoader(ILogger<CurrencyLoader> logger, IJsonFileLoader fileLoader)
        {
            this.logger = logger;
            this.fileLoader = fileLoader;
        }

        public Currency[] Load()
        {
            JToken CurrencyJson;
            Currency[]? currencies = Array.Empty<Currency>();

            try
            {
                CurrencyJson = fileLoader.LoadFile(CurrencyFileLocation);
                currencies = CurrencyJson.ToObject<Currency[]>();
            }
            catch (Exception e)
            {
                logger.LogCritical(e, "Unable to load currencies using JSON from '{fileLocation}'", CurrencyFileLocation);
                throw;
            }

            if (currencies == null || currencies.Length == 0)
            {
                logger.LogCritical("The file '{fileLocation}' did not contain any currencies", CurrencyFileLocation);
                throw new Exception();
            }

            return currencies;
        }
    }
}
