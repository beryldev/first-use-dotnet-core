using System;
using System.Linq;
using Wrhs.Data.Repository;
using Wrhs.Operations.Delivery;
using Xunit;

namespace Wrhs.Data.Tests
{
    public class DeliveryDocumentRepositoryTests : TestsBase
    {
        WrhsContext context;

        public DeliveryDocumentRepositoryTests()
        {
            context = CreateContext();
        }

        [Fact]
        public void ShouldSaveDocumentWithLines()
        {
            var repo = new DeliveryDocumentRepository(context);
            var doc = CreateDocument();

            repo.Save(doc);

            var readed = context.DeliveryDocuments.First();
            Assert.Equal(1, context.DeliveryDocuments.Count());
            Assert.Equal("D/001/2016", readed.FullNumber);
            Assert.Equal(1, readed.Lines.Count);
            Assert.Equal(100, readed.Lines.First().Quantity);
            Assert.Equal("PROD1", readed.Lines.First().Product.Code);
        }

        [Fact]
        public void ShouldRetriveDocumentById()
        {
            context.DeliveryDocuments.Add(CreateDocument());
            context.SaveChanges();
            var repo = new DeliveryDocumentRepository(context);

            var document = repo.GetById(1);

            Assert.Equal("D/001/2016", document.FullNumber);
            Assert.Equal(1, document.Lines.Count);
            Assert.Equal(100, document.Lines.First().Quantity);
            Assert.Equal("PROD1", document.Lines.First().Product.Code);
        }

        [Fact]
        public void ShouldUpdateDocument()
        {
            context.DeliveryDocuments.Add(CreateDocument());
            context.SaveChanges();
            var repo = new DeliveryDocumentRepository(context);

            var document = repo.GetById(1);
            document.FullNumber = "D/002/2016";
            document.Lines.First().Quantity = 200;
            repo.Update(document);

            var doc = context.DeliveryDocuments.First();
            Assert.Equal(1, context.DeliveryDocuments.Count());
            Assert.Equal("D/002/2016", doc.FullNumber);
            Assert.Equal(200, doc.Lines.First().Quantity);
        }

        [Fact]
        public void ShouldDeleteDocumentWithLines()
        {
            context.DeliveryDocuments.Add(CreateDocument());
            context.SaveChanges();
            var repo = new DeliveryDocumentRepository(context);

            var document = repo.GetById(1);
            repo.Delete(document);

            Assert.Empty(context.DeliveryDocuments);
            Assert.Empty(context.DeliveryDocumentLines);
        }

        DeliveryDocument CreateDocument()
        {
            var doc = new DeliveryDocument
            {
                FullNumber = "D/001/2016",
                IssueDate = DateTime.UtcNow
            };
            var product = CreateProduct(context);
            doc.Lines.Add(new DeliveryDocumentLine
            {
                Product = product,
                EAN = product.EAN,
                SKU = product.SKU,
                Quantity = 100,
                Remarks = "some remarks"
            });

            return doc;
        }
    }
}