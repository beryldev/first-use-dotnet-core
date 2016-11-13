using Wrhs.Operations.Relocation;

namespace Wrhs.Tests
{
    public class RelocationDocumentRegistratorTests : DocumentRegistratorTests<RelocationDocument>
    {
        protected override string GetDocumentPrefix()
        {
            return "RLC";
        }

        protected override RelocationDocument CreateDocument()
        {
            return new RelocationDocument();
        }
    }
}