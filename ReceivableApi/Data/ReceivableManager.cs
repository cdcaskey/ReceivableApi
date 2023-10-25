using ReceivableApi.Models;
using ReceivableApi.Models.Responses;

namespace ReceivableApi.Data
{
    public class ReceivableManager
    {
        private readonly ReceivableApiContext context;
        private readonly CurrencyConverter converter;

        public ReceivableManager(ReceivableApiContext context, CurrencyConverter converter)
        {
            this.context = context;
            this.converter = converter;
        }

        public async Task StoreReceivable(Receivable receivable)
        {
            var existingDebtor = await context.Debtors.FindAsync(receivable.DebtorId);
            if (existingDebtor == null)
            {
                if (receivable.Debtor == null)
                {
                    throw new ArgumentException("Debtor must either be specified or have an existing entry in the system");
                }

                context.Debtors.Add(receivable.Debtor);
            }
            else
            {
                receivable.Debtor = existingDebtor;
            }

            var existingReceivable = await context.Receivables.FindAsync(receivable.Reference);
            if (existingReceivable == null)
            {
                context.Receivables.Add(receivable);
            }
            else
            {
                existingReceivable.PaidValue = receivable.PaidValue;
                existingReceivable.Cancelled = receivable.Cancelled;

                if (receivable.ClosedDate != null)
                {
                    existingReceivable.ClosedDate = receivable.ClosedDate;
                }
            }

            await context.SaveChangesAsync();
        }

        public ReceivableSummary GetSummary(string targetCurrency)
        {
            var summary = new ReceivableSummary()
            {
                CurrencyUsed = targetCurrency
            };

            foreach (var receivable in context.Receivables)
            {
                if (receivable.Closed)
                {
                    summary.ClosedReceivables++;

                    var convertedValue = converter.Convert(receivable.CurrencyCode, targetCurrency, receivable.PaidValue);
                    summary.TotalClosedReceivableValue += convertedValue;
                }
                else
                {
                    summary.OpenReceivables++;

                    var convertedValue = converter.Convert(receivable.CurrencyCode, targetCurrency, receivable.OpeningValue - receivable.PaidValue);
                    summary.TotalOpenReceivableValue += convertedValue;
                }
            }

            return summary;
        }
    }
}
