namespace DataBase.Entity;

public class ContactUsCard : BaseEntity
{
    public string Icon { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }

    public int ContactUsId { get; set; }
    public ContactUs ContactUs { get; set; } = null!;
}