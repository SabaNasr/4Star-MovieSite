using System.ComponentModel.DataAnnotations;
namespace Service.Admin.Movies.MovieDescription;

public class MovieDescriptionListDto
{
    public int Id { get; set; }

    public int MovieId { get; set; }

    public string MovieTitle { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}


public class MovieDescriptionCreateDto
{
    [Required(ErrorMessage = "انتخاب فیلم الزامی است.")]
    public int MovieId { get; set; }

    [Required(ErrorMessage = "شرح کامل فیلم الزامی است.")]
    public string Content { get; set; } = string.Empty;

    public List<MovieDescriptionMovieItemDto> Movies { get; set; } = new();
}


public class MovieDescriptionEditDto
{
    public int Id { get; set; }

    public int MovieId { get; set; }

    public string MovieTitle { get; set; } = string.Empty;

    [Required(ErrorMessage = "شرح کامل فیلم الزامی است.")]
    public string Content { get; set; } = string.Empty;
}


public class MovieDescriptionDetailsDto
{
    public int Id { get; set; }

    public int MovieId { get; set; }

    public string MovieTitle { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}


public class MovieDescriptionMovieItemDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;
}