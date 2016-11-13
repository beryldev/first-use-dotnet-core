using System;
using Wrhs.Operations.Release;

namespace Wrhs.Tests
{
    public class ReleaseDocumentSearchTests : DocumentSearchTests<ReleaseDocument>
    {
        protected override ReleaseDocument CreateDocument(string fullNumber, DateTime issueDate)
        {
            return new ReleaseDocument { FullNumber = fullNumber, IssueDate = issueDate} ;
        }
    }
}