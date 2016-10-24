using System;
using Wrhs.Core.Search;
using Wrhs.Core.Search.Interfaces;

namespace Wrhs.Operations.Delivery
{
    public class DeliveryDocSearchCriteria : SearchCriteria<DeliveryDocument>
    {
        public override event EventHandler<Func<DeliveryDocument, bool>> OnBuildQuery;

        public DeliveryDocSearchCriteria WhereFullNumber(Condition cond, string value)
        {
            if(cond == Condition.Equal)
                OnBuildQuery(this, (DeliveryDocument doc) => { return doc.FullNumber == value; });
            else if(cond == Condition.Contains)
                OnBuildQuery(this, (DeliveryDocument doc) => { return doc.FullNumber.Contains(value); });

            return this;
        }

        public DeliveryDocSearchCriteria WhereIssueDate(Condition cond, DateTime date)
        {
            OnBuildQuery(this, (DeliveryDocument doc) => { return doc.IssueDate == date; });

            return this;
        }
    }


    public class DeliveryDocSearchCriteriaFactory : ISearchCriteriaFactory<DeliveryDocument>
    {
        public ISearchCriteria<DeliveryDocument> Create()
        {
            return new DeliveryDocSearchCriteria();
        }
    }
}