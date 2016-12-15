using Wrhs.Core;

namespace Wrhs.Common
{
    public interface IDocumentService
    {
         bool CheckDocumentExistsById(int id);

         Document GetDocumentById(int id);

         ResultPage<Document> GetDocuments();
    }
}