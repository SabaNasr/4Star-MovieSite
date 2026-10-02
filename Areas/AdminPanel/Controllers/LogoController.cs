using Microsoft.AspNetCore.Mvc;
using Service.Admin.Settings.Logo;
namespace _4Star.Areas.AdminPanel.Controllers;

[Area("AdminPanel")]
public class LogoController : Controller
{
    private readonly ILogoService _logoService;

    public LogoController(ILogoService logoService)
    {
        _logoService = logoService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var model = await _logoService.GetAsync();

        if (model == null)
            return NotFound();

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(LogoManageDto dto)
    {
        if (!ModelState.IsValid)
        {
            var currentLogo = await _logoService.GetAsync();

            if (currentLogo != null)
            {
                dto.ExistingMainLogo = currentLogo.ExistingMainLogo;
                dto.ExistingFooterLogo = currentLogo.ExistingFooterLogo;
                dto.ExistingFavicon = currentLogo.ExistingFavicon;
            }

            return View(dto);
        }

        var result = await _logoService.UpdateAsync(dto);

        if (!result)
            return NotFound();

        TempData["SuccessMessage"] = "تنظیمات لوگو با موفقیت ذخیره شد.";

        return RedirectToAction(nameof(Index));
    }
}