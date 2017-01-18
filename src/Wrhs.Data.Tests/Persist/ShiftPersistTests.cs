using System;
using System.Linq;
using FluentAssertions;
using Wrhs.Common;
using Wrhs.Data.Persist;
using Wrhs.Products;
using Xunit;

namespace Wrhs.Data.Tests.Persist
{
    public class ShiftPersistTests : TestsBase, IDisposable
    {
        private readonly ShiftPersist shiftPersist;

        public ShiftPersistTests() 
        {
            shiftPersist = new ShiftPersist(context);
            context.Documents.Add(new Document());
            context.Operations.Add(new Operation{DocumentId = 1});
            context.Products.Add(new Product());
        }

        [Fact]
        public void ShouldStoreShiftInContextOnSave()
        {
            var shift = new Shift
            {
                OperationId = 1,
                ProductId = 1,
                Quantity = 10,
                Location = "loc"
            };

            shiftPersist.Save(shift);

            context.Shifts.Should().HaveCount(1);
            context.Shifts.First().OperationId.Should().Be(1);
            context.Shifts.First().ProductId.Should().Be(1);
            context.Shifts.First().Quantity.Should().Be(10);
            context.Shifts.First().Location.Should().Be("loc");
        }

        [Fact]
        public void ShouldUpdateDataInContextOnUpdate()
        {
            var shift = new Shift
            {
                OperationId = 1,
                ProductId = 1,
                Quantity = 10,
                Location = "loc",
                Confirmed = false
            };
            context.Shifts.Add(shift);
            context.SaveChanges();

            shift.Confirmed = true;
            shiftPersist.Update(shift);

            context.Shifts.Should().HaveCount(1);
            context.Shifts.First().Confirmed.Should().BeTrue();
            context.Shifts.First().Location.Should().Be("loc");
            context.Shifts.First().Quantity.Should().Be(10);
        }   
    }
}