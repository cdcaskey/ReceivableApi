namespace ReceivableApi.Models.Responses
{
    public class ReceivableSummary
    {
        public int OpenReceivables { get; set; }

        public decimal TotalOpenReceivableValue { get; set; }

        public int ClosedReceivables { get; set; }

        public decimal TotalClosedReceivableValue { get; set; }
    }
}
