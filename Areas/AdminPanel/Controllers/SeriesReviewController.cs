using Microsoft.AspNetCore.Mvc;
using Service.Admin.TvSeries.SeriesReview;
namespace _4Star.Areas.AdminPanel.Controllers;

[Area("AdminPanel")]
public class SeriesReviewController : Controller
{
    private readonly ISeriesReviewService
        _seriesReviewService;

    public SeriesReviewController(
        ISeriesReviewService seriesReviewService)
    {
        _seriesReviewService =
            seriesReviewService;
    }

    // ==================================================
    // Index
    // ==================================================

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var reviews =
            await _seriesReviewService
                .GetAllAsync();

        return View(reviews);
    }

    // ==================================================
    // Details
    // ==================================================

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var review =
            await _seriesReviewService
                .GetDetailsAsync(id);

        if (review == null)
            return NotFound();

        return View(review);
    }

    // ==================================================
    // Approve
    // ==================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve(int id)
    {
        var result =
            await _seriesReviewService
                .ApproveAsync(id);

        TempData[
            result
                ? "SuccessMessage"
                : "ErrorMessage"] =
            result
                ? "نظر سریال با موفقیت تایید شد."
                : "نظر مورد نظر پیدا نشد.";

        return RedirectToAction(nameof(Index));
    }

    // ==================================================
    // Reject
    // ==================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reject(int id)
    {
        var result =
            await _seriesReviewService
                .RejectAsync(id);

        TempData[
            result
                ? "SuccessMessage"
                : "ErrorMessage"] =
            result
                ? "نظر سریال از حالت تایید خارج شد."
                : "نظر مورد نظر پیدا نشد.";

        return RedirectToAction(nameof(Index));
    }

    // ==================================================
    // Delete
    // ==================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var result =
            await _seriesReviewService
                .DeleteAsync(id);

        TempData[
            result
                ? "SuccessMessage"
                : "ErrorMessage"] =
            result
                ? "نظر سریال با موفقیت حذف شد."
                : "نظر مورد نظر پیدا نشد.";

        return RedirectToAction(nameof(Index));
    }
}