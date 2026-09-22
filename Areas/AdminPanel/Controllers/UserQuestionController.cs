using Microsoft.AspNetCore.Mvc;
using Service.Admin.Content.UserQuestion;
namespace _4Star.Areas.AdminPanel.Controllers;

[Area("AdminPanel")]
public class UserQuestionController : Controller
{
    private readonly IUserQuestionService
        _userQuestionService;

    public UserQuestionController(
        IUserQuestionService userQuestionService)
    {
        _userQuestionService = userQuestionService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var questions =
            await _userQuestionService.GetAllAsync();

        return View(questions);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var question =
            await _userQuestionService
                .GetDetailsAsync(id);

        if (question == null)
            return NotFound();

        return View(question);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var result =
            await _userQuestionService.DeleteAsync(id);

        TempData[result
            ? "SuccessMessage"
            : "ErrorMessage"] =
            result
                ? "سوال کاربر با موفقیت حذف شد."
                : "سوال مورد نظر پیدا نشد.";

        return RedirectToAction(nameof(Index));
    }
}