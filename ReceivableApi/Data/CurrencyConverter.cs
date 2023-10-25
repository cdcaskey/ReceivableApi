using ReceivableApi.Models;

namespace ReceivableApi.Data
{
    public class CurrencyConverter
    {
        private readonly Currency[] currencies;

        public CurrencyConverter(CurrencyLoader currencyLoader)
        {
            currencies = currencyLoader.Load();
        }


        public decimal Convert(string from, string to, decimal amount)
        {
            var startingCurrency = currencies.FirstOrDefault(x => x.Code.Equals(from, StringComparison.InvariantCultureIgnoreCase));
            if (startingCurrency == null)
            {
                throw new ArgumentException($"Currency {from} does not exist in database", nameof(from));
            }

            var targetCurrency = currencies.FirstOrDefault(x => x.Code.Equals(to, StringComparison.InvariantCultureIgnoreCase));
            if (targetCurrency == null)
            {
                throw new ArgumentException($"Currency {to} does not exist in database", nameof(to));
            }

            // This code is to show that a conversion is being attempted
            // It is not accurate and would in reality point to an external service
            var result = amount / (1 + (decimal)Array.IndexOf(currencies, startingCurrency) / currencies.Length) * (1 + (decimal)Array.IndexOf(currencies, targetCurrency) / currencies.Length);

            return Math.Round(result, 2);
        }
    }
}
