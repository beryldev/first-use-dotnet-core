namespace Wrhs.Delivery
{
    public class UpdateDeliveryDocumentCommand
        : CreateDeliveryDocumentCommand
    {
        public int DocumentId { get; set; }
    }
}