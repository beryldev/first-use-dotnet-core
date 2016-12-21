using System;
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