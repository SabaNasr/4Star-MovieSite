using Microsoft.AspNetCore.Mvc;
using Service.Admin.Content.AboutUsTeamMember;
namespace _4Star.Areas.AdminPanel.Controllers;

[Area("AdminPanel")]
public class AboutUsTeamMemberController : Controller
{
    private readonly IAboutUsTeamMemberService
        _aboutUsTeamMemberService;

    public AboutUsTeamMemberController(
        IAboutUsTeamMemberService
            aboutUsTeamMemberService)
    {
        _aboutUsTeamMemberService =
            aboutUsTeamMemberService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var members =
            await _aboutUsTeamMemberService
                .GetAllAsync();

        return View(members);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var aboutUs =
            await _aboutUsTeamMemberService
                .GetAllAsync();

        return View(
            new AboutUsTeamMemberCreateDto
            {
                IsActive = true,
                AboutUsId =
                    aboutUs.FirstOrDefault()?.Id ?? 0
            });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        AboutUsTeamMemberCreateDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var result =
            await _aboutUsTeamMemberService
                .CreateAsync(dto);

        if (!result)
        {
            ModelState.AddModelError(
                string.Empty,
                "امکان ثبت عضو تیم وجود ندارد.");

            return View(dto);
        }

        TempData["SuccessMessage"] =
            "عضو تیم با موفقیت ثبت شد.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var member =
            await _aboutUsTeamMemberService
                .GetEditDataAsync(id);

        if (member == null)
            return NotFound();

        return View(member);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        AboutUsTeamMemberEditDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var result =
            await _aboutUsTeamMemberService
                .UpdateAsync(dto);

        if (!result)
        {
            ModelState.AddModelError(
                string.Empty,
                "امکان ویرایش عضو تیم وجود ندارد.");

            return View(dto);
        }

        TempData["SuccessMessage"] =
            "عضو تیم با موفقیت ویرایش شد.";

        return RedirectToAction(
            nameof(Details),
            new { id = dto.Id });
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var member =
            await _aboutUsTeamMemberService
                .GetDetailsAsync(id);

        if (member == null)
            return NotFound();

        return View(member);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeStatus(
        int id)
    {
        var result =
            await _aboutUsTeamMemberService
                .ChangeStatusAsync(id);

        TempData[result
            ? "SuccessMessage"
            : "ErrorMessage"] =
            result
                ? "وضعیت عضو تیم با موفقیت تغییر کرد."
                : "عضو تیم مورد نظر پیدا نشد.";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var result =
            await _aboutUsTeamMemberService
                .DeleteAsync(id);

        TempData[result
            ? "SuccessMessage"
            : "ErrorMessage"] =
            result
                ? "عضو تیم با موفقیت حذف شد."
                : "عضو تیم مورد نظر پیدا نشد.";

        return RedirectToAction(nameof(Index));
    }
}