using guestline.reservations.app.Helpers;
using guestline.reservations.app.Models;

namespace guestline.reservations.app.Service;

public interface IAvailabilityService
{
    int GetAvailability(string hotelId, DateOnly dateFrom, DateOnly? dateTo, string roomType);
}

public class AvailabilityService : IAvailabilityService
{
    private readonly List<Hotel> _hotels;
    private readonly List<Booking> _bookings;

    public AvailabilityService(List<Hotel> hotels, List<Booking> bookings)
    {
        _hotels = hotels;
        _bookings = bookings;
    }

    public int GetAvailability(string hotelId, DateOnly dateFrom, DateOnly? dateTo, string roomType)
    {
        var hotel = _hotels.FirstOrDefault(h => h.Id == hotelId) ??
                    throw new Exception($"Hotel with Id {hotelId} not found");
        
        var totalRoomsOfType = hotel.Rooms
            .Count(r => r.RoomType.Equals(roomType, StringComparison.OrdinalIgnoreCase));
 
        var overlappingBookings = _bookings.Where(b =>
            b.HotelId == hotelId
            && b.RoomType.Equals(roomType, StringComparison.OrdinalIgnoreCase)
        );

        if (dateTo.HasValue)
        {
            overlappingBookings = overlappingBookings.Where(b =>
                DateHelpers.ParseDate(b.Arrival) < dateTo && DateHelpers.ParseDate(b.Departure) > dateFrom);
        }
        else
        {
            overlappingBookings = overlappingBookings.Where(b =>
                DateHelpers.ParseDate(b.Arrival) == dateFrom);
        }

        var bookingCount = overlappingBookings.Count();

        return totalRoomsOfType - bookingCount;
    }
}