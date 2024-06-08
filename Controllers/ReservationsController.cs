using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;

public class ReservationsController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
