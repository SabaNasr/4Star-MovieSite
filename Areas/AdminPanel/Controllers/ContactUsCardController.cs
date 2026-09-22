using Microsoft.AspNetCore.Mvc;
using Service.Admin.Content.ContactUsCard;
namespace _4Star.Areas.AdminPanel.Controllers;

[Area("AdminPanel")]
public class ContactUsCardController : Controller
{
    private readonly IContactUsCardService
        _contactUsCardService;

    public ContactUsCardController(
        IContactUsCardService contactUsCardService)
    {
        _contactUsCardService =
            contactUsCardService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var cards =
            await _contactUsCardService.GetAllAsync();

        return View(cards);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var dto =
            await _contactUsCardService
                .GetCreateDataAsync();

        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        ContactUsCardCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        var result =
            await _contactUsCardService
                .CreateAsync(dto);

        if (!result)
        {
            ModelState.AddModelError(
                string.Empty,
                "امکان ثبت کارت وجود ندارد.");

            return View(dto);
        }

        TempData["SuccessMessage"] =
            "کارت تماس با ما با موفقیت ثبت شد.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var card =
            await _contactUsCardService
                .GetEditDataAsync(id);

        if (card == null)
        {
            return NotFound();
        }

        return View(card);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        ContactUsCardEditDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        var result =
            await _contactUsCardService
                .UpdateAsync(dto);

        if (!result)
        {
            ModelState.AddModelError(
                string.Empty,
                "امکان ویرایش کارت وجود ندارد.");

            return View(dto);
        }

        TempData["SuccessMessage"] =
            "کارت تماس با ما با موفقیت ویرایش شد.";

        return RedirectToAction(
            nameof(Details),
            new { id = dto.Id });
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var card =
            await _contactUsCardService
                .GetDetailsAsync(id);

        if (card == null)
        {
            return NotFound();
        }

        return View(card);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeStatus(int id)
    {
        var result =
            await _contactUsCardService
                .ChangeStatusAsync(id);

        TempData[result
            ? "SuccessMessage"
            : "ErrorMessage"] =
            result
                ? "وضعیت کارت با موفقیت تغییر کرد."
                : "کارت مورد نظر پیدا نشد.";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var result =
            await _contactUsCardService
                .DeleteAsync(id);

        TempData[result
            ? "SuccessMessage"
            : "ErrorMessage"] =
            result
                ? "کارت با موفقیت حذف شد."
                : "کارت مورد نظر پیدا نشد.";

        return RedirectToAction(nameof(Index));
    }
}
