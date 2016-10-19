using System;
using Wrhs.Core;
using Wrhs.Documents;
using Wrhs.Products;

namespace Wrhs.Operations.Relocation
{
    public class RelocationDocumentBuilder 
        : DocumentBuilder<RelocationDocument, RelocationDocumentLine, RelocDocBuilderAddLineCmd>
    {
        IRepository<Product> productRepository;

        public RelocationDocumentBuilder(IRepository<Product> productRepository, IValidator<RelocDocBuilderAddLineCmd> addLineValidator) 
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

        protected override RelocationDocumentLine CommandToDocumentLine(RelocDocBuilderAddLineCmd command)
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

        protected override RelocDocBuilderAddLineCmd DocumentLineToAddLineCommand(RelocationDocumentLine line)
        {
            var cmd = new RelocDocBuilderAddLineCmd
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