namespace Informedica.DataAccess.Entities
{
    public interface IEntity<TEnt, TId> 
        where TEnt: IEntity<TEnt, TId>
    {
        TId Id { get; }
        bool IsIdentical(TEnt entity);
        bool IsTransient();
    }
}