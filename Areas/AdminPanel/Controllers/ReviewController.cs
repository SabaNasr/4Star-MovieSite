using Microsoft.AspNetCore.Mvc;
using Service.Admin.Movies.Review;
namespace _4Star.Areas.AdminPanel.Controllers;

// ==================================================
// Controller - Review
// ==================================================

[Area("AdminPanel")]
public class ReviewController : Controller
{
    private readonly IReviewService _reviewService;


    public ReviewController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }


    // ==================================================
    // لیست نظرات
    // ==================================================

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var reviews = await _reviewService.GetAllAsync();

        return View(reviews);
    }


    // ==================================================
    // جزئیات نظر
    // ==================================================

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var review = await _reviewService.GetDetailsAsync(id);


        if (review == null)
            return NotFound();


        return View(review);
    }


    // ==================================================
    // تایید نظر
    // ==================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve(int id)
    {
        var result = await _reviewService.ApproveAsync(id);


        TempData[result ? "SuccessMessage" : "ErrorMessage"] =
            result
                ? "نظر با موفقیت تایید شد."
                : "نظر مورد نظر پیدا نشد.";


        return RedirectToAction(nameof(Index));
    }


    // ==================================================
    // عدم تایید نظر
    // ==================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reject(int id)
    {
        var result = await _reviewService.RejectAsync(id);


        TempData[result ? "SuccessMessage" : "ErrorMessage"] =
            result
                ? "نظر از حالت تایید خارج شد."
                : "نظر مورد نظر پیدا نشد.";


        return RedirectToAction(nameof(Index));
    }


    // ==================================================
    // حذف نظر
    // ==================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _reviewService.DeleteAsync(id);


        TempData[result ? "SuccessMessage" : "ErrorMessage"] =
            result
                ? "نظر با موفقیت حذف شد."
                : "نظر مورد نظر پیدا نشد.";


        return RedirectToAction(nameof(Index));
    }
}