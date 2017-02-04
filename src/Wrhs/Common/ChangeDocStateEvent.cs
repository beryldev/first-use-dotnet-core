using Wrhs.Core;

namespace Wrhs.Common
{
    public class ChangeDocStateEvent : IEvent
    {
        public int DocumentId { get; set; }

        public DocumentState OldState { get; set; }

        public DocumentState NewState { get; set; } 
    }
}