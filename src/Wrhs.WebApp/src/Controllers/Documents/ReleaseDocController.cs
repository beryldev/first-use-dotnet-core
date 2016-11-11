using Wrhs.Core;
using Wrhs.Operations.Release;

namespace Wrhs.WebApp.Controllers.Documents
{
    public class ReleaseDocController : DocumentController<ReleaseDocument, ReleaseDocumentLine>
    {
        public ReleaseDocController(IRepository<ReleaseDocument> docRepo) : base(docRepo)
        {
        }
    }
}