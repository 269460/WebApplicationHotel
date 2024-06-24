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
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using HotelBookingApp.Domain.Services;

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
        
        private IRoomService _roomService;
        public IActionResult Index()
        {
            return View();
        }
        public ReservationsController(IRoomService roomService)
        {
            _roomService = roomService;
        }
        public IActionResult ConfirmReservation(int roomId, string startDate, string endDate)
        {
            var room = _roomService.GetRoomByIdAsync(roomId);
            if (room == null)
            {
                return NotFound();
            }
            var viewModel = new ReservationsViewModel
            {
                RoomId = roomId,
                // RoomNumber = room.Number,
                StartDate = startDate,
                EndDate = endDate
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult ConfirmReservation(int roomId, string startDate, string endDate, string customerName)
        {
        

        var room = _roomService.GetRoomByIdAsync(roomId);

            if (room == null)
            {
                return NotFound();
            }

            var viewModel = new ReservationsViewModel
            {
                RoomId = roomId,
                // RoomNumber = room.Number,
                StartDate = startDate,
                EndDate = endDate,
                CustomerName = customerName
            };

            // Przetwarzanie rezerwacji i zapis do bazy danych

            return View(viewModel);
        }
    }
}
