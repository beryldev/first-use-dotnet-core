namespace Wrhs.Common
{
    public interface IOperationService
    {
         bool CheckOperationGuidExists(string guid);

         Operation GetOperationByGuid(string guid);
    }
}