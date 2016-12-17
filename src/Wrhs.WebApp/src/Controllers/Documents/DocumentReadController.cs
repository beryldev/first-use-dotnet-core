using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Wrhs.Common;
using Wrhs.Core;

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
        public IActionResult GetDeliveryDocuments(DateTime? issueDate, string fullNumber="",
            int page=1, int pageSize=20)
        {
            var result = GetDocuments(issueDate, fullNumber, page, pageSize,
                DocumentType.Delivery);
                
            return Ok(result);
        }
]
        [HttpGet("relocation")]
        public IActionResult GetRelocationDocuments(DateTime? issueDate, string fullNumber="",
            int page=1, int pageSize=20)
        {
            var result = GetDocuments(issueDate, fullNumber, page, pageSize,
                DocumentType.Relocation);
                
            return Ok(result);
        }

        [HttpGet("release")]
        public IActionResult GetReleaseDocuments(DateTime? issueDate, string fullNumber="",
            int page=1, int pageSize=20)
        {
            var result = GetDocuments(issueDate, fullNumber, page, pageSize,
                DocumentType.Release);
                
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

        protected ResultPage<Document> GetDocuments(DateTime? issueDate, string fullNumber, 
            int page, int pageSize, DocumentType type)
        {
            page = page < 1 ? 1 : page;
            pageSize = (pageSize < 1 || pageSize > 100) ? 20 : pageSize; 

            var filter = new Dictionary<string, object>
            {
                {"fullNumber", fullNumber}
            };
            if(issueDate != null)
                filter.Add("issueDate", issueDate);

            return documentSrv.FilterDocuments(type, filter, page, pageSize);
        }
    }
}