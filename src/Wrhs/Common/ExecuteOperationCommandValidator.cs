using System;
using System.Collections.Generic;
using System.Linq;
using Wrhs.Core;

namespace Wrhs.Common
{
    public class ExecuteOperationCommandValidator
        : CommandValidator<ExecuteOperationCommand>
    {
        protected readonly IOperationService operationSrv;
        protected readonly OperationInfoValidator operationValidator;

        public ExecuteOperationCommandValidator(IOperationService operationSrv)
        {
            this.operationSrv = operationSrv;
            operationValidator = new OperationInfoValidator(operationSrv);
        }

        public override IEnumerable<ValidationResult> Validate(ExecuteOperationCommand command)
        {
            results.AddRange(operationValidator.Validate(command));

            var operation = operationSrv.GetOperationByGuid(command.OperationGuid);
            switch(operation.Type)
            {
                case OperationType.Delivery:
                    ValidateDelivery(operation);
                    break;
                default:
                    break;
            }

            return results;
        }

        protected void ValidateDelivery(Operation operation)
        {
             var docShifts = operation.Document.Lines
                .GroupBy(l => new {a = l.ProductId, b = l.DstLocation})
                .Select(i => new Shift
                {
                    ProductId = i.First().ProductId,
                    Location = i.First().DstLocation,
                    Quantity = i.Sum(x=>x.Quantity)            
                }).ToList();

            var operShifts = operation.Shifts
                .GroupBy(s => new {a = s.ProductId, b = s.Location})
                .Select(i => new Shift
                {
                    ProductId = i.First().ProductId,
                    Location = i.First().Location,
                    Quantity = i.Sum(x=>x.Quantity)            
                }).ToList();

            foreach(var docShift in docShifts)
            {
                if(!operShifts.Contains(docShift, new ShiftComparer()))
                {
                    AddValidationResult("OperationGuid", "Document not yet processed.");
                    break;
                }
            }
        }


        class ShiftComparer : IEqualityComparer<Shift>
        {
            public bool Equals(Shift x, Shift y)
            {
                return x.ProductId == y.ProductId 
                    && x.Location.ToLower() == y.Location.ToLower()
                    && x.Quantity == y.Quantity;
            }

            public int GetHashCode(Shift obj)
            {
                return $"{obj.ProductId}|{obj.Quantity}|{obj.Location}".GetHashCode();
            }
        }
    }
}