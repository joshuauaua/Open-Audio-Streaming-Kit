using Microsoft.AspNetCore.Mvc;

namespace HLS_Tool.Controllers;


public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
    
}