using DataBase.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Service.Extention.ImageExtention;

namespace Service.Admin.Movies.CastMember;

public interface ICastMemberService
{
    Task<List<CastMemberListDto>> GetAllAsync();

    Task<CastMemberEditDto?> GetEditDataAsync(int id);

    Task<CastMemberDetailsDto?> GetDetailsAsync(int id);

    Task<bool> CreateAsync(CastMemberCreateDto dto);

    Task<bool> UpdateAsync(CastMemberEditDto dto);

    Task<bool> ChangeStatusAsync(int id);
}

public class CastMemberService : ICastMemberService
{
    private readonly MyContext _context;
    private readonly IWebHostEnvironment _environment;

    public CastMemberService(
        MyContext context,
        IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    public async Task<List<CastMemberListDto>> GetAllAsync()
    {
        return await _context.CastMembers
            .AsNoTracking()
            .OrderBy(x => x.FullName)
            .Select(x => new CastMemberListDto
            {
                Id = x.Id,
                FullName = x.FullName,
                PhotoPath = x.PhotoPath,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            })
            .ToListAsync();
    }

    public async Task<CastMemberEditDto?> GetEditDataAsync(int id)
    {
        return await _context.CastMembers
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new CastMemberEditDto
            {
                Id = x.Id,
                FullName = x.FullName,
                ExistingPhotoPath = x.PhotoPath
            })
            .FirstOrDefaultAsync();
    }

    public async Task<CastMemberDetailsDto?> GetDetailsAsync(int id)
    {
        var castMember = await _context.CastMembers
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new CastMemberDetailsDto
            {
                Id = x.Id,
                FullName = x.FullName,
                PhotoPath = x.PhotoPath,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            })
            .FirstOrDefaultAsync();

        if (castMember == null)
            return null;

        var movies = await _context.Movies
            .AsNoTracking()
            .Where(x => x.CastMembers.Any(c => c.Id == id))
            .Select(x => new CastMemberMovieDto
            {
                MovieId = x.Id,
                MovieTitle = x.Title,
                CastType = _context
                    .Set<Dictionary<string, object>>("MovieCastMembers")
                    .Where(j =>
                        (int)j["MovieId"] == x.Id &&
                        (int)j["CastMemberId"] == id)
                    .Select(j => (DataBase.Enum.CastType)j["CastType"])
                    .First()
            })
            .ToListAsync();

        castMember.Movies = movies;

        return castMember;
    }

    public async Task<bool> CreateAsync(CastMemberCreateDto dto)
    {
        var fullName = dto.FullName.Trim();

        var exists = await _context.CastMembers
            .AnyAsync(x => x.FullName == fullName);

        if (exists)
            return false;

        var photoPath = string.Empty;

        if (dto.Photo != null)
        {
            photoPath = await dto.Photo.SaveImageAsync(
                _environment.WebRootPath,
                "uploads",
                "cast-members");
        }

        var castMember = new DataBase.Entity.CastMember
        {
            FullName = fullName,
            PhotoPath = photoPath,
            IsActive = true
        };

        _context.CastMembers.Add(castMember);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UpdateAsync(CastMemberEditDto dto)
    {
        var castMember = await _context.CastMembers
            .FirstOrDefaultAsync(x => x.Id == dto.Id);

        if (castMember == null)
            return false;

        var fullName = dto.FullName.Trim();

        var exists = await _context.CastMembers
            .AnyAsync(x =>
                x.Id != dto.Id &&
                x.FullName == fullName);

        if (exists)
            return false;

        castMember.FullName = fullName;

        if (dto.Photo != null)
        {
            castMember.PhotoPath = await dto.Photo.SaveImageAsync(
                _environment.WebRootPath,
                "uploads",
                "cast-members");
        }

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ChangeStatusAsync(int id)
    {
        var castMember = await _context.CastMembers
            .FirstOrDefaultAsync(x => x.Id == id);

        if (castMember == null)
            return false;

        castMember.IsActive = !castMember.IsActive;

        await _context.SaveChangesAsync();

        return true;
    }
}