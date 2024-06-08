using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;

public class AdministrationController : Controller
{
    public IActionResult Index()
    {
        return View(); 
    }
}