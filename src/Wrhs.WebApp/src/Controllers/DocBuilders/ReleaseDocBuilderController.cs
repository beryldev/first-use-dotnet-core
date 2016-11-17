using Microsoft.AspNetCore.Mvc;
using Wrhs.Core;
using Wrhs.Documents;
using Wrhs.Operations.Release;
using Wrhs.Products;
using Wrhs.WebApp.Utils;

namespace Wrhs.WebApp.Controllers.DocBuilders
{
    [Route("api/document/release")]
    public class ReleaseDocBuilderController
        : DocBuilderController<ReleaseDocument, ReleaseDocumentLine, ReleaseDocAddLineCmd>
    {
        public ReleaseDocBuilderController(ICache cache, IRepository<Product> productRepo, 
            IValidator<ReleaseDocAddLineCmd> validator) 
            : base(cache, productRepo, validator)
        {
        }

        protected override DocumentBuilder<ReleaseDocument, ReleaseDocumentLine, ReleaseDocAddLineCmd> CreateDocBuilder()
        {
            return new ReleaseDocumentBuilder(productRepo, validator);
        }

        protected override DocumentBuilder<ReleaseDocument, ReleaseDocumentLine, ReleaseDocAddLineCmd> CreateDocBuilder(ReleaseDocument baseDoc)
        {
            return new ReleaseDocumentBuilder(productRepo, validator, baseDoc);
        }
    }
}