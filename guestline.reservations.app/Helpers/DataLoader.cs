using guestline.reservations.app.Models;

namespace guestline.reservations.app.Helpers;

using System.Text.Json;

public class DataLoader
{
    public static List<Hotel> LoadHotels(string path)
    {
        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<Hotel>>(json);
    }

    public static List<Booking> LoadBookings(string path)
    {
        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<Booking>>(json);
    }
}
