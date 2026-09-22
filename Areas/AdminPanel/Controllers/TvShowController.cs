using Microsoft.AspNetCore.Mvc;
using Service.Admin.TVShows.TvShow;
namespace _4Star.Areas.AdminPanel.Controllers;

[Area("AdminPanel")]
public class TVShowController : Controller
{
    private readonly ITVShowService
        _tvShowService;

    public TVShowController(
        ITVShowService tvShowService)
    {
        _tvShowService = tvShowService;
    }


    // ==================================================
    // Index
    // ==================================================

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var tvShows =
            await _tvShowService.GetAllAsync();

        return View(tvShows);
    }


    // ==================================================
    // Create - GET
    // ==================================================

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var dto =
            await _tvShowService
                .GetCreateDataAsync();

        return View(dto);
    }


    // ==================================================
    // Create - POST
    // ==================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        TVShowCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            await _tvShowService
                .FillCreateFormDataAsync(dto);

            return View(dto);
        }

        var id =
            await _tvShowService
                .CreateAsync(dto);

        TempData["SuccessMessage"] =
            "برنامه تلویزیونی با موفقیت ایجاد شد.";

        return RedirectToAction(
            nameof(Details),
            new { id });
    }


    // ==================================================
    // Edit - GET
    // ==================================================

    [HttpGet]
    public async Task<IActionResult> Edit(
        int id)
    {
        var tvShow =
            await _tvShowService
                .GetEditDataAsync(id);

        if (tvShow == null)
            return NotFound();

        return View(tvShow);
    }


    // ==================================================
    // Edit - POST
    // ==================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        TVShowEditDto dto)
    {
        if (!ModelState.IsValid)
        {
            await _tvShowService
                .FillEditFormDataAsync(dto);

            return View(dto);
        }

        var result =
            await _tvShowService
                .UpdateAsync(dto);

        if (!result)
        {
            ModelState.AddModelError(
                string.Empty,
                "برنامه تلویزیونی مورد نظر پیدا نشد.");

            await _tvShowService
                .FillEditFormDataAsync(dto);

            return View(dto);
        }

        TempData["SuccessMessage"] =
            "برنامه تلویزیونی با موفقیت ویرایش شد.";

        return RedirectToAction(
            nameof(Details),
            new { id = dto.Id });
    }


    // ==================================================
    // Details
    // ==================================================

    [HttpGet]
    public async Task<IActionResult> Details(
        int id)
    {
        var tvShow =
            await _tvShowService
                .GetDetailsAsync(id);

        if (tvShow == null)
            return NotFound();

        return View(tvShow);
    }


    // ==================================================
    // Archived
    // ==================================================

    [HttpGet]
    public async Task<IActionResult> Archived()
    {
        var tvShows =
            await _tvShowService
                .GetArchivedAsync();

        return View(tvShows);
    }


    // ==================================================
    // Deleted
    // ==================================================

    [HttpGet]
    public async Task<IActionResult> Deleted()
    {
        var tvShows =
            await _tvShowService
                .GetDeletedAsync();

        return View(tvShows);
    }


    // ==================================================
    // Delete
    // ==================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(
        int id)
    {
        var result =
            await _tvShowService
                .DeleteAsync(id);

        TempData[result
            ? "SuccessMessage"
            : "ErrorMessage"] =
            result
                ? "برنامه تلویزیونی با موفقیت حذف شد."
                : "برنامه تلویزیونی مورد نظر پیدا نشد.";

        return RedirectToAction(
            nameof(Index));
    }


    // ==================================================
    // Restore
    // ==================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Restore(
        int id)
    {
        var result =
            await _tvShowService
                .RestoreAsync(id);

        TempData[result
            ? "SuccessMessage"
            : "ErrorMessage"] =
            result
                ? "برنامه تلویزیونی با موفقیت بازیابی شد."
                : "برنامه تلویزیونی مورد نظر پیدا نشد.";

        return RedirectToAction(
            nameof(Deleted));
    }


    // ==================================================
    // Change Status
    // ==================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeStatus(
        int id)
    {
        var result =
            await _tvShowService
                .ChangeStatusAsync(id);

        TempData[result
            ? "SuccessMessage"
            : "ErrorMessage"] =
            result
                ? "وضعیت برنامه تلویزیونی با موفقیت تغییر کرد."
                : "برنامه تلویزیونی مورد نظر پیدا نشد.";

        return RedirectToAction(
            nameof(Index));
    }


    // ==================================================
    // Archive
    // ==================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Archive(
        int id)
    {
        var result =
            await _tvShowService
                .ArchiveAsync(id);

        TempData[result
            ? "SuccessMessage"
            : "ErrorMessage"] =
            result
                ? "برنامه تلویزیونی با موفقیت آرشیو شد."
                : "برنامه تلویزیونی مورد نظر پیدا نشد.";

        return RedirectToAction(
            nameof(Index));
    }


    // ==================================================
    // Unarchive
    // ==================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Unarchive(
        int id)
    {
        var result =
            await _tvShowService
                .UnarchiveAsync(id);

        TempData[result
            ? "SuccessMessage"
            : "ErrorMessage"] =
            result
                ? "برنامه تلویزیونی از آرشیو خارج شد."
                : "برنامه تلویزیونی مورد نظر پیدا نشد.";

        return RedirectToAction(
            nameof(Archived));
    }
}