namespace VTR.Framework.Domain.Contracts;

public interface ICryptoAppService
{
    string? Encrypt(string? value);

    string GenerateRandomNumber(int length);

    string HideCharactersEmail(string email);
}