namespace VTR.Framework.Domain.Contracts;

public interface ICryptoAppService
{
    string? Encrypt(string? value);

    string GenerateNumberPassword(int length);
}