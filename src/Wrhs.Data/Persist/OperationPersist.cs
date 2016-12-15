using Wrhs.Common;

namespace Wrhs.Data.Persist
{
    public class OperationPersist : BaseData, IOperationPersist
    {
        public OperationPersist(WrhsContext context) : base(context)
        {
        }

        public int Save(Operation operation)
        {
            context.Operations.Add(operation);
            context.SaveChanges();

            return operation.Id;
        }

        public void Update(Operation operation)
        {
            context.SaveChanges();
        }
    }
}