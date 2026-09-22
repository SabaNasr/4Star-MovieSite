using DataBase.Context;
using Microsoft.EntityFrameworkCore;
namespace Service.Admin.Content.AboutUsSocialLink;

public interface IAboutUsSocialLinkService
{
    Task<List<AboutUsSocialLinkListDto>> GetAllAsync();
    Task<AboutUsSocialLinkCreateDto> GetCreateDataAsync();
    Task FillCreateFormDataAsync(
        AboutUsSocialLinkCreateDto dto);
    Task<bool> CreateAsync(
        AboutUsSocialLinkCreateDto dto);
    Task<AboutUsSocialLinkEditDto?> GetEditDataAsync(
        int id);
    Task<bool> UpdateAsync(
        AboutUsSocialLinkEditDto dto);
    Task<AboutUsSocialLinkDetailsDto?> GetDetailsAsync(
        int id);
    Task<bool> ChangeStatusAsync(int id);
    Task<bool> DeleteAsync(int id);
}

public class AboutUsSocialLinkService
    : IAboutUsSocialLinkService
{
    private readonly MyContext _context;

    public AboutUsSocialLinkService(MyContext context)
    {
        _context = context;
    }

    public async Task<List<AboutUsSocialLinkListDto>>
        GetAllAsync()
    {
        return await _context.AboutUsSocialLinks
            .AsNoTracking()
            .OrderBy(x => x.TeamMember.FullName)
            .ThenBy(x => x.Order)
            .Select(x => new AboutUsSocialLinkListDto
            {
                Id = x.Id,
                Icon = x.Icon,
                Url = x.Url,
                Order = x.Order,
                IsActive = x.IsActive,
                TeamMemberId = x.TeamMemberId,
                TeamMemberName =
                    x.TeamMember.FullName,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<AboutUsSocialLinkCreateDto>
        GetCreateDataAsync()
    {
        var dto =
            new AboutUsSocialLinkCreateDto();

        await FillCreateFormDataAsync(dto);

        return dto;
    }

    public async Task FillCreateFormDataAsync(
        AboutUsSocialLinkCreateDto dto)
    {
        dto.TeamMembers =
            await _context.AboutUsTeamMembers
                .AsNoTracking()
                .OrderBy(x => x.Order)
                .ThenBy(x => x.FullName)
                .Select(x =>
                    new AboutUsSocialLinkTeamMemberItemDto
                    {
                        Id = x.Id,
                        FullName = x.FullName
                    })
                .ToListAsync();
    }

    public async Task<bool> CreateAsync(
        AboutUsSocialLinkCreateDto dto)
    {
        var teamMemberExists =
            await _context.AboutUsTeamMembers
                .AnyAsync(x => x.Id == dto.TeamMemberId);

        if (!teamMemberExists)
            return false;

        var socialLink = new DataBase.Entity.AboutUsSocialLink
        {
            TeamMemberId = dto.TeamMemberId,
            Icon = dto.Icon.Trim(),
            Url = dto.Url.Trim(),
            Order = dto.Order,
            IsActive = dto.IsActive
        };

        _context.AboutUsSocialLinks.Add(socialLink);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<AboutUsSocialLinkEditDto?>
        GetEditDataAsync(int id)
    {
        return await _context.AboutUsSocialLinks
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new AboutUsSocialLinkEditDto
            {
                Id = x.Id,
                TeamMemberId = x.TeamMemberId,
                Icon = x.Icon,
                Url = x.Url,
                Order = x.Order,
                IsActive = x.IsActive
            })
            .FirstOrDefaultAsync();
    }

    public async Task<bool> UpdateAsync(
        AboutUsSocialLinkEditDto dto)
    {
        var socialLink =
            await _context.AboutUsSocialLinks
                .FirstOrDefaultAsync(x => x.Id == dto.Id);

        if (socialLink == null)
            return false;

        var teamMemberExists =
            await _context.AboutUsTeamMembers
                .AnyAsync(x => x.Id == dto.TeamMemberId);

        if (!teamMemberExists)
            return false;

        socialLink.TeamMemberId =
            dto.TeamMemberId;

        socialLink.Icon = dto.Icon.Trim();
        socialLink.Url = dto.Url.Trim();
        socialLink.Order = dto.Order;
        socialLink.IsActive = dto.IsActive;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<AboutUsSocialLinkDetailsDto?>
        GetDetailsAsync(int id)
    {
        return await _context.AboutUsSocialLinks
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new AboutUsSocialLinkDetailsDto
            {
                Id = x.Id,
                Icon = x.Icon,
                Url = x.Url,
                Order = x.Order,
                IsActive = x.IsActive,
                TeamMemberId = x.TeamMemberId,
                TeamMemberName =
                    x.TeamMember.FullName,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<bool> ChangeStatusAsync(int id)
    {
        var socialLink =
            await _context.AboutUsSocialLinks
                .FirstOrDefaultAsync(x => x.Id == id);

        if (socialLink == null)
            return false;

        socialLink.IsActive =
            !socialLink.IsActive;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var socialLink =
            await _context.AboutUsSocialLinks
                .FirstOrDefaultAsync(x => x.Id == id);

        if (socialLink == null)
            return false;

        _context.AboutUsSocialLinks.Remove(socialLink);

        await _context.SaveChangesAsync();

        return true;
    }
}