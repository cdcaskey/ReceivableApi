using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReceivableApi.Tests.Fakes;
using Shouldly;

namespace ReceivableApi.Tests.Unit.TestObjects
{
    [TestFixture]
    public class DatabaseInitialiserTests
    {
        [Test]
        public void Ctor_WhenCalled_CreatesTestData()
        {
            // Arrange/Act
            var sut = new DatabaseInitialiser(true);
            var context = sut.CreateContext();

            // Assert
            context.Debtors.Count().ShouldBe(3);
            context.Receivables.Count().ShouldBe(6);
        }
    }
}
