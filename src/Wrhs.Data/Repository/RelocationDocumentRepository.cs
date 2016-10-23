using System;
using System.Collections.Generic;
using System.Linq;
using Wrhs.Core;
using Wrhs.Operations.Relocation;

namespace Wrhs.Data.Repository
{
    public class RelocationDocumentRepository : IRepository<RelocationDocument>
    {
        WrhsContext context;

        public RelocationDocumentRepository(WrhsContext context)
        {
            this.context = context;
        }

        public void Delete(RelocationDocument item)
        {
            foreach(var line in item.Lines)
                context.RelocationDocumentLines.Remove(line);

            context.RelocationDocuments.Remove(item);

            context.SaveChanges();
        }

        public IEnumerable<RelocationDocument> Get()
        {
            throw new NotImplementedException();
        }

        public RelocationDocument GetById(int id)
        {
            return context.RelocationDocuments
                .Where(item=>item.Id == id)
                .FirstOrDefault();
        }

        public RelocationDocument Save(RelocationDocument item)
        {
            context.RelocationDocuments.Add(item);
            context.SaveChanges();

            return item;
        }

        public void Update(RelocationDocument item)
        {
            context.RelocationDocuments.Update(item);
            context.SaveChanges();
        }
    }
}