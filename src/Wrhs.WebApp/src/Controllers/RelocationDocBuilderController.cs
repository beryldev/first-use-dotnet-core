using System;
using Microsoft.AspNetCore.Mvc;
using Wrhs.Core;
using Wrhs.Operations.Relocation;
using Wrhs.Products;
using Wrhs.WebApp.Controllers;
using Wrhs.WebApp.Utils;

namespace Wrhs.WebApp.Controllers
{
    [Route("api/document/relocation/new")]
    public class RelocationDocBuilderController
        : DocBuilderController<RelocationDocument, RelocationDocumentLine, RelocDocAddLineCmd>
    {
        public RelocationDocBuilderController(ICache cache, IRepository<Product> productRepo, IValidator<RelocDocAddLineCmd> validator) 
            : base(cache, productRepo, validator)
        {
        }

        protected override Documents.DocumentBuilder<RelocationDocument, RelocationDocumentLine, RelocDocAddLineCmd> CreateDocBuilder()
        {
            return new RelocationDocumentBuilder(productRepo, validator);
        }

        protected override Documents.DocumentBuilder<RelocationDocument, RelocationDocumentLine, RelocDocAddLineCmd> CreateDocBuilder(RelocationDocument baseDoc)
        {
            return new RelocationDocumentBuilder(productRepo, validator, baseDoc);
        }
    }
}
