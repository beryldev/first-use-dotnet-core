using Wrhs.Documents;
using Wrhs.Operations.Delivery;
using Wrhs.Products;
using Wrhs.WebApp.Controllers.DocBuilders;

namespace Wrhs.WebApp.Tests
{
    public class DeliveryDocBuilderControllerTests
       : DocBuilderControllerTests<DeliveryDocument, DeliveryDocumentLine, DocAddLineCmd>
    {
        protected override DocAddLineCmd CreateAddLineCmd()
        {
            return new DocAddLineCmd();
        }

        protected override DocBuilderController<DeliveryDocument, DeliveryDocumentLine, DocAddLineCmd> CreateController(CreateParameterObject parameter)
        {
            return new DeliveryDocBuilderController(cache.Object, prodRepository.Object, 
                validator.Object);
        }

        protected override DeliveryDocument CreateDocument()
        {
            return new DeliveryDocument();
        }

        protected override DeliveryDocumentLine CreateDocumentLine()
        {
            return new DeliveryDocumentLine(){Product = new Product(), Quantity = 1};
        }
    }
}