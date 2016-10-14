namespace Wrhs.Core
{
    public interface ICommandHandler<TCommand>
    {
        void Handle(TCommand command);
    }
}