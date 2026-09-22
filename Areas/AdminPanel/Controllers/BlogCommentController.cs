using Microsoft.AspNetCore.Mvc;
using Service.Admin.Blog.BlogComment;
namespace _4Star.Areas.AdminPanel.Controllers;

[Area("AdminPanel")]
public class BlogCommentController : Controller
{
    private readonly IBlogCommentService _blogCommentService;

    public BlogCommentController(
        IBlogCommentService blogCommentService)
    {
        _blogCommentService = blogCommentService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var comments = await _blogCommentService.GetAllAsync();

        return View(comments);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var comment = await _blogCommentService.GetDetailsAsync(id);

        if (comment == null)
            return NotFound();

        return View(comment);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve(int id)
    {
        var result = await _blogCommentService.ApproveAsync(id);

        TempData[result ? "SuccessMessage" : "ErrorMessage"] =
            result
                ? "کامنت با موفقیت تایید شد."
                : "کامنت مورد نظر پیدا نشد.";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reject(int id)
    {
        var result = await _blogCommentService.RejectAsync(id);

        TempData[result ? "SuccessMessage" : "ErrorMessage"] =
            result
                ? "کامنت از حالت تایید خارج شد."
                : "کامنت مورد نظر پیدا نشد.";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _blogCommentService.DeleteAsync(id);

        TempData[result ? "SuccessMessage" : "ErrorMessage"] =
            result
                ? "کامنت با موفقیت حذف شد."
                : "کامنت مورد نظر پیدا نشد.";

        return RedirectToAction(nameof(Index));
    }
}