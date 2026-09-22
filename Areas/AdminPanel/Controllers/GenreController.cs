using Microsoft.AspNetCore.Mvc;
using Service.Admin.Movies.Genre;
namespace _4Star.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class GenreController : Controller
    {
        private readonly IGenreService _genreService;

        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        // ==================================================
        // Index
        // ==================================================

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var genres = await _genreService.GetAllAsync();

            return View(genres);
        }

        // ==================================================
        // Create - GET
        // ==================================================

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // ==================================================
        // Create - POST
        // ==================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GenreCreateDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _genreService.CreateAsync(dto);

            if (!result)
            {
                ModelState.AddModelError(
                    nameof(dto.Name),
                    "این ژانر قبلاً ثبت شده است.");

                return View(dto);
            }

            TempData["SuccessMessage"] = "ژانر با موفقیت ثبت شد.";

            return RedirectToAction(nameof(Index));
        }

        // ==================================================
        // Edit - GET
        // ==================================================

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _genreService.GetEditDataAsync(id);

            if (model == null)
                return NotFound();

            return View(model);
        }

        // ==================================================
        // Edit - POST
        // ==================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(GenreEditDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _genreService.UpdateAsync(dto);

            if (!result)
            {
                ModelState.AddModelError(
                    nameof(dto.Name),
                    "ژانر پیدا نشد یا نام وارد شده قبلاً استفاده شده است.");

                return View(dto);
            }

            TempData["SuccessMessage"] = "ژانر با موفقیت بروزرسانی شد.";

            return RedirectToAction(nameof(Index));
        }

        // ==================================================
        // Change Status
        // ==================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            var result = await _genreService.ChangeStatusAsync(id);

            TempData[result ? "SuccessMessage" : "ErrorMessage"] =
                result
                    ? "وضعیت ژانر با موفقیت تغییر کرد."
                    : "ژانر مورد نظر پیدا نشد.";

            return RedirectToAction(nameof(Index));
        }
    }
}
