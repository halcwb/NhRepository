namespace Informedica.DataAccess.Entities
{
    public abstract class Entity<TEnt, TId>: IEntity<TEnt, TId>
        where TEnt: IEntity<TEnt, TId>
    {
        private TId _id;

        protected Entity() : this(default(TId))
        {
        }

        protected Entity(TId id)
        {
            _id = id;
        }


        public TId Id
        {
             get
            {
                return _id;
            }
        }

        public abstract bool IsIdentical(TEnt entity);

        public bool IsTransient()
        {
            return false;
        }

    }
}