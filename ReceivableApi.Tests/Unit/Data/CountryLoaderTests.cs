using Microsoft.Extensions.Logging.Abstractions;
using ReceivableApi.Data;
using ReceivableApi.Models;
using ReceivableApi.Tests.Fakes;
using Shouldly;
using static ReceivableApi.Tests.Fakes.FakeCountryFileLoader;

namespace ReceivableApi.Tests.Unit.Data
{
    [TestFixture]
    public class CountryLoaderTests
    {
        [TestCase(LoadType.EmptyJson)]
        [TestCase(LoadType.MalformedJson)]
        public void Load_WhenNoCountriesLoaded_ThrowsException(LoadType loadType)
        {
            // Arrange
            var loader = new FakeCountryFileLoader(loadType);
            var sut = CreateSut(loader);

            // Act
            Should.Throw<Exception>(() => sut.Load());
        }

        [Test]
        public void Load_WhenValidCountriesLoaded_ReturnsCountries()
        {
            // Arrange
            var loader = new FakeCountryFileLoader(LoadType.ValidJson);
            var sut = CreateSut(loader);

            // Act
            var result = sut.Load();

            // Assert
            var expected = new[]
            {
                new Country { Name = "Afghanistan", Alpha2Code = "AF" },
                new Country { Name = "Åland Islands", Alpha2Code = "AX" },
                new Country { Name = "Albania", Alpha2Code = "AL" },
                new Country { Name = "Algeria", Alpha2Code = "DZ" },
                new Country { Name = "American Samoa", Alpha2Code = "AS" }
            };

            result.ShouldBe(expected);
        }

        private CountryLoader CreateSut(FakeCountryFileLoader fileLoader) => new(NullLogger<CountryLoader>.Instance, fileLoader);
    }
}
