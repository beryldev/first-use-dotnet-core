using Wrhs.Core;

namespace Wrhs.Documents
{
    public interface IDocumentRegistrator<T> where T : IEntity, INumerableDocument
    {
        void Register(T document);
    }
}