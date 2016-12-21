using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Wrhs.Common;

namespace Wrhs.Data.Service
{
    public class OperationService : BaseService<Operation>, IOperationService
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

        protected override Dictionary<string, Func<Operation, object, bool>> GetFilterMapping()
        {
            return new Dictionary<string, Func<Operation, object, bool>>
            {
                {"operationGuid", (Operation o, object value) => o.OperationGuid == (String)value}
            };
        }

        protected override IQueryable<Operation> GetQuery()
        {
            return context.Operations;
        }
    }
}