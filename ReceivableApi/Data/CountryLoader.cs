using Newtonsoft.Json.Linq;
using ReceivableApi.Models;

namespace ReceivableApi.Data
{
    public class CountryLoader
    {
        private const string CountryFileLocation = "Data\\countries.json";
        private readonly ILogger<CountryLoader> logger;
        private readonly IJsonFileLoader fileLoader;

        public CountryLoader(ILogger<CountryLoader> logger, IJsonFileLoader fileLoader)
        {
            this.logger = logger;
            this.fileLoader = fileLoader;
        }

        public Country[] Load()
        {
            JToken countryJson;
            Country[]? countries = Array.Empty<Country>();

            try
            {
                countryJson = fileLoader.LoadFile(CountryFileLocation);
                countries = countryJson.ToObject<Country[]>();
            }
            catch (Exception e)
            {
                logger.LogCritical(e, "Unable to load countries using JSON from '{fileLocation}'", CountryFileLocation);
                throw;
            }

            if (countries == null || countries.Length == 0)
            {
                logger.LogCritical("The file '{fileLocation}' did not contain any countries", CountryFileLocation);
                throw new Exception();
            }

            return countries;
        }
    }
}
