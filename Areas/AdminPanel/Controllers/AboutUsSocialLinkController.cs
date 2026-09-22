using Microsoft.AspNetCore.Mvc;
using Service.Admin.Content.AboutUsSocialLink;
namespace _4Star.Areas.AdminPanel.Controllers;

[Area("AdminPanel")]
public class AboutUsSocialLinkController : Controller
{
    private readonly IAboutUsSocialLinkService
        _aboutUsSocialLinkService;

    public AboutUsSocialLinkController(
        IAboutUsSocialLinkService
            aboutUsSocialLinkService)
    {
        _aboutUsSocialLinkService =
            aboutUsSocialLinkService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var links =
            await _aboutUsSocialLinkService
                .GetAllAsync();

        return View(links);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var dto =
            await _aboutUsSocialLinkService
                .GetCreateDataAsync();

        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        AboutUsSocialLinkCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            await _aboutUsSocialLinkService
                .FillCreateFormDataAsync(dto);

            return View(dto);
        }

        var result =
            await _aboutUsSocialLinkService
                .CreateAsync(dto);

        if (!result)
        {
            ModelState.AddModelError(
                string.Empty,
                "امکان ثبت شبکه اجتماعی وجود ندارد.");

            await _aboutUsSocialLinkService
                .FillCreateFormDataAsync(dto);

            return View(dto);
        }

        TempData["SuccessMessage"] =
            "شبکه اجتماعی با موفقیت ثبت شد.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var link =
            await _aboutUsSocialLinkService
                .GetEditDataAsync(id);

        if (link == null)
            return NotFound();

        return View(link);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        AboutUsSocialLinkEditDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var result =
            await _aboutUsSocialLinkService
                .UpdateAsync(dto);

        if (!result)
        {
            ModelState.AddModelError(
                string.Empty,
                "امکان ویرایش شبکه اجتماعی وجود ندارد.");

            return View(dto);
        }

        TempData["SuccessMessage"] =
            "شبکه اجتماعی با موفقیت ویرایش شد.";

        return RedirectToAction(
            nameof(Details),
            new { id = dto.Id });
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var link =
            await _aboutUsSocialLinkService
                .GetDetailsAsync(id);

        if (link == null)
            return NotFound();

        return View(link);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeStatus(
        int id)
    {
        var result =
            await _aboutUsSocialLinkService
                .ChangeStatusAsync(id);

        TempData[result
            ? "SuccessMessage"
            : "ErrorMessage"] =
            result
                ? "وضعیت شبکه اجتماعی با موفقیت تغییر کرد."
                : "شبکه اجتماعی مورد نظر پیدا نشد.";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var result =
            await _aboutUsSocialLinkService
                .DeleteAsync(id);

        TempData[result
            ? "SuccessMessage"
            : "ErrorMessage"] =
            result
                ? "شبکه اجتماعی با موفقیت حذف شد."
                : "شبکه اجتماعی مورد نظر پیدا نشد.";

        return RedirectToAction(nameof(Index));
    }
}