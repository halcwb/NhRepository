using System.Collections.Generic;
using Informedica.DataAccess.Entities;

namespace Informedica.DataAccess.Repositories
{
    public interface IRepository<TEnt, TId>: IEnumerable<TEnt>
        where TEnt: IEntity<TEnt, TId>
    {
        TEnt GetById(TId id);

        bool Contains(TEnt entity);
        void Add(TEnt entity);
        void Remove(TEnt entity);
        int Count { get; }
    }
}
