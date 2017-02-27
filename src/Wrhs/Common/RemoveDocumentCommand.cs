using Wrhs.Core;

namespace Wrhs.Common
{
    public class RemoveDocumentCommand : ICommand
    {
        public int DocumentId { get; set; }
    }
}