using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Wrhs.Common;
using Wrhs.Data.Service;
using Wrhs.Products;
using Xunit;

namespace Wrhs.Data.Tests.Service
{
    public class OperationServiceTests : ServiceTestsBase<Operation>
    {
        private readonly OperationService operationSrv;

        public OperationServiceTests() : base()
        {
            operationSrv = new OperationService(context);
            context.Products.Add(new Product());
            context.Documents.Add(new Document());
            context.SaveChanges();
        }

        [Fact]
        public void ShouldReturnFalseOnCheckGuidExistsWhenNot()
        {
            context.Operations.Add(new Operation{ OperationGuid = "xyz", DocumentId=1});
            context.SaveChanges();

            var result = operationSrv.CheckOperationGuidExists("guid");

            result.Should().BeFalse();
        }

        [Fact]
        public void ShouldReturnTrueOnCheckGuidExistsWhenExists()
        {
            context.Operations.Add(new Operation{ OperationGuid = "guid", DocumentId=1});
            context.SaveChanges();

            var result = operationSrv.CheckOperationGuidExists("guid");

            result.Should().BeTrue();
        }

        [Fact]
        public void ShouldReturnOperationWithRelatedDataOnGetByGuid()
        {
            context.Operations.Add(new Operation
            {
                OperationGuid = "some-guid",
                Shifts = new List<Shift>
                {
                    new Shift { Location = "loc", ProductId=1},
                    new Shift { Location = "loc", ProductId=1}
                },
                Document = new Document 
                { 
                    Type = DocumentType.Delivery,
                    Lines = new List<DocumentLine>
                    {
                        new DocumentLine{ProductId=1}
                    }
                }
            }); 
            context.SaveChanges();

            var result = operationSrv.GetOperationByGuid("some-guid");

            result.Should().NotBeNull();
            result.OperationGuid.Should().Be("some-guid");
            result.Shifts.Where(s=>s.Location=="loc").Should().HaveCount(2);
            result.Document.Type.Should().Be(DocumentType.Delivery);
            result.Document.Lines.Should().HaveCount(1);
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

            operationSrv.Save(operation);

            context.Operations.Should().HaveCount(5);
            context.Shifts.Should().HaveCount(1);
            context.Operations.Last().OperationGuid.Should().Be("guid");
            context.Operations.Last().DocumentId.Should().Be(1);
            context.Operations.Last().Type.Should().Be(OperationType.Delivery);
            context.Operations.Last().Status.Should().Be(OperationStatus.InProgress);            
            context.Shifts.Last().ProductId.Should().Be(1);
            context.Shifts.Last().Quantity.Should().Be(10);
            context.Shifts.Last().Location.Should().Be("loc");
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
            operationSrv.Update(operation);

            context.Operations.Should().HaveCount(5);
            context.Shifts.Should().HaveCount(1);
            var updated = context.Operations.Last();
            updated.Status.Should().Be(OperationStatus.Done);
            updated.Shifts.Last().Confirmed.Should().BeTrue();

        }

        protected override Operation CreateEntity(int i)
        {
            return new Operation();
        }

        protected override BaseService<Operation> GetService()
        {
            return operationSrv as BaseService<Operation>;
        }
    }
}