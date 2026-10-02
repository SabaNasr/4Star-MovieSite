using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
namespace Service.Admin.Settings.Logo;

public class LogoManageDto
{
    public int Id { get; set; }

    [Display(Name = "لوگوی اصلی (هدر)")]
    public IFormFile? MainLogo { get; set; }
    public string? ExistingMainLogo { get; set; }

    [Display(Name = "لوگوی فوتر")]
    public IFormFile? FooterLogo { get; set; }
    public string? ExistingFooterLogo { get; set; }

    [Display(Name = "فاوآیکون (Favicon)")]
    public IFormFile? Favicon { get; set; }
    public string? ExistingFavicon { get; set; }
}