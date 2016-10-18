using NUnit.Framework;
using Wrhs.Core;
using Wrhs.Operations.Relocation;
using Wrhs.Products;

namespace Wrhs.Tests
{
    [TestFixture]
    public class RelocationDocumentBuilderTests
    {
        [Test]
        public void OnBuildReturnRelocationDocument()
        {
            var repo = MakeProductRepository();
            var addLineValidator = new RelocationDocumentAddLineCommandValidator(repo);
            var builder = new RelocationDocumentBuilder(repo, addLineValidator);

            var document = builder.Build();

            Assert.IsInstanceOf<RelocationDocument>(document);
        }

        IRepository<Product> MakeProductRepository()
        {
            var repo = RepositoryFactory<Product>.Make();
            for(var i=0; i<20; i++)
            {
                repo.Save(new Product
                {
                    Id = i+1,
                    Code = $"PROD{i+1}",
                    Name = $"Product {i+1}",
                    EAN = $"000{i+1}"
                });
            }

            return repo;
        }
    }
}