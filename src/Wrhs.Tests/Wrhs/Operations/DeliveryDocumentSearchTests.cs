using System;
using NUnit.Framework;
using Wrhs.Operations.Delivery;

namespace Wrhs.Tests
{
    [TestFixture]
    public class DeliveryDocumentSearchTests : DocumentSearchTests<DeliveryDocument>
    {
        protected override DeliveryDocument CreateDocument(string fullNumber, DateTime issueDate)
        {
            return new DeliveryDocument { FullNumber = fullNumber, IssueDate = issueDate} ;
        }
    }
}