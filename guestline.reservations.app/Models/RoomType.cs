using System.Text.Json.Serialization;

namespace guestline.reservations.app.Models;

public class RoomType
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("amenities")]
    public List<string> Amenities { get; set; }

    [JsonPropertyName("features")]
    public List<string> Features { get; set; }
}