using Microsoft.AspNetCore.Mvc;
using Service.Admin.Movies.MovieAdditionalInfo;
namespace _4Star.Areas.AdminPanel.Controllers;

[Area("AdminPanel")]
public class MovieAdditionalInfoController : Controller
{
    private readonly IMovieAdditionalInfoService
        _movieAdditionalInfoService;


    public MovieAdditionalInfoController(
        IMovieAdditionalInfoService movieAdditionalInfoService)
    {
        _movieAdditionalInfoService =
            movieAdditionalInfoService;
    }


    // ==================================================
    // Index
    // ==================================================

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var additionalInfos =
            await _movieAdditionalInfoService
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
            await _movieAdditionalInfoService
                .GetCreateDataAsync();

        return View(dto);
    }


    // ==================================================
    // Create - POST
    // ==================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        MovieAdditionalInfoCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            await _movieAdditionalInfoService
                .FillCreateFormDataAsync(dto);

            return View(dto);
        }


        var result =
            await _movieAdditionalInfoService
                .CreateAsync(dto);


        if (!result)
        {
            ModelState.AddModelError(
                string.Empty,
                "امکان ثبت اطلاعات تکمیلی فیلم وجود ندارد. ممکن است برای این فیلم قبلاً اطلاعات تکمیلی ثبت شده باشد.");

            await _movieAdditionalInfoService
                .FillCreateFormDataAsync(dto);

            return View(dto);
        }


        TempData["SuccessMessage"] =
            "اطلاعات تکمیلی فیلم با موفقیت ثبت شد.";


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
        var additionalInfo =
            await _movieAdditionalInfoService
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
        MovieAdditionalInfoEditDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);


        var result =
            await _movieAdditionalInfoService
                .UpdateAsync(dto);


        if (!result)
        {
            ModelState.AddModelError(
                string.Empty,
                "امکان ویرایش اطلاعات تکمیلی فیلم وجود ندارد. ممکن است Slug واردشده قبلاً استفاده شده باشد.");

            return View(dto);
        }


        TempData["SuccessMessage"] =
            "اطلاعات تکمیلی فیلم و تنظیمات سئو با موفقیت ویرایش شد.";


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
        var additionalInfo =
            await _movieAdditionalInfoService
                .GetDetailsAsync(id);


        if (additionalInfo == null)
            return NotFound();


        return View(additionalInfo);
    }
}