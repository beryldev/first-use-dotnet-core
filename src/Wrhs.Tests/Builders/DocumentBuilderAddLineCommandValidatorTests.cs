using System.Linq;
using NUnit.Framework;
using Wrhs.Core;
using Wrhs.Documents;
using Wrhs.Products;

namespace Wrhs.Tests
{
    [TestFixture]
    public class DocumentBuilderAddLineCommandValidatorTests
    {
        [Test]
        [TestCase(0)]
        [TestCase(-5)]
        [TestCase(9021)]
        public void WhenInvalidProductIdOnValidateNewLineReturnValidationFailResult(int productId)
        {
            var repo = RepositoryFactory<Product>.Make();
            FillRepository(repo);            
            var validator = new DocBuilderAddLineCmdValidator(repo);
            var command = new DocBuilderAddLineCmd{ ProductId = productId, Quantity = 4 };
            
            var result = validator.Validate(command);

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("ProductId", result.First().Field);
        }  

        [Test]
        [TestCase(0)]
        [TestCase(-0.1)]
        [TestCase(-23)]
        public void WhenInvalidQuantityOnAddLineReturnValidationFailResult(decimal quanitity) 
        {
            var repo = RepositoryFactory<Product>.Make();
            FillRepository(repo);
            var validator = new DocBuilderAddLineCmdValidator(repo);
            var command = new DocBuilderAddLineCmd { ProductId = 5, Quantity = quanitity };

            var result = validator.Validate(command);

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Quantity", result.First().Field);
        }

        void FillRepository(IRepository<Product> repo,  int count=20)
        {
            for(var i=0; i<count; i++)
            {
                var prod = new Product
                {
                    Id = i+1,
                    Code = $"PROD{i+1}",
                    Name = $"Product {i+1}",
                    EAN = $"000{i+1}"
                };

                repo.Save(prod);
            }
        }
    }
}