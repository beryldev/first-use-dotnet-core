using System;
using Wrhs.Core;
using Wrhs.Documents;
using Wrhs.Products;

namespace Wrhs.Operations.Relocation
{
    public class RelocationDocumentBuilder 
        : DocumentBuilder<RelocationDocument, RelocationDocumentLine, RelocDocAddLineCmd>
    {
        IRepository<Product> productRepository;

        public RelocationDocumentBuilder(IRepository<Product> productRepository, IValidator<RelocDocAddLineCmd> addLineValidator) 
            : base(addLineValidator)
        {
            this.productRepository = productRepository;
        }

        public override RelocationDocument Build()
        {
            var document = new RelocationDocument();
            document.Lines.AddRange(lines);
            return document;
        }

        protected override RelocationDocumentLine CommandToDocumentLine(RelocDocAddLineCmd command)
        {
            var line = new RelocationDocumentLine
            {
                Product = productRepository.GetById(command.ProductId),
                Quantity = command.Quantity,
                From = command.From,
                To = command.To
            };

            return line;
        }

        protected override RelocDocAddLineCmd DocumentLineToAddLineCommand(RelocationDocumentLine line)
        {
            var cmd = new RelocDocAddLineCmd
            {
                ProductId = line.Product.Id,
                Quantity = line.Quantity,
                From = line.From,
                To = line.To
            };

            return cmd;
        }
    }
}