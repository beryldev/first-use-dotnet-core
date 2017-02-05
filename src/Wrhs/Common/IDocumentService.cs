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

         ResultPage<Document> FilterDocuments(DocumentType type,
            Dictionary<string, object> filter);

         ResultPage<Document> FilterDocuments(DocumentType type,
            Dictionary<string, object> filter, int page);

         ResultPage<Document> FilterDocuments(DocumentType type,
            Dictionary<string, object> filter, int page, int pageSize);

        void Update(Document document);
    }
}