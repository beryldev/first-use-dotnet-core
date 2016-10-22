using System;
using System.Collections.Generic;
using System.Linq;
using Wrhs.Core;
using Wrhs.Operations.Delivery;

namespace Wrhs.Data.Repository
{
    public class DeliveryDocumentRepository 
        : IRepository<DeliveryDocument>
    {
        WrhsContext context;

        public DeliveryDocumentRepository(WrhsContext context)
        {
            this.context = context;
        }

        public void Delete(DeliveryDocument item)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DeliveryDocument> Get()
        {
            return context.DeliveryDocuments;
        }

        public DeliveryDocument GetById(int id)
        {
            return context.DeliveryDocuments
                .Where(item=>item.Id==id)
                .FirstOrDefault();
        }

        public DeliveryDocument Save(DeliveryDocument item)
        {
            context.DeliveryDocuments.Add(item);
            context.SaveChanges();

            return item;
        }

        public void Update(DeliveryDocument item)
        {
            context.DeliveryDocuments.Update(item);
            context.SaveChanges();
        }
    }
}