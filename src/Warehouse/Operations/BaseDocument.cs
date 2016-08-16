using System;

namespace Warehouse.Operations
{
    public class BaseDocument : IOperationDocument
    {
        public DateTime OperationDate { get; set; }
    }
}