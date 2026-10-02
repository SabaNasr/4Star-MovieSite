using DataBase.Context;
using Microsoft.EntityFrameworkCore;
namespace Service.Admin.Settings.SiteSettings;

public interface ISiteSettingsService
{
    Task<List<SiteSettingGroupDto>> GetAllGroupedAsync();
    Task SaveGroupAsync(string groupName, Dictionary<string, string> settings);
}

public class SiteSettingsService : ISiteSettingsService
{
    private readonly MyContext _context;

    public SiteSettingsService(MyContext context) => _context = context;

    public async Task<List<SiteSettingGroupDto>> GetAllGroupedAsync()
    {
        var settings = await _context.SiteSettings.ToListAsync();

        // گروه‌بندی دستی برای نمایش مرتب در پنل ادمین
        var groups = new Dictionary<string, string>
        {
            { "General", "تنظیمات عمومی" },
            { "Contact", "اطلاعات تماس" },
            { "SEO", "تنظیمات سئو" }
        };

        var result = new List<SiteSettingGroupDto>();

        foreach (var grp in groups)
        {
            var groupSettings = settings
                .Where(x => x.Group == grp.Key)
                .Select(x => new SiteSettingItemDto
                {
                    Key = x.Key,
                    Value = x.Value ?? "",
                    Description = x.Description
                })
                .ToList();

            result.Add(new SiteSettingGroupDto
            {
                GroupName = grp.Key,
                GroupDisplayName = grp.Value,
                Items = groupSettings
            });
        }

        return result;
    }

    public async Task SaveGroupAsync(string groupName, Dictionary<string, string> settings)
    {
        // دریافت تمام تنظیمات این گروه از دیتابیس
        var dbSettings = await _context.SiteSettings
            .Where(x => x.Group == groupName)
            .ToListAsync();

        foreach (var item in settings)
        {
            var setting = dbSettings.FirstOrDefault(x => x.Key == item.Key);
            if (setting != null)
            {
                setting.Value = item.Value; // فقط مقدار را آپدیت می‌کنیم
            }
        }

        await _context.SaveChangesAsync();
    }
}