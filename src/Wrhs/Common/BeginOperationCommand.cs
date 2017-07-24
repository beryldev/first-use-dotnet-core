using System;
using Wrhs.Core;

namespace Wrhs.Common
{
    public class BeginOperationCommand : ICommand, IValidableOperationInfo
    {
        public int DocumentId { get; set;}

        public string OperationGuid { get; }

        public OperationType OperationType { get; set; }

        public BeginOperationCommand()
        {
            OperationGuid = Guid.NewGuid().ToString();
        }
    }
}