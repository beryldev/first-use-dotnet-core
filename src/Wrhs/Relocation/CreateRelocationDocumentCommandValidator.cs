using System.Collections.Generic;
using Wrhs.Common;
using Wrhs.Core;
using Wrhs.Services;

namespace Wrhs.Relocation
{
    public class CreateRelocationDocumentCommandValidator 
        : CommandValidator<CreateRelocationDocumentCommand>
    {
        private readonly ProductInfoValidator productValidator;
        private readonly IStockService stockSrv;

        public CreateRelocationDocumentCommandValidator(IProductService productSrv, IStockService stockSrv)
        {
            productValidator = new ProductInfoValidator(productSrv);
            this.stockSrv = stockSrv;
        }

        public override IEnumerable<ValidationResult> Validate(CreateRelocationDocumentCommand command)
        {
            foreach(var line in command.Lines)
                results.AddRange(productValidator.Validate(line));

            foreach (var line in command.Lines)
            {
                if(string.IsNullOrWhiteSpace(line.SrcLocation))
                    AddValidationResult("SrcLocation", "Invalid (empty) source location.");
                
                if(string.IsNullOrWhiteSpace(line.DstLocation))
                    AddValidationResult("DstLocation", "Invalid (empty) destiny location.");

                var stock = stockSrv.GetStockAtLocation(line.ProductId, line.SrcLocation??"");
                if(stock?.Quantity < line.Quantity)
                    AddValidationResult("Quantity", "Invalid quantity. Try relocate more than at source location.");
            }
                      
            return results;
        }
    }
}