using DataBase.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Service.Extention.ImageExtention;
namespace Service.Admin.Settings.Logo;

public interface ILogoService
{
    Task<LogoManageDto?> GetAsync();

    Task<bool> UpdateAsync(LogoManageDto dto);
}

public class LogoService : ILogoService
{
    private readonly MyContext _context;
    private readonly IWebHostEnvironment _environment;

    public LogoService(
        MyContext context,
        IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    public async Task<LogoManageDto?> GetAsync()
    {
        var logo = await _context.Logos
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == 1);

        if (logo == null)
            return null;

        return new LogoManageDto
        {
            Id = logo.Id,
            ExistingMainLogo = logo.MainLogoUrl,
            ExistingFooterLogo = logo.FooterLogoUrl,
            ExistingFavicon = logo.FaviconUrl
        };
    }

    public async Task<bool> UpdateAsync(LogoManageDto dto)
    {
        var logo = await _context.Logos
            .FirstOrDefaultAsync(x => x.Id == 1);

        if (logo == null)
            return false;

        if (dto.MainLogo != null && dto.MainLogo.Length > 0)
        {
            logo.MainLogoUrl = await dto.MainLogo.SaveImageAsync(
                _environment.WebRootPath,
                "logos",
                "main");
        }

        if (dto.FooterLogo != null && dto.FooterLogo.Length > 0)
        {
            logo.FooterLogoUrl = await dto.FooterLogo.SaveImageAsync(
                _environment.WebRootPath,
                "logos",
                "footer");
        }

        if (dto.Favicon != null && dto.Favicon.Length > 0)
        {
            logo.FaviconUrl = await dto.Favicon.SaveImageAsync(
                _environment.WebRootPath,
                "logos",
                "favicon");
        }

        await _context.SaveChangesAsync();

        return true;
    }
}