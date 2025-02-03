namespace VTR.Framework.Domain;

public class EntityInfo : Entity
{
    public Guid? InfoInsertUserId { get; private set; }

    public DateTime? InfoInsertDataUTC { get; private set; }

    public Guid? InfoUpdateUserId { get; private set; }

    public DateTime? InfoUpdateDataUTC { get; private set; }

    public void InformInsert(Guid? userId = null)
    {
        InfoInsertUserId = userId;
        InfoInsertDataUTC = DateTime.UtcNow;
    }

    public void InformUpdate(Guid? userId = null)
    {
        InfoInsertUserId = userId;
        InfoUpdateDataUTC = DateTime.UtcNow;
    }
}