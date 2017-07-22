using Wrhs.Core;

namespace Wrhs.Common
{
    public class ChangeDocStateCommandHandler : ICommandHandler<ChangeDocStateCommand>
    {
        private readonly IEventBus eventBus;
        private readonly IDocumentService docSrv;

        public ChangeDocStateCommandHandler(IEventBus eventBus, IDocumentService docSrv) 
        {
            this.eventBus = eventBus;
            this.docSrv = docSrv;
        }

        public void Handle(ChangeDocStateCommand command)
        {
            var document = docSrv.GetDocumentById(command.DocumentId);
            var oldState = document.State;
            document.State = command.NewState;
            docSrv.Update(document);

            var evt = new ChangeDocStateEvent
            {
                DocumentId = command.DocumentId,
                OldState = oldState,
                NewState = command.NewState
            };

            eventBus.Publish(evt);
        }
    }
}