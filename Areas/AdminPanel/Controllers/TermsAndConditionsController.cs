using Microsoft.AspNetCore.Mvc;
using Service.Admin.Content.TermsAndConditions;
namespace _4Star.Areas.AdminPanel.Controllers;

[Area("AdminPanel")]
public class TermsAndConditionsController : Controller
{
    private readonly ITermsAndConditionsService
        _termsAndConditionsService;

    public TermsAndConditionsController(
        ITermsAndConditionsService termsAndConditionsService)
    {
        _termsAndConditionsService =
            termsAndConditionsService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var terms =
            await _termsAndConditionsService.GetAllAsync();

        return View(terms);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var dto =
            await _termsAndConditionsService
                .GetCreateDataAsync();

        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        TermsAndConditionsCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        var result =
            await _termsAndConditionsService
                .CreateAsync(dto);

        if (!result)
        {
            ModelState.AddModelError(
                nameof(dto.Content),
                "محتوای شرایط و ضوابط نمی‌تواند خالی باشد.");

            return View(dto);
        }

        TempData["SuccessMessage"] =
            "شرایط و ضوابط با موفقیت ثبت شد.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var terms =
            await _termsAndConditionsService
                .GetEditDataAsync(id);

        if (terms == null)
        {
            return NotFound();
        }

        return View(terms);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        TermsAndConditionsEditDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        var result =
            await _termsAndConditionsService
                .UpdateAsync(dto);

        if (!result)
        {
            ModelState.AddModelError(
                string.Empty,
                "امکان ویرایش شرایط و ضوابط وجود ندارد.");

            return View(dto);
        }

        TempData["SuccessMessage"] =
            "شرایط و ضوابط با موفقیت ویرایش شد.";

        return RedirectToAction(
            nameof(Details),
            new { id = dto.Id });
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var terms =
            await _termsAndConditionsService
                .GetDetailsAsync(id);

        if (terms == null)
        {
            return NotFound();
        }

        return View(terms);
    }
}