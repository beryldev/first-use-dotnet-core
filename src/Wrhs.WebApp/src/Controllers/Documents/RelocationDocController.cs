using Microsoft.AspNetCore.Mvc;
using Wrhs.Core;
using Wrhs.Operations.Relocation;

namespace Wrhs.WebApp.Controllers.Documents
{
    [Route("api/document/relocation")]
    public class RelocationDocController : DocumentController<RelocationDocument, RelocationDocumentLine>
    {
        public RelocationDocController(IRepository<RelocationDocument> docRepo) 
            : base(docRepo) { }

    }
}