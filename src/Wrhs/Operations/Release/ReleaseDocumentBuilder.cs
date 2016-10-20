using Wrhs.Core;
using Wrhs.Documents;
using Wrhs.Products;

namespace Wrhs.Operations.Release
{
    public class ReleaseDocumentBuilder
        : DocumentBuilder<ReleaseDocument, ReleaseDocumentLine, ReleaseDocAddLineCmd>
    {
        IRepository<Product> productRepository;
        
        public ReleaseDocumentBuilder(IRepository<Product> productRepository, IValidator<ReleaseDocAddLineCmd> addLineValidator) 
            : base(addLineValidator)
        {
            this.productRepository = productRepository;
        }

        public override ReleaseDocument Build()
        {
            var document = new ReleaseDocument();
            document.Lines.AddRange(lines);

            return document;
        }

        protected override ReleaseDocumentLine CommandToDocumentLine(ReleaseDocAddLineCmd command)
        {
            var line = new ReleaseDocumentLine
            {
                Product = productRepository.GetById(command.ProductId),
                Quantity = command.Quantity,
                Location = command.Location
            };

            return line;
        }

        protected override ReleaseDocAddLineCmd DocumentLineToAddLineCommand(ReleaseDocumentLine line)
        {
            var cmd = new ReleaseDocAddLineCmd
            {
                ProductId = line.Product.Id,
                Quantity = line.Quantity,
                Location = line.Location
            };

            return cmd;
        }
    }
}