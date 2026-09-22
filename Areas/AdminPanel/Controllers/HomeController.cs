using Microsoft.AspNetCore.Mvc;

namespace _4Star.Areas.AdminPanel.Controllers;

[Area("AdminPanel")]
public class HomeController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}
