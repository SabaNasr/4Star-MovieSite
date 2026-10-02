using Microsoft.AspNetCore.Mvc;
using Service.Admin.Content.Banner;
namespace _4Star.Areas.AdminPanel.Controllers;


[Area("AdminPanel")]
public class BannerController : Controller
{
    private readonly IBannerService _bannerService;

    public BannerController(
        IBannerService bannerService)
    {
        _bannerService = bannerService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var banners = await _bannerService.GetAllAsync();
        return View(banners);
    }

    [HttpGet]
    public async Task<IActionResult> Deleted()
    {
        var banners = await _bannerService.GetDeletedAsync();
        return View(banners);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var tags =
            await _bannerService.GetActiveTagsForDropdownAsync();

        ViewBag.TagSearches = tags;

        return View(new BannerCreateDto());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        BannerCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.TagSearches =
                await _bannerService
                    .GetActiveTagsForDropdownAsync();

            return View(dto);
        }

        await _bannerService.CreateAsync(dto);

        TempData["SuccessMessage"] =
            "بنر با موفقیت ایجاد شد.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var banner =
            await _bannerService.GetByIdForUpdateAsync(id);

        if (banner == null)
            return NotFound();

        ViewBag.TagSearches =
            await _bannerService
                .GetActiveTagsForDropdownAsync();

        return View(banner);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        BannerUpdateDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.TagSearches =
                await _bannerService
                    .GetActiveTagsForDropdownAsync();

            return View(dto);
        }

        await _bannerService.UpdateAsync(dto);

        TempData["SuccessMessage"] =
            "بنر با موفقیت ویرایش شد.";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _bannerService.SoftDeleteAsync(id);

        TempData["SuccessMessage"] =
            "بنر با موفقیت حذف شد.";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Restore(int id)
    {
        await _bannerService.RestoreAsync(id);

        TempData["SuccessMessage"] =
            "بنر با موفقیت بازیابی شد.";

        return RedirectToAction(nameof(Deleted));
    }
}