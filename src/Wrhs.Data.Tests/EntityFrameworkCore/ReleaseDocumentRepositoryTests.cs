using System;
using System.Linq;
using Wrhs.Data.Repository;
using Wrhs.Operations.Release;
using Xunit;

namespace Wrhs.Data.Tests
{
    public class ReleaseDocumentRepositoryTests : TestsBase
    {
        WrhsContext context;

        public ReleaseDocumentRepositoryTests()
        {
            context = CreateContext();
        }

        [Fact]
        public void ShouldSaveDocumentWithLines()
        {
            var repo = new ReleaseDocumentRepository(context);
            var doc = CreateDocument();

            repo.Save(doc);

            var readed = context.ReleaseDocuments.First();
            Assert.Equal(1, context.ReleaseDocuments.Count());
            Assert.Equal("RLS/001/2016", readed.FullNumber);
            Assert.Equal(1, readed.Lines.Count);
            Assert.Equal(100, readed.Lines.First().Quantity);
            Assert.Equal("PROD1", readed.Lines.First().Product.Code);
        }

        [Fact]
        public void ShouldRetriveDocumentById()
        {
            context.ReleaseDocuments.Add(CreateDocument());
            context.SaveChanges();
            var repo = new ReleaseDocumentRepository(context);

            var document = repo.GetById(1);

            Assert.Equal("RLS/001/2016", document.FullNumber);
            Assert.Equal(1, document.Lines.Count);
            Assert.Equal(100, document.Lines.First().Quantity);
            Assert.Equal("PROD1", document.Lines.First().Product.Code);
        }

        [Fact]
        public void ShouldUpdateDocument()
        {
            context.ReleaseDocuments.Add(CreateDocument());
            context.SaveChanges();
            var repo = new ReleaseDocumentRepository(context);

            var document = repo.GetById(1);
            document.FullNumber = "RLS/002/2016";
            document.Lines.First().Quantity = 200;
            repo.Update(document);

            var doc = context.ReleaseDocuments.First();
            Assert.Equal(1, context.ReleaseDocuments.Count());
            Assert.Equal("RLS/002/2016", doc.FullNumber);
            Assert.Equal(200, doc.Lines.First().Quantity);
        }

        [Fact]
        public void ShouldDeleteDocumentWithLines()
        {
            context.ReleaseDocuments.Add(CreateDocument());
            context.SaveChanges();
            var repo = new ReleaseDocumentRepository(context);

            var document = repo.GetById(1);
            repo.Delete(document);

            Assert.Empty(context.ReleaseDocuments);
            Assert.Empty(context.ReleaseDocumentLines);
        }

        ReleaseDocument CreateDocument()
        {
            var doc = new ReleaseDocument
            {
                FullNumber = "RLS/001/2016",
                IssueDate = DateTime.UtcNow
            };
            var product = CreateProduct(context);
            doc.Lines.Add(new ReleaseDocumentLine
            {
                Product = product,
                EAN = product.EAN,
                SKU = product.SKU,
                Quantity = 100,
                Remarks = "some remarks",
                Location = "LOC-001-01"
            });

            return doc;
        }
    }
}