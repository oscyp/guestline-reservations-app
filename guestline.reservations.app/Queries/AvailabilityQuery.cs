using MediatR;

namespace guestline.reservations.app.Query;

public class AvailabilityQuery : IRequest<int>
{
    public required string HotelId { get; init; }
    public required DateOnly DateFrom { get; init; }
    public required DateOnly? DateTo { get; init; }
    public required string RoomType { get; init; }
}   