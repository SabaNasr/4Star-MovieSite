using Microsoft.AspNetCore.Mvc;
using Service.Admin.Movies.MovieDescription;
namespace _4Star.Areas.AdminPanel.Controllers;

[Area("AdminPanel")]
public class MovieDescriptionController : Controller
{
    private readonly IMovieDescriptionService _movieDescriptionService;


    public MovieDescriptionController(
        IMovieDescriptionService movieDescriptionService)
    {
        _movieDescriptionService = movieDescriptionService;
    }


    // ==================================================
    // Index
    // ==================================================

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var descriptions =
            await _movieDescriptionService.GetAllAsync();

        return View(descriptions);
    }


    // ==================================================
    // Create - GET
    // ==================================================

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var dto =
            await _movieDescriptionService.GetCreateDataAsync();

        return View(dto);
    }


    // ==================================================
    // Create - POST
    // ==================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        MovieDescriptionCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            await _movieDescriptionService
                .FillCreateFormDataAsync(dto);

            return View(dto);
        }


        var result =
            await _movieDescriptionService.CreateAsync(dto);


        if (!result)
        {
            ModelState.AddModelError(
                string.Empty,
                "امکان ثبت شرح فیلم وجود ندارد. ممکن است برای این فیلم قبلاً شرح ثبت شده باشد.");

            await _movieDescriptionService
                .FillCreateFormDataAsync(dto);

            return View(dto);
        }


        TempData["SuccessMessage"] =
            "شرح کامل فیلم با موفقیت ثبت شد.";


        return RedirectToAction(nameof(Index));
    }


    // ==================================================
    // Edit - GET
    // ==================================================

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var description =
            await _movieDescriptionService.GetEditDataAsync(id);


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
        MovieDescriptionEditDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);


        var result =
            await _movieDescriptionService.UpdateAsync(dto);


        if (!result)
        {
            ModelState.AddModelError(
                string.Empty,
                "امکان ویرایش شرح فیلم وجود ندارد.");

            return View(dto);
        }


        TempData["SuccessMessage"] =
            "شرح کامل فیلم با موفقیت ویرایش شد.";


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
        var description =
            await _movieDescriptionService.GetDetailsAsync(id);


        if (description == null)
            return NotFound();


        return View(description);
    }
}