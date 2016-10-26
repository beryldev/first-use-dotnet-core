using System.Linq;
using NUnit.Framework;
using Wrhs.Documents;
using Wrhs.Operations.Relocation;

namespace Wrhs.Tests
{
    [TestFixture]
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