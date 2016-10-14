using System.Collections.Generic;

namespace Wrhs.Products
{
    public interface IRepository<T> where T : IEntity
    {
        T Save(T item);

        IEnumerable<T> Get();

        T GetById(int id);

        void Update(T item);

        void Delete(T item);
    }
}