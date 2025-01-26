using guestline.reservations.app.Models;
using guestline.reservations.app.Service;

namespace guestline.reservations.app.tests;

public class AvailabilityServiceTests
{
    private List<Hotel> _hotels;
    private List<Booking> _bookings;
    private IAvailabilityService _availabilityService;

    [SetUp]
    public void Setup()
    {
        _hotels = new List<Hotel>
        {
            new Hotel
            {
                Id = "H1",
                Name = "Hotel One",
                Rooms = new List<Room>
                {
                    new Room { RoomType = "DBL", RoomId = "101" },
                    new Room { RoomType = "DBL", RoomId = "102" },
                    new Room { RoomType = "SGL", RoomId = "201" }
                }
            }
        };

        _bookings = new List<Booking>
        {
            new Booking
            {
                HotelId = "H1",
                Arrival = "20240901",
                Departure = "20240903",
                RoomType = "DBL",
                RoomRate = "Prepaid"
            }
        };

        _availabilityService = new AvailabilityService(_hotels, _bookings);
    }

    [Test]
    public void GetAvailability_ShouldReturnCorrectAvailability_WhenDateRangeIsProvided()
    {
        var availability = _availabilityService.GetAvailability(
            "H1", 
            new DateOnly(2024, 9, 1), 
            new DateOnly(2024, 9, 2),
            "DBL"
        );

        Assert.AreEqual(1, availability);
    }

    [Test]
    public void GetAvailability_ShouldReturnCorrectAvailability_WhenSingleDateIsProvided()
    {
        var availability = _availabilityService.GetAvailability(
            "H1", 
            new DateOnly(2024, 9, 1), 
            null,
            "DBL"
        );

        Assert.AreEqual(1, availability);
    }

    [Test]
    public void GetAvailability_ShouldThrowException_WhenHotelNotFound()
    {
        Assert.Throws<Exception>(() =>
            _availabilityService.GetAvailability(
                "H2", // doesn't exist
                new DateOnly(2024, 9, 1), 
                null, 
                "DBL"
            )
        );
    }

    [Test]
    public void GetAvailability_ShouldReturnAllRooms_WhenNoBookingsOverlap()
    {
        var availability = _availabilityService.GetAvailability(
            "H1",
            new DateOnly(2024, 8, 28),
            new DateOnly(2024, 8, 29),
            "DBL"
        );

        Assert.AreEqual(2, availability);
    }

    [Test]
    public void GetAvailability_ShouldReturnZero_WhenAllRoomsAreBooked()
    {
        _bookings.Add(new Booking
        {
            HotelId = "H1",
            Arrival = "20240902",
            Departure = "20240905",
            RoomType = "DBL",
            RoomRate = "Standard"
        });

        var availability = _availabilityService.GetAvailability(
            "H1",
            new DateOnly(2024, 9, 2),
            new DateOnly(2024, 9, 3),
            "DBL"
        );

        Assert.AreEqual(0, availability);
    }

    [Test]
    public void GetAvailability_ShouldReturnNegative_WhenOverbooked()
    {
        _bookings.Add(new Booking
        {
            HotelId = "H1",
            Arrival = "20240901",
            Departure = "20240903",
            RoomType = "DBL",
            RoomRate = "Prepaid"
        });
        _bookings.Add(new Booking
        {
            HotelId = "H1",
            Arrival = "20240901",
            Departure = "20240903",
            RoomType = "DBL",
            RoomRate = "Prepaid"
        });

        var availability = _availabilityService.GetAvailability(
            "H1",
            new DateOnly(2024, 9, 1),
            new DateOnly(2024, 9, 3),
            "DBL"
        );

        Assert.AreEqual(-1, availability);
    }

    [Test]
    public void GetAvailability_ShouldReturnZero_WhenNoRoomsOfRequestedType()
    {
        var availability = _availabilityService.GetAvailability(
            "H1",
            new DateOnly(2024, 9, 1),
            null,
            "XYZ"
        );

        Assert.AreEqual(0, availability);
    }

    [Test]
    public void GetAvailability_ShouldAccountForPartialOverlap()
    {
        var availability = _availabilityService.GetAvailability(
            "H1",
            new DateOnly(2024, 9, 2),
            new DateOnly(2024, 9, 4),
            "DBL"
        );

        Assert.AreEqual(1, availability);
    }
}
