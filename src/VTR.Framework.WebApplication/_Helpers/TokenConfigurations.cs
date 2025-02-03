namespace VTR.Framework.API;

public class TokenConfigurations
{
    public string? Audience { get; set; }
    public string? Issuer { get; set; }
    public int Seconds { get; set; }
    public string? SecretJwtKey { get; set; }
}