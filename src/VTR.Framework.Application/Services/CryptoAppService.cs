using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace VTR.Framework.Application.Services;

public class CryptoAppService : ICryptoAppService
{
    public static string? CreateEncrypt(string value)
    {
        return new CryptoAppService().Encrypt(value);
    }

    public string? Encrypt(string? value)
    {
        if (value == null)
            return null;

        var md5Hasher = MD5.Create();

        byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(value));

        var sb = new StringBuilder();

        for (int i = 0; i < data.Length; i++)
        {
            sb.Append(data[i].ToString("x2"));
        }

        return sb.ToString();
    }

    public string GenerateRandomNumber(int length)
    {
        if (length <= 0)
        {
            return string.Empty;
        }

        var random = new Random();

        string password = random.Next(1, 9).ToString();

        while (password.Length < length)
        {
            password = $"{password}{random.Next(0, 9)}";
        }

        return password;
    }

    public string HideCharactersEmail(string email)
    {
        string regex = @"(.{2}).+@.+(.{2}(?:\..{2,3}){1,2})";
        string replace = "$1*@*$2";

        return Regex.Replace(email, regex, replace);
    }
}