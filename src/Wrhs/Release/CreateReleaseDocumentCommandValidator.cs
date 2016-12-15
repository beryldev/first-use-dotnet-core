using System.Collections.Generic;
using System.Linq;
using Wrhs.Common;
using Wrhs.Core;
using Wrhs.Services;

namespace Wrhs.Release
{
    public class CreateReleaseDocumentCommandValidator
        : CommandValidator<CreateReleaseDocumentCommand>
    {
        private readonly ProductInfoValidator productValidator;
        private readonly IStockService stockSrv;


        public CreateReleaseDocumentCommandValidator(IProductService productSrv,
            IStockService stockSrv)
        {
            productValidator = new ProductInfoValidator(productSrv);
            this.stockSrv = stockSrv;
        }

        public override IEnumerable<ValidationResult> Validate(CreateReleaseDocumentCommand command)
        {
            foreach(var line in command.Lines)
            {
                results.AddRange(productValidator.Validate(line));

                if(string.IsNullOrWhiteSpace(line.SrcLocation))
                    AddValidationResult("SrcLocation", "Invalid soruce location");
            }   

            ValidateQuantityAtSrcLocation(command);
                
            return results;
        }

        private void ValidateQuantityAtSrcLocation(CreateReleaseDocumentCommand command)
        {
            var groups = command.Lines
                .GroupBy(x => new { a=x.ProductId, b=x.SrcLocation})
                .Select(x => new Stock
                {
                    ProductId = x.First().ProductId,
                    Location = x.First().SrcLocation,
                    Quantity = x.Sum(y=>y.Quantity)
                });

            foreach(var item in groups.Where(g => !string.IsNullOrWhiteSpace(g.Location)))
            {
                var stock = stockSrv.GetStockAtLocation(item.ProductId, item.Location);
                if(stock != null && item.Quantity > stock.Quantity)
                    AddValidationResult("Quantity",
                        "Invalid quantity. Try release more than at source location");
                else if(stock == null)
                    AddValidationResult("SrcLocation",
                        $"Product with id: {item.ProductId} not found at location: {item.Location}");
            }
        }
    }
}