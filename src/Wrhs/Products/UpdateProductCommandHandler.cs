using System;
using System.Collections.Generic;
using Wrhs.Core;
using Wrhs.Core.Exceptions;
using Wrhs.Services;

namespace Wrhs.Products
{
    public class UpdateProductCommandHandler : CommandHandler<UpdateProductCommand, UpdateProductEvent>
    {
        private readonly IProductPersist persist;

        private readonly IProductService service;

        public UpdateProductCommandHandler(IValidator<UpdateProductCommand> validator, IEventBus eventBus,
            IProductPersist persist, IProductService service) 
            : base(validator, eventBus)
        {
            this.persist = persist;
            this.service = service;
        }

        protected override void ProcessInvalidCommand(UpdateProductCommand command, IEnumerable<ValidationResult> results)
        {
            throw new CommandValidationException("Ivalid update product command.", command, results);
        }

        protected override UpdateProductEvent ProcessValidCommand(UpdateProductCommand command)
        {
            var product = UpdateProduct(command);
            return new UpdateProductEvent(product, DateTime.UtcNow);
        }

        private Product UpdateProduct(UpdateProductCommand command)
        {
            var product = service.GetProductById(command.ProductId);
            product.Name = command.Name;
            product.Code = command.Code;
            product.Description = command.Description;
            
            persist.Update(product);
            return product;
        }
    }
}