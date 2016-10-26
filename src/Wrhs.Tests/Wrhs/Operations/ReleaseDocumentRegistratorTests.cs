using System.Linq;
using NUnit.Framework;
using Wrhs.Documents;
using Wrhs.Operations.Release;

namespace Wrhs.Tests
{
    [TestFixture]
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