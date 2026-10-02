namespace DataBase.Entity;

public class Banner:BaseEntity
{
    public string Image { get; set; } = string.Empty;
    public string? Alt { get; set; }
    public string? Text { get; set; }
    public string? Link { get; set; }
    public int Order { get; set; }

    public int? TagSearchId { get; set; }
    public TagSearch? TagSearch { get; set; }

}
