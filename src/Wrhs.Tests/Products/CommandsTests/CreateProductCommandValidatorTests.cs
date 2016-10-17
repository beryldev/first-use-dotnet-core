using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Wrhs.Core;
using Wrhs.Products;
using Wrhs.Products.Commands;

namespace Wrhs.Tests.Products
{
    [TestFixture]
    public class CreateProductCommandValidatorTests : ProductCommandTestsBase
    {
        [Test]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("\n")]
        [TestCase(null)]
        public void ReturnOneValidationFailMessageWhenOnlyProductCodeEmpty(string code)
        {
            var repo = MakeProductRepository();
            var validator = new CreateProductCommandValidator(repo);
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
            var repo = MakeProductRepository();
            var validator = new CreateProductCommandValidator(repo);
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
            var repo = MakeProductRepository();
            var validator = new CreateProductCommandValidator(repo);
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
            var repo = MakeProductRepository(new List<Product>{ new Product { Code=presentCode.ToUpper(), Name="Product 1", EAN="123456789012" } });
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
            var repo = MakeProductRepository(new List<Product>{ new Product { Code="PROD1", Name="Product 1", EAN="123456789012" } });
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
            var repo = MakeProductRepository(new List<Product>{ new Product { Code=presentCode.ToUpper(), Name="Product 1", EAN="123456789012" } });
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
    }
}