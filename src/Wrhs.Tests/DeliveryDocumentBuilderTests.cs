using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Wrhs.Core;
using Wrhs.Documents;
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
            var builder = MakeBuilder();

            var document = builder.Build();

            Assert.IsInstanceOf<DeliveryDocument>(document);
        }

        [Test]
        public void AfterAddLineBuildReturnDocumentWithAddedLine()
        {
            var builder = MakeBuilder();
            var command = new DocumentBuilderAddLineCommand { ProductId = 1, Quantity = 5 };
       
            builder.AddLine(command);
            var document = builder.Build();

            Assert.AreEqual(1, document.Lines.Count);
            Assert.AreEqual(1, document.Lines[0].Product.Id);
            Assert.AreEqual(5, document.Lines[0].Quantity);
        }

        [Test]
        public void AfterManyAddLineBuildReturnDocumentWithAddedLines()
        {
            var builder = MakeBuilder();
            
            var command = new DocumentBuilderAddLineCommand { ProductId = 1, Quantity = 5 };
            builder.AddLine(command);
            command = new DocumentBuilderAddLineCommand { ProductId = 3, Quantity = 2 };
            builder.AddLine(command);
            command = new DocumentBuilderAddLineCommand { ProductId = 8, Quantity = 9 };
            builder.AddLine(command);

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
            var builder = MakeBuilder();
            
            var command = new DocumentBuilderAddLineCommand { ProductId = 1, Quantity = 5 };
            builder.AddLine(command);
            
            var lines = builder.Lines;
            builder.RemoveLine(lines.First());

            var document = builder.Build();

            CollectionAssert.IsEmpty(document.Lines);
        }

        [Test]
        public void WhenExistsMoreThanOneLineAfterRemoveLineBuildReturnDocWithoutOnlyRemovedLine()
        {
            var builder = MakeBuilder();
            
            var command = new DocumentBuilderAddLineCommand { ProductId = 1, Quantity = 5};
            builder.AddLine(command);
            command = new DocumentBuilderAddLineCommand { ProductId = 3, Quantity = 2 };
            builder.AddLine(command);
            command = new DocumentBuilderAddLineCommand { ProductId = 8, Quantity = 9 };
            builder.AddLine(command);

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
            var builder = MakeBuilder();

            var command = new DocumentBuilderAddLineCommand { ProductId = 1, Quantity = 5 };
            builder.AddLine(command);

            var line = builder.Lines.First();
            line.Quantity = 20;

            builder.UpdateLine(line);
            var document = builder.Build();

            Assert.AreEqual(1, document.Lines.Count);
            Assert.AreEqual(20, document.Lines.First().Quantity);
        }

        [Test]
        public void WhenOnAddLineValidationFailBuildReturnDocWithUnchangedLines()
        {
            var repo = ProductRepositoryFactory.Make();

            var addLineValidMock = new Mock<IValidator<DocumentBuilderAddLineCommand>>();
            addLineValidMock.Setup(m=>m.Validate(It.IsAny<DocumentBuilderAddLineCommand>()))
                .Returns(new ValidationResult[]{ new ValidationResult() });

            var updateLineValidMock = new Mock<IValidator<DocumentBuilderUpdateLineCommand>>();
            updateLineValidMock.Setup(m=>m.Validate(It.IsAny<DocumentBuilderUpdateLineCommand>()))
                .Returns(new ValidationResult[0]);

            var builder = new DeliveryDocumentBuilder(repo, addLineValidMock.Object, updateLineValidMock.Object);

            var command = new DocumentBuilderAddLineCommand { ProductId = -34, Quantity = 5 };
            builder.AddLine(command);

            var document = builder.Build();

            CollectionAssert.IsEmpty(document.Lines);
        }

        [Test]
        public void WhenOnAddLineValidationFailCallOnAddLineFail()
        {
            var onAddLineFailCalled = false;
            var repo = ProductRepositoryFactory.Make();

            var addLineValidMock = new Mock<IValidator<DocumentBuilderAddLineCommand>>();
            addLineValidMock.Setup(m=>m.Validate(It.IsAny<DocumentBuilderAddLineCommand>()))
                .Returns(new ValidationResult[]{ new ValidationResult() });

            var updateLineValidMock = new Mock<IValidator<DocumentBuilderUpdateLineCommand>>();
            updateLineValidMock.Setup(m=>m.Validate(It.IsAny<DocumentBuilderUpdateLineCommand>()))
                .Returns(new ValidationResult[0]);

            var builder = new DeliveryDocumentBuilder(repo, addLineValidMock.Object, updateLineValidMock.Object);
            builder.OnAddLineFail += (object sender, IEnumerable<ValidationResult> args) => onAddLineFailCalled=true;

            var command = new DocumentBuilderAddLineCommand { ProductId = -34, Quantity = 5 };
            builder.AddLine(command);

            Assert.IsTrue(onAddLineFailCalled);
        }

        [Test]
        public void WhenAddLineEachLineHasUniqeId()
        {
            var builder = MakeBuilder();

            var command1 = new DocumentBuilderAddLineCommand { ProductId = 1, Quantity = 1 };
            var command2 = new DocumentBuilderAddLineCommand { ProductId = 2, Quantity = 2 };
            builder.AddLine(command1);
            builder.AddLine(command2);
            var line = ((DeliveryDocumentLine[])builder.Lines)[0];
            builder.RemoveLine(line);
            builder.AddLine(command1);

            Assert.AreEqual(2, builder.Lines.GroupBy(l=>l.Id).Select(l=>l).Count());
        }

        public DeliveryDocumentBuilder MakeBuilder()
        {
            var repo = MakeProductRepository();
            var addLineValidMock = new Mock<IValidator<DocumentBuilderAddLineCommand>>();
            var updateLineValidMock = new Mock<IValidator<DocumentBuilderUpdateLineCommand>>();

            var builder = new DeliveryDocumentBuilder(repo, addLineValidMock.Object, updateLineValidMock.Object);
            return builder;
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