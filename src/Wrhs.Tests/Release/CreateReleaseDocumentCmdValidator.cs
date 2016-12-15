using System.Collections.Generic;
using Moq;
using Wrhs.Common;
using Wrhs.Core;
using Wrhs.Release;
using Wrhs.Services;
using Xunit;

namespace Wrhs.Tests.Release
{
    public class CreateReleaseDocumentCmdValidatorTests
        : CreateDocumentCommandValidatorTestsBase<CreateReleaseDocumentCommand>
    {
        private Mock<IStockService> stockSrvMock;
        
        protected override CreateReleaseDocumentCommand CreateCommand()
        {
            return new CreateReleaseDocumentCommand();
        }

        protected override IValidator<CreateReleaseDocumentCommand> CreateValidator(IProductService productSrv)
        {
            stockSrvMock = new Mock<IStockService>();
            stockSrvMock.Setup(m=>m.GetStockAtLocation(It.IsNotNull<int>(), It.IsNotNull<string>()))
                .Returns(new Stock{ ProductId = 1, Quantity = 9999});

            return new CreateReleaseDocumentCommandValidator(productSrv, stockSrvMock.Object);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\n")]
        public void ShouldReturnErrorWhenInvalidSrcLocation(string location)
        {
            var command = CreateCommand();
            command.Lines = new List<CreateReleaseDocumentCommand.DocumentLine>
            {
                new CreateReleaseDocumentCommand.DocumentLine
                {
                    ProductId = 1,
                    Quantity = 10,
                    SrcLocation = location
                }
            };

            var results = Act(command);
            
            AssertSingleError(results, "SrcLocation");
        }

        [Fact]
        public void ShouldReturnErrorWhenTryReleaseMoreThanAtLocation()
        {
            var command = CreateCommand();
            command.Lines = new List<CreateReleaseDocumentCommand.DocumentLine>
            {
                new CreateReleaseDocumentCommand.DocumentLine
                {
                    ProductId = 1,
                    Quantity = 10,
                    SrcLocation = "SrcLocation"
                }
            };
            stockSrvMock.Setup(m=>m.GetStockAtLocation(It.IsNotNull<int>(), It.IsNotNull<string>()))
                .Returns(new Stock{ ProductId = 1, Quantity = 5});

            var results = Act(command);

            AssertSingleError(results, "Quantity");
        }

        [Fact]
        public void ShouldReturnErrorWhenTryReleaseInMultipleLineMoreThanAtLocation()
        {
            var command = CreateCommand();
            command.Lines = new List<CreateReleaseDocumentCommand.DocumentLine>
            {
                new CreateReleaseDocumentCommand.DocumentLine
                {
                    ProductId = 1,
                    Quantity = 2,
                    SrcLocation = "SrcLocation"
                },
                new CreateReleaseDocumentCommand.DocumentLine
                {
                    ProductId = 1,
                    Quantity = 4,
                    SrcLocation = "SrcLocation"
                }
            };
            stockSrvMock.Setup(m=>m.GetStockAtLocation(It.IsNotNull<int>(), It.IsNotNull<string>()))
                .Returns(new Stock{ ProductId = 1, Quantity = 5});

            var results = Act(command);

            AssertSingleError(results, "Quantity");
        }

        [Fact]
        public void ShouldReturnErrorWhenStockNotFound()
        {
            var command = CreateCommand();
            command.Lines = new List<CreateReleaseDocumentCommand.DocumentLine>
            {
                new CreateReleaseDocumentCommand.DocumentLine
                {
                    ProductId = 1,
                    Quantity = 10,
                    SrcLocation = "SrcLocation"
                }
            };
            stockSrvMock.Setup(m=>m.GetStockAtLocation(It.IsNotNull<int>(), It.IsNotNull<string>()))
                .Returns(null as Stock);

            var results = Act(command);

            AssertSingleError(results, "SrcLocation");
        }
    }
}