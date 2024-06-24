namespace HotelBookingApp.Domain.Models;

public class Booking
{
    
        public int BookingId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int RoomId { get; set; }
        public int UserId { get; set; } 
        public Room Room { get; set; } = new Room();
        public User User { get; set; } = new User();

}