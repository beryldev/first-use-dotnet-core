using System;
using Wrhs.Operations.Delivery;

namespace Wrhs.Tests
{
    public class DeliveryDocumentSearchTests : DocumentSearchTests<DeliveryDocument>
    {
        protected override DeliveryDocument CreateDocument(string fullNumber, DateTime issueDate)
        {
            return new DeliveryDocument { FullNumber = fullNumber, IssueDate = issueDate} ;
        }
    }
}