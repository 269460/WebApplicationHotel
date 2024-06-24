// using Microsoft.AspNetCore.Mvc;
// using  WebApplication1.Models;
//
// namespace WebApplication1.Controllers;
//
// public class ReservationsController : Controller
// {
//     // public IActionResult Index()
//     // {
//     //     return View();
//     // }
//     public IActionResult Index(int roomId, string startDate, string endDate)
//     {
//         // Tutaj możesz przekazać dane do widoku rezerwacji
//         ViewData["RoomId"] = roomId;
//         ViewData["StartDate"] = startDate;
//         ViewData["EndDate"] = endDate;
//
//         return View();
//     }
//
//     // Przykładowa akcja do obsługi potwierdzenia rezerwacji
//     [HttpPost]
//     public IActionResult ConfirmReservation(int roomId, string startDate, string endDate, string customerName)
//     {
//         // Tutaj możesz przetworzyć dane rezerwacji, np. zapisać je w bazie danych
//         // Możesz użyć modelu do przekazania danych
//
//         ViewData["RoomId"] = roomId;
//         ViewData["StartDate"] = startDate;
//         ViewData["EndDate"] = endDate;
//         ViewData["CustomerName"] = customerName;
//         
//
//         var viewModel = new ReservationsViewModel
//         {
//             RoomId = roomId,
//             StartDate = startDate,
//             EndDate = endDate,
//             CustomerName = customerName
//         };
//         
//         Console.WriteLine(roomId);
//
//         return View(viewModel);
//     }
// }

using HotelBookingApp.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using HotelBookingApp.Domain.Services;
using HotelBookingApp.Infrastructure.Data;

namespace WebApplication1.Controllers
{
    // public class ReservationsController : Controller
    // {
    //     public IActionResult Index()
    //     {
    //         return View();
    //     }
    //     // public IActionResult Index(int roomId, string startDate, string endDate)
    //     // {
    //     //     // Tutaj możesz przekazać dane do widoku rezerwacji
    //     //     ViewData["RoomId"] = roomId;
    //     //     ViewData["StartDate"] = startDate;
    //     //     ViewData["EndDate"] = endDate;
    //     //
    //     //     return View();
    //     // }
    //     public IActionResult ConfirmReservation(int roomId, string startDate, string endDate)
    //     {
    //         // Tworzymy model widoku z danymi rezerwacji
    //         var viewModel = new ReservationsViewModel
    //         {
    //             RoomId = roomId,
    //             StartDate = startDate,
    //             EndDate = endDate
    //         };
    //
    //         return View(viewModel);
    //     }
    //
    //     [HttpPost]
    //     public IActionResult ConfirmReservation(ReservationsViewModel viewModel)
    //     {
    //         // Tutaj możesz przetworzyć dane rezerwacji, np. zapisać je w bazie danych
    //         if (ModelState.IsValid)
    //         {
    //             // Logika zapisu do bazy danych lub innej logiki biznesowej
    //             return RedirectToAction("ReservationConfirmed", viewModel);
    //         }
    //
    //         return View(viewModel);
    //     }
    //
    //     public IActionResult ReservationConfirmed(ReservationsViewModel viewModel)
    //     {
    //         return View(viewModel);
    //     }
    // }
    public class ReservationsController : Controller
    {
        private readonly RoomRepository _roomRepository;
        private readonly BookingRepository _bookingRepository;

        public ReservationsController(RoomRepository roomRepository,BookingRepository bookingRepository)
        {
            _roomRepository = roomRepository;
            _bookingRepository = bookingRepository;
        }
        // private IRoomService _roomService;
        public IActionResult Index()
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
        public async Task<IActionResult> ConfirmReservation(int roomId, DateTime startDate, DateTime endDate, string username)
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
                Username = username
            };

            // Przetwarzanie rezerwacji i zapis do bazy danych
            Booking booking = new Booking
            {
                CheckInDate = startDate,
                CheckOutDate = endDate,
                RoomId = roomId,
                UserId = 1

            };
            await _bookingRepository.AddBookingAsync(booking);

            return View(viewModel);
        }
        
    
    }
}
