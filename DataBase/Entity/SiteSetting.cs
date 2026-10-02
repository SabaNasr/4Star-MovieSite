namespace DataBase.Entity;

public class SiteSetting:BaseEntity
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string? Group { get; set; }  // "Contact", "Social", "General", "SEO"
    public string? Description { get; set; }
    
}
