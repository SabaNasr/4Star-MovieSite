namespace DataBase.Entity;

public class ContactUs : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;

    public List<ContactUsCard> Cards { get; set; } = new();
}