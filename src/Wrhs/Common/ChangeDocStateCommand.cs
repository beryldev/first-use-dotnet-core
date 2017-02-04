using Wrhs.Core;

namespace Wrhs.Common
{
    public class ChangeDocStateCommand : ICommand
    {
        public int DocumentId { get; set; }

        public DocumentState NewState { get; set; }
    }
}