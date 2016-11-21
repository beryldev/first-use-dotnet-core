using System;
using Microsoft.AspNetCore.Mvc;
using Wrhs.Operations;
using Wrhs.Operations.Relocation;
using Wrhs.WebApp.Utils;

namespace Wrhs.WebApp.Controllers.Operations
{
    [Route("api/operation/relocation")]
    public class RelocationOperationController
        : OperationController<RelocationOperation, RelocationDocument, RelocationRequest>
    {
        public RelocationOperationController(ICache cache) : base(cache)
        {
        }

        protected override RelocationOperation CreateOperation(OperationState<RelocationDocument> state)
        {
            throw new NotImplementedException();
        }

        protected override RelocationOperation CreateOperation(RelocationDocument baseDocument)
        {
            throw new NotImplementedException();
        }

        protected override void DoStep(RelocationOperation operation, RelocationRequest request)
        {
            throw new NotImplementedException();
        }

        protected override OperationState<RelocationDocument> ReadOperationState(RelocationOperation operation)
        {
            throw new NotImplementedException();
        }
    }


    public class RelocationRequest
    {
        
    }
}