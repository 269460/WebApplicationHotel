namespace HotelBookingApp.Domain.Models;

public class Room
{
    public int RoomId { get; set; }
    public string Number { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public bool IsAvailable { get; set; }
    
    
}

// public Room(int roomId, string number, string description, int capacity, bool isAvailable)
// {
//     RoomId = roomId;
//     Number = number;
//     Description = description;
//     Capacity = capacity;
//     IsAvailable = isAvailable;
// }