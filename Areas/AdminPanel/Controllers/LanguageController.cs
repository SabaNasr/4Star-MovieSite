using Microsoft.AspNetCore.Mvc;
using Service.Admin.Movies.Language;
namespace _4Star.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class LanguageController : Controller
    {
        private readonly ILanguageService _languageService;

        public LanguageController(ILanguageService languageService)
        {
            _languageService = languageService;
        }

        // ==================================================
        // Index
        // ==================================================

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var languages = await _languageService.GetAllAsync();

            return View(languages);
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
        public async Task<IActionResult> Create(LanguageCreateDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _languageService.CreateAsync(dto);

            if (!result)
            {
                ModelState.AddModelError(
                    nameof(dto.Name),
                    "این زبان قبلاً ثبت شده است.");

                return View(dto);
            }

            TempData["SuccessMessage"] = "زبان با موفقیت ثبت شد.";

            return RedirectToAction(nameof(Index));
        }

        // ==================================================
        // Edit - GET
        // ==================================================

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _languageService.GetEditDataAsync(id);

            if (model == null)
                return NotFound();

            return View(model);
        }

        // ==================================================
        // Edit - POST
        // ==================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LanguageEditDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _languageService.UpdateAsync(dto);

            if (!result)
            {
                ModelState.AddModelError(
                    nameof(dto.Name),
                    "زبان پیدا نشد یا نام وارد شده قبلاً استفاده شده است.");

                return View(dto);
            }

            TempData["SuccessMessage"] = "زبان با موفقیت بروزرسانی شد.";

            return RedirectToAction(nameof(Index));
        }

        // ==================================================
        // Change Status
        // ==================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            var result = await _languageService.ChangeStatusAsync(id);

            TempData[result ? "SuccessMessage" : "ErrorMessage"] =
                result
                    ? "وضعیت زبان با موفقیت تغییر کرد."
                    : "زبان مورد نظر پیدا نشد.";

            return RedirectToAction(nameof(Index));
        }
    }
}
