using System.ComponentModel.DataAnnotations;

namespace ReceivableApi.Models
{
    public class Debtor
    {
        [Key]
        public string Reference { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string? Address1 { get; set; } = string.Empty;

        public string? Address2 { get; set; } = string.Empty;

        public string? Town { get; set; } = string.Empty;

        public string? State { get; set; } = string.Empty;

        public string? Zip { get; set; } = string.Empty;

        public string CountryCode { get; set; } = string.Empty;

        public string? RegistrationNumber { get; set; } = string.Empty;
    }
}