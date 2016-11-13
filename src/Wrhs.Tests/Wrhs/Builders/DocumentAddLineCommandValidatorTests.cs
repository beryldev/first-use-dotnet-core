using System.Linq;
using Wrhs.Core;
using Wrhs.Documents;
using Wrhs.Products;
using Xunit;

namespace Wrhs.Tests
{
    public class DocumentAddLineCommandValidatorTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        [InlineData(9021)]
        public void WhenInvalidProductIdOnValidateNewLineReturnValidationFailResult(int productId)
        {
            var repo = RepositoryFactory<Product>.Make();
            FillRepository(repo);            
            var validator = new DocAddLineCmdValidator(repo);
            var command = new DocAddLineCmd{ ProductId = productId, Quantity = 4 };
            
            var result = validator.Validate(command);

            Assert.Equal(1, result.Count());
            Assert.Equal("ProductId", result.First().Field);
        }  

        [Theory]
        [InlineData(0)]
        [InlineData(-0.1)]
        [InlineData(-23)]
        public void WhenInvalidQuantityOnAddLineReturnValidationFailResult(decimal quanitity) 
        {
            var repo = RepositoryFactory<Product>.Make();
            FillRepository(repo);
            var validator = new DocAddLineCmdValidator(repo);
            var command = new DocAddLineCmd { ProductId = 5, Quantity = quanitity };

            var result = validator.Validate(command);

            Assert.Equal(1, result.Count());
            Assert.Equal("Quantity", result.First().Field);
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