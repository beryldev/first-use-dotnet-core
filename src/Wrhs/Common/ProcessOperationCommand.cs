using Wrhs.Core;

namespace Wrhs.Common
{
    public abstract class ProcessOperationCommand : ICommand, IValidableProductInfo
        ,IValidableOperationInfo
    {
        public string OperationGuid { get; set; }

        public string DstLocation { get; set; }

        public string SrcLocation { get; set; }

        public int ProductId { get; set; }

        public decimal Quantity { get; set; }
    }
}