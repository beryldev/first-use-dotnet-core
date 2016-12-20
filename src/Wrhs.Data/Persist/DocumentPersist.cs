using System;
using Wrhs.Common;

namespace Wrhs.Data.Persist
{
    public class DocumentPersist : BaseData, IDocumentPersist
    {
        private readonly IDocumentNumerator docNumerator;

        public DocumentPersist(WrhsContext context, IDocumentNumerator docNumerator) 
            : base(context)
        {
            this.docNumerator = docNumerator;
            this.docNumerator.SetContext(context);
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
    }
}