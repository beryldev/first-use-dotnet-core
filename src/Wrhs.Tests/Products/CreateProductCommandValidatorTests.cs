using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Wrhs.Products.Products;

namespace Wrhs.Products.Tests.Products
{
    [TestFixture]
    public class CreateProductCommandValidatorTests
    {
        [Test]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("\n")]
        [TestCase(null)]
        public void ReturnOneValidationFailMessageWhenOnlyProductCodeEmpty(string code)
        {
            var validator = new CreateProductCommandValidator(MakeProductRepository());
            var command = new CreateProductCommand
            {
                Code = code,
                Name = "Product 1",
                EAN = "123456789011"
            };

            var result = validator.Validate(command);

            Assert.AreEqual(1, result.Count());
            StringAssert.AreEqualIgnoringCase("code", result.First().Field);
        }

        [Test]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("\n")]
        [TestCase(null)]
        public void ReturnOneValidationFailMessageWhenOnlyProductNameEmpty(string name)
        {
            var validator = new CreateProductCommandValidator(MakeProductRepository());
            var command = new CreateProductCommand
            {
                Code = "PROD1",
                Name = name,
                EAN = "123456789011"
            };

            var result = validator.Validate(command);

            Assert.AreEqual(1, result.Count());
            StringAssert.AreEqualIgnoringCase("name", result.First().Field);
        }

        [Test]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("\n")]
        [TestCase(null)]
        public void ReturnTwoValidationFailMessagesWhenProductCodeAndNameEmpty(string value)
        {
            var validator = new CreateProductCommandValidator(MakeProductRepository());
            var command = new CreateProductCommand
            {
                Code = value,
                Name = value,
                EAN = "123456789011"
            };

            var result = validator.Validate(command);

            Assert.AreEqual(2, result.Count());
        }

        [Test]
        [TestCase("PROD1", "PROD1")]
        [TestCase("prod1", "PROD1")]
        [TestCase("PROD1", "prod1")]
        [TestCase("prod1", "prod1")]
        [TestCase("pRoD1", "PrOd1")]
        public void ReturnValidationFailMessageWhenOnlyProducCodeDuplicated(string presentCode, string newCode)
        {
            var items = new List<Product>{ new Product { Code=presentCode.ToUpper(), Name="Product 1", EAN="123456789012" } };
            var repo = MakeProductRepository(items);
            var validator = new CreateProductCommandValidator(repo);
            var command = new CreateProductCommand
            {
                Code = newCode,
                Name = "Product 1",
                EAN = "123456789011"
            };

            var result = validator.Validate(command);

            Assert.AreEqual(1, result.Count());
            StringAssert.AreEqualIgnoringCase("code", result.First().Field);
        }

        [Test]
        public void ReturnValidationFailMessageWhenOnlyProducEANDuplicated()
        {
            var items = new List<Product>{ new Product { Code="PROD1", Name="Product 1", EAN="123456789012" } };
            var repo = MakeProductRepository(items);
            var validator = new CreateProductCommandValidator(repo);
            var command = new CreateProductCommand
            {
                Code = "PROD2",
                Name = "Product 2",
                EAN = "123456789012"
            };

            var result = validator.Validate(command);

            Assert.AreEqual(1, result.Count());
            StringAssert.AreEqualIgnoringCase("EAN", result.First().Field);
        }

        [Test]
        [TestCase("PROD1", "PROD1")]
        [TestCase("prod1", "PROD1")]
        [TestCase("PROD1", "prod1")]
        [TestCase("prod1", "prod1")]
        [TestCase("pRoD1", "PrOd1")]
        public void ReturnValidationFailMessagesWhenEANAndCodeDuplicated(string presentCode, string newCode)
        {
            var items = new List<Product>{ new Product { Code=presentCode.ToUpper(), Name="Product 1", EAN="123456789012" } };
            var repo = MakeProductRepository(items);
            var validator = new CreateProductCommandValidator(repo);
            var command = new CreateProductCommand
            {
                Code = newCode,
                Name = "Product 2",
                EAN = "123456789012"
            };

            var result = validator.Validate(command);

            Assert.AreEqual(2, result.Count());
        }

        protected IRepository<Product> MakeProductRepository()
        {
            return MakeProductRepository(new List<Product>());
        }

        protected IRepository<Product> MakeProductRepository(IEnumerable<Product> items)
        {
            var mock = new Mock<IRepository<Product>>();
            mock.Setup(m=>m.Get())
                .Returns(items);

            return mock.Object;
        }
    }
}