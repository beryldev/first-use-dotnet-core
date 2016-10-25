using Wrhs.Core;

namespace Wrhs.Operations.Delivery
{
    public class DeliveryDocumentRegistrator
    {
        IRepository<DeliveryDocument> repository;

        public DeliveryDocumentRegistrator(IRepository<DeliveryDocument> repository)
        {
            this.repository = repository;
        }

        public void Register(DeliveryDocument document)
        {
            repository.Save(document);
        }
    }
}