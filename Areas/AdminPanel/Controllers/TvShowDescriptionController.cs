using Microsoft.AspNetCore.Mvc;
using Service.Admin.TVShows.TVShowDescription;
namespace _4Star.Areas.AdminPanel.Controllers;

[Area("AdminPanel")]
public class TVShowDescriptionController : Controller
{
    private readonly ITVShowDescriptionService
        _tvShowDescriptionService;


    public TVShowDescriptionController(
        ITVShowDescriptionService tvShowDescriptionService)
    {
        _tvShowDescriptionService =
            tvShowDescriptionService;
    }


    // ==================================================
    // Index
    // ==================================================

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var descriptions =
            await _tvShowDescriptionService
                .GetAllAsync();

        return View(descriptions);
    }


    // ==================================================
    // Create - GET
    // ==================================================

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var dto =
            await _tvShowDescriptionService
                .GetCreateDataAsync();

        return View(dto);
    }


    // ==================================================
    // Create - POST
    // ==================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TVShowDescriptionCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            await _tvShowDescriptionService
                .FillCreateFormDataAsync(dto);

            return View(dto);
        }


        var result =
            await _tvShowDescriptionService
                .CreateAsync(dto);


        if (!result)
        {
            ModelState.AddModelError(string.Empty,
                "امکان ثبت شرح تی‌وی شو وجود ندارد. ممکن است برای این تی‌وی شو قبلاً شرح ثبت شده باشد.");

            await _tvShowDescriptionService
                .FillCreateFormDataAsync(dto);

            return View(dto);
        }


        TempData["SuccessMessage"] =
            "شرح کامل تی‌وی شو با موفقیت ثبت شد.";


        return RedirectToAction(nameof(Index));
    }


    // ==================================================
    // Edit - GET
    // ==================================================

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var description =
            await _tvShowDescriptionService
                .GetEditDataAsync(id);


        if (description == null)
            return NotFound();


        return View(description);
    }


    // ==================================================
    // Edit - POST
    // ==================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(TVShowDescriptionEditDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);


        var result =
            await _tvShowDescriptionService
                .UpdateAsync(dto);


        if (!result)
        {
            ModelState.AddModelError(
                string.Empty,
                "امکان ویرایش شرح تی‌وی شو وجود ندارد. ممکن است Slug واردشده قبلاً استفاده شده باشد.");

            return View(dto);
        }


        TempData["SuccessMessage"] =
            "شرح کامل تی‌وی شو و تنظیمات سئو با موفقیت ویرایش شد.";


        return RedirectToAction(nameof(Details),
            new
            {
                id = dto.Id
            });
    }


    // ==================================================
    // Details
    // ==================================================

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var description =
            await _tvShowDescriptionService
                .GetDetailsAsync(id);


        if (description == null)
            return NotFound();


        return View(description);
    }
}
