using Wrhs.Core;

namespace Wrhs.Common
{
    public class BeginOperationCommand : ICommand, IValidableOperationInfo
    {
        public int DocumentId { get; set;}

        public string OperationGuid { get; set; }

        public OperationType OperationType { get; set; }
    }
}