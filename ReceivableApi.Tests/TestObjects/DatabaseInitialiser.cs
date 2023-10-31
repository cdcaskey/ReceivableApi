using Microsoft.EntityFrameworkCore;
using ReceivableApi.Data;
using ReceivableApi.Models;

namespace ReceivableApi.Tests.Fakes
{
    public class DatabaseInitialiser
    {
        private const string connectionString = "Data Source=TestReceivablesDatabase.db";

        private static readonly object dbLock = new();
        private static bool databaseInitialised;

        public DatabaseInitialiser(bool forceDelete = false)
        {
            lock (dbLock)
            {
                if (forceDelete || !databaseInitialised)
                {
                    using var context = CreateContext();

                    context.Database.EnsureDeleted();
                    context.Database.Migrate();

                    AddSeedData(context);

                    databaseInitialised = true;
                }
            }
        }

        public ReceivableApiContext CreateContext() => new(new DbContextOptionsBuilder<ReceivableApiContext>()
                                                               .UseSqlite(connectionString)
                                                               .Options);

        private void AddSeedData(ReceivableApiContext context)
        {
            var debtors = new List<Debtor>
            {
                new Debtor
                {
                    Name = "Sam Smith",
                    Reference = "DEB-001",
                    CountryCode = "GB",
                    Address1 = "123 Fake Street",
                    Town = "Sheffield",
                    Zip = "S35 1AA"
                },
                new Debtor
                {
                    Name = "Adam Brown",
                    Reference = "1234",
                    CountryCode = "US"
                },
                new Debtor
                {
                    Name = "Matt Young",
                    Reference = "XXX-111-123",
                    CountryCode = "AU",
                    Address1 = "1401 Melbourne Road",
                    Address2 = "Lara",
                    Town = "Geelong",
                    State = "Victoria",
                    Zip = "021948",
                    RegistrationNumber = "Com-1-08"
                }
            };

            context.AddRange(debtors);

            var receivables = new List<Receivable>
            {
                new Receivable
                {
                    Reference = "001",
                    CurrencyCode = "AFN",
                    Issued = new DateTime(2021, 08, 12),
                    OpeningValue = 12345.67M,
                    PaidValue = 0,
                    Due = new DateTime(2028, 12, 01),
                    Debtor = debtors[0],
                    DebtorId = debtors[0].Reference
                },
                new Receivable
                {
                    Reference = "002",
                    CurrencyCode = "USD",
                    Issued = new DateTime(2021, 09, 21),
                    OpeningValue = 654321,
                    PaidValue = 654321,
                    Due = new DateTime(2028, 12, 01),
                    Debtor = debtors[0],
                    DebtorId = debtors[0].Reference
                },
                new Receivable
                {
                    Reference = "003",
                    CurrencyCode = "EUR",
                    Issued = new DateTime(2021, 08, 15),
                    OpeningValue = 10000,
                    PaidValue = 5000,
                    Due = new DateTime(2025, 01, 01),
                    Debtor = debtors[0],
                    DebtorId = debtors[0].Reference
                },
                new Receivable
                {
                    Reference = "A1",
                    CurrencyCode = "ALL",
                    Issued = new DateTime(2021, 08, 12),
                    OpeningValue = 18124,
                    PaidValue = 0,
                    Due = new DateTime(2028, 12, 01),
                    Debtor = debtors[1],
                    DebtorId = debtors[1].Reference,
                    Cancelled = true
                },
                new Receivable
                {
                    Reference = "A16",
                    CurrencyCode = "AFN",
                    Issued = new DateTime(2021, 08, 12),
                    OpeningValue = 150000,
                    PaidValue = 0,
                    Due = new DateTime(2028, 12, 01),
                    Debtor = debtors[1],
                    DebtorId = debtors[1].Reference,
                    ClosedDate = new DateTime(2023, 01, 14)
                },
                new Receivable
                {
                    Reference = "AX-1",
                    CurrencyCode = "USD",
                    Issued = new DateTime(2021, 08, 12),
                    OpeningValue = 160000,
                    PaidValue = 12500,
                    Due = new DateTime(2028, 12, 01),
                    Debtor = debtors[2],
                    DebtorId = debtors[2].Reference
                }
            };

            context.AddRange(receivables);
            context.SaveChanges();
        }
    }
}
