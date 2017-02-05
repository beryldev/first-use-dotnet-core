using System;
using System.Collections.Generic;
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
            return PaginateQuery(context.Documents, DEFAULT_PAGE, DEFAULT_PAGE_SIZE);
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

        public ResultPage<Document> FilterDocuments(DocumentType type,
            Dictionary<string, object> filter)
        {
            return FilterDocuments(type, filter, DEFAULT_PAGE, DEFAULT_PAGE_SIZE);
        }

        public ResultPage<Document> FilterDocuments(DocumentType type,
            Dictionary<string, object> filter, int page)
        {
            return FilterDocuments(type, filter, page, DEFAULT_PAGE_SIZE);
        }

        public ResultPage<Document> FilterDocuments(DocumentType type,
            Dictionary<string, object> filter, int page, int pageSize)
        {
            var query = context.Documents.Where(d => d.Type == type);
            return Filter(query, filter, page, pageSize);
        }

        public void Update(Document document)
        {
            context.Documents.Update(document);
            context.SaveChanges();
        }

        protected override Dictionary<string, Func<Document, object, bool>> GetFilterMapping()
        {
            var mapping = new Dictionary<string, Func<Document, object, bool>>
            {
                {"fullnumber", (Document p, object val) => p.FullNumber.Contains(val as string) },
                {"issuedate", (Document p, object val) => p.IssueDate == (DateTime)val },
            };

            return mapping;
        }

        protected override IQueryable<Document> GetQuery()
        {
            return context.Documents;
        }
    }
}