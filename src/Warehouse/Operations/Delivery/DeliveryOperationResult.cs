using System;
using System.Collections.Generic;

namespace Warehouse.Operations.Delivery
{
    public class DeliveryOperationResult
    {
        public ResultStatus Status { get; set; } = ResultStatus.Ok;

        public List<string> ErrorMessages { get; set; } = new List<string>();

        public enum ResultStatus
        {
            Ok = 1,
            Error = 2
        }
    }
}