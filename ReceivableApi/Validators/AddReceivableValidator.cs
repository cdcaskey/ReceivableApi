using System.Globalization;
using FluentValidation;
using ReceivableApi.Data;
using ReceivableApi.Models;
using ReceivableApi.Models.Requests;

namespace ReceivableApi.Validators
{
    public class AddReceivableValidator : AbstractValidator<AddReceivable>
    {
        private readonly Country[] countries;
        private readonly Currency[] currencies;
        private readonly Func<DateTime> now;

        public AddReceivableValidator(CountryLoader countryLoader, CurrencyLoader currencyLoader, Func<DateTime> now)
        {
            countries = countryLoader.Load();
            currencies = currencyLoader.Load();
            this.now = now;

            RuleFor(x => x.CurrencyCode).Must(BeAValidCurrencyCode);
            RuleFor(x => x.IssueDate).Must(BeIsoDateInPast);
            RuleFor(x => x.OpeningValue).GreaterThan(0);
            RuleFor(x => x.PaidValue).GreaterThanOrEqualTo(0);
            RuleFor(x => x.DueDate).Must(BeIsoDate);
            RuleFor(x => x.ClosedDate).Must(BeNullOrIsoDateInPast);
            RuleFor(x => x.DebtorCountryCode).Must(BeAValidCountryCode);
        }

        private bool BeAValidCurrencyCode(string currencyCode)
            => currencies.Any(x => x.Code.Equals(currencyCode, StringComparison.InvariantCultureIgnoreCase));

        private bool BeAValidCountryCode(string countryCode)
            => countries.Any(x => x.Alpha2Code.Equals(countryCode, StringComparison.InvariantCultureIgnoreCase));

        private bool BeIsoDateInPast(string date)
        {
            if (!IsValidIsoDate(date, out var parsedDate))
            {
                return false;
            }

            return parsedDate.CompareTo(now()) <= 0;
        }

        private bool BeNullOrIsoDateInPast(string? date)
            => date == null || BeIsoDateInPast(date);

        private bool BeIsoDate(string date)
            => IsValidIsoDate(date, out _);

        private bool IsValidIsoDate(string date, out DateTime parsedDate)
        {
            if (DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
            {
                return true;
            }

            if (DateTime.TryParseExact(date, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
            {
                return true;
            }

            return false;
        }
    }
}
