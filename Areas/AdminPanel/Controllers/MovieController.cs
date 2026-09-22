using Microsoft.AspNetCore.Mvc;
namespace _4Star.Areas.AdminPanel.Controllers;

[Area("AdminPanel")]
public class MovieController : Controller
{
    private readonly IMovieService _movieService;


    public MovieController(IMovieService movieService)
    {
        _movieService = movieService;
    }


    // ==================================================
    // لیست فیلم‌ها
    // ==================================================

    public async Task<IActionResult> Index()
    {
        var movies = await _movieService.GetAllAsync();

        return View(movies);
    }


    // ==================================================
    // فرم ایجاد فیلم
    // ==================================================

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var model = await _movieService.GetCreateDataAsync();

        return View(model);
    }


    // ==================================================
    // ثبت فیلم جدید
    // ==================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MovieCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            await _movieService.FillCreateFormDataAsync(dto);

            return View(dto);
        }


        var movieId = await _movieService.CreateAsync(dto);


        TempData["SuccessMessage"] =
            "فیلم با موفقیت ثبت شد.";


        return RedirectToAction(nameof(Edit), new
        {
            id = movieId
        });
    }


    // ==================================================
    // فرم ویرایش فیلم
    // ==================================================

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var model = await _movieService.GetEditDataAsync(id);


        if (model == null)
        {
            return NotFound();
        }


        return View(model);
    }


    // ==================================================
    // ذخیره تغییرات فیلم
    // ==================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(MovieEditDto dto)
    {
        if (!ModelState.IsValid)
        {
            // دوباره پر کردن Checkboxها
            await _movieService.FillEditFormDataAsync(dto);

            return View(dto);
        }


        var result = await _movieService.UpdateAsync(dto);


        if (!result)
        {
            return NotFound();
        }


        TempData["SuccessMessage"] =
            "اطلاعات فیلم با موفقیت بروزرسانی شد.";


        return RedirectToAction(nameof(Edit), new
        {
            id = dto.Id
        });
    }


    // ==================================================
    // جزئیات فیلم
    // ==================================================

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var movie = await _movieService.GetDetailsAsync(id);


        if (movie == null)
        {
            return NotFound();
        }


        return View(movie);
    }

    // ==================================================
    // فیلم‌های آرشیو شده
    // ==================================================

    [HttpGet]
    public async Task<IActionResult> Archived()
    {
        var movies = await _movieService.GetArchivedAsync();

        return View(movies);
    }


    // ==================================================
    // فیلم‌های حذف شده
    // ==================================================

    [HttpGet]
    public async Task<IActionResult> Deleted()
    {
        var movies = await _movieService.GetDeletedAsync();

        return View(movies);
    }


    // ==================================================
    // حذف نرم فیلم
    // ==================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _movieService.DeleteAsync(id);


        if (!result)
        {
            TempData["ErrorMessage"] =
                "فیلم مورد نظر پیدا نشد.";

            return RedirectToAction(nameof(Index));
        }


        TempData["SuccessMessage"] =
            "فیلم با موفقیت حذف شد.";


        return RedirectToAction(nameof(Index));
    }


    // ==================================================
    // Restore فیلم حذف‌شده
    // ==================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Restore(int id)
    {
        var result = await _movieService.RestoreAsync(id);


        if (result)
        {
            TempData["SuccessMessage"] =
                "فیلم با موفقیت بازیابی شد.";
        }
        else
        {
            TempData["ErrorMessage"] =
                "بازیابی فیلم انجام نشد.";
        }


        return RedirectToAction(nameof(Index));
    }


    // ==================================================
    // فعال / غیرفعال کردن فیلم
    // ==================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeStatus(int id)
    {
        var result = await _movieService.ChangeStatusAsync(id);


        if (result)
        {
            TempData["SuccessMessage"] =
                "وضعیت فیلم با موفقیت تغییر کرد.";
        }
        else
        {
            TempData["ErrorMessage"] =
                "فیلم مورد نظر پیدا نشد.";
        }


        return RedirectToAction(nameof(Index));
    }


    // ==================================================
    // آرشیو فیلم
    // ==================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Archive(int id)
    {
        var result = await _movieService.ArchiveAsync(id);


        if (result)
        {
            TempData["SuccessMessage"] =
                "فیلم با موفقیت آرشیو شد.";
        }
        else
        {
            TempData["ErrorMessage"] =
                "عملیات آرشیو انجام نشد.";
        }


        return RedirectToAction(nameof(Index));
    }


    // ==================================================
    // خارج کردن از آرشیو
    // ==================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Unarchive(int id)
    {
        var result = await _movieService.UnarchiveAsync(id);


        if (result)
        {
            TempData["SuccessMessage"] =
                "فیلم از آرشیو خارج شد.";
        }
        else
        {
            TempData["ErrorMessage"] =
                "عملیات انجام نشد.";
        }


        return RedirectToAction(nameof(Index));
    }
}