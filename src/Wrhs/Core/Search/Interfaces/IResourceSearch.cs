namespace Wrhs.Core.Search.Interfaces
{
    public interface IResourceSearch<T> where T : IEntity
    {
        IPaginateResult<T> Exec(ISearchCriteria<T>  criteria);

        ISearchCriteria<T> MakeCriteria();
    }
}