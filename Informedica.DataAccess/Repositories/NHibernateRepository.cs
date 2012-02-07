using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Informedica.EntityRepository;
using Informedica.EntityRepository.Entities;
using NHibernate;
using NHibernate.Linq;

namespace Informedica.DataAccess.Repositories
{
    public class NHibernateRepository<TEnt, TId> : NHibernateBase, IRepository<TEnt, TId>
        where TEnt : class, IEntity<TEnt, TId>
    {
        #region Repository

        public NHibernateRepository(ISessionFactory factory) : base(factory) { }

        public IEnumerator<TEnt> GetEnumerator()
        {
            return Transact(() => Session.Query<TEnt>().GetEnumerator());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Transact(() => GetEnumerator());
        }

        #endregion        
        
        #region Query

        public TEnt GetById(TId id)
        {
            return Transact(() => Session.Get<TEnt>(id));
        }

        public virtual int Count
        {
            // ToDo: This causes N+1 select problem
            get { return Transact(() => Session.Query<TEnt>().Count()); }
        }

        public IEnumerable<TEnt> RunQuery(IQuery query)
        {
            return Transact(() => query.Enumerable<TEnt>());
        }

        public IQuery CreateQuery(string queryString)
        {
            return Transact(() => Session.CreateQuery(queryString));
        }

        public IEnumerable<TEnt> RunQueryOver(IQueryOver<TEnt> query)
        {
            return Transact(() => query.List<TEnt>());
        }

        public IQueryOver<TEnt> CreateQueryOver()
        {
            return Transact(() => Session.QueryOver<TEnt>());
        }

        #endregion

        #region Add and Remove

        public virtual void Add(TEnt item, IEqualityComparer<TEnt> comparer)
        {
            // Need to check
            // because item can be added by associated item
            if (this.Contains(item, comparer)) return;
            Transact(() => Session.Save(item));
        }

        public virtual bool Contains(TEnt item)
        {
            return Transact(() => ContainsItem(item));
        }

        private bool ContainsItem(TEnt ent)
        {
            var test = this.FirstOrDefault();
            return this.Any(item => ReferenceEquals(item, ent));
        }

        public virtual void Add(TEnt entity)
        {
            Transact(() => Session.Save(entity));
        }

        public virtual void Remove(TEnt item)
        {
            Transact(() => Session.Delete(item));
        }

        public virtual void Remove(TId id)
        {
            var ent = GetById(id);
            Remove(ent);
        }

        #endregion

    }
}
