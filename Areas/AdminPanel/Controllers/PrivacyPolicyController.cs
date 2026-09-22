using Microsoft.AspNetCore.Mvc;
using Service.Admin.Content.PrivacyPolicies;
namespace _4Star.Areas.AdminPanel.Controllers;

[Area("AdminPanel")]
public class PrivacyPolicyController : Controller
{
    private readonly IPrivacyPolicyService
        _privacyPolicyService;

    public PrivacyPolicyController(
        IPrivacyPolicyService privacyPolicyService)
    {
        _privacyPolicyService = privacyPolicyService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var privacyPolicies =
            await _privacyPolicyService.GetAllAsync();

        return View(privacyPolicies);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var dto =
            await _privacyPolicyService.GetCreateDataAsync();

        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        PrivacyPolicyCreateDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var result =
            await _privacyPolicyService.CreateAsync(dto);

        if (!result)
        {
            ModelState.AddModelError(
                string.Empty,
                "سیاست حفظ حریم خصوصی قبلاً ثبت شده است یا متن وارد شده معتبر نیست.");

            return View(dto);
        }

        TempData["SuccessMessage"] =
            "سیاست حفظ حریم خصوصی با موفقیت ایجاد شد.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var privacyPolicy =
            await _privacyPolicyService
                .GetEditDataAsync(id);

        if (privacyPolicy == null)
            return NotFound();

        return View(privacyPolicy);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        PrivacyPolicyEditDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var result =
            await _privacyPolicyService.UpdateAsync(dto);

        if (!result)
        {
            ModelState.AddModelError(
                string.Empty,
                "امکان ویرایش سیاست حفظ حریم خصوصی وجود ندارد.");

            return View(dto);
        }

        TempData["SuccessMessage"] =
            "سیاست حفظ حریم خصوصی با موفقیت ویرایش شد.";

        return RedirectToAction(
            nameof(Details),
            new { id = dto.Id });
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var privacyPolicy =
            await _privacyPolicyService
                .GetDetailsAsync(id);

        if (privacyPolicy == null)
            return NotFound();

        return View(privacyPolicy);
    }
}