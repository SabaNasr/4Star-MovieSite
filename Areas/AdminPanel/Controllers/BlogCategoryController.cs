using Microsoft.AspNetCore.Mvc;
using Service.Admin.Blog.BlogCategory;
namespace _4Star.Areas.AdminPanel.Controllers;

[Area("AdminPanel")]
public class BlogCategoryController : Controller
{
    private readonly IBlogCategoryService _blogCategoryService;

    public BlogCategoryController(
        IBlogCategoryService blogCategoryService)
    {
        _blogCategoryService = blogCategoryService;
    }


    // ==================================================
    // Index
    // ==================================================

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var categories = await _blogCategoryService.GetAllAsync();

        return View(categories);
    }


    // ==================================================
    // Create - GET
    // ==================================================

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var dto = await _blogCategoryService.GetCreateDataAsync();

        return View(dto);
    }


    // ==================================================
    // Create - POST
    // ==================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        BlogCategoryCreateDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);


        var result = await _blogCategoryService.CreateAsync(dto);

        if (!result)
        {
            ModelState.AddModelError(
                string.Empty,
                "دسته‌بندی با این نام یا Slug قبلاً ثبت شده است.");

            return View(dto);
        }


        TempData["SuccessMessage"] =
            "دسته‌بندی وبلاگ با موفقیت ایجاد شد.";

        return RedirectToAction(nameof(Index));
    }


    // ==================================================
    // Edit - GET
    // ==================================================

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var category =
            await _blogCategoryService.GetEditDataAsync(id);

        if (category == null)
            return NotFound();

        return View(category);
    }


    // ==================================================
    // Edit - POST
    // ==================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        BlogCategoryEditDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);


        var result =
            await _blogCategoryService.UpdateAsync(dto);

        if (!result)
        {
            ModelState.AddModelError(
                string.Empty,
                "امکان ویرایش دسته‌بندی وجود ندارد. ممکن است نام یا Slug تکراری باشد.");

            return View(dto);
        }


        TempData["SuccessMessage"] =
            "دسته‌بندی وبلاگ با موفقیت ویرایش شد.";

        return RedirectToAction(
            nameof(Details),
            new { id = dto.Id });
    }


    // ==================================================
    // Details
    // ==================================================

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var category =
            await _blogCategoryService.GetDetailsAsync(id);

        if (category == null)
            return NotFound();

        return View(category);
    }


    // ==================================================
    // Change Status
    // ==================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeStatus(int id)
    {
        var result =
            await _blogCategoryService.ChangeStatusAsync(id);

        TempData[result
            ? "SuccessMessage"
            : "ErrorMessage"] =
            result
                ? "وضعیت دسته‌بندی با موفقیت تغییر کرد."
                : "دسته‌بندی مورد نظر پیدا نشد.";

        return RedirectToAction(nameof(Index));
    }
}