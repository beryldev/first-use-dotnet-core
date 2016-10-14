using Wrhs.Core;

namespace Wrhs.Products
{
    public class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand>
    {
        IRepository<Product> productRepository;

        public DeleteProductCommandHandler(IRepository<Product> productRepository)
        {
            this.productRepository = productRepository;
        }

        public void Handle(DeleteProductCommand command)
        {
            var product = productRepository.GetById(command.ProductId);
            if(product != null)
                productRepository.Delete(product);
        }
    }
}