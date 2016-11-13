using System.Linq;
using Moq;
using Wrhs.Core;
using Wrhs.Operations.Relocation;
using Xunit;

namespace Wrhs.Tests
{
    public class RelocationDocumentBuilderTests : DocumentBuilderTestsBase
    {
        [Fact]
        public void OnBuildReturnRelocationDocument()
        {
            var repo = MakeProductRepository();
            var warehouse = MakeWarehouse(repo);
            var addLineValidator = new RelocDocAddLineCmdValidator(repo, warehouse);
            var builder = new RelocationDocumentBuilder(repo, addLineValidator);

            var document = builder.Build();

            Assert.IsType<RelocationDocument>(document);
        }

        [Fact]
        public void AfterAddLineBuildReturnDocumentWithAddedLine()
        {
            var builder = MakeBuilder();
            var command = new RelocDocAddLineCmd { ProductId = 1, Quantity = 5, From = "LOC-001-01", To = "LOC-001-02" };
       
            builder.AddLine(command);
            var document = builder.Build();

            Assert.Equal(1, document.Lines.Count);
            Assert.Equal(1, document.Lines[0].Product.Id);
            Assert.Equal(5, document.Lines[0].Quantity);
            Assert.Equal("LOC-001-01", ((RelocationDocumentLine)document.Lines[0]).From);
            Assert.Equal("LOC-001-02", ((RelocationDocumentLine)document.Lines[0]).To);
        }

        [Fact]
        public void AfterUpdateLineBuildReturnDocumentWithUpdatedLine()
        {
            var builder = MakeBuilder();

            var command = new RelocDocAddLineCmd 
            { 
                ProductId = 1, 
                Quantity = 5,  
                From = "LOC-001-01",
                To = "LOC-001-02"
            };
            builder.AddLine(command);

            var line = builder.Lines.First() as RelocationDocumentLine;
            line.Quantity = 20;
            line.To = "LOC-001-03";

            builder.UpdateLine(line);
            var document = builder.Build();

            Assert.Equal(1, document.Lines.Count);
            Assert.Equal(20, document.Lines.First().Quantity);
            Assert.Equal("LOC-001-03", ((RelocationDocumentLine)document.Lines.First()).To);
        }

        RelocationDocumentBuilder MakeBuilder()
        {
            var repo = MakeProductRepository();
            var addLineValidMock = new Mock<IValidator<RelocDocAddLineCmd>>();

            var builder = new RelocationDocumentBuilder(repo, addLineValidMock.Object);
            return builder;
        }

       
    }
}