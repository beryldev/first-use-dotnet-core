using Wrhs.Documents;

namespace Wrhs.Operations.Relocation
{
    public class RelocationDocumentBuilderAddLineCommand : DocumentBuilderAddLineCommand
    {
        public string From { get; set; }

        public string To { get; set; }
    }
}