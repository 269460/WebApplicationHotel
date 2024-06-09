using System.ComponentModel.DataAnnotations;

namespace HotelBookingApp.Domain.Models
{
    public class Room
    {
        public int RoomId { get; set; }

        [MaxLength(255)]
        public string Number { get; set; }

        public string Description { get; set; }

        public int Capacity { get; set; }

        public bool IsAvailable { get; set; }
    }
}