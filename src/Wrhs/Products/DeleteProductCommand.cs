using Wrhs.Core;

namespace Wrhs.Products
{
    public class DeleteProductCommand : ICommand
    {
        public int ProductId { get; set; }
    }
}