using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Wrhs.Common;

namespace Wrhs.WebApp.Controllers.Documents
{
    [Route("api/document")]
    public class DocumentReadController : BaseController 
    {
        protected readonly IDocumentService documentSrv;

        public DocumentReadController(IDocumentService documentSrv)
        {
            this.documentSrv = documentSrv;
        }

        [HttpGet("delivery")]
        public IActionResult GetDeliveryDocuments(DocumentFilter filter, int page=1, int pageSize=20)
        {
            filter.Type = DocumentType.Delivery;
            return GetDocuments(filter, page, pageSize);
        }

        [HttpGet("relocation")]
        public IActionResult GetRelocationDocuments(DocumentFilter filter, int page=1, int pageSize=20)
        {
            filter.Type = DocumentType.Relocation;
            return GetDocuments(filter, page, pageSize);
        }

        [HttpGet("release")]
        public IActionResult GetReleaseDocuments(DocumentFilter filter, int page=1, int pageSize=20)
        {
            filter.Type = DocumentType.Release;
            return GetDocuments(filter, page, pageSize);
        }

        public IActionResult GetDocuments(DocumentFilter filter, int page=1, int pageSize=20)
        {
            var f = new Dictionary<string, object>();
            f.Add("fullNumber", filter.FullNumber);

            if(filter.Type != null)
                f.Add("type", filter.Type);
            if(filter.IssueDate != null)
                f.Add("issuedate", filter.IssueDate);
            if(filter.State != null)
                f.Add("state", filter.State);

            var result = documentSrv.FilterDocuments(f);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetDocument(int id)
        {
            var document = documentSrv.GetDocumentById(id);
            if(document == null)
                return NotFound();

            return Ok(document);
        } 
    }
}