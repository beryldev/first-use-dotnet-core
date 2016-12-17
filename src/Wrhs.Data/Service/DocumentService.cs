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

        public ResultPage<Document> GetDocuments(DocumentType type)
        {
            return GetDocuments(type, DEFAULT_PAGE, DEFAULT_PAGE_SIZE);
        }

        public ResultPage<Document> GetDocuments(DocumentType type, int page)
        {
            return GetDocuments(type, page, DEFAULT_PAGE_SIZE);
        }

        public ResultPage<Document> GetDocuments(DocumentType type, int page, int pageSize)
        {
            var query = context.Documents.Where(d => d.Type == type);
            return PaginateQuery(query, page, pageSize);
        }

        protected override IQueryable<Document> GetQuery()
        {
            return context.Documents;
        }
    }
}