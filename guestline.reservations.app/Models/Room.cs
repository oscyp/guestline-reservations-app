using System.Text.Json.Serialization;

namespace guestline.reservations.app.Models;

public class Room
{
    [JsonPropertyName("roomType")]
    public string RoomType { get; set; }

    [JsonPropertyName("roomId")]
    public string RoomId { get; set; }

}