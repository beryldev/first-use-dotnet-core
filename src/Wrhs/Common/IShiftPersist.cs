namespace Wrhs.Common
{
    public interface IShiftPersist
    {
         int Save(Shift shift);

         void Update(Shift shift);
    }
}