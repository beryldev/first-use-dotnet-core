using Wrhs.Operations.Release;
using Wrhs.Products;
using Wrhs.WebApp.Controllers;

namespace Wrhs.WebApp.Tests
{
    public class ReleaseDocBuilderControllerTests
        : DocBuilderControllerTests<ReleaseDocument, ReleaseDocumentLine, ReleaseDocAddLineCmd>
    {
        protected override ReleaseDocAddLineCmd CreateAddLineCmd()
        {
            return new ReleaseDocAddLineCmd();
        }

        protected override DocBuilderController<ReleaseDocument, ReleaseDocumentLine, ReleaseDocAddLineCmd> CreateController(CreateParameterObject parameter)
        {
            return new ReleaseDocBuilderController(cache.Object, prodRepository.Object, 
                validator.Object);
        }

        protected override ReleaseDocument CreateDocument()
        {
            return new ReleaseDocument();
        }

        protected override ReleaseDocumentLine CreateDocumentLine()
        {
            return new ReleaseDocumentLine(){Product = new Product(), Quantity = 1, Location = "LOC-001-01"};
        }
    }
}