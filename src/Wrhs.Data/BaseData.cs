namespace Wrhs.Data
{
    public abstract class BaseData
    {
        protected readonly WrhsContext context;

        protected BaseData(WrhsContext context)
        {
            this.context = context;
        }
    }
}