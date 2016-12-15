using Wrhs.Common;

namespace Wrhs.Data.Persist
{
    public class DocumentPersist : BaseData, IDocumentPersist
    {
        public DocumentPersist(WrhsContext context) : base(context)
        {
            
        }

        public int Save(Document document)
        {
            context.Documents.Add(document);
            context.SaveChanges();

            return document.Id;
        }

        public void Update(Document document)
        {
            context.SaveChanges();
        }
    }
}