using System.ComponentModel.DataAnnotations;

namespace ReceivableApi.Models
{
    public class Receivable
    {
        [Key]
        public string Reference { get; set; } = string.Empty;

        public string CurrencyCode { get; set; } = string.Empty;

        public DateTime Issued { get; set; }

        public decimal OpeningValue { get; set; }

        public decimal PaidValue { get; set; }

        public DateTime Due { get; set; }

        public bool Closed => OpeningValue == PaidValue || ClosedDate != null;

        public DateTime? ClosedDate { get; set; }

        public bool Cancelled { get; set; }

        public string DebtorId { get; set; } = string.Empty;

        public virtual Debtor? Debtor { get; set; }
    }
}
