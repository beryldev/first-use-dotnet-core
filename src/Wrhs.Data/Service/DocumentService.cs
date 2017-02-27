using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Wrhs.Common;
using Wrhs.Core;

namespace Wrhs.Data.Service
{
    public class DocumentService : BaseService<Document>,  IDocumentService
    {
        private readonly IDocumentNumerator docNumerator;

        public DocumentService(WrhsContext context, IDocumentNumerator docNumerator) : base(context)
        {
            this.docNumerator = docNumerator;
            this.docNumerator.SetContext(context);
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

        public ResultPage<Document> FilterDocuments(DocumentFilter filter)
        {
            return FilterDocuments(filter, DEFAULT_PAGE, DEFAULT_PAGE_SIZE);
        }

        public ResultPage<Document> FilterDocuments(DocumentFilter filter, int page)
        {
            return FilterDocuments(filter, page, DEFAULT_PAGE_SIZE);
        }

        public ResultPage<Document> FilterDocuments(DocumentFilter filter, int page, int pageSize)
        {
            var query = context.Documents
                .Where(x => !filter.State.HasValue || x.State == filter.State)
                .Where(x => !filter.Type.HasValue || x.Type == filter.Type)
                .Where(x => !filter.IssueDate.HasValue || x.IssueDate == filter.IssueDate)
                .Where(x => string.IsNullOrWhiteSpace(filter.FullNumber) 
                    || x.FullNumber.ToUpper().Contains(filter.FullNumber.ToUpper()));

            return PaginateQuery(query, page, pageSize);
        }

        public int Save(Document document)
        {
            document.IssueDate = DateTime.Now;
            document = docNumerator.AssignNumber(document);
            context.Documents.Add(document);
            context.SaveChanges();

            return document.Id;
        }

        public void Update(Document document)
        {
            context.Documents.Update(document);
            context.SaveChanges();
        }

        public void Delete(Document document)
        {
            if(document != null)
            {
                context.Documents.Remove(document);
                context.SaveChanges();
            }
        }

        protected override IQueryable<Document> GetQuery()
        {
            return context.Documents;
        }
    }
}