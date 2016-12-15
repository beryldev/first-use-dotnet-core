namespace Wrhs.Common
{
    public interface IOperationPersist
    {
         int Save(Operation operation);

         void Update(Operation operation);
    }
}