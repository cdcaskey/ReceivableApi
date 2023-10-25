using System.Net;
using System.Net.Http.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using ReceivableApi.Data;
using ReceivableApi.Models.Requests;
using ReceivableApi.Tests.Fakes;
using Shouldly;

namespace ReceivableApi.Tests.EndToEnd
{
    [TestFixture]
    public class PostReceivablesTests
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
        public async Task PostReceivables_WithExamplePayload_DoesNotReturn400()
        {
            // Arrange
            var contentString = @"[
  {
    ""reference"": ""string"",
    ""currencyCode"": ""AFN"",
    ""issueDate"": ""2000-01-01"",
    ""openingValue"": 1234.56,
    ""paidValue"": 1234.56,
    ""dueDate"": ""2000-01-01"",
    ""closedDate"": ""2000-01-01"",
    ""cancelled"": true,
    ""debtorName"": ""string"",
    ""debtorReference"": ""string"",
    ""debtorAddress1"": ""string"",
    ""debtorAddress2"": ""string"",
    ""debtorTown"": ""string"",
    ""debtorState"": ""string"", 
    ""debtorZip"": ""string"", 
    ""debtorCountryCode"": ""AF"", 
    ""debtorRegistrationNumber"": ""string"" 
  }
]";

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/Receivables")
            {
                Content = new StringContent(contentString, Encoding.UTF8, "application/json")
            };

            // Act
            var response = await client.SendAsync(request);

            // Assert
            response.StatusCode.ShouldNotBe(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task PostReceivables_WithExamplePayloadWithoutOptionalValues_DoesNotReturn400()
        {
            // Arrange
            var contentString = @"[
  {
    ""reference"": ""string"",
    ""currencyCode"": ""AFN"",
    ""issueDate"": ""2000-01-01"",
    ""openingValue"": 1234.56,
    ""paidValue"": 1234.56,
    ""dueDate"": ""2000-01-01"",
    ""debtorName"": ""string"",
    ""debtorReference"": ""string"",
    ""debtorCountryCode"": ""AF""
  }
]";

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/Receivables")
            {
                Content = new StringContent(contentString, Encoding.UTF8, "application/json")
            };

            // Act
            var response = await client.SendAsync(request);

            // Assert
            response.StatusCode.ShouldNotBe(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task PostReceivables_WhenAllInfoIsAddedAndValid_Returns200Ok()
        {
            // Arrange
            var receivables = new List<AddReceivable>
            {
                new()
                {
                    Reference = "ABC-123",
                    CurrencyCode = "GBP",
                    IssueDate = "2023-01-01",
                    OpeningValue = 123.45M,
                    PaidValue = 0,
                    DueDate = "2024-01-01",
                    ClosedDate = "2023-06-01",
                    Cancelled = false,
                    DebtorName = "Max Jones",
                    DebtorReference = "MJ-01",
                    DebtorAddress1 = "123",
                    DebtorAddress2 = "Main Street",
                    DebtorTown = "Town",
                    DebtorState = "State",
                    DebtorZip = "S1 1AA",
                    DebtorCountryCode = "GB",
                    DebtorRegistrationNumber = "123456"
                }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/Receivables")
            {
                Content = JsonContent.Create(receivables)
            };

            // Act
            var response = await client.SendAsync(request);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Test]
        public async Task PostReceivables_WhenMultipleValidReceivablesSent_Returns200Ok()
        {
            // Arrange
            var receivables = new List<AddReceivable>
            {
                new()
                {
                    Reference = "ABC-123",
                    CurrencyCode = "GBP",
                    IssueDate = "2023-01-01",
                    OpeningValue = 123.45M,
                    PaidValue = 0,
                    DueDate = "2024-01-01",
                    ClosedDate = "2023-06-01",
                    Cancelled = false,
                    DebtorName = "Max Jones",
                    DebtorReference = "MJ-01",
                    DebtorAddress1 = "123",
                    DebtorAddress2 = "Main Street",
                    DebtorTown = "Town",
                    DebtorState = "State",
                    DebtorZip = "S1 1AA",
                    DebtorCountryCode = "GB",
                    DebtorRegistrationNumber = "123456"
                },
                new()
                {
                    Reference = "ABC-124",
                    CurrencyCode = "GBP",
                    IssueDate = "2023-01-01",
                    OpeningValue = 123.45M,
                    PaidValue = 0,
                    DueDate = "2024-01-01",
                    ClosedDate = "2023-06-01",
                    Cancelled = false,
                    DebtorName = "Max Jones",
                    DebtorReference = "MJ-01",
                    DebtorAddress1 = "123",
                    DebtorAddress2 = "Main Street",
                    DebtorTown = "Town",
                    DebtorState = "State",
                    DebtorZip = "S1 1AA",
                    DebtorCountryCode = "GB",
                    DebtorRegistrationNumber = "123456"
                },
                new()
                {
                    Reference = "ABC-125",
                    CurrencyCode = "GBP",
                    IssueDate = "2023-01-01",
                    OpeningValue = 123.45M,
                    PaidValue = 0,
                    DueDate = "2024-01-01",
                    ClosedDate = "2023-06-01",
                    Cancelled = false,
                    DebtorName = "Max Jones",
                    DebtorReference = "MJ-01",
                    DebtorAddress1 = "123",
                    DebtorAddress2 = "Main Street",
                    DebtorTown = "Town",
                    DebtorState = "State",
                    DebtorZip = "S1 1AA",
                    DebtorCountryCode = "GB",
                    DebtorRegistrationNumber = "123456"
                }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/Receivables")
            {
                Content = JsonContent.Create(receivables)
            };

            // Act
            var response = await client.SendAsync(request);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);
        }
    }
}
