using System;
using System.Linq;
using Wrhs.Core;

namespace Wrhs.Documents
{
    public class DocumentRegistrator<T> : IDocumentRegistrator<T> where T : IEntity, INumerableDocument
    {
        IRepository<T> repository;

        string documentSymbol;

        public DocumentRegistrator(IRepository<T> repository, string documentSymbol)
        {
            this.repository = repository;
            this.documentSymbol = documentSymbol;
        }

        public void Register(T document)
        {
            AssignDocumentNumber(document);
            repository.Save(document);
        }

        protected void AssignDocumentNumber(T document)
        {
            var number = repository.Get().Count() > 0 
                ? repository.Get().Max(doc=>doc.Number)+1 : 1;
            var year = DateTime.Today.Year;
            document.Number = number;
            document.FullNumber = $"{documentSymbol}/{number}/{year}";
        }
    }
}