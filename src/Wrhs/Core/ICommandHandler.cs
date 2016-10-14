namespace Wrhs.Products.Core
{
    public interface ICommandHandler<TCommand>
    {
        void Handle(TCommand command);
    }
}