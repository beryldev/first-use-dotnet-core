using System;
using System.Collections.Generic;
using System.Linq;
using Wrhs.Core;
using Wrhs.Operations.Release;

namespace Wrhs.Data.Repository
{
    public class ReleaseDocumentRepository : IRepository<ReleaseDocument>
    {
        WrhsContext context;

        public ReleaseDocumentRepository(WrhsContext context)
        {
            this.context = context;
        }

        public void Delete(ReleaseDocument item)
        {
            foreach(var line in item.Lines)
                context.ReleaseDocumentLines.Remove(line);

            context.ReleaseDocuments.Remove(item);
            context.SaveChanges();
        }

        public IEnumerable<ReleaseDocument> Get()
        {
            return context.ReleaseDocuments;
        }

        public ReleaseDocument GetById(int id)
        {
            return context.ReleaseDocuments
                .Where(item=>item.Id == id)
                .FirstOrDefault();
        }

        public ReleaseDocument Save(ReleaseDocument item)
        {
            context.ReleaseDocuments.Add(item);
            context.SaveChanges();

            return item;
        }

        public void Update(ReleaseDocument item)
        {
            context.ReleaseDocuments.Update(item);
            context.SaveChanges();
        }
    }
}