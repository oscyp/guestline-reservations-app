using guestline.reservations.app.Models;
using guestline.reservations.app.Query;
using guestline.reservations.app.Service;
using MediatR;

namespace guestline.reservations.app.QueryHandler;

public class SearchQueryHandler(IAvailabilityService availabilityService)
    : IRequestHandler<SearchQuery, List<AvailabilityResult>>
{
    public Task<List<AvailabilityResult>> Handle(SearchQuery request, CancellationToken cancellationToken)
    {
        var results = new List<AvailabilityResult>();
        
        var day = DateOnly.FromDateTime(DateTime.Today);
        var lastDate = day.AddDays(request.DaysAhead);

        DateOnly? blockStart = null;
        int blockAvailability = 0;

        while (day < lastDate)
        {
            var availability = availabilityService.GetAvailability(
                request.HotelId,
                day,
                day.AddDays(1),
                request.RoomType
            );

            if (availability > 0)
            {
                if (blockStart == null)
                {
                    blockStart = day;
                    blockAvailability = availability;
                }
                else
                {
                    blockAvailability = Math.Min(blockAvailability, availability);
                }
            }
            else
            {
                if (blockStart.HasValue)
                {
                    results.Add(new AvailabilityResult() { start = blockStart.Value, end = day, availability = blockAvailability });
                    blockStart = null;
                }
            }

            day = day.AddDays(1);
        }

        if (blockStart.HasValue)
        {
            results.Add(new AvailabilityResult() { start = blockStart.Value, end = day, availability = blockAvailability });
        }

        return Task.FromResult(results);
    }
}