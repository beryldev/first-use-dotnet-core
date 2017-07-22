using System;
using Wrhs.Core;
using Wrhs.Services;

namespace Wrhs.Products
{
    public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand>
    {
        private readonly IEventBus eventBus;
        private readonly IProductService service;

        public CreateProductCommandHandler(IEventBus eventBus, IProductService service)
        {
            this.eventBus = eventBus;
            this.service = service;
        }

        public void Handle(CreateProductCommand command)
        {
            var product = SaveProduct(command);
            var evt = new CreateProductEvent(product, DateTime.UtcNow);
            eventBus.Publish(evt);
        }

        private Product SaveProduct(CreateProductCommand command)
        {
            var product = new Product
            {
                Name = command.Name,
                Code = command.Code?.ToUpper(),
                Ean = command.Ean,
                Sku = command.Sku,
                Description = command.Description
            };

            service.Save(product);
            return product;
        }
    }
}