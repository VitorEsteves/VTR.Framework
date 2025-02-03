namespace VTR.Framework.Common;

public class Age
{
    public static int CalculateYears(DateTime? date)
    {
        DateTime today = DateTime.Today;

        if (date is null || today < date)
        {
            return 0;
        }

        int years = today.Year - date.Value.Year;
        if (today.Month < date.Value.Month || (today.Month == date.Value.Month && today.Day < date.Value.Day))
        {
            years--;
        }

        return years;
    }
}