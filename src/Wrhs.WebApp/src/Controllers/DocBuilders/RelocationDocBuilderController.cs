using Microsoft.AspNetCore.Mvc;
using Wrhs.Core;
using Wrhs.Documents;
using Wrhs.Operations.Relocation;
using Wrhs.Products;
using Wrhs.WebApp.Utils;

namespace Wrhs.WebApp.Controllers.DocBuilders
{
    [Route("api/document/relocation")]
    public class RelocationDocBuilderController
        : DocBuilderController<RelocationDocument, RelocationDocumentLine, RelocDocAddLineCmd>
    {
        public RelocationDocBuilderController(ICache cache, IRepository<Product> productRepo, 
            IValidator<RelocDocAddLineCmd> validator) 
            : base(cache, productRepo, validator)
        {
        }

        protected override DocumentBuilder<RelocationDocument, RelocationDocumentLine, RelocDocAddLineCmd> CreateDocBuilder()
        {
            return new RelocationDocumentBuilder(productRepo, validator);
        }

        protected override DocumentBuilder<RelocationDocument, RelocationDocumentLine, RelocDocAddLineCmd> CreateDocBuilder(RelocationDocument baseDoc)
        {
            return new RelocationDocumentBuilder(productRepo, validator, baseDoc);
        }
    }
}
