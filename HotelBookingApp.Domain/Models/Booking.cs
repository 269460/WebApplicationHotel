namespace HotelBookingApp.Domain.Models;

public class Booking
{
    
        public int BookingId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int RoomId { get; set; }
        public int UserId { get; set; } 
        public Room Room { get; set; } = new Room();
        public User User { get; set; } = new User();

}