using HotelBookingApp.Domain.Models;

namespace WebApplication1.Models
{
    public class ReservationsViewModel
    {
        public int RoomId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Username { get; set; } 
        public int RoomNumber { get; set; }
        public int Capacity { get; set; }
        public float Price { get; set; }
    }
}