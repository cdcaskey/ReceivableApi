using Newtonsoft.Json;

namespace ReceivableApi.Models
{
    public class Country
    {
        public string Name { get; init; } = string.Empty;

        [JsonProperty("alpha-2", Required = Required.Always)]
        public string Alpha2Code { get; init; } = string.Empty;

        public override bool Equals(object? obj) => obj is Country country && Name == country.Name && Alpha2Code == country.Alpha2Code;

        public override int GetHashCode() => HashCode.Combine(Name, Alpha2Code);
    }
}
