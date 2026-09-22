using Microsoft.AspNetCore.Mvc;
using Service.Admin.TvSeries.Seasons;
namespace _4Star.Areas.AdminPanel.Controllers;

[Area("AdminPanel")]
public class SeasonController : Controller
{
    private readonly ISeasonService _seasonService;

    public SeasonController(ISeasonService seasonService)
    {
        _seasonService = seasonService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var seasons = await _seasonService.GetAllAsync();

        return View(seasons);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var dto = await _seasonService.GetCreateDataAsync();

        await _seasonService.FillCreateFormDataAsync(dto);

        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SeasonCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            await _seasonService.FillCreateFormDataAsync(dto);

            return View(dto);
        }

        var result = await _seasonService.CreateAsync(dto);

        if (!result)
        {
            await _seasonService.FillCreateFormDataAsync(dto);

            ModelState.AddModelError(
                string.Empty,
                "امکان ایجاد فصل وجود ندارد. ممکن است سریال انتخاب‌شده وجود نداشته باشد یا این فصل قبلاً ثبت شده باشد.");

            return View(dto);
        }

        TempData["SuccessMessage"] =
            "فصل با موفقیت ایجاد شد.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var season = await _seasonService.GetEditDataAsync(id);

        if (season == null)
            return NotFound();

        return View(season);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(SeasonEditDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var result = await _seasonService.UpdateAsync(dto);

        if (!result)
        {
            ModelState.AddModelError(
                string.Empty,
                "امکان ویرایش فصل وجود ندارد. ممکن است این فصل برای سریال مورد نظر قبلاً ثبت شده باشد.");

            return View(dto);
        }

        TempData["SuccessMessage"] =
            "فصل با موفقیت ویرایش شد.";

        return RedirectToAction(
            nameof(Details),
            new { id = dto.Id });
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var season = await _seasonService.GetDetailsAsync(id);

        if (season == null)
            return NotFound();

        return View(season);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeStatus(int id)
    {
        var result = await _seasonService.ChangeStatusAsync(id);

        TempData[result ? "SuccessMessage" : "ErrorMessage"] =
            result
                ? "وضعیت فصل با موفقیت تغییر کرد."
                : "فصل مورد نظر پیدا نشد.";

        return RedirectToAction(nameof(Index));
    }
}