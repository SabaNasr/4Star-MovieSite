using Microsoft.AspNetCore.Mvc;
using Service.Admin.TvSeries.SeriesDescription;
namespace _4Star.Areas.AdminPanel.Controllers;

[Area("AdminPanel")]
public class SeriesDescriptionController: Controller
{
    private readonly ISeriesDescriptionService
        _seriesDescriptionService;


    public SeriesDescriptionController(
        ISeriesDescriptionService
            seriesDescriptionService)
    {
        _seriesDescriptionService =
            seriesDescriptionService;
    }


    // ==================================================
    // Index
    // ==================================================

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var descriptions =
            await _seriesDescriptionService
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
            await _seriesDescriptionService
                .GetCreateDataAsync();

        return View(dto);
    }


    // ==================================================
    // Create - POST
    // ==================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        SeriesDescriptionCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            await _seriesDescriptionService
                .FillCreateFormDataAsync(dto);

            return View(dto);
        }


        var result =
            await _seriesDescriptionService
                .CreateAsync(dto);


        if (!result)
        {
            ModelState.AddModelError(
                string.Empty,
                "امکان ثبت شرح کامل سریال وجود ندارد. ممکن است برای این سریال قبلاً شرح ثبت شده باشد.");

            await _seriesDescriptionService
                .FillCreateFormDataAsync(dto);

            return View(dto);
        }


        TempData["SuccessMessage"] =
            "شرح کامل سریال با موفقیت ثبت شد.";


        return RedirectToAction(
            nameof(Index));
    }


    // ==================================================
    // Edit - GET
    // ==================================================

    [HttpGet]
    public async Task<IActionResult> Edit(
        int id)
    {
        var description =
            await _seriesDescriptionService
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
    public async Task<IActionResult> Edit(
        SeriesDescriptionEditDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);


        var result =
            await _seriesDescriptionService
                .UpdateAsync(dto);


        if (!result)
        {
            ModelState.AddModelError(
                string.Empty,
                "امکان ویرایش شرح کامل سریال وجود ندارد. ممکن است Slug واردشده قبلاً استفاده شده باشد.");

            return View(dto);
        }


        TempData["SuccessMessage"] =
            "شرح کامل سریال و تنظیمات سئو با موفقیت ویرایش شد.";


        return RedirectToAction(
            nameof(Details),
            new
            {
                id = dto.Id
            });
    }


    // ==================================================
    // Details
    // ==================================================

    [HttpGet]
    public async Task<IActionResult> Details(
        int id)
    {
        var description =
            await _seriesDescriptionService
                .GetDetailsAsync(id);


        if (description == null)
            return NotFound();


        return View(description);
    }
}