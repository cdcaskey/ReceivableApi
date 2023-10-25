namespace ReceivableApi.Models
{
    public class Currency
    {
        public string Name { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public override bool Equals(object? obj) => obj is Currency currency && Name == currency.Name && Code == currency.Code;

        public override int GetHashCode() => HashCode.Combine(Name, Code);
    }
}
