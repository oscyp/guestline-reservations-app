namespace guestline.reservations.app.Helpers;

public static class ArgsParserHelper
{
    public static (string hotelId, DateOnly dateFrom, DateOnly? dateTo, string roomType) ParseAvailability(string input)
    {
        var rawParams = input.Trim();
        rawParams = rawParams.Substring("Availability(".Length);
        rawParams = rawParams.Remove(rawParams.Length - 1); // remove closing ')' -> "H1, 20240901-20240903, DBL"

        var parts = rawParams.Split(',');

        var hotelId = parts[0].Trim(); // "H1"
        var dateSegment = parts[1].Trim(); // "20240901-20240903" or "20240901"
        var roomType = parts[2].Trim(); // "DBL" or "SGL"
    
        DateOnly dateFrom;
        DateOnly? dateTo = null;
        if (dateSegment.Contains('-'))
        {
            var dateRange = dateSegment.Split('-');
            dateFrom = DateHelpers.ParseDate(dateRange[0]);
            dateTo = DateHelpers.ParseDate(dateRange[1]);
        }
        else
        {
            dateFrom = DateHelpers.ParseDate(dateSegment);
        }
    
        return (hotelId, dateFrom, dateTo, roomType);
    }

    public static (string hotelId, int daysAhead, string roomTypeCode) ParseSearch(string input)
    {
        var rawParams = input.Trim();
        rawParams = rawParams.Substring("Search(".Length);
        rawParams = rawParams.Remove(rawParams.Length - 1); // remove closing ')' -> "H1, 365, DBL"

        var parts = rawParams.Split(',');

        var hotelId = parts[0].Trim(); // "H1"
        var daysAhead = int.Parse(parts[1].Trim()); // 365
        var roomType = parts[2].Trim(); // "DBL" or "SGL"

        return (hotelId, daysAhead, roomType);
    }
}