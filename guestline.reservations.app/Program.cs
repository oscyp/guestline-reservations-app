using guestline.reservations.app.Helpers;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using guestline.reservations.app.Query;
using guestline.reservations.app.Service;

string hotelsPath = null;
string bookingsPath = null;

for (int i = 0; i < args.Length; i++)
{
    if (args[i] == "--hotels" && i + 1 < args.Length)
    {
        hotelsPath = args[i + 1];
    }
    else if (args[i] == "--bookings" && i + 1 < args.Length)
    {
        bookingsPath = args[i + 1];
    }
}

if (hotelsPath == null || bookingsPath == null)
{
    Console.WriteLine("Usage: myapp --hotels hotels.json --bookings bookings.json");
    return;
}

var services = new ServiceCollection();
ConfigureService(services, hotelsPath, bookingsPath);
var serviceProvider = services.BuildServiceProvider();
var mediator = serviceProvider.GetRequiredService<IMediator>();

static void ConfigureService(IServiceCollection services, string hotelsPath, string bookingsPath)
{
    var hotels = DataLoader.LoadHotels(hotelsPath);
    var bookings = DataLoader.LoadBookings(bookingsPath);
    services.AddSingleton<IAvailabilityService>(serviceProvider => new AvailabilityService(hotels, bookings));
    services.AddMediatR(cfg => {
        cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    });
}


while (true)
{
    Console.Write("> ");
    string input = Console.ReadLine().Trim();
    if (string.IsNullOrEmpty(input))
    {
        break;
    }

    if (input.StartsWith("Availability("))
    {
        var (hotelId, dateFrom, dateTo, roomType) = ArgsParserHelper.ParseAvailability(input);
        var result = await mediator.Send(new AvailabilityQuery() { HotelId = hotelId, DateFrom = dateFrom, DateTo = dateTo, RoomType = roomType });
        ConsoleOutputHelper.AvailabilityOutput(result);
    }
    else if (input.StartsWith("Search("))
    {
        var (hotelId, daysAhead, roomType) = ArgsParserHelper.ParseSearch(input);
        var result = await mediator.Send(new SearchQuery() { HotelId = hotelId, DaysAhead = daysAhead, RoomType = roomType });
        
        ConsoleOutputHelper.SearchOutput(result);
    }
    else
    {
        Console.WriteLine("Unknown command");
    }
}


