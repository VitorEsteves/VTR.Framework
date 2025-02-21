namespace VTR.Framework.Domain.Email;

public class EMailAddress(string emailAddress, string? name = null)
{
    public string? Name { get; set; } = name;
    public string EmailAddress { get; set; } = emailAddress;

    public override string ToString()
    {
        return Name == null ? EmailAddress : $"{Name} <{EmailAddress}>";
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
            EMailAddress otherAddress = (EMailAddress)obj;
            return this.EmailAddress == otherAddress.EmailAddress && this.Name == otherAddress.Name;
        }
    }
}
