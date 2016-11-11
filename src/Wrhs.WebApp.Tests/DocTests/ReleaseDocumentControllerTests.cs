using Wrhs.Operations.Release;
using Wrhs.WebApp.Controllers.Documents;

namespace Wrhs.WebApp.Tests
{
    public class ReleaseDocumentControllerTests
        : DocumentControllerTests<ReleaseDocument, ReleaseDocumentLine>
    {
        protected override DocumentController<ReleaseDocument, ReleaseDocumentLine> CreateDocController()
        {
            var controller = new ReleaseDocController(repository.Object);
            return controller as DocumentController<ReleaseDocument, ReleaseDocumentLine>;
        }
    }
}