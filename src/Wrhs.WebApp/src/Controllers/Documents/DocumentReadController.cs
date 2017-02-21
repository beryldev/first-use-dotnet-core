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
        public IActionResult GetDeliveryDocuments(DateTime? issueDate, DocumentState? state, string fullNumber="", 
            int page=1, int pageSize=20)
        {
            return GetDocuments(DocumentType.Delivery, issueDate, state, fullNumber, page, pageSize);
        }

        [HttpGet("relocation")]
        public IActionResult GetRelocationDocuments(DateTime? issueDate, DocumentState? state, string fullNumber="",
            int page=1, int pageSize=20)
        {
            return GetDocuments(DocumentType.Relocation, issueDate, state, fullNumber, page, pageSize);
        }

        [HttpGet("release")]
        public IActionResult GetReleaseDocuments(DateTime? issueDate, DocumentState? state, string fullNumber="",
            int page=1, int pageSize=20)
        {
            return GetDocuments(DocumentType.Release, issueDate, state, fullNumber, page, pageSize);
        }

        public IActionResult GetDocuments(DocumentType? type, DateTime? issueDate, DocumentState? state, 
            string fullNumber="", int page=1, int pageSize=20)
        {
            var filter = new Dictionary<string, object>();
            filter.Add("fullNumber", fullNumber);

            if(type != null)
                filter.Add("type", type);
            if(issueDate != null)
                filter.Add("issuedate", issueDate);
            if(state != null)
                filter.Add("state", state);

            var result = documentSrv.FilterDocuments(filter);

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