using ReceivableApi.Models;
using Shouldly;

namespace ReceivableApi.Tests.Unit.Models
{
    public class ReceivableTests
    {
        [Test]
        public void Cancelled_WhenNotSetDuringConstruction_IsFalse()
        {
            // Arrange/Act
            var sut = new Receivable();

            sut.Cancelled.ShouldBeFalse();
        }

        [Test]
        public void Closed_WhenCancelledIsTrue_IsTrue()
        {
            // Arrange/Act
            var sut = new Receivable
            {
                Cancelled = true,
            };

            sut.Closed.ShouldBeTrue();
        }

        [Test]
        public void Closed_WhenOpeningValueEqualsPaidValue_IsTrue()
        {
            // Arrange/Act
            var sut = new Receivable
            {
                OpeningValue = 123.45M,
                PaidValue = 123.45M
            };

            sut.Closed.ShouldBeTrue();
        }

        [Test]
        public void Closed_WhenClosedDateIsSet_IsTrue()
        {
            // Arrange/Act
            var sut = new Receivable
            {
                ClosedDate = new DateTime(2023, 06, 12)
            };

            sut.Closed.ShouldBeTrue();
        }

        [TestCase(123.45, 12.34)]
        [TestCase(12.34, 123.45)]
        public void Closed_WhenNotCancelledAndClosedDateIsNullAndOpeningValueIsNotEqualToPaidValue_IsFalse(decimal openingValue, decimal paidValue)
        {
            // Arrange/Act
            var sut = new Receivable
            {
                Cancelled = false,
                ClosedDate = null,
                OpeningValue = openingValue,
                PaidValue = paidValue
            };

            sut.Closed.ShouldBeFalse();
        }
    }
}
