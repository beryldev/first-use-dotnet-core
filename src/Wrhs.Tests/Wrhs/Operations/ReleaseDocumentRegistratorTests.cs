using Wrhs.Operations.Release;

namespace Wrhs.Tests
{
    public class ReleaseDocumentRegistratorTests : DocumentRegistratorTests<ReleaseDocument>
    {
        protected override string GetDocumentPrefix()
        {
            return "RLS";
        }

        protected override ReleaseDocument CreateDocument()
        {
            return new ReleaseDocument();
        }
    }
}