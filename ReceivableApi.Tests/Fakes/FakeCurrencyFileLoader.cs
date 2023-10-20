using Newtonsoft.Json.Linq;
using ReceivableApi.Data;

namespace ReceivableApi.Tests.Fakes
{
    public class FakeCurrencyFileLoader : IJsonFileLoader
    {
        public const string EmptyJsonArray = "[]";
        public const string MalformedJson = "[{{]";
        public const string ValidCurrencyJson = @"[
    { ""name"": ""Afghan Afghani"", ""code"": ""AFN"" },
    { ""name"": ""Euro"", ""code"": ""EUR"" },
    { ""name"": ""Albanian Lek"", ""code"": ""ALL"" },
    { ""name"": ""Algerian Dinar"", ""code"": ""DZD"" },
    { ""name"": ""United States Dollar"", ""code"": ""USD"" }
]";

        private readonly string loadedJson;

        public FakeCurrencyFileLoader(LoadType loadType)
        {
            loadedJson = loadType switch
            {
                LoadType.EmptyJson => EmptyJsonArray,
                LoadType.MalformedJson => MalformedJson,
                _ => ValidCurrencyJson
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
