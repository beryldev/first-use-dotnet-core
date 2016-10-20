using System.Linq;
using NUnit.Framework;
using Wrhs.Products;
using Wrhs.Products.Commands;

namespace Wrhs.Tests.Products
{
    [TestFixture]
    public class UpdateProductCommandValidatorTests : ProductCommandTestsBase
    {
        [Test]
        public void ReturnValidationFailMessageWhenProductDoesNotExists()
        {
            var repo = MakeProductRepository(MakeProductList());
            var validator = new UpdateProductCommandValidator(repo);
            var command = new UpdateProductCommand
            {
                ProductId = 3,
                Code = "PROD2",
                Name = "Product"
            };

            var result = validator.Validate(command);

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("ProductId", result.First().Field);
        }

        [Test]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("\n")]
        [TestCase(null)]
        public void ReturnValidationFailMessageWhenNewCodeIsEmpty(string code)
        {
            var repo = MakeProductRepository(MakeProductList());
            var validator = new UpdateProductCommandValidator(repo);
            var command = new UpdateProductCommand
            {
                ProductId = 1,
                Code = code,
                Name = "Product"
            };

            var result = validator.Validate(command);

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Code", result.First().Field);
        }

        [Test]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("\n")]
        [TestCase(null)]
        public void ReturnValidationFailMessageWhenNewNameIsEmpty(string name)
        {
            var repo = MakeProductRepository( MakeProductList());
            var validator = new UpdateProductCommandValidator(repo);
            var command = new UpdateProductCommand
            {
                ProductId = 1,
                Code = "PROD1",
                Name = name
            };

            var result = validator.Validate(command);

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Name", result.First().Field);
        }

        [Test]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("\n")]
        [TestCase(null)]
        public void ReturnValidationFailMessagesWhenNewCodeAndNameAreEmpty(string value)
        {
            var repo = MakeProductRepository(MakeProductList());
            var validator = new UpdateProductCommandValidator(repo);
            var command = new UpdateProductCommand
            {
                ProductId = 1,
                Code = value,
                Name = value
            };

            var result = validator.Validate(command);

            Assert.AreEqual(2, result.Count());
            CollectionAssert.Contains(result.Select(item=>item.Field), "Code");
            CollectionAssert.Contains(result.Select(item=>item.Field), "Name");
        }

        [Test]
        public void ReturnValidationFailMessageWhenNewCodeDuplicated()
        {
            var repo = MakeProductRepository(MakeProductList());
            repo.Save(new Product{Id = 2, Code = "PROD2", Name="Product 2", EAN = "111111111" });
            var validator = new UpdateProductCommandValidator(repo);
            var command = new UpdateProductCommand
            {
                ProductId = 1,
                Code = "PROD2",
                Name = "Product 1",
                EAN = "111111111111"
            };

            var result = validator.Validate(command);

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Code", result.First().Field);
        }

        [Test]
        public void ReturnNoValidationFailMessageWhenCodeAreNotChanged()
        {
            var repo = MakeProductRepository(MakeProductList());
            repo.Save(new Product{Id = 2, Code = "PROD2", Name="Product 2", EAN = "111111111" });
            var validator = new UpdateProductCommandValidator(repo);
            var command = new UpdateProductCommand
            {
                ProductId = 1,
                Code = "PROD1",
                Name = "Product 1 new",
                EAN = "111111111111"
            };

            var result = validator.Validate(command);

            CollectionAssert.IsEmpty(result);
        }

         [Test]
        public void ReturnValidationFailMessageWhenNewEANDuplicated()
        {
            var repo = MakeProductRepository(MakeProductList());
            repo.Save(new Product{Id = 2, Code = "PROD2", Name="Product 2", EAN = "111111111111" });
            var validator = new UpdateProductCommandValidator(repo);
            var command = new UpdateProductCommand
            {
                ProductId = 1,
                Code = "PROD1",
                Name = "Product 1",
                EAN = "111111111111"
            };

            var result = validator.Validate(command);

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("EAN", result.First().Field);
        }

        [Test]
        public void ReturnNoValidationFailMessageWhenEANAreNotChanged()
        {
            var repo = MakeProductRepository(MakeProductList());
            repo.Save(new Product{Id = 2, Code = "PROD2", Name="Product 2", EAN = "111111111" });
            var validator = new UpdateProductCommandValidator(repo);
            var command = new UpdateProductCommand
            {
                ProductId = 1,
                Code = "PROD1",
                Name = "Product 1 new",
                EAN = "111111111111"
            };

            var result = validator.Validate(command);

            CollectionAssert.IsEmpty(result);
        }

    }
}