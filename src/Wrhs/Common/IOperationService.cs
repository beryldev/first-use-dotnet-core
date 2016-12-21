using Wrhs.Core;

namespace Wrhs.Common
{
    public interface IOperationService
    {
        bool CheckOperationGuidExists(string guid);

        Operation GetOperationByGuid(string guid);

        ResultPage<Operation> Get();

        ResultPage<Operation> Get(int page);

        ResultPage<Operation> Get(int page, int pageSize);
    }
}