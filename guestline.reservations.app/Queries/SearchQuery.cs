using guestline.reservations.app.Models;
using MediatR;

namespace guestline.reservations.app.Query;

public class SearchQuery : IRequest<List<AvailabilityResult>>
{
    public required string HotelId { get; init; }
    public required int DaysAhead { get; init; }
    public required string RoomType { get; init; }
}