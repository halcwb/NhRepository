using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Informedica.EntityRepository;
using Informedica.EntityRepository.Entities;
using NHibernate;
using NHibernate.Linq;

namespace Informedica.NhRepository.NHibernate
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
            return Transact(() => Session.Get<TEnt>(item.Id)) != null;
        }

        public virtual void Add(TEnt entity)
        {
            Transact(() => Session.Save(entity));
        }

        public virtual void Remove(TEnt item)
        {
            // ToDo: Check tests whether this can be avoided
            // item can be removed by removal of associated item
            if (!Contains(item)) return;

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
