using Informedica.EntityRepository.Entities;
using NHibernate;

namespace Informedica.DataAccess.Repositories
{
    public class Repository<TEnt, TId>: EntityRepository.Repository<TEnt, TId> 
        where TEnt : class, IEntity<TEnt, TId>
    {
        public Repository(ISessionFactory factory) : base(new NHibernateRepository<TEnt, TId>(factory))
        {
        }
    }
}
