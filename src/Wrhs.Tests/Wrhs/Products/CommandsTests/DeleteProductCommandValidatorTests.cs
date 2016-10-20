using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Wrhs.Products;
using Wrhs.Products.Commands;

namespace Wrhs.Tests.Products
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
            var repo = MakeProductRepository(MakeProductList());
            var validator = new DeleteProductCommandValidator(repo);
            var command = new DeleteProductCommand
            {
                ProductId = repo.Get().First().Id
            };

            var result = validator.Validate(command);

            CollectionAssert.IsEmpty(result);
        }
    }
}