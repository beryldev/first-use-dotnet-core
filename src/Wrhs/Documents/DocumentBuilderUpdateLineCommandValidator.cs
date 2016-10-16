using Wrhs.Core;
using Wrhs.Products;

namespace Wrhs.Documents
{
    public class DocumentBuilderUpdateLineCommandValidator : DocumentBuilderAddLineCommandValidator
    {
        public DocumentBuilderUpdateLineCommandValidator(IRepository<Product> productRepository) : base(productRepository)
        {
        }
    }
}