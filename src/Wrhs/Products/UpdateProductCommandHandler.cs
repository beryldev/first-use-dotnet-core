using System;
using Wrhs.Core;
using Wrhs.Services;

namespace Wrhs.Products
{
    public class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand>
    {
        private readonly IEventBus eventBus;
        private readonly IProductService service;

        public UpdateProductCommandHandler(IEventBus eventBus, IProductService service) 
        {
            this.eventBus = eventBus;
            this.service = service;
        }

        public void Handle(UpdateProductCommand command)
        {
            var product = UpdateProduct(command);
            var evt = new UpdateProductEvent(product, DateTime.UtcNow);
            eventBus.Publish(evt);
        }

        private Product UpdateProduct(UpdateProductCommand command)
        {
            var product = service.GetProductById(command.ProductId);
            product.Name = command.Name;
            product.Code = command.Code?.ToUpper();
            product.Ean = command.Ean;
            product.Sku = command.Sku;
            product.Description = command.Description;
            
            service.Update(product);
            return product;
        }
    }
}