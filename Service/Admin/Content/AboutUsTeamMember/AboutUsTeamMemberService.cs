using DataBase.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Service.Extention.ImageExtention;
namespace Service.Admin.Content.AboutUsTeamMember;

public interface IAboutUsTeamMemberService
{
    Task<List<AboutUsTeamMemberListDto>> GetAllAsync();
    Task<bool> CreateAsync(AboutUsTeamMemberCreateDto dto);
    Task<AboutUsTeamMemberEditDto?> GetEditDataAsync(int id);
    Task<bool> UpdateAsync(AboutUsTeamMemberEditDto dto);
    Task<AboutUsTeamMemberDetailsDto?> GetDetailsAsync(int id);
    Task<bool> ChangeStatusAsync(int id);
    Task<bool> DeleteAsync(int id);
}

public class AboutUsTeamMemberService
    : IAboutUsTeamMemberService
{
    private readonly MyContext _context;
    private readonly IWebHostEnvironment _environment;

    public AboutUsTeamMemberService(
        MyContext context,
        IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    public async Task<List<AboutUsTeamMemberListDto>>
        GetAllAsync()
    {
        return await _context.AboutUsTeamMembers
            .AsNoTracking()
            .OrderBy(x => x.Order)
            .ThenBy(x => x.FullName)
            .Select(x => new AboutUsTeamMemberListDto
            {
                Id = x.Id,
                FullName = x.FullName,
                JobTitle = x.JobTitle,
                ImagePath = x.ImagePath,
                Order = x.Order,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<bool> CreateAsync(
        AboutUsTeamMemberCreateDto dto)
    {
        var aboutUsExists =
            await _context.AboutUs
                .AnyAsync(x => x.Id == dto.AboutUsId);

        if (!aboutUsExists)
            return false;

        if (dto.Image == null || dto.Image.Length == 0)
            return false;

        var imagePath =
            await dto.Image.SaveImageAsync(
                _environment.WebRootPath);

        if (string.IsNullOrWhiteSpace(imagePath))
            return false;

        var member = new DataBase.Entity.AboutUsTeamMember
        {
            FullName = dto.FullName.Trim(),
            JobTitle = dto.JobTitle.Trim(),
            ImagePath = imagePath,
            Order = dto.Order,
            IsActive = dto.IsActive,
            AboutUsId = dto.AboutUsId
        };

        _context.AboutUsTeamMembers.Add(member);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<AboutUsTeamMemberEditDto?>
        GetEditDataAsync(int id)
    {
        return await _context.AboutUsTeamMembers
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new AboutUsTeamMemberEditDto
            {
                Id = x.Id,
                ExistingImagePath = x.ImagePath,
                FullName = x.FullName,
                JobTitle = x.JobTitle,
                Order = x.Order,
                IsActive = x.IsActive,
                AboutUsId = x.AboutUsId
            })
            .FirstOrDefaultAsync();
    }

    public async Task<bool> UpdateAsync(
        AboutUsTeamMemberEditDto dto)
    {
        var member =
            await _context.AboutUsTeamMembers
                .FirstOrDefaultAsync(x => x.Id == dto.Id);

        if (member == null)
            return false;

        var aboutUsExists =
            await _context.AboutUs
                .AnyAsync(x => x.Id == dto.AboutUsId);

        if (!aboutUsExists)
            return false;

        if (dto.Image != null && dto.Image.Length > 0)
        {
            var imagePath =
                await dto.Image.SaveImageAsync(
                    _environment.WebRootPath);

            if (!string.IsNullOrWhiteSpace(imagePath))
                member.ImagePath = imagePath;
        }

        member.FullName = dto.FullName.Trim();
        member.JobTitle = dto.JobTitle.Trim();
        member.Order = dto.Order;
        member.IsActive = dto.IsActive;
        member.AboutUsId = dto.AboutUsId;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<AboutUsTeamMemberDetailsDto?>
        GetDetailsAsync(int id)
    {
        return await _context.AboutUsTeamMembers
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new AboutUsTeamMemberDetailsDto
            {
                Id = x.Id,
                FullName = x.FullName,
                JobTitle = x.JobTitle,
                ImagePath = x.ImagePath,
                Order = x.Order,
                IsActive = x.IsActive,
                AboutUsId = x.AboutUsId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<bool> ChangeStatusAsync(int id)
    {
        var member =
            await _context.AboutUsTeamMembers
                .FirstOrDefaultAsync(x => x.Id == id);

        if (member == null)
            return false;

        member.IsActive = !member.IsActive;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var member =
            await _context.AboutUsTeamMembers
                .FirstOrDefaultAsync(x => x.Id == id);

        if (member == null)
            return false;

        _context.AboutUsTeamMembers.Remove(member);

        await _context.SaveChangesAsync();

        return true;
    }
}
