namespace DataBase.Entity;

public class AboutUsTeamMember : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public string ImagePath { get; set; } = string.Empty;
    public int Order { get; set; }

    public AboutUs AboutUs { get; set; } = null!;
    public int AboutUsId { get; set; }

    public List<AboutUsSocialLink> SocialLinks { get; set; } = new();
}
