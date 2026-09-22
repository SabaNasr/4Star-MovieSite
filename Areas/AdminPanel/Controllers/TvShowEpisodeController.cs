using Microsoft.AspNetCore.Mvc;
using Service.Admin.TVShows.TVShowEpisode;
namespace _4Star.Areas.AdminPanel.Controllers;

[Area("AdminPanel")]
public class TVShowEpisodeController : Controller
{
    private readonly ITVShowEpisodeService
        _tvShowEpisodeService;


    public TVShowEpisodeController(
        ITVShowEpisodeService tvShowEpisodeService)
    {
        _tvShowEpisodeService =
            tvShowEpisodeService;
    }


    // ==================================================
    // Index
    // ==================================================

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var episodes =
            await _tvShowEpisodeService.GetAllAsync();

        return View(episodes);
    }


    // ==================================================
    // Create - GET
    // ==================================================

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var dto =
            await _tvShowEpisodeService
                .GetCreateDataAsync();

        return View(dto);
    }


    // ==================================================
    // Create - POST
    // ==================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        TVShowEpisodeCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            await _tvShowEpisodeService
                .FillCreateFormDataAsync(dto);

            return View(dto);
        }


        var result =
            await _tvShowEpisodeService
                .CreateAsync(dto);


        if (!result)
        {
            ModelState.AddModelError(
                string.Empty,
                "امکان ثبت قسمت وجود ندارد. ممکن است این قسمت قبلاً برای همین فصل و تی‌وی شو ثبت شده باشد.");

            await _tvShowEpisodeService
                .FillCreateFormDataAsync(dto);

            return View(dto);
        }


        TempData["SuccessMessage"] =
            "قسمت تی‌وی شو با موفقیت ثبت شد.";


        return RedirectToAction(nameof(Index));
    }


    // ==================================================
    // Edit - GET
    // ==================================================

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var episode =
            await _tvShowEpisodeService
                .GetEditDataAsync(id);


        if (episode == null)
            return NotFound();


        return View(episode);
    }


    // ==================================================
    // Edit - POST
    // ==================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        TVShowEpisodeEditDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);


        var result =
            await _tvShowEpisodeService
                .UpdateAsync(dto);


        if (!result)
        {
            ModelState.AddModelError(
                string.Empty,
                "امکان ویرایش قسمت وجود ندارد. ممکن است شماره قسمت تکراری باشد.");

            return View(dto);
        }


        TempData["SuccessMessage"] =
            "قسمت تی‌وی شو با موفقیت ویرایش شد.";


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
        var episode =
            await _tvShowEpisodeService
                .GetDetailsAsync(id);


        if (episode == null)
            return NotFound();


        return View(episode);
    }


    // ==================================================
    // Change Status
    // ==================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeStatus(int id)
    {
        var result =
            await _tvShowEpisodeService
                .ChangeStatusAsync(id);


        TempData[result
            ? "SuccessMessage"
            : "ErrorMessage"] =
            result
                ? "وضعیت قسمت با موفقیت تغییر کرد."
                : "قسمت مورد نظر پیدا نشد.";


        return RedirectToAction(nameof(Index));
    }


    // ==================================================
    // Delete
    // ==================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var result =
            await _tvShowEpisodeService
                .DeleteAsync(id);


        TempData[result
            ? "SuccessMessage"
            : "ErrorMessage"] =
            result
                ? "قسمت با موفقیت حذف شد."
                : "قسمت مورد نظر پیدا نشد.";


        return RedirectToAction(nameof(Index));
    }
}