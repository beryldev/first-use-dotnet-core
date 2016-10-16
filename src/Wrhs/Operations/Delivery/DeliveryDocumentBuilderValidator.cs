using System.Collections.Generic;
using Wrhs.Core;
using Wrhs.Products;

namespace Wrhs.Operations.Delivery
{
    public class DeliveryDocumentBuilderValidator
    {
        IRepository<Product> productRepository;

        public DeliveryDocumentBuilderValidator(IRepository<Product> productRepository)
        {
            this.productRepository = productRepository;
        }

        public IEnumerable<ValidationResult> ValidateAddLine(int productId, decimal quantity)
        {
            var result = new List<ValidationResult>();

            var product = productRepository.GetById(productId);
            if(product == null)
                result.Add(new ValidationResult("productId", $"Invalid product id: {productId}. Product not found."));

            return result;   
        }
    }

}