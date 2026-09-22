using Microsoft.AspNetCore.Mvc;
using Service.Admin.Content.ContactUs;
namespace _4Star.Areas.AdminPanel.Controllers;

[Area("AdminPanel")]
public class ContactUsController : Controller
{
    private readonly IContactUsService _contactUsService;

    public ContactUsController(
        IContactUsService contactUsService)
    {
        _contactUsService = contactUsService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var messages =
            await _contactUsService.GetAllAsync();

        return View(messages);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var message =
            await _contactUsService.GetDetailsAsync(id);

        if (message == null)
        {
            return NotFound();
        }

        return View(message);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var result =
            await _contactUsService.DeleteAsync(id);

        TempData[result
            ? "SuccessMessage"
            : "ErrorMessage"] =
            result
                ? "پیام با موفقیت حذف شد."
                : "پیام مورد نظر پیدا نشد.";

        return RedirectToAction(nameof(Index));
    }
}