using Wrhs.Core;

namespace Wrhs.Common
{
    public class ExecuteOperationCommand : ICommand, IValidableOperationInfo
    {
        public string OperationGuid { get; set; }
    }
}