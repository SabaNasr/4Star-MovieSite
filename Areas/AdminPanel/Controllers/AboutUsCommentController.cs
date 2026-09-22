using Microsoft.AspNetCore.Mvc;
using Service.Admin.Content.AboutUsComment;
namespace _4Star.Areas.AdminPanel.Controllers;

[Area("AdminPanel")]
public class AboutUsCommentController : Controller
{
    private readonly IAboutUsCommentService
        _aboutUsCommentService;

    public AboutUsCommentController(
        IAboutUsCommentService
            aboutUsCommentService)
    {
        _aboutUsCommentService =
            aboutUsCommentService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var comments =
            await _aboutUsCommentService
                .GetAllAsync();

        return View(comments);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var comment =
            await _aboutUsCommentService
                .GetDetailsAsync(id);

        if (comment == null)
            return NotFound();

        return View(comment);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve(int id)
    {
        var result =
            await _aboutUsCommentService
                .ApproveAsync(id);

        TempData[result
            ? "SuccessMessage"
            : "ErrorMessage"] =
            result
                ? "نظر با موفقیت تایید شد."
                : "نظر مورد نظر پیدا نشد.";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reject(int id)
    {
        var result =
            await _aboutUsCommentService
                .RejectAsync(id);

        TempData[result
            ? "SuccessMessage"
            : "ErrorMessage"] =
            result
                ? "نظر از حالت تایید خارج شد."
                : "نظر مورد نظر پیدا نشد.";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var result =
            await _aboutUsCommentService
                .DeleteAsync(id);

        TempData[result
            ? "SuccessMessage"
            : "ErrorMessage"] =
            result
                ? "نظر با موفقیت حذف شد."
                : "نظر مورد نظر پیدا نشد.";

        return RedirectToAction(nameof(Index));
    }
}