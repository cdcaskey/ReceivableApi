using System.Management;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using ReceivableApi.Data;
using ReceivableApi.Models;
using ReceivableApi.Tests.Fakes;
using Shouldly;
using static ReceivableApi.Tests.Fakes.FakeCurrencyFileLoader;

namespace ReceivableApi.Tests.Unit.Data
{
    public class ReceivableManagerTests
    {
        private ReceivableApiContext context = default!;

        [SetUp]
        public void Setup()
        {
            var database = new DatabaseInitialiser(true);
            context = database.CreateContext();
        }

        [Test]
        public async Task StoreReceivable_WhenDebtorNotProvidedAndNotInDatabase_ThrowsException()
        {
            // Arrange
            var receivable = new Receivable()
            {
                DebtorId = "Not a Real ID",
                Debtor = null
            };

            var sut = CreateSut();

            // Act/Assert
            await Should.ThrowAsync<ArgumentException>(async () => await sut.StoreReceivable(receivable));
        }

        [Test]
        public async Task StoreReceivable_WhenReceivableIsNotInDatabaseAndDebtorIsNotInDatabase_CreatesBoth()
        {
            var debtor = new Debtor
            {
                Name = "Jeff Jones",
                Reference = "JEFF-123",
                CountryCode = "DE",
            };

            var receivable = new Receivable
            {
                Reference = "J-123",
                CurrencyCode = "EUR",
                Issued = new DateTime(2023, 01, 01),
                OpeningValue = 123,
                PaidValue = 0,
                Due = new DateTime(2023, 01, 02),
                DebtorId = debtor.Reference,
                Debtor = debtor
            }; 
            
            var sut = CreateSut();

            // Act
            await sut.StoreReceivable(receivable);

            // Assert
            var dbDebtor = await context.Debtors.FindAsync(debtor.Reference);
            var dbReceivable = await context.Receivables.FindAsync(receivable.Reference);

            dbDebtor.ShouldNotBeNull();
            dbDebtor!.ShouldSatisfyAllConditions(
                x => x.Name.ShouldBe(debtor.Name),
                x => x.CountryCode.ShouldBe(debtor.CountryCode));

            dbReceivable.ShouldNotBeNull();
            dbReceivable!.ShouldSatisfyAllConditions(
                x => x.CurrencyCode.ShouldBe(receivable.CurrencyCode),
                x => x.Issued.ShouldBe(receivable.Issued),
                x => x.OpeningValue.ShouldBe(receivable.OpeningValue),
                x => x.PaidValue.ShouldBe(receivable.PaidValue),
                x => x.Due.ShouldBe(receivable.Due),
                x => x.DebtorId.ShouldBe(receivable.DebtorId));
        }

        [Test]
        public async Task StoreReceivable_WhenReceivableIsNotInDatabaseAndDebtorIsInDatabase_UsesExistingDebtorData()
        {
            // Arrange
            const string DebtorName = "Jeff Jones";

            var debtor = new Debtor
            {
                Name = DebtorName,
                Reference = "JEFF-123",
                CountryCode = "DE",
            };

            await context.AddAsync(debtor);
            await context.SaveChangesAsync();

            var receivable = new Receivable
            {
                Reference = "J-123",
                CurrencyCode = "EUR",
                Issued = new DateTime(2023, 01, 01),
                OpeningValue = 123,
                PaidValue = 0,
                Due = new DateTime(2023, 01, 02),
                DebtorId = debtor.Reference,
                Debtor = new Debtor
                {
                    Reference = "JEFF-123",
                    Name = "Not Jeff",
                    CountryCode = "FR"
                }
            };

            var sut = CreateSut();

            // Act
            await sut.StoreReceivable(receivable);

            // Assert
            var dbReceivable = await context.Receivables.FindAsync(receivable.Reference);

            dbReceivable.ShouldNotBeNull();
            dbReceivable!.Debtor!.Name.ShouldBe(DebtorName);
        }

        [Test]
        public async Task StoreReceivable_WhenReceivableIsInDatabase_UpdatesExistingEntry()
        {
            // Arrange
            var debtor = new Debtor
            {
                Name = "Jeff Jones",
                Reference = "JEFF-123",
                CountryCode = "DE",
            };

            var receivable = new Receivable
            {
                Reference = "J-123",
                CurrencyCode = "EUR",
                Issued = new DateTime(2023, 01, 01),
                OpeningValue = 123,
                PaidValue = 0,
                Due = new DateTime(2023, 01, 02),
                DebtorId = debtor.Reference,
                Debtor = debtor
            };

            await context.Debtors.AddAsync(debtor);
            await context.Receivables.AddAsync(receivable);
            await context.SaveChangesAsync();

            var newReceivable = new Receivable
            {
                Reference = "J-123",
                CurrencyCode = "GBP",
                Issued = new DateTime(0001, 01, 01),
                OpeningValue = 987654,
                PaidValue = 100,
                Due = new DateTime(2023, 01, 02),
                DebtorId = debtor.Reference,
                Debtor = debtor,
                Cancelled = true,
                ClosedDate = new DateTime(2023, 01, 01)
            };

            var sut = CreateSut();

            // Act
            await sut.StoreReceivable(newReceivable);

            // Assert
            var dbReceivable = await context.Receivables.FindAsync(newReceivable.Reference);

            dbReceivable.ShouldNotBeNull();
            dbReceivable.ShouldSatisfyAllConditions(
                x => x.CurrencyCode.ShouldBe(receivable.CurrencyCode), // EUR
                x => x.Issued.ShouldBe(receivable.Issued), // 2023-01-01
                x => x.OpeningValue.ShouldBe(receivable.OpeningValue), // 123
                x => x.PaidValue.ShouldBe(newReceivable.PaidValue), // 100
                x => x.Cancelled.ShouldBe(newReceivable.Cancelled), // true
                x => x.ClosedDate.ShouldBe(newReceivable.ClosedDate)); // 2023-01-01
        }

        [TestCase("AFN", 111402.49, 363511.67)]
        [TestCase("EUR", 133682.99, 436214)]
        [TestCase("ALL", 155963.49, 508916.33)]
        [TestCase("DZD", 178243.99, 581618.67)]
        [TestCase("USD", 200524.50, 654321)]
        public void GetSummary_WhenCalled_CorrectlyCalculatesValuesInSelectedCurrency(string currency, decimal expectedOpenValue, decimal expectedClosedValue)
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.GetSummary(currency);

            // Assert
            result.ShouldSatisfyAllConditions(
                x => x.CurrencyUsed.ShouldBe(currency),
                x => x.OpenReceivables.ShouldBe(4),
                x => x.TotalOpenReceivableValue.ShouldBe(expectedOpenValue),
                x => x.ClosedReceivables.ShouldBe(2),
                x => x.TotalClosedReceivableValue.ShouldBe(expectedClosedValue));
        }

        private ReceivableManager CreateSut() => new(context, new CurrencyConverter(new CurrencyLoader(NullLogger<CurrencyLoader>.Instance, new FakeCurrencyFileLoader(LoadType.ValidJson))));
    }
}
