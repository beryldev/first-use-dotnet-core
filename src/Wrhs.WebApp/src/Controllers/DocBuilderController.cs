using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Wrhs.Core;
using Wrhs.Documents;
using Wrhs.Products;
using Wrhs.WebApp.Utils;

namespace Wrhs.WebApp.Controllers
{
    public abstract class DocBuilderController<TDoc, TLine, TCmd> : BaseController
        where TDoc : class, IEntity, IDocument<TLine>, INumerableDocument
        where TLine : IDocumentLine
        where TCmd : IDocAddLineCmd
    {
        protected ICache cache;

        protected IRepository<Product> productRepo;

        protected IValidator<TCmd> validator;

        public DocBuilderController(ICache cache, IRepository<Product> productRepo, IValidator<TCmd> validator)
        {
            this.cache = cache;
            this.productRepo = productRepo;
            this.validator = validator;
        }

        [HttpGet("new")]
        public string NewDocument()
        {
            var builder = CreateDocBuilder();
            var guid = System.Guid.NewGuid().ToString();
            cache.SetValue(guid, builder.Build());

            return guid;
        }

        [HttpPost("new/{guid}/line")]
        public IActionResult AddLine(string guid, [FromBody]TCmd cmd)
        {
            var errors = new List<ValidationResult>();
            var doc = cache.GetValue(guid) as TDoc;
            if(doc == null)
                return NotFound();

            var builder = CreateDocBuilder(doc);
            builder.OnAddLineFail += (object sender, IEnumerable<ValidationResult> result) => { errors = (List<ValidationResult>)result; };

            builder.AddLine(cmd);

            if(errors.Count > 0)
                return BadRequest(errors);

            cache.SetValue(guid, builder.Build());
            return Ok(builder.Lines);
        }

        [HttpGet("new/{guid}")]
        public IActionResult GetDocument(string guid)
        {
            var doc = cache.GetValue(guid) as TDoc;
            if(doc == null)
                return NotFound();

            var builder = CreateDocBuilder(doc);

            var document = builder.Build();
            return Ok(document);
        }

        [HttpGet("new/{guid}/line")]
        public IActionResult GetDocumentLines(string guid)
        {
            var document = cache.GetValue(guid) as TDoc;
            if(document == null)
                return NotFound();

            var builder = CreateDocBuilder(document);

            return Ok(builder.Lines);
        }

        [HttpPut("new/{guid}/line")]
        public IActionResult UpdateLine(string guid, [FromBody]TLine line)
        {
            var errors = new List<ValidationResult>();
            var doc = cache.GetValue(guid) as TDoc;
            if(doc == null)
                return NotFound();
                
            var builder = CreateDocBuilder(doc);
            builder.OnUpdateLineFail += (object sender, IEnumerable<ValidationResult> result) => { errors = (List<ValidationResult>)result; };
            builder.UpdateLine(line);

            if(errors.Count > 0)
                return BadRequest(errors);

            cache.SetValue(guid, builder.Build());
            return Ok(builder.Lines);
        }  

        [HttpDelete("new/{guid}/line")]
        public IActionResult DeleteLine(string guid, [FromBody]TLine line)
        {
            var doc = cache.GetValue(guid) as TDoc;
            if(doc == null)
                return NotFound();

            var builder = CreateDocBuilder(doc);
            builder.RemoveLine(line);

            var document = builder.Build();
            cache.SetValue(guid, document);

            return Ok(builder.Lines);
        }  

        [HttpPost("new/{guid}")]
        public IActionResult Register(string guid, [FromServices]IDocumentRegistrator<TDoc> registrator,
            [FromBody]TDoc doc = null)
        {
            var document = cache.GetValue(guid) as TDoc;
            if(document == null)
                return NotFound();

            document.Remarks = doc?.Remarks;

            registrator.Register(document);

            return Ok(document);
        }

        protected abstract DocumentBuilder<TDoc, TLine, TCmd> CreateDocBuilder();

        protected abstract DocumentBuilder<TDoc, TLine, TCmd> CreateDocBuilder(TDoc baseDoc);
    }
}