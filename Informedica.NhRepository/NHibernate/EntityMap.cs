using FluentNHibernate.Mapping;
using Informedica.EntityRepository.Entities;

namespace Informedica.NhRepository.NHibernate
{
    public abstract class EntityMap<TEnt, TId> : ClassMap<TEnt>
        where TEnt: IEntity<TEnt, TId> 
    {
        protected EntityMap()
        {
            Map();
        } 

        private void Map()
        {
            Id(x => x.Id).GeneratedBy.Increment();
            SelectBeforeUpdate();
        }
    }
}