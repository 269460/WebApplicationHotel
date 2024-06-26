using HotelBookingApp.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using HotelBookingApp.Domain.Services;
using HotelBookingApp.Infrastructure.Data;

namespace WebApplication1.Controllers
{

    public class ReservationsController : Controller
    {
        private readonly RoomRepository _roomRepository;
        private readonly BookingRepository _bookingRepository;
        private readonly UserRepository _userRepository;

        public ReservationsController(RoomRepository roomRepository,BookingRepository bookingRepository,UserRepository userRepository)
        {
            _roomRepository = roomRepository;
            _bookingRepository = bookingRepository;
            _userRepository = userRepository;
        }
        // private IRoomService _roomService;
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult FinishedReservation()
        {
            return View();
        }
        public async Task<IActionResult> ConfirmReservation(int roomId, DateTime startDate, DateTime endDate)
        {
            Room room = await _roomRepository.GetRoomByIdAsync(roomId);
            if (room == null)
            {
                return NotFound();
            }
            var viewModel = new ReservationsViewModel
            {
                RoomId = roomId,
                RoomNumber = room.Number,
                Price = room.Price,
                Capacity = room.Capacity,
                StartDate = startDate,
                EndDate = endDate
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmReservation(int roomId, DateTime startDate, DateTime endDate, string username, string email)
        {
            Room room = await _roomRepository.GetRoomByIdAsync(roomId);
            if (room == null)
            {
                return NotFound();
            }

            var viewModel = new ReservationsViewModel
            {
                RoomId = roomId,
                RoomNumber = room.Number,
                StartDate = startDate,
                EndDate = endDate,
                Username = username,
                Email = email
            };
            
            User user = new User
            {
                Username = username,
                Email = email
            };
            
            await _userRepository.AddUserAsync(user);
            Console.WriteLine(user.UserId);
            User user2 = await _userRepository.GetUserByMailAsync(email);
            Console.WriteLine(user.UserId);
            // Przetwarzanie rezerwacji i zapis do bazy danych
            Booking booking = new Booking
            {
                CheckInDate = startDate,
                CheckOutDate = endDate,
                RoomId = roomId,
                UserId = user2.UserId
                
            };
            
            
            await _bookingRepository.AddBookingAsync(booking);

            return View(viewModel);
        }
        
        
    }
}