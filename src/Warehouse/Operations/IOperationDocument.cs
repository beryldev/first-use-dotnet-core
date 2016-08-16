using System;

namespace Warehouse.Operations
{
    public interface IOperationDocument
    {
        DateTime OperationDate { get; set; }
    }
}