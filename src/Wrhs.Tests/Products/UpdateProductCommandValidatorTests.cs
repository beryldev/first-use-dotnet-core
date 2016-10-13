using System.Linq;
using NUnit.Framework;
using Wrhs.Products;

namespace Wrhs.Tests.Products
{
    [TestFixture]
    public class UpdateProductCommandValidatorTests : ProductCommandTestsBase
    {
        [Test]
        public void ReturnValidationFailMessageWhenProductDoesNotExists()
        {
            var items = MakeProductList();
            var repo = MakeProductRepository(items);
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
            var items = MakeProductList();
            var repo = MakeProductRepository(items);
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
            var items = MakeProductList();
            var repo = MakeProductRepository(items);
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
            var items = MakeProductList();
            var repo = MakeProductRepository(items);
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

    }
}