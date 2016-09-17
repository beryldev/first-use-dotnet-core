using System;

namespace Wrhs
{
    public interface IRepository<T> where T : IEntity
    {
        T Save(T item);
    }
}