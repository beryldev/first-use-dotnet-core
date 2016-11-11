using Microsoft.AspNetCore.Mvc;
using Wrhs.Core;
using Wrhs.Operations.Relocation;

namespace Wrhs.WebApp.Controllers.Documents
{
    [Route("api/document/relocation")]
    public class RelocationDocumentController : DocumentController<RelocationDocument, RelocationDocumentLine>
    {
        public RelocationDocumentController(IRepository<RelocationDocument> docRepo) 
            : base(docRepo) { }

    }
}