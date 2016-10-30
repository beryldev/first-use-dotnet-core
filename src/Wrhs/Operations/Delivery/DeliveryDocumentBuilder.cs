using Wrhs.Core;
using Wrhs.Documents;
using Wrhs.Products;

namespace Wrhs.Operations.Delivery
{
    public class DeliveryDocumentBuilder : DocumentBuilder<DeliveryDocument, DeliveryDocumentLine, IDocAddLineCmd>
    {
        IRepository<Product> productRepository;

        public DeliveryDocumentBuilder(IRepository<Product> productRepository,
            IValidator<IDocAddLineCmd> addLineValidator)
            : base(addLineValidator)
        {
            this.productRepository = productRepository;
        }
        
        public override DeliveryDocument Build()
        {
            var document = new DeliveryDocument();
            document.Lines.AddRange(lines);

            return document;
        }

        protected override DeliveryDocumentLine CommandToDocumentLine(IDocAddLineCmd command)
        {
            return new DeliveryDocumentLine
            {
                Product = productRepository.GetById(command.ProductId),
                Quantity = command.Quantity
            };
        }

        protected override IDocAddLineCmd DocumentLineToAddLineCommand(DeliveryDocumentLine line)
        {
            return new DocAddLineCmd
            {
                ProductId = line.Product.Id,
                Quantity = line.Quantity
            };
        }
    }
}