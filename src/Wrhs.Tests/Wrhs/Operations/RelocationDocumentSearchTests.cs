using System;
using Wrhs.Operations.Relocation;

namespace Wrhs.Tests
{
    public class RelocationDocumentSearchTests : DocumentSearchTests<RelocationDocument>
    {
        protected override RelocationDocument CreateDocument(string fullNumber, DateTime issueDate)
        {
            return new RelocationDocument { FullNumber = fullNumber, IssueDate = issueDate} ;
        }
    }
}