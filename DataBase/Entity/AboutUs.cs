namespace DataBase.Entity;

public class AboutUs : BaseEntity
{
    public string ImagePath { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;

    // شمارنده‌های بخش بالای صفحه
    public int CustomerCount { get; set; }
    public int ActiveUserCount { get; set; }

    // شمارنده‌های بخش پایین صفحه
    public int TotalVideoCount { get; set; }
    public int SubscriberCount { get; set; }
    public int AwardCount { get; set; }

    public List<AboutUsComment> Comments { get; set; } = new();
    public List<AboutUsTeamMember> TeamMembers { get; set; } = new();
}