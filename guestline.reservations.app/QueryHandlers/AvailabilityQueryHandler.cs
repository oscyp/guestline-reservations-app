using guestline.reservations.app.Query;
using guestline.reservations.app.Service;
using MediatR;

namespace guestline.reservations.app.QueryHandler;

public class AvailabilityQueryHandler(IAvailabilityService availabilityService) : IRequestHandler<AvailabilityQuery, int>
{
    public Task<int> Handle(AvailabilityQuery request, CancellationToken cancellationToken)
    {
        int availability = availabilityService.GetAvailability(
            request.HotelId,
            request.DateFrom,
            request.DateTo,
            request.RoomType
        );
        return Task.FromResult(availability);
    }
}