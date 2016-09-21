using System;
using Wrhs.Documents;

namespace Wrhs.Operations.Relocation
{
    public class RelocationOperation
    {
        public Document BaseDocument
        {
            get { return baseDocument; }
        }

        public RelocationDocument BaseRelocationDocument
        {
            get 
            { 
                return baseDocument.GetType() == typeof(RelocationDocument) ? (RelocationDocument)baseDocument : null; 
            }
            set { baseDocument = value; }
        }

        Document baseDocument;

        public void SetBaseDocument(RelocationDocument doc)
        {
            baseDocument = doc;
        }

        public OperationResult Preform(IAllocationService allocService)
        {
            return null;
        }
    }
}