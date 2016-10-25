using System;
using NUnit.Framework;
using Wrhs.Operations.Relocation;

namespace Wrhs.Tests
{
    [TestFixture]
    public class RelocationDocumentSearchTests : DocumentSearchTests<RelocationDocument>
    {
        protected override RelocationDocument CreateDocument(string fullNumber, DateTime issueDate)
        {
            return new RelocationDocument { FullNumber = fullNumber, IssueDate = issueDate} ;
        }
    }
}