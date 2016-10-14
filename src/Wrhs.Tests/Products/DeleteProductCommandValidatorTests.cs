using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Wrhs.Products.Products;

namespace Wrhs.Products.Tests.Products
{
    [TestFixture]
    public class DeleteProductCommandValidatorTests : ProductCommandTestsBase
    {
        [Test]
        [TestCase(-1)]
        [TestCase(-103)]
        [TestCase(-82)]
        public void ReturnValidationFailMessageWhenPasedProductIdIsNegative(int id)
        {
            var repo = MakeProductRepository(new List<Product>());
            var validator = new DeleteProductCommandValidator(repo);
            var command = new DeleteProductCommand
            {
                ProductId = id
            };

            var result = validator.Validate(command);

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("ProductId", result.First().Field);
        }

        [Test]
        [TestCase(-1)]
        [TestCase(-132)]
        [TestCase(-83)]
        public void ReturnValidationFailMessageWhenProductDoesNotExists(int id)
        {
            var repo = MakeProductRepository(new List<Product>());
            var validator = new DeleteProductCommandValidator(repo);
            var command = new DeleteProductCommand
            {
                ProductId = id
            };

            var result = validator.Validate(command);

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("ProductId", result.First().Field);
        }

        [Test]
        public void ReturnNoValidationFailMessageWhenCommandIsValid()
        {
            var items = MakeProductList();
            var repo = MakeProductRepository(items);
            var validator = new DeleteProductCommandValidator(repo);
            var command = new DeleteProductCommand
            {
                ProductId = items.First().Id
            };

            var result = validator.Validate(command);

            CollectionAssert.IsEmpty(result);
        }
    }
}