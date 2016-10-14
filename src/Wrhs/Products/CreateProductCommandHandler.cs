using Wrhs.Products.Core;

namespace Wrhs.Products.Products
{
    public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand>
    {
        IRepository<Product> productRepository;

        public CreateProductCommandHandler(IRepository<Product> productRepository)
        {
            this.productRepository = productRepository;
        }

        public void Handle(CreateProductCommand command)
        {
            var product = new Product
            {
                Code = command.Code.ToUpper(),
                Name = command.Name,
                EAN = command.EAN,
                Description = command.Description
            };

            productRepository.Save(product);
        }
    }
}