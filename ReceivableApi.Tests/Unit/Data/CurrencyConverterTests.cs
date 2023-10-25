using Microsoft.Extensions.Logging.Abstractions;
using ReceivableApi.Data;
using ReceivableApi.Tests.Fakes;
using Shouldly;
using static ReceivableApi.Tests.Fakes.FakeCurrencyFileLoader;

namespace ReceivableApi.Tests.Unit.Data
{
    [TestFixture]
    public class CurrencyConverterTests
    {
        [Test]
        public void Convert_WhenCalledWithInvalidFromCurrency_ThrowsArgumentException()
        {
            // Arrange
            var sut = CreateSut();

            // Act/Assert
            Should.Throw<ArgumentException>(() => sut.Convert("Not a currency", "USD", 0));
        }

        [Test]
        public void Convert_WhenCalledWithInvalidToCurrency_ThrowsArgumentException()
        {
            // Arrange
            var sut = CreateSut();

            // Act/Assert
            Should.Throw<ArgumentException>(() => sut.Convert("USD", "Not a currency", 0));
        }

        [TestCase("AFN", "USD", 1.8)]
        [TestCase("USD", "USD", 1)]
        [TestCase("EUR", "USD", 1.5)]
        [TestCase("DZD", "EUR", 0.75)]
        public void Convert_WhenCalledWithValidCurrencies_ConvertsCorrectly(string from, string to, decimal expectedResult)
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Convert(from, to, 1);

            // Assert
            result.ShouldBe(expectedResult);
        }

        [TestCase("USD", "AFN", 0.56)]
        [TestCase("EUR", "ALL", 1.17)]
        [TestCase("EUR", "AFN", 0.83)]
        public void Convert_WhenResultIsRecurringNumber_RoundsCorrectly(string from, string to, decimal expectedResult)
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Convert(from, to, 1);

            // Assert
            result.ShouldBe(expectedResult);
        }

        private CurrencyConverter CreateSut() => new(new CurrencyLoader(NullLogger<CurrencyLoader>.Instance, new FakeCurrencyFileLoader(LoadType.ValidJson)));
    }
}
