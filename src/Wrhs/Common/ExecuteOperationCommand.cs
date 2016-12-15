using Wrhs.Core;

namespace Wrhs.Common
{
    public abstract class ExecuteOperationCommand : ICommand, IValidableOperationInfo
    {
        public string OperationGuid { get; set; }
    }
}