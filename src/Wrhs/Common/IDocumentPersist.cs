namespace Wrhs.Common
{
    public interface IDocumentPersist
    {
         int Save(Document document);

         void Update(Document document);

         void Delete(Document document);
    }
}