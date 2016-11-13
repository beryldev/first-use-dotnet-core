using System.Linq;
using Wrhs.Products;
using Wrhs.Products.Commands;
using Xunit;

namespace Wrhs.Tests.Products
{
    public class DeleteProductCommandHandlerTests : ProductCommandTestsBase
    {
        [Fact]
        public void OnHandleRemoveProductFromRepository()
        {
            var repo = MakeProductRepository(MakeProductList());
            var handler = new DeleteProductCommandHandler(repo);
            var command = new DeleteProductCommand
            {
                ProductId = 1
            };

            handler.Handle(command);

            Assert.Empty(repo.Get());
        }

        [Fact]
        public void OnHandleRemoveUnexistedProductRepositoryInUnchaged()
        {
            var repo = MakeProductRepository(MakeProductList());
            var handler = new DeleteProductCommandHandler(repo);
            var command = new DeleteProductCommand
            {
                ProductId = 3
            };

            handler.Handle(command);

            Assert.Equal(1, repo.Get().Count());
            Assert.Equal("PROD1", repo.Get().First().Code);
        }
    }
}