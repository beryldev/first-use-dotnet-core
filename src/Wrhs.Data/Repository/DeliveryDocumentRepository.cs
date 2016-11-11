using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
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
            //TODO temporary
            foreach(var line in item.Lines)
                context.DeliveryDocumentLines.Remove(line as DeliveryDocumentLine);

            context.DeliveryDocuments.Remove(item);
            context.SaveChanges();
        }

        public IEnumerable<DeliveryDocument> Get()
        {
            return context.DeliveryDocuments.Include(m=>m.Lines);
        }

        public DeliveryDocument GetById(int id)
        {
            return context.DeliveryDocuments
                .Where(item=>item.Id==id)
                .FirstOrDefault();
        }

        public DeliveryDocument Save(DeliveryDocument item)
        {
            item.Lines.ForEach(l=>l.Product = context.Products.Where(p=>p.Id==l.Product.Id).First());
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