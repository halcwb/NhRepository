using Informedica.DataAccess.Databases;
using Informedica.EntityRepository;
using Informedica.EntityRepository.Entities;
using NHibernate;
using NHibernate.Context;

namespace Informedica.DataAccess.Repositories
{
    public class Repository<TEnt, TId>: EntityRepository.Repository<TEnt, TId> 
        where TEnt : class, IEntity<TEnt, TId>
    {
        public Repository(ISessionFactory factory) : base(new NHibernateRepository<TEnt, TId>(factory))
        {
        }

        public static IRepository<TEnt,TId> CreateInMemorySqLiteRepository<TMap>()
        {
            var creator = SessionFactoryCreator.CreateInMemorySqlLiteFactoryCreator<TMap>();
            var fact = creator.CreateSessionFactory();
            var session = fact.OpenSession();
            creator.BuildSchema(session);
            CurrentSessionContext.Bind(session);

            return new Repository<TEnt, TId>(creator.CreateSessionFactory());
        }
    }
}
