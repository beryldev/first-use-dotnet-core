using Microsoft.AspNetCore.Mvc;
using Wrhs.Common;
using Wrhs.Core;

namespace Wrhs.WebApp.Controllers.Documents
{
    [Route("api/document/delivery")]
    public class DeliveryDocController : DocumentController
    {
        public DeliveryDocController(IDocumentService documentSrv) : base(documentSrv)
        {
        }

        public override ResultPage<Document> Get()
        {
            return null;
        }
    }
}

