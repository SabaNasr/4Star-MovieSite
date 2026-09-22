using Microsoft.AspNetCore.Mvc;
using Service.Admin.Content.AboutUs;
namespace _4Star.Areas.AdminPanel.Controllers;

[Area("AdminPanel")]
public class AboutUsController : Controller
{
    private readonly IAboutUsService _aboutUsService;

    public AboutUsController(
        IAboutUsService aboutUsService)
    {
        _aboutUsService = aboutUsService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var aboutUs =
            await _aboutUsService.GetAllAsync();

        return View(aboutUs);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var dto =
            await _aboutUsService.GetCreateDataAsync();

        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        AboutUsCreateDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var result =
            await _aboutUsService.CreateAsync(dto);

        if (!result)
        {
            ModelState.AddModelError(
                string.Empty,
                "اطلاعات درباره ما قبلاً ثبت شده است یا محتوای واردشده معتبر نیست.");

            return View(dto);
        }

        TempData["SuccessMessage"] =
            "اطلاعات درباره ما با موفقیت ثبت شد.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var aboutUs =
            await _aboutUsService.GetEditDataAsync(id);

        if (aboutUs == null)
            return NotFound();

        return View(aboutUs);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        AboutUsEditDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var result =
            await _aboutUsService.UpdateAsync(dto);

        if (!result)
        {
            ModelState.AddModelError(
                string.Empty,
                "امکان ویرایش اطلاعات درباره ما وجود ندارد.");

            return View(dto);
        }

        TempData["SuccessMessage"] =
            "اطلاعات درباره ما با موفقیت ویرایش شد.";

        return RedirectToAction(
            nameof(Details),
            new { id = dto.Id });
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var aboutUs =
            await _aboutUsService.GetDetailsAsync(id);

        if (aboutUs == null)
            return NotFound();

        return View(aboutUs);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeStatus(int id)
    {
        var result =
            await _aboutUsService.ChangeStatusAsync(id);

        TempData[result
            ? "SuccessMessage"
            : "ErrorMessage"] =
            result
                ? "وضعیت درباره ما با موفقیت تغییر کرد."
                : "اطلاعات درباره ما پیدا نشد.";

        return RedirectToAction(nameof(Index));
    }
}