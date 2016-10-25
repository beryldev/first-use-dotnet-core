using System;
using NUnit.Framework;
using Wrhs.Operations.Release;

namespace Wrhs.Tests
{
    [TestFixture]
    public class ReleaseDocumentSearchTests : DocumentSearchTests<ReleaseDocument>
    {
        protected override ReleaseDocument CreateDocument(string fullNumber, DateTime issueDate)
        {
            return new ReleaseDocument { FullNumber = fullNumber, IssueDate = issueDate} ;
        }
    }
}