using Microsoft.AspNetCore.Mvc;
using Service.Admin.TvSeries.Episodes;
namespace _4Star.Areas.AdminPanel.Controllers;

[Area("AdminPanel")]
public class EpisodeController : Controller
{
    private readonly IEpisodeService _episodeService;

    public EpisodeController(IEpisodeService episodeService)
    {
        _episodeService = episodeService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var episodes = await _episodeService.GetAllAsync();

        return View(episodes);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var dto = await _episodeService.GetCreateDataAsync();

        await _episodeService.FillCreateFormDataAsync(dto);

        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EpisodeCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            await _episodeService.FillCreateFormDataAsync(dto);

            return View(dto);
        }

        var result = await _episodeService.CreateAsync(dto);

        if (!result)
        {
            await _episodeService.FillCreateFormDataAsync(dto);

            ModelState.AddModelError(
                string.Empty,
                "امکان ایجاد قسمت وجود ندارد. ممکن است فصل انتخاب‌شده وجود نداشته باشد یا این قسمت قبلاً ثبت شده باشد.");

            return View(dto);
        }

        TempData["SuccessMessage"] =
            "قسمت با موفقیت ایجاد شد.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var episode = await _episodeService.GetEditDataAsync(id);

        if (episode == null)
            return NotFound();

        return View(episode);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EpisodeEditDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var result = await _episodeService.UpdateAsync(dto);

        if (!result)
        {
            ModelState.AddModelError(
                string.Empty,
                "امکان ویرایش قسمت وجود ندارد. ممکن است این قسمت برای فصل مورد نظر قبلاً ثبت شده باشد.");

            return View(dto);
        }

        TempData["SuccessMessage"] =
            "قسمت با موفقیت ویرایش شد.";

        return RedirectToAction(
            nameof(Details),
            new { id = dto.Id });
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var episode = await _episodeService.GetDetailsAsync(id);

        if (episode == null)
            return NotFound();

        return View(episode);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeStatus(int id)
    {
        var result = await _episodeService.ChangeStatusAsync(id);

        TempData[result ? "SuccessMessage" : "ErrorMessage"] =
            result
                ? "وضعیت قسمت با موفقیت تغییر کرد."
                : "قسمت مورد نظر پیدا نشد.";

        return RedirectToAction(nameof(Index));
    }
}