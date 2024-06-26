using HotelBookingApp.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using HotelBookingApp.Domain.Services;
using HotelBookingApp.Infrastructure.Data;

namespace WebApplication1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BookingsController : Controller
    {
        private readonly RoomRepository _roomRepository;
        private readonly BookingRepository _bookingRepository;
        private readonly UserRepository _userRepository;

        public BookingsController(RoomRepository roomRepository,BookingRepository bookingRepository,UserRepository userRepository)
        {
            _roomRepository = roomRepository;
            _bookingRepository = bookingRepository;
            _userRepository = userRepository;
        }
        

        [HttpGet()]
        public async Task<IActionResult> GetAllBookings()
        {
            var rooms = await _bookingRepository.GetAllBookingsAsync();
            return Ok(rooms);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            await _bookingRepository.DeleteBookingAsync(id);
            return NoContent(); // Standard response for a successful DELETE request
        }
    }
}