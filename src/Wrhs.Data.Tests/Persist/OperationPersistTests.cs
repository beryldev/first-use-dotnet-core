using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Wrhs.Common;
using Wrhs.Data.Persist;
using Wrhs.Products;
using Xunit;

namespace Wrhs.Data.Tests.Persist
{
    public class OperationPersistTests : TestsBase
    {
        private readonly OperationPersist operationPersist;

        public OperationPersistTests()
        {
            operationPersist = new OperationPersist(context);
            context.Products.Add(new Product());
            context.Documents.Add(new Document());
            context.SaveChanges();
        }

        [Fact]
        public void ShouldStoreInContextOperationWithShiftsOnSave()
        {

            var operation = new Operation
            {
                DocumentId = 1,
                OperationGuid = "guid",
                Type = OperationType.Delivery,
                Status = OperationStatus.InProgress,
                Shifts = new List<Shift>
                {
                    new Shift
                    {
                        ProductId = 1,
                        Quantity = 10,
                        Location = "loc"
                    }
                }
            };

            operationPersist.Save(operation);

            context.Operations.Should().HaveCount(1);
            context.Shifts.Should().HaveCount(1);
            context.Operations.First().OperationGuid.Should().Be("guid");
            context.Operations.First().DocumentId.Should().Be(1);
            context.Operations.First().Type.Should().Be(OperationType.Delivery);
            context.Operations.First().Status.Should().Be(OperationStatus.InProgress);            
            context.Shifts.First().ProductId.Should().Be(1);
            context.Shifts.First().Quantity.Should().Be(10);
            context.Shifts.First().Location.Should().Be("loc");
        }

        [Fact]
        public void ShouldUpdateDataInContextOnUpdate()
        {
            var operation = new Operation
            {
                DocumentId = 1,
                OperationGuid = "guid",
                Type = OperationType.Delivery,
                Status = OperationStatus.InProgress,
                Shifts = new List<Shift>
                {
                    new Shift
                    {
                        ProductId = 1,
                        Quantity = 10,
                        Location = "loc"
                    }
                }
            };
            context.Operations.Add(operation);
            context.SaveChanges();

            operation.Status = OperationStatus.Done;
            operation.Shifts.First().Confirmed = true;
            operationPersist.Update(operation);

            context.Operations.Should().HaveCount(1);
            context.Shifts.Should().HaveCount(1);
            var updated = context.Operations.First();
            updated.Status.Should().Be(OperationStatus.Done);
            updated.Shifts.First().Confirmed.Should().BeTrue();

        }
    }
}