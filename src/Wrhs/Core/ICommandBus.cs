namespace Wrhs.Core
{
    public interface ICommandBus
    {
         void Send<TCommand>(TCommand command) where TCommand : ICommand;
    }
}