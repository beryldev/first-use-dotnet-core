using Wrhs.Documents;

namespace Wrhs.Operations.Relocation
{
    public class RelocDocBuilderAddLineCmd : DocBuilderAddLineCmd
    {
        public string From { get; set; }

        public string To { get; set; }
    }
}