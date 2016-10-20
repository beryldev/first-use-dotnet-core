namespace Wrhs.Documents
{
    public class DocAddLineCmd : IDocAddLineCmd
    {
        public int ProductId { get; set; }

        public decimal Quantity { get; set; }
    }


    public interface IDocAddLineCmd
    {
        int ProductId { get; set; }

        decimal Quantity { get; set; }
    }
}