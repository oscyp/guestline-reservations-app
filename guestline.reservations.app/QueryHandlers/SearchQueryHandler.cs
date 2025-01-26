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
        
        DateOnly day = DateOnly.FromDateTime(DateTime.Today);
        DateOnly lastDate = day.AddDays(request.DaysAhead);

        DateOnly? blockStart = null;
        int blockAvailability = 0;

        while (day < lastDate)
        {
            // Check 1-night availability
            int availability = availabilityService.GetAvailability(
                request.HotelId,
                day,
                day.AddDays(1),
                request.RoomType
            );

            if (availability > 0)
            {
                if (blockStart == null)
                {
                    // start a new block
                    blockStart = day;
                    blockAvailability = availability;
                }
                else
                {
                    // continue the block, update min availability
                    blockAvailability = Math.Min(blockAvailability, availability);
                }
            }
            else
            {
                // close off a block if we are in one
                if (blockStart.HasValue)
                {
                    results.Add(new AvailabilityResult() { start = blockStart.Value, end = day, availability = blockAvailability });
                    blockStart = null;
                }
            }

            day = day.AddDays(1);
        }

        // handle final block if any
        if (blockStart.HasValue)
        {
            results.Add(new AvailabilityResult() { start = blockStart.Value, end = day, availability = blockAvailability });
        }

        return Task.FromResult(results);
    }
}