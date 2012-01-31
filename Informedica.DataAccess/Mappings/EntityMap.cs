using FluentNHibernate.Mapping;
using Informedica.EntityRepository.Entities;

namespace Informedica.DataAccess.Mappings
{
    public abstract class EntityMap<TEnt, TId> : ClassMap<TEnt>
        where TEnt:class, IEntity<TEnt, TId> 
    {
        protected EntityMap()
        {
            Map();
        } 

        private void Map()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            SelectBeforeUpdate();
        }
    }
}