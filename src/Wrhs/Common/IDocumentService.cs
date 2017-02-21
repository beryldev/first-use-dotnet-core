using System.Collections.Generic;
using Wrhs.Core;

namespace Wrhs.Common
{
    public interface IDocumentService
    {
        bool CheckDocumentExistsById(int id);

        Document GetDocumentById(int id);

        ResultPage<Document> GetDocuments();

        ResultPage<Document> GetDocuments(DocumentType type);

        ResultPage<Document> FilterDocuments(Dictionary<string, object> filter);

        ResultPage<Document> FilterDocuments(Dictionary<string, object> filter, int page);

        ResultPage<Document> FilterDocuments(Dictionary<string, object> filter, int page, int pageSize);

        int Save(Document document);

        void Update(Document document);

        void Delete(Document document);
    }
}