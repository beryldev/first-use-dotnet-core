namespace Wrhs.Products
{
    public class CreateProductCommand
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string EAN { get; set; }

        public string Description { get; set; }
    }
}