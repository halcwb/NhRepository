using Informedica.EntityRepository.Entities;
using Informedica.NhRepository.NHibernate;
using NHibernate;

namespace Informedica.NhRepository
{
    public class Repository<TEnt, TId>: EntityRepository.Repository<TEnt, TId> 
        where TEnt : class, IEntity<TEnt, TId>
    {
        public Repository(ISessionFactory factory) : base(new NHibernateRepository<TEnt, TId>(factory))
        {
        }
    }
}
