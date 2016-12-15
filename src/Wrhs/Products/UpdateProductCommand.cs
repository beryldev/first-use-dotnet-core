using Wrhs.Core;

namespace Wrhs.Products
{
    public class UpdateProductCommand : ICommand
    {
        public int ProductId { get; set; }
        
        public string Name { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }
    }
}