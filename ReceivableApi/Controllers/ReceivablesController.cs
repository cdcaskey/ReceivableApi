using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReceivableApi.Data;
using ReceivableApi.Models;
using ReceivableApi.Models.Requests;
using ReceivableApi.Models.Responses;

namespace ReceivableApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceivablesController : ControllerBase
    {
        private const string DefaultCurrency = "USD";
        private readonly ReceivableApiContext context;
        private readonly Country[] countries;
        private readonly Currency[] currencies;

        public ReceivablesController(ReceivableApiContext context, CountryLoader countryLoader, CurrencyLoader currencyLoader)
        {
            this.context = context;
            countries = countryLoader.Load();
            currencies = currencyLoader.Load();
        }

        [HttpPost]
        public IActionResult PostReceivables(List<AddReceivable> receivables)
        {
            return NoContent();
        }

        [HttpGet("Summary/{currency?}")]
        public ActionResult<ReceivableSummary> GetReceivableSummary(string? currency = DefaultCurrency)
        {
            return NoContent();
        }
    }
}
