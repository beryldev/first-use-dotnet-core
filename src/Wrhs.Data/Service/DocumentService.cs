using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Wrhs.Common;
using Wrhs.Core;

namespace Wrhs.Data.Service
{
    public class DocumentService : BaseService<Document>,  IDocumentService
    {
        public DocumentService(WrhsContext context) : base(context)
        {
        }

        public bool CheckDocumentExistsById(int id)
        {
            return context.Documents.Any(d => d.Id == id);
        }

        public Document GetDocumentById(int id)
        {
            return context.Documents
                .Where(d => d.Id == id)
                .Include(d => d.Lines)
                    .ThenInclude(l => l.Product)
                .FirstOrDefault();
        }

        public ResultPage<Document> GetDocuments()
        {
            var items = context.Documents.ToList();
            return new ResultPage<Document>(items, 0, 0);
        }

        protected override IQueryable<Document> GetQuery()
        {
            return context.Documents;
        }
    }
}