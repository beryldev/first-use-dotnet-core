using System.Collections.Generic;
using System.Linq;
using Moq;
using Wrhs.Common;
using Wrhs.Core;
using Wrhs.Relocation;
using Wrhs.Services;
using Xunit;

namespace Wrhs.Tests.Relocation
{
    public class CreateRelocationDocumentCmdValidatorTests
        : CreateDocumentCommandValidatorTestsBase<CreateRelocationDocumentCommand>
    {
        private Mock<IStockService> stockSrvMock; 

        protected override CreateRelocationDocumentCommand CreateCommand()
        {
            var command = new CreateRelocationDocumentCommand();
            command.Lines = new List<CreateDocumentCommand.DocumentLine>
            {
                new CreateDocumentCommand.DocumentLine
                {
                    ProductId = 1,
                    Quantity = 10,
                    SrcLocation = "SrcLocation",
                    DstLocation = "DstLocation"
                }   
            };
            return command;
        }

        protected override IValidator<CreateRelocationDocumentCommand> CreateValidator(IProductService productSrv)
        {
            stockSrvMock = new Mock<IStockService>();
            stockSrvMock.Setup(m=>m.GetStockAtLocation(It.IsNotNull<int>(), It.IsNotNull<string>()))
                .Returns(new Stock{ProductId=1, Quantity=20});

            return new CreateRelocationDocumentCommandValidator(productSrv, stockSrvMock.Object);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\n")]
        public void ShouldReturnErrorWhenInvalidSrcLocation(string location)
        {
            var command = CreateCommand();
            command.Lines.First().SrcLocation = location;

            var results = Act(command);

            AssertSingleError(results, "SrcLocation");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\n")]
        public void ShouldReturnErrorWhenInvalidDstLocation(string location)
        {
            var command = CreateCommand();
            command.Lines.First().DstLocation = location;

            var results = Act(command);

            AssertSingleError(results, "DstLocation");
        }

        [Fact]
        public void ShouldReturnErrorWhenTryRelocateMoreThanExistsAtSourceLocation()
        {
            var command = CreateCommand();
            stockSrvMock.Setup(m=>m.GetStockAtLocation(It.IsNotNull<int>(), It.IsNotNull<string>()))
                .Returns(new Stock { ProductId=1, Quantity=5 });

            var results = Act(command);

            AssertSingleError(results, "Quantity");
        }
    }
}