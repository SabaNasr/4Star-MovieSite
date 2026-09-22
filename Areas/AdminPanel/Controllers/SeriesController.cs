using Microsoft.AspNetCore.Mvc;
using Service.Admin.TvSeries.Series;
namespace _4Star.Areas.AdminPanel.Controllers;

[Area("AdminPanel")]
public class SeriesController : Controller
{
    private readonly ISeriesService _seriesService;

    public SeriesController(ISeriesService seriesService)
    {
        _seriesService = seriesService;
    }


    // ==================================================
    // Index
    // ==================================================

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var series = await _seriesService.GetAllAsync();

        return View(series);
    }


    // ==================================================
    // Create - GET
    // ==================================================

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var dto = await _seriesService.GetCreateDataAsync();

        return View(dto);
    }


    // ==================================================
    // Create - POST
    // ==================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SeriesCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            await _seriesService.FillCreateFormDataAsync(dto);

            return View(dto);
        }


        var id = await _seriesService.CreateAsync(dto);


        TempData["SuccessMessage"] =
            "سریال با موفقیت ایجاد شد.";


        return RedirectToAction(
            nameof(Details),
            new { id });
    }


    // ==================================================
    // Edit - GET
    // ==================================================

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var series =
            await _seriesService.GetEditDataAsync(id);


        if (series == null)
            return NotFound();


        return View(series);
    }


    // ==================================================
    // Edit - POST
    // ==================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(SeriesEditDto dto)
    {
        if (!ModelState.IsValid)
        {
            await _seriesService.FillEditFormDataAsync(dto);

            return View(dto);
        }


        var result =
            await _seriesService.UpdateAsync(dto);


        if (!result)
            return NotFound();


        TempData["SuccessMessage"] =
            "سریال با موفقیت ویرایش شد.";


        return RedirectToAction(
            nameof(Details),
            new { id = dto.Id });
    }


    // ==================================================
    // Details
    // ==================================================

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var series =
            await _seriesService.GetDetailsAsync(id);


        if (series == null)
            return NotFound();


        return View(series);
    }


    // ==================================================
    // Delete
    // ==================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var result =
            await _seriesService.DeleteAsync(id);


        TempData[result
            ? "SuccessMessage"
            : "ErrorMessage"] =
            result
                ? "سریال با موفقیت حذف شد."
                : "سریال مورد نظر پیدا نشد.";


        return RedirectToAction(nameof(Index));
    }


    // ==================================================
    // Change Status
    // ==================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeStatus(int id)
    {
        var result =
            await _seriesService.ChangeStatusAsync(id);


        TempData[result
            ? "SuccessMessage"
            : "ErrorMessage"] =
            result
                ? "وضعیت سریال با موفقیت تغییر کرد."
                : "سریال مورد نظر پیدا نشد.";


        return RedirectToAction(nameof(Index));
    }


    // ==================================================
    // Archive
    // ==================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Archive(int id)
    {
        var result =
            await _seriesService.ArchiveAsync(id);


        TempData[result
            ? "SuccessMessage"
            : "ErrorMessage"] =
            result
                ? "سریال با موفقیت آرشیو شد."
                : "سریال مورد نظر پیدا نشد.";


        return RedirectToAction(nameof(Index));
    }


    // ==================================================
    // Unarchive
    // ==================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Unarchive(int id)
    {
        var result =
            await _seriesService.UnarchiveAsync(id);


        TempData[result
            ? "SuccessMessage"
            : "ErrorMessage"] =
            result
                ? "سریال از آرشیو خارج شد."
                : "سریال مورد نظر پیدا نشد.";


        return RedirectToAction(nameof(Index));
    }


    // ==================================================
    // Archived
    // ==================================================

    [HttpGet]
    public async Task<IActionResult> Archived()
    {
        var series =
            await _seriesService.GetArchivedAsync();

        return View(series);
    }


    // ==================================================
    // Deleted
    // ==================================================

    [HttpGet]
    public async Task<IActionResult> Deleted()
    {
        var series =
            await _seriesService.GetDeletedAsync();

        return View(series);
    }


    // ==================================================
    // Restore
    // ==================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Restore(int id)
    {
        var result =
            await _seriesService.RestoreAsync(id);


        TempData[result
            ? "SuccessMessage"
            : "ErrorMessage"] =
            result
                ? "سریال با موفقیت بازیابی شد."
                : "سریال مورد نظر پیدا نشد.";


        return RedirectToAction(nameof(Deleted));
    }
}