using Wrhs.Core;
using Wrhs.Documents;

namespace Wrhs.Operations.Relocation
{
    public class RelocationDocumentLine : DocumentLine, IEntity
    {
        public string From { get; set; }

        public string To { get; set; }
    }
}