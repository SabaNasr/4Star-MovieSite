using Microsoft.AspNetCore.Mvc;
using Service.Admin.TvSeries.SeriesAdditionalInfo;
namespace _4Star.Areas.AdminPanel.Controllers;

[Area("AdminPanel")]
public class SeriesAdditionalInfoController : Controller
{
    private readonly ISeriesAdditionalInfoService
        _seriesAdditionalInfoService;

    public SeriesAdditionalInfoController(
        ISeriesAdditionalInfoService seriesAdditionalInfoService)
    {
        _seriesAdditionalInfoService =
            seriesAdditionalInfoService;
    }

    // ==================================================
    // Index
    // ==================================================

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var additionalInfos =
            await _seriesAdditionalInfoService
                .GetAllAsync();

        return View(additionalInfos);
    }

    // ==================================================
    // Create - GET
    // ==================================================

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var dto =
            await _seriesAdditionalInfoService
                .GetCreateDataAsync();

        return View(dto);
    }

    // ==================================================
    // Create - POST
    // ==================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        SeriesAdditionalInfoCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            await _seriesAdditionalInfoService
                .FillCreateFormDataAsync(dto);

            return View(dto);
        }

        var result =
            await _seriesAdditionalInfoService
                .CreateAsync(dto);

        if (!result)
        {
            ModelState.AddModelError(
                string.Empty,
                "امکان ثبت اطلاعات تکمیلی سریال وجود ندارد. ممکن است برای این سریال قبلاً اطلاعات تکمیلی ثبت شده باشد.");

            await _seriesAdditionalInfoService
                .FillCreateFormDataAsync(dto);

            return View(dto);
        }

        TempData["SuccessMessage"] =
            "اطلاعات تکمیلی سریال با موفقیت ثبت شد.";

        return RedirectToAction(nameof(Index));
    }

    // ==================================================
    // Edit - GET
    // ==================================================

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var additionalInfo =
            await _seriesAdditionalInfoService
                .GetEditDataAsync(id);

        if (additionalInfo == null)
            return NotFound();

        return View(additionalInfo);
    }

    // ==================================================
    // Edit - POST
    // ==================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        SeriesAdditionalInfoEditDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var result =
            await _seriesAdditionalInfoService
                .UpdateAsync(dto);

        if (!result)
        {
            ModelState.AddModelError(
                string.Empty,
                "امکان ویرایش اطلاعات تکمیلی سریال وجود ندارد.");

            return View(dto);
        }

        TempData["SuccessMessage"] =
            "اطلاعات تکمیلی سریال با موفقیت ویرایش شد.";

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
        var additionalInfo =
            await _seriesAdditionalInfoService
                .GetDetailsAsync(id);

        if (additionalInfo == null)
            return NotFound();

        return View(additionalInfo);
    }
}