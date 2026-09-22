using DataBase.Enum;
namespace DataBase.Entity;

public class AboutUsComment : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public StarRating StarRating { get; set; }
    public bool IsApproved { get; set; }

    public int AboutUsId { get; set; }
    public AboutUs AboutUs { get; set; } = null!;
}