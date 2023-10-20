using Newtonsoft.Json.Linq;
using ReceivableApi.Data;

namespace ReceivableApi.Tests.Fakes
{
    public class FakeCountryFileLoader : IJsonFileLoader
    {
        public const string EmptyJsonArray = "[]";
        public const string MalformedJson = "[{{]";
        public const string ValidCountryJson = @"[
    { ""name"": ""Afghanistan"", ""alpha-2"": ""AF"" },
    { ""name"": ""Åland Islands"", ""alpha-2"": ""AX"" },
    { ""name"": ""Albania"", ""alpha-2"": ""AL"" },
    { ""name"": ""Algeria"", ""alpha-2"": ""DZ"" },
    { ""name"": ""American Samoa"", ""alpha-2"": ""AS"" }
]";

        private readonly string loadedJson;

        public FakeCountryFileLoader(LoadType loadType)
        {
            loadedJson = loadType switch
            {
                LoadType.EmptyJson => EmptyJsonArray,
                LoadType.MalformedJson => MalformedJson,
                _ => ValidCountryJson
            };
        }

        public JToken LoadFile(string path) => JToken.Parse(loadedJson);

        public enum LoadType
        {
            EmptyJson,
            MalformedJson,
            ValidJson
        }
    }
}
