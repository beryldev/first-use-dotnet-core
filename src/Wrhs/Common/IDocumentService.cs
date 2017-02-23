using System;
using Wrhs.Core;

namespace Wrhs.Common
{
    public interface IDocumentService
    {
        bool CheckDocumentExistsById(int id);

        Document GetDocumentById(int id);

        ResultPage<Document> GetDocuments();

        ResultPage<Document> GetDocuments(DocumentType type);

        ResultPage<Document> FilterDocuments(DocumentFilter filter);

        ResultPage<Document> FilterDocuments(DocumentFilter filter, int page);

        ResultPage<Document> FilterDocuments(DocumentFilter filter, int page, int pageSize);

        int Save(Document document);

        void Update(Document document);

        void Delete(Document document);
    }


    public class DocumentFilter
    {
        public DocumentType? Type { get; set; }
        public DocumentState? State { get; set; }
        public DateTime? IssueDate { get; set; }
        public string FullNumber { get; set; } = string.Empty;
    }
}