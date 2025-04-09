namespace VTR.Framework.Domain.Email;

public class EmailAddress(string email, string? name = null)
{
    public string? Name { get; set; } = name;
    public string Email { get; set; } = email;

    public override string ToString()
    {
        return Name == null ? Email : $"{Name} <{Email}>";
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            EmailAddress otherAddress = (EmailAddress)obj;
            return this.Email == otherAddress.Email && this.Name == otherAddress.Name;
        }
    }
}