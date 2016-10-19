using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Wrhs.Core;
using Wrhs.Operations.Relocation;
using Wrhs.Products;

namespace Wrhs.Tests
{
    [TestFixture]
    public class RelocDocBuilderAddLineCmdValidatorTests
    {
        [Test]
        [TestCase(0)]
        [TestCase(-5)]
        [TestCase(9021)]
        public void WhenInvalidProductIdOnValidateNewLineReturnValidationFailResult(int productId)
        {
            var repo = RepositoryFactory<Product>.Make();
            FillRepository(repo);  
            var warehouse = MakeWarehouse(repo);          
            var validator = new RelocDocAddLineCmdValidator(repo, warehouse);
            var command = new RelocDocBuilderAddLineCmd{ ProductId = productId, Quantity = 4, From="LOC-001-01", To="Loc-001-02" };
            
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
            var warehouse = MakeWarehouse(repo);
            var validator = new RelocDocAddLineCmdValidator(repo, warehouse);
            var command = new RelocDocBuilderAddLineCmd { ProductId = 5, Quantity = quanitity, From="LOC-001-01", To="Loc-001-02" };

            var result = validator.Validate(command);

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Quantity", result.First().Field);
        }

        [Test]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("\n")]
        [TestCase(null)]
        public void WhenEmptyFromAddressOnAddLineReturnValidationFailResult(string from)
        {
            var repo = RepositoryFactory<Product>.Make();
            FillRepository(repo);
            var warehouse = MakeWarehouse(repo);
            var validator = new RelocDocAddLineCmdValidator(repo, warehouse);
            var command = new RelocDocBuilderAddLineCmd { ProductId = 5, Quantity = 3, From=from, To="Loc-001-02" };

            var result = validator.Validate(command);

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("From" ,result.First().Field);
        }

        [Test]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("\n")]
        [TestCase(null)]
        public void WhenEmptyToAddressOnAddLineReturnValidationFailResult(string to)
        {
            var repo = RepositoryFactory<Product>.Make();
            FillRepository(repo);
            var warehouse = MakeWarehouse(repo);
            var validator = new RelocDocAddLineCmdValidator(repo, warehouse);
            var command = new RelocDocBuilderAddLineCmd { ProductId = 5, Quantity = 3, From="LOC-001-01", To=to };

            var result = validator.Validate(command);

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("To" ,result.First().Field);
        }

        [Test]
        [TestCase("LOC-001-01", "LOC-001-01")]
        [TestCase("LOC-001-01", "loc-001-01")]
        [TestCase("loc-001-01", "LOC-001-01")]
        [TestCase("loc-001-01", "loc-001-01")]
        [TestCase("lOc-001-01", "LoC-001-01")]
        public void WhenFromAddresAndToAddresAreEqualReturnValidationFailResult(string from, string to)
        {
            var repo = RepositoryFactory<Product>.Make();
            FillRepository(repo);
            var warehouse = MakeWarehouse(repo);
            var validator = new RelocDocAddLineCmdValidator(repo, warehouse);
            var command = new RelocDocBuilderAddLineCmd { ProductId = 5, Quantity = 3, From=from, To=to };

            var result = validator.Validate(command).ToArray();

            Assert.AreEqual(2, result.Length);
            Assert.AreEqual("From",result[0].Field);
            Assert.AreEqual("To",result[1].Field);
        }

        [Test]
        public void WhenAtFromLocationNotEnoughQuantityReturnValidationFailResult()
        {
            
            var repo = RepositoryFactory<Product>.Make();
            FillRepository(repo);
            var warehouse = MakeWarehouse(repo);
            var validator = new RelocDocAddLineCmdValidator(repo, warehouse);
            var command = new RelocDocBuilderAddLineCmd { ProductId = 5, Quantity = 6, From="LOC-001-01", To="LOC-001-02" };

            var result = validator.Validate(command).ToArray();

            Assert.AreEqual(1, result.Length);
            Assert.AreEqual("Quantity",result[0].Field);
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

        IWarehouse MakeWarehouse(IRepository<Product> repo)
        {
            var warehouseMock = new Mock<IWarehouse>();
            warehouseMock.Setup(m=>m.CalculateStocks(It.IsAny<string>()))
                .Returns(new List<Stock>
                {
                    new Stock { Product=repo.GetById(5), Location="LOC-001-01", Quantity=5},
                    new Stock { Product=repo.GetById(5), Location="LOC-002-01", Quantity=24},
                    new Stock { Product=repo.GetById(5), Location="LOC-001-02", Quantity=15}
                });

            return warehouseMock.Object;
        }
    }
}