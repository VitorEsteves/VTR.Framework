using System.Globalization;

namespace VTR.Framework.Common.Extensions;

public static class StringExtension
{
    public static int? ToInt(this string? value, params string[] listRemover)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        if (listRemover != null && listRemover.Length > 0)
        {
            foreach (var remover in listRemover)
            {
                value = value.Replace(remover, string.Empty);
            }
        }

        return int.Parse(value.Replace(".", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty));
    }

    public static long? ToLong(this string? value, params string[] listRemover)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        if (listRemover != null && listRemover.Length > 0)
        {
            foreach (var remover in listRemover)
            {
                value = value.Replace(remover, string.Empty);
            }
        }

        return long.Parse(value.Replace(".", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty));
    }

    public static DateTime? ToDate(this string? value, string format)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        if (DateTime.TryParseExact(value, format, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out DateTime result))
        {
            return result;
        }

        return null;
    }

    public static TimeSpan? ToTime(this string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        if (value.Length > 5)
        {
            if (TimeSpan.TryParseExact(value, "hh\\:mm\\:ss", CultureInfo.InvariantCulture, out TimeSpan result))
                return result;
        }
        else
        {
            if (TimeSpan.TryParseExact(value, "hh\\:mm", CultureInfo.InvariantCulture, out TimeSpan result))
                return result;
        }

        return null;
    }

    public static string? ToPascalCase(this string? value, int minLength = 2, string[]? exceptions = null)
    {
        if (value is null)
            return null;

        string[] values = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < values.Length; i++)
        {
            values[i] = values[i].ToLower();

            if (values[i].Length > minLength)
            {
                if (exceptions != null && exceptions.Contains(values[i]))
                    continue;

                values[i] = char.ToUpper(values[i][0]) + values[i].Substring(1);
            }
        }

        return string.Join(' ', values);
    }
}