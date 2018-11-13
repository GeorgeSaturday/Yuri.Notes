using System.Collections.Generic;

namespace Yuri.Notes.DB
{
    public interface IEntityRepository<T> where T : class, IEntity
    {
        T Create();

        T Load(long id);

        void Save(T entity);

        void Delete(T entity);

        IEnumerable<T> GetAll();
    }
}
