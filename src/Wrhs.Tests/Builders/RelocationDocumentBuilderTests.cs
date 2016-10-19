using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Wrhs.Core;
using Wrhs.Operations.Relocation;
using Wrhs.Products;

namespace Wrhs.Tests
{
    [TestFixture]
    public class RelocationDocumentBuilderTests : DocumentBuilderTestsBase
    {
        [Test]
        public void OnBuildReturnRelocationDocument()
        {
            var repo = MakeProductRepository();
            var warehouse = MakeWarehouse(repo);
            var addLineValidator = new RelocDocAddLineCmdValidator(repo, warehouse);
            var builder = new RelocationDocumentBuilder(repo, addLineValidator);

            var document = builder.Build();

            Assert.IsInstanceOf<RelocationDocument>(document);
        }

        [Test]
        public void AfterAddLineBuildReturnDocumentWithAddedLine()
        {
            var builder = MakeBuilder();
            var command = new RelocDocBuilderAddLineCmd { ProductId = 1, Quantity = 5 };
       
            builder.AddLine(command);
            var document = builder.Build();

            Assert.AreEqual(1, document.Lines.Count);
            Assert.AreEqual(1, document.Lines[0].Product.Id);
            Assert.AreEqual(5, document.Lines[0].Quantity);
        }

        RelocationDocumentBuilder MakeBuilder()
        {
            var repo = MakeProductRepository();
            var addLineValidMock = new Mock<IValidator<RelocDocBuilderAddLineCmd>>();

            var builder = new RelocationDocumentBuilder(repo, addLineValidMock.Object);
            return builder;
        }

        IWarehouse MakeWarehouse(IRepository<Product> repo)
        {
            var warehouseMock = new Mock<IWarehouse>();
            warehouseMock.Setup(m=>m.CalculateStocks(It.IsAny<string>()))
                .Returns(new List<Stock>
                {
                    new Stock { Product=repo.GetById(5), Location="LOC-001-01", Quantity=5},
                    new Stock { Product=repo.GetById(8), Location="LOC-001-01", Quantity=24},
                    new Stock { Product=repo.GetById(5), Location="LOC-001-02", Quantity=15}
                });

            return warehouseMock.Object;
        }
    }
}