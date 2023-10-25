using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReceivableApi.Data;
using ReceivableApi.Models;
using ReceivableApi.Models.Requests;
using ReceivableApi.Models.Responses;
using ReceivableApi.Validators;

namespace ReceivableApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceivablesController : ControllerBase
    {
        private const string DefaultCurrency = "USD";

        private readonly ReceivableApiContext context;
        private readonly ReceivableManager receivableManager;
        private readonly Country[] countries;
        private readonly Currency[] currencies;

        public ReceivablesController(ReceivableApiContext context, ReceivableManager receivableManager, CountryLoader countryLoader, CurrencyLoader currencyLoader)
        {
            this.context = context;
            this.receivableManager = receivableManager;
            countries = countryLoader.Load();
            currencies = currencyLoader.Load();
        }

        [HttpPost]
        public async Task<IActionResult> PostReceivables(List<AddReceivable> receivables, [FromServices] AddReceivableValidator validator)
        {
            foreach (var receivable in receivables)
            {
                if (!validator.Validate(receivable).IsValid)
                {
                    return BadRequest();
                }
            }

            foreach (var requestReceivable in receivables)
            {
                var debtor = new Debtor()
                {
                    Name = requestReceivable.DebtorName,
                    Reference = requestReceivable.Reference,
                    Address1 = requestReceivable.DebtorAddress1,
                    Address2 = requestReceivable.DebtorAddress2,
                    Town = requestReceivable.DebtorTown,
                    State = requestReceivable.DebtorState,
                    Zip = requestReceivable.DebtorZip,
                    CountryCode = requestReceivable.DebtorCountryCode,
                    RegistrationNumber = requestReceivable.DebtorRegistrationNumber
                };

                var receivable = new Receivable()
                {
                    Reference = requestReceivable.Reference,
                    CurrencyCode = requestReceivable.CurrencyCode,
                    Issued = DateTime.Parse(requestReceivable.IssueDate),
                    OpeningValue = requestReceivable.OpeningValue,
                    PaidValue = requestReceivable.PaidValue,
                    Due = DateTime.Parse(requestReceivable.DueDate),
                    Cancelled = requestReceivable.Cancelled ?? false,
                    DebtorId = debtor.Reference,
                    Debtor = debtor
                };

                if (requestReceivable.ClosedDate != null)
                {
                    receivable.ClosedDate = DateTime.Parse(requestReceivable.ClosedDate);
                }

                await receivableManager.StoreReceivable(receivable);
            }

            return Ok();
        }

        [HttpGet("Summary/{currency?}")]
        public ActionResult<ReceivableSummary> GetReceivableSummary(string? currency = DefaultCurrency)
        {
            if (!currencies.Any(x => x.Code == currency))
            {
                return BadRequest();
            }

            return receivableManager.GetSummary(currency ?? DefaultCurrency);
        }
    }
}
