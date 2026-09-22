using Microsoft.AspNetCore.Http;
namespace Service.Extention.ImageExtention;

public static class ImageExtensions
{
    // 1. ذخیره تصویر با مسیر دلخواه و پوشه لحظه‌ای
    public static async Task<string> SaveImageAsync(
        this IFormFile file,
        string webRootPath,
        string customFolder = "uploads",
        string? subFolder = null)  // پوشه لحظه‌ای مثل: "products/2024/12"
    {
        if (file == null || file.Length == 0)
            return string.Empty;

        // اعتبارسنجی
        var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".svg", ".webp" };
        var fileExtension = Path.GetExtension(file.FileName).ToLower(); //پسوند فایل رو بگیره

        if (!validExtensions.Contains(fileExtension)) //بررسی پسوند
            throw new Exception("فرمت فایل پشتیبانی نمی‌شود");

        if (file.Length > 2 * 1024 * 1024) //محدود کردن حجم
            throw new Exception("حجم فایل باید کمتر از 2 مگابایت باشد");

        // ساخت مسیر کامل پوشه
        var fullFolderPath = string.IsNullOrEmpty(subFolder)
            ? Path.Combine(webRootPath, customFolder)
            : Path.Combine(webRootPath, customFolder, subFolder);

        if (!Directory.Exists(fullFolderPath))
            Directory.CreateDirectory(fullFolderPath);

        // نام یکتا
        var uniqueFileName = $"{Guid.NewGuid()}_{DateTime.Now:yyyyMMddHHmmss}{fileExtension}";
        var filePath = Path.Combine(fullFolderPath, uniqueFileName);

        // ذخیره
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }

        // مسیر نسبی برای ذخیره در دیتابیس
        var relativePath = string.IsNullOrEmpty(subFolder)
            ? $"/{customFolder}/{uniqueFileName}"
            : $"/{customFolder}/{subFolder}/{uniqueFileName}";

        return relativePath;
    }

    // 2. ویرایش تصویر با مسیر و پوشه دلخواه
    public static async Task<string> UpdateImageAsync(
        this IFormFile newFile,
        string oldImagePath,
        string webRootPath,
        string customFolder = "uploads",
        string? subFolder = null)
    {
        // حذف تصویر قدیمی
        if (!string.IsNullOrEmpty(oldImagePath))
        {
            var oldFilePath = Path.Combine(webRootPath, oldImagePath.TrimStart('/'));
            if (File.Exists(oldFilePath))
                File.Delete(oldFilePath);
        }

        // ذخیره تصویر جدید
        return await newFile.SaveImageAsync(webRootPath, customFolder, subFolder);
    }

    // 3. ذخیره چند تصویر با مسیر و پوشه دلخواه
    public static async Task<List<string>> SaveMultipleImagesAsync(
        this List<IFormFile> files,
        string webRootPath,
        string customFolder = "uploads",
        string? subFolder = null)
    {
        if (files == null || !files.Any())
            return new List<string>();

        var imagePaths = new List<string>();

        foreach (var file in files)
        {
            if (file != null && file.Length > 0)
            {
                var path = await file.SaveImageAsync(webRootPath, customFolder, subFolder);
                if (!string.IsNullOrEmpty(path))
                    imagePaths.Add(path);
            }
        }

        return imagePaths;
    }

    // 4. ذخیره تصویر با مسیر کامل دلخواه (دقیقاً خودت مسیر رو مشخص کن)
    public static async Task<string> SaveImageWithFullPathAsync(
        this IFormFile file,
        string fullPath)  // مثلاً: "D:/MyProject/wwwroot/images/logo.jpg"
    {
        if (file == null || file.Length == 0)
            return string.Empty;

        // اعتبارسنجی
        var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".svg", ".webp" };
        var fileExtension = Path.GetExtension(file.FileName).ToLower();

        if (!validExtensions.Contains(fileExtension))
            throw new Exception("فرمت فایل پشتیبانی نمی‌شود");

        // ساخت پوشه اگر وجود نداشت
        var directory = Path.GetDirectoryName(fullPath);
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory!);

        // ذخیره
        using (var fileStream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }

        return fullPath;
    }
}