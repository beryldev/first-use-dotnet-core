using System;
using System.Collections.Generic;
using Wrhs.Core;
using Wrhs.Core.Exceptions;

namespace Wrhs.Products
{
    public class CreateProductCommandHandler : CommandHandler<CreateProductCommand, CreateProductEvent>
    {
        private readonly IProductPersist persist;

        public CreateProductCommandHandler(IValidator<CreateProductCommand> validator,
             IEventBus eventBus, IProductPersist persist) : base(validator, eventBus)
        {
            this.persist = persist;
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
                Code = command.Code,
                Description = command.Description
            };

            persist.Save(product);
            return product;
        }
    }
}