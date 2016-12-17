using Microsoft.AspNetCore.Mvc;
using Wrhs.Common;

namespace Wrhs.WebApp.Controllers.Documents
{
    [Route("api/document/delivery")]
    public class DeliveryDocController : DocumentController
    {
        public DeliveryDocController(IDocumentService documentSrv) : base(documentSrv)
        {
        }

        protected override DocumentType GetDocumentType()
        {
            return DocumentType.Delivery;
        }
    }
}

