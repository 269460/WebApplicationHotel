using System.ComponentModel.DataAnnotations;

namespace HotelBookingApp.Domain.Models
{
    public class Room
    {
        public int RoomId { get; set; }

        [Required]
        public int Number { get; set; }

        [Required]
        [MaxLength(255)]
        public string Description { get; set; }

        [Required]
        public int Capacity { get; set; }

        [Required]
        public float Price { get; set; }

        [Required]
        public bool IsAvailable { get; set; }
    }
}