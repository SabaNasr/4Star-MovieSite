using Microsoft.AspNetCore.Mvc;
using Service.Admin.Movies.CastMember;
namespace _4Star.Areas.AdminPanel.Controllers;

[Area("AdminPanel")]
public class CastMemberController : Controller
{
    private readonly ICastMemberService _castMemberService;

    public CastMemberController(ICastMemberService castMemberService)
    {
        _castMemberService = castMemberService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var castMembers = await _castMemberService.GetAllAsync();

        return View(castMembers);
    }
    // ==================================================
    // فرم ایجادعوامل
    // ==================================================
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CastMemberCreateDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var result = await _castMemberService.CreateAsync(dto);

        if (!result)
        {
            ModelState.AddModelError(
                nameof(dto.FullName),
                "این شخص قبلاً ثبت شده است.");

            return View(dto);
        }

        TempData["SuccessMessage"] = "شخص با موفقیت ثبت شد.";

        return RedirectToAction(nameof(Index));
    }
    // ==================================================
    // ویرایش عوامل فیلم
    // ==================================================
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var model = await _castMemberService.GetEditDataAsync(id);

        if (model == null)
            return NotFound();

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(CastMemberEditDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var result = await _castMemberService.UpdateAsync(dto);

        if (!result)
        {
            ModelState.AddModelError(
                nameof(dto.FullName),
                "شخص پیدا نشد یا نام وارد شده قبلاً استفاده شده است.");

            return View(dto);
        }

        TempData["SuccessMessage"] =
            "اطلاعات شخص با موفقیت بروزرسانی شد.";

        return RedirectToAction(nameof(Index));
    }
    // ==================================================
    // جزئیات عوامل فیلم
    // ==================================================
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var model = await _castMemberService.GetDetailsAsync(id);

        if (model == null)
            return NotFound();

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeStatus(int id)
    {
        var result = await _castMemberService.ChangeStatusAsync(id);

        TempData[result ? "SuccessMessage" : "ErrorMessage"] =
            result
                ? "وضعیت شخص با موفقیت تغییر کرد."
                : "شخص مورد نظر پیدا نشد.";

        return RedirectToAction(nameof(Index));
    }
}