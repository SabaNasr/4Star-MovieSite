using Microsoft.AspNetCore.Mvc;
using Service.Admin.TVShows.TVShowReview;
namespace _4Star.Areas.AdminPanel.Controllers;

[Area("AdminPanel")]
public class TVShowReviewController : Controller
{
    private readonly ITVShowReviewService
        _tvShowReviewService;

    public TVShowReviewController(
        ITVShowReviewService tvShowReviewService)
    {
        _tvShowReviewService =
            tvShowReviewService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var reviews =
            await _tvShowReviewService
                .GetAllAsync();

        return View(reviews);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var review =
            await _tvShowReviewService
                .GetDetailsAsync(id);

        if (review == null)
            return NotFound();

        return View(review);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve(int id)
    {
        var result =
            await _tvShowReviewService
                .ApproveAsync(id);

        TempData[
            result
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
            await _tvShowReviewService
                .RejectAsync(id);

        TempData[
            result
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
            await _tvShowReviewService
                .DeleteAsync(id);

        TempData[
            result
                ? "SuccessMessage"
                : "ErrorMessage"] =
            result
                ? "نظر با موفقیت حذف شد."
                : "نظر مورد نظر پیدا نشد.";

        return RedirectToAction(nameof(Index));
    }
}