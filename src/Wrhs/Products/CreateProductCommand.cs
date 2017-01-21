using Wrhs.Core;

namespace Wrhs.Products
{
    public class CreateProductCommand : ICommand
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public string Ean { get; set; }

        public string Sku { get; set; }
        
        public string Description { get; set; }
    }
}