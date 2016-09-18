using System;
using System.Collections.Generic;

namespace Wrhs
{
    public interface IRepository<T> where T : IEntity
    {
        T Save(T item);

        IEnumerable<T> Get();
    }
}