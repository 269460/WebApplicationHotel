namespace HotelBookingApp.Domain.Models;

public class Booking
{
    public int BookingId { get; set; }  // Primary Key
    public DateTime BookingDate { get; set; }

    // Navigation properties
    public int RoomId { get; set; }
    //public Room Room { get; set; }  // Ensure 'Room' is defined in your context

    public int UserId { get; set; }
   // public User User { get; set; }
    public Room Room { get; set; } = new Room();
    public User User { get; set; } = new User();

}