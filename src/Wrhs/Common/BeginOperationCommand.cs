using Wrhs.Core;

namespace Wrhs.Common
{
    public abstract class BeginOperationCommand : ICommand, IValidableOperationInfo
    {
        public int DocumentId { get; set;}

        public string OperationGuid { get; set; }
    }
}