using System.Collections.Generic;
using Wrhs.Core;

namespace Wrhs.Products
{
    public class DeleteProductCommandValidator : IValidator<DeleteProductCommand>
    {
        public IEnumerable<ValidationResult> Validate(DeleteProductCommand command)
        {
            return new List<ValidationResult>();
        }
    }
}