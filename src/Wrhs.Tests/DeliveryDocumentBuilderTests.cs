using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Wrhs.Core;
using Wrhs.Operations.Delivery;
using Wrhs.Products;

namespace Wrhs.Tests
{
    [TestFixture]
    public class DeliveryDocumentBuilderTests
    {
        [Test]
        public void BuildReturnsDeliveryDocument()
        {
            var repo = MakeProductRepository();
            var builder = new DeliveryDocumentBuilder(repo);

            var document = builder.Build();

            Assert.IsInstanceOf<DeliveryDocument>(document);
        }

        [Test]
        public void AfterAddLineBuildReturnDocumentWithAddedLine()
        {
            var repo = MakeProductRepository();
            var builder = new DeliveryDocumentBuilder(repo);
            var productId = 1;
            var quantity = 5;
            
            builder.AddLine(productId, quantity);
            var document = builder.Build();

            Assert.AreEqual(1, document.Lines.Count);
            Assert.AreEqual(productId, document.Lines[0].Product.Id);
            Assert.AreEqual(quantity, document.Lines[0].Quantity);
        }

        [Test]
        public void AfterManyAddLineBuildReturnDocumentWithAddedLines()
        {
            var repo = MakeProductRepository();
            var builder = new DeliveryDocumentBuilder(repo);
            
            var productId = 1;
            var quantity = 5;
            builder.AddLine(productId, quantity);
            productId = 3;
            quantity = 2;
            builder.AddLine(productId, quantity);
            productId = 8;
            quantity = 9;
            builder.AddLine(productId, quantity);

            var document = builder.Build();

            Assert.AreEqual(3, document.Lines.Count);
            Assert.AreEqual(1, document.Lines[0].Product.Id);
            Assert.AreEqual(5, document.Lines[0].Quantity);
            Assert.AreEqual(3, document.Lines[1].Product.Id);
            Assert.AreEqual(2, document.Lines[1].Quantity);
            Assert.AreEqual(8, document.Lines[2].Product.Id);
            Assert.AreEqual(9, document.Lines[2].Quantity);
        }

        [Test]
        public void AfterRemoveLineBuildReturnDocWithoutRemovedLine()
        {
            var repo = MakeProductRepository();
            var builder = new DeliveryDocumentBuilder(repo);
            
            var productId = 1;
            var quantity = 5;
            builder.AddLine(productId, quantity);
            
            var lines = builder.Lines;
            builder.RemoveLine(lines.First());

            var document = builder.Build();

            CollectionAssert.IsEmpty(document.Lines);
        }

        [Test]
        public void WhenExistsMoreThanOneLineAfterRemoveLineBuildReturnDocWithoutOnlyRemovedLine()
        {
            var repo = MakeProductRepository();
            var builder = new DeliveryDocumentBuilder(repo);
            
            var productId = 1;
            var quantity = 5;
            builder.AddLine(productId, quantity);
            productId = 3;
            quantity = 2;
            builder.AddLine(productId, quantity);
            productId = 8;
            quantity = 9;
            builder.AddLine(productId, quantity);

            var lineToRemove = ((DeliveryDocumentLine[])builder.Lines)[1];
            builder.RemoveLine(lineToRemove);

            var document = builder.Build();

            Assert.AreEqual(2, document.Lines.Count);
            Assert.AreEqual(1, document.Lines[0].Product.Id);
            Assert.AreEqual(8, document.Lines[1].Product.Id);
        }

        [Test]
        public void AfterUpdateLineBuildReturnDocumentWithUpdatedLine()
        {
            var repo = MakeProductRepository();
            var builder = new DeliveryDocumentBuilder(repo);

            var productId = 1;
            var quantity = 5;
            builder.AddLine(productId, quantity);

            var line = builder.Lines.First();
            line.Quantity = 20;

            builder.UpdateLine(line);
            var document = builder.Build();

            Assert.AreEqual(1, document.Lines.Count);
            Assert.AreEqual(20, document.Lines.First().Quantity);
        }

        public IRepository<Product> MakeProductRepository()
        {
            return MakeProductRepository(MakeItems(20));
        }

        protected IRepository<Product> MakeProductRepository(List<Product> items)
        {
            var mock = new Mock<IRepository<Product>>();
            mock.Setup(m=>m.Save(It.IsAny<Product>()))
                .Callback((Product prod)=>{ items.Add(prod); });

            mock.Setup(m=>m.Get())
                .Returns(items);

            mock.Setup(m=>m.GetById(It.IsAny<int>()))
                .Returns((int id)=>
                { 
                    return items.ToArray().Where(item=>item.Id==id).FirstOrDefault(); 
                });

            mock.Setup(m=>m.Update(It.IsAny<Product>()))
                .Callback((Product product)=>
                {
                    var p = items.Where(item=>item.Id==product.Id).FirstOrDefault();
                    if(p!=null)
                    {
                        items.Remove(p);
                        items.Add(product);
                    }
                });

            mock.Setup(m=>m.Delete(It.IsAny<Product>()))
                .Callback((Product product) =>
                {
                    items.Remove(product);
                });

            return mock.Object;
        }

        protected List<Product> MakeItems(int count)
        {
            var items = new List<Product>();
            for(var i=0; i<count; i++)
            {
                items.Add(new Product
                {
                    Id = i+1,
                    Code = $"PROD{i+1}",
                    Name = $"Product {i+1}",
                    EAN = $"000{i+1}"
                });
            }

            return items;
        }
    }
}