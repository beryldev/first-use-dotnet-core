namespace Wrhs.Products.Products
{
    public class UpdateProductCommand
    {
        public int ProductId { get; set; }

        public string Code { get; set; }
        
        public string Name { get; set; }

        public string EAN { get; set; }

        public string Description { get; set; }
    }
}