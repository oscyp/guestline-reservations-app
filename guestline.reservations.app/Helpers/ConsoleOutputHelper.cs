using guestline.reservations.app.Models;

namespace guestline.reservations.app.Helpers;

public class ConsoleOutputHelper
{
    public static void SearchOutput(List<AvailabilityResult> availabilityResults)
    {
        if (!availabilityResults.Any())
        {
            Console.WriteLine("No availability found");
        }
        
        var formatted = new List<string>();
        foreach (var availability in availabilityResults)
        {
            string startStr = availability.start.ToString("yyyyMMdd");
            DateOnly lastInclusiveDate = availability.end.AddDays(-1);
            string endStr = lastInclusiveDate.ToString("yyyyMMdd");
            

            formatted.Add($"({startStr}-{endStr}, {availability.availability})");
        }
    
        Console.WriteLine(string.Join(", ", formatted));
    }

    public static void AvailabilityOutput(int result)
    {
        if (result < 0)
        {
            Console.WriteLine($"Overbooked by {-result}");
        }
        else
        {
            Console.WriteLine($"{result} room(s) available");
        }   
    }
}