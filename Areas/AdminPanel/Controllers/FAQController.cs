using Microsoft.AspNetCore.Mvc;
using Service.Admin.Content.FAQ;
namespace _4Star.Areas.AdminPanel.Controllers;

[Area("AdminPanel")]
public class FAQController : Controller
{
    private readonly IFAQService _faqService;

    public FAQController(IFAQService faqService)
    {
        _faqService = faqService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var faqs = await _faqService.GetAllAsync();

        return View(faqs);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var dto = await _faqService.GetCreateDataAsync();

        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(FAQCreateDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var result = await _faqService.CreateAsync(dto);

        if (!result)
        {
            ModelState.AddModelError(
                string.Empty,
                "امکان ثبت سوال متداول وجود ندارد.");

            return View(dto);
        }

        TempData["SuccessMessage"] =
            "سوال متداول با موفقیت ثبت شد.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var faq = await _faqService.GetEditDataAsync(id);

        if (faq == null)
            return NotFound();

        return View(faq);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(FAQEditDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var result = await _faqService.UpdateAsync(dto);

        if (!result)
        {
            ModelState.AddModelError(
                string.Empty,
                "امکان ویرایش سوال متداول وجود ندارد.");

            return View(dto);
        }

        TempData["SuccessMessage"] =
            "سوال متداول با موفقیت ویرایش شد.";

        return RedirectToAction(
            nameof(Details),
            new { id = dto.Id });
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var faq = await _faqService.GetDetailsAsync(id);

        if (faq == null)
            return NotFound();

        return View(faq);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeStatus(int id)
    {
        var result =
            await _faqService.ChangeStatusAsync(id);

        TempData[result
            ? "SuccessMessage"
            : "ErrorMessage"] =
            result
                ? "وضعیت سوال متداول با موفقیت تغییر کرد."
                : "سوال متداول مورد نظر پیدا نشد.";

        return RedirectToAction(nameof(Index));
    }
}