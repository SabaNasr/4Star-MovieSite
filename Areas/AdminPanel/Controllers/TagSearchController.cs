using Microsoft.AspNetCore.Mvc;
using Service.Admin.Content.TagSearch;
namespace _4Star.Areas.AdminPanel.Controllers;

[Area("AdminPanel")]
public class TagSearchController : Controller
{
    private readonly ITagSearchesService _tagSearchesService;

    public TagSearchController(
        ITagSearchesService tagSearchesService)
    {
        _tagSearchesService = tagSearchesService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var tags = await _tagSearchesService.GetAllAsync();
        return View(tags);
    }

    [HttpGet]
    public async Task<IActionResult> Deleted()
    {
        var tags = await _tagSearchesService.GetDeletedAsync();
        return View(tags);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new TagSearchCreateDto());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        TagSearchCreateDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        await _tagSearchesService.CreateAsync(dto);

        TempData["SuccessMessage"] =
            "تگ جستجو با موفقیت ایجاد شد.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var tag =
            await _tagSearchesService.GetByIdForUpdateAsync(id);

        if (tag == null)
            return NotFound();

        return View(tag);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        TagSearchUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        await _tagSearchesService.UpdateAsync(dto);

        TempData["SuccessMessage"] =
            "تگ جستجو با موفقیت ویرایش شد.";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _tagSearchesService.SoftDeleteAsync(id);

        TempData["SuccessMessage"] =
            "تگ جستجو با موفقیت حذف شد.";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Restore(int id)
    {
        await _tagSearchesService.RestoreAsync(id);

        TempData["SuccessMessage"] =
            "تگ جستجو با موفقیت بازیابی شد.";

        return RedirectToAction(nameof(Deleted));
    }
}