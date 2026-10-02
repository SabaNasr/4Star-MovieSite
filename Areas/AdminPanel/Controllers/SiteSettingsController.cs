using Microsoft.AspNetCore.Mvc;
using Service.Admin.Settings.SiteSettings;
namespace _4Star.Areas.AdminPanel.Controllers;

[Area("AdminPanel")]
public class SiteSettingsController : Controller
{
    private readonly ISiteSettingsService _siteSettingsService;

    public SiteSettingsController(
        ISiteSettingsService siteSettingsService)
    {
        _siteSettingsService = siteSettingsService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var groups = await _siteSettingsService.GetAllGroupedAsync();

        return View(groups);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveGroup(
        string groupName,
        Dictionary<string, string> settings)
    {
        if (string.IsNullOrWhiteSpace(groupName))
        {
            TempData["ErrorMessage"] =
                "گروه تنظیمات مشخص نشده است.";

            return RedirectToAction(nameof(Index));
        }

        await _siteSettingsService.SaveGroupAsync(
            groupName,
            settings);

        TempData["SuccessMessage"] =
            "تنظیمات با موفقیت ذخیره شد.";

        return RedirectToAction(nameof(Index));
    }
}