using System;
using Wrhs.Core;
using Wrhs.Services;

namespace Wrhs.Products
{
    public class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand>
    {
        private readonly IEventBus eventBus;
        private readonly IProductService prodSrv;

        public DeleteProductCommandHandler(IEventBus eventBus, IProductService prodSrv) 
        {
            this.eventBus = eventBus;
            this.prodSrv = prodSrv;
        }

        public void Handle(DeleteProductCommand command)
        {
            var product = prodSrv.GetProductById(command.ProductId);
            prodSrv.Delete(product);

            var evt = new DeleteProductEvent(product, DateTime.UtcNow);
            eventBus.Publish(evt);
        }
    }
}