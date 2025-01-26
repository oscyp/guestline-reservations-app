namespace guestline.reservations.app.Helpers;

public static class DateHelpers
{
    public static DateOnly ParseDate(string dateToParse)
    {
        if (DateOnly.TryParseExact(
                dateToParse,
                "yyyyMMdd",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out var date))
        {
            return date;
        }
        throw new FormatException($"Invalid date format: {dateToParse}");
    }
}