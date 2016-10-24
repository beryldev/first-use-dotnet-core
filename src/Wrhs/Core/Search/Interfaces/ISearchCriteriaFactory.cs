
namespace Wrhs.Core.Search.Interfaces
{
    public interface ISearchCriteriaFactory<T> where T : IEntity
    {
        ISearchCriteria<T> Create();
    }
}