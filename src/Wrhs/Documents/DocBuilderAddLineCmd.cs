namespace Wrhs.Documents
{
    public class DocBuilderAddLineCmd : IDocBuilderAddLineCmd
    {
        public int ProductId { get; set; }

        public decimal Quantity { get; set; }
    }


    public interface IDocBuilderAddLineCmd
    {
        int ProductId { get; set; }

        decimal Quantity { get; set; }
    }
}