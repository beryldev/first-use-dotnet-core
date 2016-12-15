using System;

namespace Wrhs.Core
{
    public class CommandBus : ICommandBus
    {
        private readonly Func<Type, ICommandHandler> handlersFactory;

        public CommandBus(Func<Type, ICommandHandler> handlersFactory)
        {
            this.handlersFactory = handlersFactory;
        }

        public void Send<TCommand>(TCommand command) where TCommand : ICommand
        {
            var handler = handlersFactory(typeof(TCommand)) as ICommandHandler<TCommand>;
            handler.Handle(command);
        }
    }
}