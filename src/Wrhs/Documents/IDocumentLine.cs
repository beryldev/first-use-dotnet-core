using Wrhs.Products;

namespace Wrhs.Documents
{
    public interface IDocumentLine
    {
        int Id { get; set; }

        Product Product { get; set; }

        decimal Quantity { get; set; }
    }
}