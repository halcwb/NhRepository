using Informedica.DataAccess.Configurations;
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

        public static IRepository<TEnt,TId> CreateInMemorySqLiteRepository<TMap>(string name)
        {
            ConfigurationManager.Instance.AddInMemorySqLiteEnvironment<TMap>(name);
            var envConf = ConfigurationManager.Instance.GetConfiguration(name);
            var session = envConf.GetSessionFactory().OpenSession();
            envConf.BuildSchema(session);
            CurrentSessionContext.Bind(session);

            return new Repository<TEnt, TId>(envConf.GetSessionFactory());
        }
    }
}
