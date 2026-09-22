using System.ComponentModel.DataAnnotations;
namespace Service.Admin.TvSeries.SeriesDescription;

public class SeriesDescriptionListDto
{
    public int Id { get; set; }
    public int SeriesId { get; set; }
    public string SeriesTitle { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class SeriesDescriptionCreateDto
{
    [Required(ErrorMessage = "انتخاب سریال الزامی است.")]
    public int SeriesId { get; set; }

    [Required(ErrorMessage = "شرح کامل سریال الزامی است.")]
    public string Content { get; set; } = string.Empty;

    public List<SeriesDescriptionSeriesItemDto> Series { get; set; } = new();
}

public class SeriesDescriptionEditDto
{
    public int Id { get; set; }

    public int SeriesId { get; set; }

    public string SeriesTitle { get; set; } = string.Empty;

    [Required(ErrorMessage = "شرح کامل سریال الزامی است.")]
    public string Content { get; set; } = string.Empty;
}

public class SeriesDescriptionDetailsDto
{
    public int Id { get; set; }
    public int SeriesId { get; set; }
    public string SeriesTitle { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class SeriesDescriptionSeriesItemDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
}