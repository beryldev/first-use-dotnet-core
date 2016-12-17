using System.Collections.Generic;
using Moq;
using Wrhs.Common;
using Wrhs.Core;
using Wrhs.WebApp.Controllers.Documents;

namespace Wrhs.WebApp.Tests
{
    public class DeliveryDocumentControllerTests : DocumentControllerTests
    {
        protected override DocumentController CreateDocController()
        {
            var result = new ResultPage<Document>(
                    new List<Document>
                    {
                        new Document { Type = DocumentType.Delivery},
                        new Document { Type = DocumentType.Delivery},
                        new Document { Type = DocumentType.Delivery}
                    }, 1, 20 );

            documentSrvMock.Setup(m=>m.GetDocuments(DocumentType.Delivery))
                .Returns(result);

            documentSrvMock.Setup(m=>m.FilterDocuments(DocumentType.Delivery, It.IsNotNull<Dictionary<string, object>>()))
                .Returns(result);

            documentSrvMock.Setup(m=>m.FilterDocuments(DocumentType.Delivery, It.IsNotNull<Dictionary<string, object>>(),
                It.IsNotNull<int>()))
                .Returns(result);

            documentSrvMock.Setup(m=>m.FilterDocuments(DocumentType.Delivery, It.IsNotNull<Dictionary<string, object>>(),
                It.IsNotNull<int>(), It.IsNotNull<int>()))
                .Returns(result);
                
            return new DeliveryDocController(documentSrvMock.Object);
        }
    }
}