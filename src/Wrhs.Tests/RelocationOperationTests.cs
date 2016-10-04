using System;
using Moq;
using NUnit.Framework;
using Wrhs.Operations;
using Wrhs.Operations.Relocation;

namespace Wrhs.Tests
{
    [TestFixture]
    public class RelocationOperationTests
    {
        [Test]
        public void OperationCanBasedOnRelocationDocument()
        {
            var relocationDoc = MakeRelocationDocument();
            var operation = new RelocationOperation();

            operation.SetBaseDocument(relocationDoc);

            Assert.AreEqual(relocationDoc, operation.BaseDocument);
        }

        [Test]
        public void CanAccessDirectlyToBaseRelocationDocument()
        {
            var relocDoc = MakeRelocationDocument();
            var operation = new RelocationOperation();

            operation.BaseRelocationDocument = relocDoc;

            Assert.AreEqual(relocDoc, operation.BaseDocument);
        }

        [Test]
        public void ThrowsExceptionWhenPerformOperationWithoutBaseDocument()
        {
            var mock = new Mock<IAllocationService>();
            var operation = new RelocationOperation();

            Assert.Throws<InvalidOperationException>(()=>
            {
               operation.Perform(mock.Object); 
            });
        }

        [Test]
        public void RegisterRelocationByEAN()
        {
            var document = MakeRelocationDocument();
            var operation = new RelocationOperation();
            operation.SetBaseDocument(document);

            
            operation.RegisterRelocation(document.Lines[0].Product, "LOC-001-01-1", "LOC-001-01-2", 1);
        }

        [Test]
        public void CantRegisterMoreThanOnDocument()
        {
            var document = MakeRelocationDocument();
            var operation = new RelocationOperation();
            operation.SetBaseDocument(document);

            Assert.Throws<ArgumentException>(()=>
            {
                operation.RegisterRelocation(document.Lines[0].Product, "LOC-001-01-1", "LOC-001-01-2", 2);
            });
        }

        [Test]
        public void CantRegisterRelocationNonPresentOnDocProduct()
        {
            var product = new Product();
            var document = MakeRelocationDocument();
            var operation = new RelocationOperation();
            operation.SetBaseDocument(document);

            Assert.Throws<ArgumentException>(()=>
            {
                operation.RegisterRelocation(product, "LOC-001-01-1", "LOC-001-01-2", 1);
            });
        }

        protected RelocationDocument MakeRelocationDocument()
        {
            var product = new Product()
            {
                Name =  "Product 1",
                Code = "PROD1",
                EAN = "123456789012",
                SKU = "P1234"
            };

            var docLine = new RelocationDocumentLine()
            {
                Product = product,
                Quantity = 1,
                From = "LOC-001-01-1",
                To = "LOC-001-01-2"
            };

            var doc = new RelocationDocument();
            doc.Lines.Add(docLine);

            return doc;
        }
    }
}