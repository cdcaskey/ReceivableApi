using Microsoft.Extensions.Logging.Abstractions;
using ReceivableApi.Data;
using ReceivableApi.Models;
using ReceivableApi.Tests.Fakes;
using Shouldly;
using static ReceivableApi.Tests.Fakes.FakeCurrencyFileLoader;

namespace ReceivableApi.Tests.Unit.Data
{
    [TestFixture]
    public class CurrencyLoaderTests
    {
        [TestCase(LoadType.EmptyJson)]
        [TestCase(LoadType.MalformedJson)]
        public void Load_WhenNoCountriesLoaded_ThrowsException(LoadType loadType)
        {
            // Arrange
            var loader = new FakeCurrencyFileLoader(loadType);
            var sut = CreateSut(loader);

            // Act
            Should.Throw<Exception>(() => sut.Load());
        }

        [Test]
        public void Load_WhenValidCountriesLoaded_ReturnsCountries()
        {
            // Arrange
            var loader = new FakeCurrencyFileLoader(LoadType.ValidJson);
            var sut = CreateSut(loader);

            // Act
            var result = sut.Load();

            // Assert
            var expected = new[]
            {
                new Currency { Name = "Afghan Afghani", Code = "AFN" },
                new Currency { Name = "Euro", Code = "EUR" },
                new Currency { Name = "Albanian Lek", Code = "ALL" },
                new Currency { Name = "Algerian Dinar", Code = "DZD" },
                new Currency { Name = "United States Dollar", Code = "USD" }
            };

            result.ShouldBe(expected);
        }

        private CurrencyLoader CreateSut(FakeCurrencyFileLoader fileLoader) => new(NullLogger<CurrencyLoader>.Instance, fileLoader);
    }
}
