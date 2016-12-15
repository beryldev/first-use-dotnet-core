using Wrhs.Common;

namespace Wrhs.Data.Persist
{
    public class ShiftPersist : BaseData, IShiftPersist
    {
        public ShiftPersist(WrhsContext context) : base(context)
        {
        }

        public int Save(Shift shift)
        {
            context.Shifts.Add(shift);
            context.SaveChanges();

            return shift.Id;
        }

        public void Update(Shift shift)
        {
            context.SaveChanges();
        }
    }
}