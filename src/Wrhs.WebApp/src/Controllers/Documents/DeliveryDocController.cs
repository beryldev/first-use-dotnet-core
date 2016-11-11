using Microsoft.AspNetCore.Mvc;
using Wrhs.Core;
using Wrhs.Operations.Delivery;

namespace Wrhs.WebApp.Controllers.Documents
{
    [Route("api/document/delivery")]
    public class DeliveryDocController : DocumentController<DeliveryDocument, DeliveryDocumentLine>
    {
        public DeliveryDocController(IRepository<DeliveryDocument> docRepo) : base(docRepo)
        {
        }
    }
}

