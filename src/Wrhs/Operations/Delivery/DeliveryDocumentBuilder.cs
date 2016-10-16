using Wrhs.Core;
using Wrhs.Documents;
using Wrhs.Products;

namespace Wrhs.Operations.Delivery
{
    public class DeliveryDocumentBuilder : DocumentBuilder<DeliveryDocument, DeliveryDocumentLine>
    {
        IRepository<Product> productRepository;

        public DeliveryDocumentBuilder(IRepository<Product> productRepository,
            IValidator<DocumentBuilderAddLineCommand> addLineValidator,
            IValidator<DocumentBuilderUpdateLineCommand> updateLineValidator)
            : base(addLineValidator, updateLineValidator)
        {
            this.productRepository = productRepository;
        }
        
        public override DeliveryDocument Build()
        {
            var document = new DeliveryDocument();
            document.Lines.AddRange(lines);

            return document;
        }

        protected override void AddValidatedLine(DocumentBuilderAddLineCommand command)
        {
            lines.Add(new DeliveryDocumentLine
            {
                Id = lines.Count+1, //TODO napisac test ktory pokaze ze to jest zle rozwiazanie
                Product = productRepository.GetById(command.ProductId),
                Quantity = command.Quantity
            });  
        }

        protected override DocumentBuilderUpdateLineCommand DocumentLineToUpdateCommand(DeliveryDocumentLine line)
        {
            return new DocumentBuilderUpdateLineCommand
            {
                ProductId = line.Product.Id,
                Quantity = line.Quantity
            };
        }
    }
}