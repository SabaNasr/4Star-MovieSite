using Microsoft.AspNetCore.Mvc;
using Service.Admin.Blog.BlogPost;
namespace _4Star.Areas.AdminPanel.Controllers;

[Area("AdminPanel")]
public class BlogPostController : Controller
{
    private readonly IBlogPostService _blogPostService;

    public BlogPostController(IBlogPostService blogPostService)
    {
        _blogPostService = blogPostService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var posts = await _blogPostService.GetAllAsync();

        return View(posts);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var dto = await _blogPostService.GetCreateDataAsync();

        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BlogPostCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            await _blogPostService.FillCreateFormDataAsync(dto);

            return View(dto);
        }

        var result = await _blogPostService.CreateAsync(dto);

        if (!result)
        {
            ModelState.AddModelError(
                string.Empty,
                "امکان ثبت پست وجود ندارد. ممکن است Slug وارد شده قبلاً استفاده شده باشد یا اطلاعات پست معتبر نباشد.");

            await _blogPostService.FillCreateFormDataAsync(dto);

            return View(dto);
        }

        TempData["SuccessMessage"] =
            "پست وبلاگ با موفقیت ثبت شد.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var post = await _blogPostService.GetEditDataAsync(id);

        if (post == null)
            return NotFound();

        return View(post);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(BlogPostEditDto dto)
    {
        if (!ModelState.IsValid)
        {
            await _blogPostService.FillEditFormDataAsync(dto);

            return View(dto);
        }

        var result = await _blogPostService.UpdateAsync(dto);

        if (!result)
        {
            ModelState.AddModelError(
                string.Empty,
                "امکان ویرایش پست وجود ندارد. ممکن است Slug وارد شده قبلاً استفاده شده باشد.");

            await _blogPostService.FillEditFormDataAsync(dto);

            return View(dto);
        }

        TempData["SuccessMessage"] =
            "پست وبلاگ با موفقیت ویرایش شد.";

        return RedirectToAction(
            nameof(Details),
            new { id = dto.Id });
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var post = await _blogPostService.GetDetailsAsync(id);

        if (post == null)
            return NotFound();

        return View(post);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _blogPostService.DeleteAsync(id);

        TempData[result
            ? "SuccessMessage"
            : "ErrorMessage"] =
            result
                ? "پست وبلاگ با موفقیت حذف شد."
                : "پست مورد نظر پیدا نشد.";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeStatus(int id)
    {
        var result = await _blogPostService.ChangeStatusAsync(id);

        TempData[result
            ? "SuccessMessage"
            : "ErrorMessage"] =
            result
                ? "وضعیت پست با موفقیت تغییر کرد."
                : "پست مورد نظر پیدا نشد.";

        return RedirectToAction(nameof(Index));
    }
}
