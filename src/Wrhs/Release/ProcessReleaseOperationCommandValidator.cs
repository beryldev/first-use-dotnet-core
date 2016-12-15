using Wrhs.Common;
using Wrhs.Services;

namespace Wrhs.Release
{
    public class ProcessReleaseOperationCommandValidator
        : ProcessOperationCommandValidator<ProcessReleaseOperationCommand>
    {
        public ProcessReleaseOperationCommandValidator(IOperationService operationSrv, 
            IProductService productSrv) : base(operationSrv, productSrv)
        {
        }

        protected override OperationType GetOperationType()
        {
            return OperationType.Release;
        }
    }
}