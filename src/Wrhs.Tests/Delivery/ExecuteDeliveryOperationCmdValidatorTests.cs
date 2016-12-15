using System;
using Wrhs.Common;
using Wrhs.Core;
using Wrhs.Delivery;

namespace Wrhs.Tests.Delivery
{
    public class ExecuteDeliveryOperationCmdValidatorTests
        : ExecuteOperationCmdValidatorTestsBase<ExecuteDeliveryOperationCommand>
    {
        protected override ExecuteDeliveryOperationCommand CreateCommand()
        {
            return new ExecuteDeliveryOperationCommand { OperationGuid = "guid" };
        }

        protected override IValidator<ExecuteDeliveryOperationCommand> CreateValidator(IOperationService operationSrv)
        {
            return new ExecuteDeliveryOperationCommandValidator(operationSrvMock.Object);
        }

        protected override OperationType GetInvalidOperationType()
        {
            return OperationType.Relocation;
        }

        protected override OperationType GetValidOperationType()
        {
            return OperationType.Delivery;
        }
    }
}