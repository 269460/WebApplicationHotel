using Microsoft.AspNetCore.Mvc;
using  WebApplication1.Models;

namespace WebApplication1.Controllers;

public class ReservationsController : Controller
{
    // public IActionResult Index()
    // {
    //     return View();
    // }
    public IActionResult Index(int roomId, string startDate, string endDate)
    {
        // Tutaj możesz przekazać dane do widoku rezerwacji
        ViewData["RoomId"] = roomId;
        ViewData["StartDate"] = startDate;
        ViewData["EndDate"] = endDate;

        return View();
    }

    // Przykładowa akcja do obsługi potwierdzenia rezerwacji
    [HttpPost]
    public IActionResult ConfirmReservation(int roomId, string startDate, string endDate, string customerName)
    {
        // Tutaj możesz przetworzyć dane rezerwacji, np. zapisać je w bazie danych
        // Możesz użyć modelu do przekazania danych

        ViewData["RoomId"] = roomId;
        ViewData["StartDate"] = startDate;
        ViewData["EndDate"] = endDate;
        ViewData["CustomerName"] = customerName;
        

        var viewModel = new ReservationsViewModel
        {
            RoomId = roomId,
            StartDate = startDate,
            EndDate = endDate,
            CustomerName = customerName
        };
        
        Console.WriteLine(roomId);

        return View(viewModel);
    }
}
