using ReceivableApi.Models;

namespace ReceivableApi.Data
{
    public class ReceivableManager
    {
        private readonly ReceivableApiContext context;

        public ReceivableManager(ReceivableApiContext context)
        {
            this.context = context;
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
    }
}
