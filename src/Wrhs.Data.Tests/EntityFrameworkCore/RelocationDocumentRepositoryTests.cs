using System;
using System.Linq;
using Wrhs.Data.Repository;
using Wrhs.Operations.Relocation;
using Xunit;

namespace Wrhs.Data.Tests
{
    public class RelocationDocumentRepositoryTests : TestsBase
    {
        WrhsContext context;

        public RelocationDocumentRepositoryTests()
        {
            context = CreateContext();
        }

        [Fact]
        public void ShouldSaveDocumentWithLines()
        {
            var repo = new RelocationDocumentRepository(context);
            var doc = CreateDocument();

            repo.Save(doc);

            var readed = context.RelocationDocuments.First();
            Assert.Equal(1, context.RelocationDocuments.Count());
            Assert.Equal("RLC/001/2016", readed.FullNumber);
            Assert.Equal(1, readed.Lines.Count);
            Assert.Equal(100, readed.Lines.First().Quantity);
            Assert.Equal("PROD1", readed.Lines.First().Product.Code);
        }

        [Fact]
        public void ShouldRetriveDocumentById()
        {
            context.RelocationDocuments.Add(CreateDocument());
            context.SaveChanges();
            var repo = new RelocationDocumentRepository(context);

            var document = repo.GetById(1);

            Assert.Equal("RLC/001/2016", document.FullNumber);
            Assert.Equal(1, document.Lines.Count);
            Assert.Equal(100, document.Lines.First().Quantity);
            Assert.Equal("PROD1", document.Lines.First().Product.Code);
        }

        [Fact]
        public void ShouldUpdateDocument()
        {
            context.RelocationDocuments.Add(CreateDocument());
            context.SaveChanges();
            var repo = new RelocationDocumentRepository(context);

            var document = repo.GetById(1);
            document.FullNumber = "RLC/002/2016";
            document.Lines.First().Quantity = 200;
            repo.Update(document);

            var doc = context.RelocationDocuments.First();
            Assert.Equal(1, context.RelocationDocuments.Count());
            Assert.Equal("RLC/002/2016", doc.FullNumber);
            Assert.Equal(200, doc.Lines.First().Quantity);
        }

        [Fact]
        public void ShouldDeleteDocumentWithLines()
        {
            context.RelocationDocuments.Add(CreateDocument());
            context.SaveChanges();
            var repo = new RelocationDocumentRepository(context);

            var document = repo.GetById(1);
            repo.Delete(document);

            Assert.Empty(context.RelocationDocuments);
            Assert.Empty(context.RelocationDocumentLines);
        }

        RelocationDocument CreateDocument()
        {
            var doc = new RelocationDocument
            {
                FullNumber = "RLC/001/2016",
                IssueDate = DateTime.UtcNow
            };
            var product = CreateProduct(context);
            doc.Lines.Add(new RelocationDocumentLine
            {
                Product = product,
                EAN = product.EAN,
                SKU = product.SKU,
                Quantity = 100,
                Remarks = "some remarks",
                From = "LOC-001-01",
                To = "LOC-001-02"
            });

            return doc;
        }
    }
}