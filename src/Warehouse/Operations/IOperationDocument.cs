using System;
using System.Collections.Generic;

namespace Warehouse.Operations
{
    public interface IOperationDocument
    {
        DateTime OperationDate { get; set; }
    }
}