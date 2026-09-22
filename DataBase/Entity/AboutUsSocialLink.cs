namespace DataBase.Entity;

public class AboutUsSocialLink : BaseEntity
{
    public string Icon { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public int Order { get; set; }

    public int TeamMemberId { get; set; }
    public AboutUsTeamMember TeamMember { get; set; } = null!;
}