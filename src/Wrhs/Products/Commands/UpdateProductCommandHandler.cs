using System;
using Wrhs.Core;

namespace Wrhs.Products.Commands
{
    public class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand>
    {
        IRepository<Product> productRepository;

        public UpdateProductCommandHandler(IRepository<Product> productRepository)
        {
            this.productRepository = productRepository;
        }

        public void Handle(UpdateProductCommand command)
        {
            var product = productRepository.GetById(command.ProductId);

            if(product==null)
                throw new InvalidOperationException($"Product with id: {command.ProductId} not found.");

            product.Name = command.Name;
            product.Code = command.Code.ToUpper();
            product.EAN = command.EAN;
            product.Description = command.Description;

            productRepository.Update(product);
        }
    }
}