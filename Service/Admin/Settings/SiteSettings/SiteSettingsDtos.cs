namespace Service.Admin.Settings.SiteSettings;

public class SiteSettingItemDto
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class SiteSettingGroupDto
{
    public string GroupName { get; set; } = string.Empty; // مثلاً "General" یا "Contact"
    public string GroupDisplayName { get; set; } = string.Empty; // مثلاً "تنظیمات عمومی"
    public List<SiteSettingItemDto> Items { get; set; } = new List<SiteSettingItemDto>();
}