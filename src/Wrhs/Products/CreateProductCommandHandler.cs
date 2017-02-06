using System;
using System.Collections.Generic;
using Wrhs.Core;
using Wrhs.Core.Exceptions;
using Wrhs.Services;

namespace Wrhs.Products
{
    public class CreateProductCommandHandler : CommandHandler<CreateProductCommand, CreateProductEvent>
    {
        private readonly IProductService service;

        public CreateProductCommandHandler(IValidator<CreateProductCommand> validator,
             IEventBus eventBus, IProductService service) : base(validator, eventBus)
        {
            this.service = service;
        }

        protected override CreateProductEvent ProcessValidCommand(CreateProductCommand command)
        {
            var product = SaveProduct(command);
            return new CreateProductEvent(product, DateTime.UtcNow);
        }

        protected override void ProcessInvalidCommand(CreateProductCommand command, 
            IEnumerable<ValidationResult> results)
        {
             throw new CommandValidationException("Invalid create product command.",
                    command, results);
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