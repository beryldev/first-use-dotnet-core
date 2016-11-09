using Wrhs.Documents;

namespace Wrhs.Operations.Relocation
{
    public class RelocDocAddLineCmd : DocAddLineCmd, IDocAddLineCmd
    {
        public string From { get; set; }

        public string To { get; set; }
    }
}