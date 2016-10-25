using System;
using Wrhs.Core.Search.Interfaces;
using Wrhs.Documents;

namespace Wrhs.Core.Search
{
    public class DocumentSearchCriteria<T> : SearchCriteria<T> where T : ISearchableDocument, IEntity
    {
        public override event EventHandler<Func<T, bool>> OnBuildQuery;

        public DocumentSearchCriteria<T> WhereFullNumber(Condition cond, string value)
        {
            if(cond == Condition.Equal)
                OnBuildQuery(this, (T doc) => { return doc.FullNumber == value; });
            else if(cond == Condition.Contains)
                OnBuildQuery(this, (T doc) => { return doc.FullNumber.Contains(value); });

            return this;
        }

        public DocumentSearchCriteria<T> WhereIssueDate(Condition cond, DateTime date)
        {
            OnBuildQuery(this, (T doc) => { return doc.IssueDate == date; });

            return this;
        }
    }


    public class DocumentSearchCriteriaFactory<T> : ISearchCriteriaFactory<T> where T :ISearchableDocument, IEntity
    {
        public ISearchCriteria<T> Create()
        {
            return new DocumentSearchCriteria<T>();
        }
    }
}