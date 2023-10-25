using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using ReceivableApi.Data;
using ReceivableApi.Models;
using ReceivableApi.Models.Requests;
using ReceivableApi.Models.Responses;
using ReceivableApi.Tests.Fakes;
using Shouldly;

namespace ReceivableApi.Tests.EndToEnd
{
    [TestFixture]
    public class GetSummaryTests
    {
        private WebApplicationFactory<Program> application = default!;
        private HttpClient client = default!;

        [SetUp]
        public void Setup()
        {
            var databaseInitialiser = new DatabaseInitialiser();

            application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.AddScoped<ReceivableApiContext>(x => databaseInitialiser.CreateContext());
                    });
                });

            client = application.CreateClient();
        }

        [Test]
        public async Task GetSummary_WhenNoCurrencySelected_ReturnsValuesInUSD()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/Receivables/Summary");

            // Act
            var response = await client.SendAsync(request);
            var summary = await response.Content.ReadFromJsonAsync<ReceivableSummary>();

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            summary.ShouldNotBeNull();
            summary.CurrencyUsed.ShouldBe("USD");
        }

        [TestCase("AFN")]
        [TestCase("EUR")]
        public async Task GetSummary_WhenCurrencySelected_ReturnsValuesInSelectedCurrency(string currency)
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/Receivables/Summary/{currency}");

            // Act
            var response = await client.SendAsync(request);
            var summary = await response.Content.ReadFromJsonAsync<ReceivableSummary>();

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
            summary.ShouldNotBeNull();
            summary.CurrencyUsed.ShouldBe(currency);
        }

        [Test]
        public async Task GetSummary_WhenCurrencySelectedDoesNotExist_ReturnsBadRequest()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/Receivables/Summary/NOT_CURRENCY");

            // Act
            var response = await client.SendAsync(request);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }
    }
}
