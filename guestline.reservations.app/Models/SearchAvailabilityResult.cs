namespace guestline.reservations.app.Models;

public class AvailabilityResult
{
    public DateOnly start { get; set; }
    public DateOnly end { get; set; }
    public int availability { get; set; }
}

public class SearchAvailabilityResult
{
    public List<AvailabilityResult> Availability { get; set; }
}