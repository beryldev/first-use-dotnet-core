using System.Collections.Generic;
using System.Linq;
using Wrhs.Common;

namespace Wrhs.Data
{
    public class DocumentNumerator : IDocumentNumerator
    {
        private WrhsContext context;

        private readonly Dictionary<DocumentType, string> prefixMapping;

        public DocumentNumerator(Dictionary<DocumentType, string> prefixMapping)
        {
            this.prefixMapping = prefixMapping;
        }

        public Document AssignNumber(Document document)
        {
            document.FullNumber = "number";
            document.Number = GetNextNumber(document);
            document.Month = document.CreatedAt.Month;
            document.Year = document.CreatedAt.Year;

            return document;
        }

        public void SetContext(WrhsContext context)
        {
            this.context = context;
        }

        private int GetNextNumber(Document  document)
        {
            var query = context.Documents.Where(d => d.Type == document.Type
                    && d.Year == document.CreatedAt.Year
                    && d.Month == document.CreatedAt.Month);

            if(!query.Any())
                return 1;

            var max = query.Max(d => d.Number);
            return max + 1;
        }
    }
}