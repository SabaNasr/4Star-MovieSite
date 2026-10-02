using Microsoft.AspNetCore.Mvc;
using Service.Admin.TVShows.TVShowAdditionalInfo;
namespace _4Star.Areas.AdminPanel.Controllers;

[Area("AdminPanel")]
public class TVShowAdditionalInfoController : Controller
{
    private readonly ITVShowAdditionalInfoService
        _tvShowAdditionalInfoService;


    public TVShowAdditionalInfoController(
        ITVShowAdditionalInfoService tvShowAdditionalInfoService)
    {
        _tvShowAdditionalInfoService =
            tvShowAdditionalInfoService;
    }


    // ==================================================
    // Index
    // ==================================================

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var additionalInfos =
            await _tvShowAdditionalInfoService
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
            await _tvShowAdditionalInfoService
                .GetCreateDataAsync();

        return View(dto);
    }


    // ==================================================
    // Create - POST
    // ==================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TVShowAdditionalInfoCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            await _tvShowAdditionalInfoService
                .FillCreateFormDataAsync(dto);

            return View(dto);
        }


        var result =
            await _tvShowAdditionalInfoService
                .CreateAsync(dto);


        if (!result)
        {
            ModelState.AddModelError(string.Empty,
                "امکان ثبت اطلاعات تکمیلی تی‌وی شو وجود ندارد. ممکن است برای این تی‌وی شو قبلاً اطلاعات تکمیلی ثبت شده باشد.");

            await _tvShowAdditionalInfoService
                .FillCreateFormDataAsync(dto);

            return View(dto);
        }


        TempData["SuccessMessage"] =
            "اطلاعات تکمیلی تی‌وی شو با موفقیت ثبت شد.";


        return RedirectToAction(nameof(Index));
    }


    // ==================================================
    // Edit - GET
    // ==================================================

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var additionalInfo =
            await _tvShowAdditionalInfoService
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
    public async Task<IActionResult> Edit(TVShowAdditionalInfoEditDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);


        var result =
            await _tvShowAdditionalInfoService
                .UpdateAsync(dto);


        if (!result)
        {
            ModelState.AddModelError(
                string.Empty,
                "امکان ویرایش اطلاعات تکمیلی تی‌وی شو وجود ندارد. ممکن است Slug واردشده قبلاً استفاده شده باشد.");

            return View(dto);
        }


        TempData["SuccessMessage"] =
            "اطلاعات تکمیلی تی‌وی شو و تنظیمات سئو با موفقیت ویرایش شد.";


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
        var additionalInfo =
            await _tvShowAdditionalInfoService
                .GetDetailsAsync(id);


        if (additionalInfo == null)
            return NotFound();


        return View(additionalInfo);
    }
}