using System;
using System.Collections.Generic;
using Wrhs.Core;
using Wrhs.Core.Exceptions;
using Wrhs.Services;

namespace Wrhs.Products
{
    public class DeleteProductCommandHandler
        : CommandHandler<DeleteProductCommand, DeleteProductEvent>
    {
        private readonly IProductService prodSrv;
        private readonly IProductPersist prodPersist;

        public DeleteProductCommandHandler(IValidator<DeleteProductCommand> validator, 
            IEventBus eventBus, IProductService prodSrv, IProductPersist prodPersist) 
            : base(validator, eventBus)
        {
            this.prodSrv = prodSrv;
            this.prodPersist = prodPersist;
        }

        protected override void ProcessInvalidCommand(DeleteProductCommand command, IEnumerable<ValidationResult> results)
        {
            throw new CommandValidationException("Invalid commnand", command, results);
        }

        protected override DeleteProductEvent ProcessValidCommand(DeleteProductCommand command)
        {
            var product = prodSrv.GetProductById(command.ProductId);
            prodPersist.Delete(product);

            return new DeleteProductEvent(product, DateTime.UtcNow);
        }
    }
}