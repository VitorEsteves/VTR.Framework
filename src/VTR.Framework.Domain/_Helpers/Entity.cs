namespace VTR.Framework.Domain;

public class Entity
{
    public Guid Id { get; private set; }

    public void CreateGuid()
    {
        Id = Guid.NewGuid();
    }
}