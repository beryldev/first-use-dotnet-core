using System.Linq;
using Microsoft.EntityFrameworkCore;
using Wrhs.Common;

namespace Wrhs.Data.Service
{
    public class OperationService : BaseData, IOperationService
    {
        public OperationService(WrhsContext context) : base(context)
        {
        }

        public bool CheckOperationGuidExists(string guid)
        {
            return context.Operations.Any(o => o.OperationGuid == guid);
        }

        public Operation GetOperationByGuid(string guid)
        {
            return context.Operations
                .Where(o => o.OperationGuid == guid)
                .Include(o => o.Document)
                    .ThenInclude(d => d.Lines)
                        .ThenInclude(l => l.Product)
                .Include(o => o.Shifts)
                    .ThenInclude(s => s.Product)
                .FirstOrDefault();
        }
    }
}