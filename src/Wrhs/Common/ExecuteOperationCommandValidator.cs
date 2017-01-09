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
                case OperationType.Release:
                    ValidateRelease(operation);
                    break;
                case OperationType.Relocation:
                    ValidateRelocation(operation);
                    break;
                default:
                    break;
            }

            return results;
        }

        protected void ValidateDelivery(Operation operation)
        {
            var docShifts = GetDocInShifts(operation);

            var operShifts = GetOperShifts(operation);

            CompareShiftSets(docShifts, operShifts);
        }

        protected void ValidateRelease(Operation operation)
        {
            var docShifts = GetDocOutShifts(operation);

            var operShifts = GetOperShifts(operation);

            CompareShiftSets(docShifts, operShifts);
        }

        protected void ValidateRelocation(Operation operation)
        {
            var docShifts = GetDocOutShifts(operation);
            docShifts.AddRange(GetDocInShifts(operation));
                
            var operShifts = GetOperShifts(operation);

            CompareShiftSets(docShifts, operShifts);
        }

        protected List<Shift> GetDocInShifts(Operation operation)
        {
            return operation.Document.Lines
                .GroupBy(l => new { a = l.ProductId, b = l.DstLocation})
                .Select(i => new Shift
                {
                    ProductId = i.First().ProductId,
                    Location = i.First().DstLocation,
                    Quantity = i.Sum(x=>x.Quantity)
                }).ToList();
        }

        protected List<Shift> GetDocOutShifts(Operation operation)
        {
            return operation.Document.Lines
                .GroupBy(l => new {a = l.ProductId, b = l.SrcLocation})
                .Select(i => new Shift
                {
                    ProductId = i.First().ProductId,
                    Location = i.First().SrcLocation,
                    Quantity = i.Sum(x=>x.Quantity) * (-1)            
                }).ToList();
        }

        protected List<Shift> GetOperShifts(Operation operation)
        {
            return operation.Shifts
                .GroupBy(s => new {a = s.ProductId, b = s.Location})
                .Select(i => new Shift
                {
                    ProductId = i.First().ProductId,
                    Location = i.First().Location,
                    Quantity = i.Sum(x=>x.Quantity)            
                }).ToList();
        }

        protected void CompareShiftSets(IEnumerable<Shift> docShifts, IEnumerable<Shift> operShifts)
        {
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