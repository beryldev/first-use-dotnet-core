using System.Linq;
using Wrhs.Core.Exceptions;

namespace Wrhs.Core.CommandHandlers
{
    public class ValidationCmdHndDecorator<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
    {
        private readonly ICommandHandler<TCommand> handler;
        private readonly IValidator<TCommand> validator;

        public ValidationCmdHndDecorator(ICommandHandler handler, IValidator validator)
        {
            this.handler = (ICommandHandler<TCommand>)handler;
            this.validator = (IValidator<TCommand>)validator;
        }

        public void Handle(TCommand command)
        {
            var results = validator.Validate(command);
            if (results.Any())
                throw new CommandValidationException("Invalid command", command, results);

            handler.Handle(command);
        }
    }
}
