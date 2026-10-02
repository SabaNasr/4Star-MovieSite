using DataBase.Context;
using Microsoft.EntityFrameworkCore;
namespace Service.Admin.Content.TagSearch;

public interface ITagSearchesService
{
    Task<List<TagSearchListDto>> GetAllAsync();
    Task<List<TagSearchListDto>> GetDeletedAsync();
    Task<TagSearchUpdateDto?> GetByIdForUpdateAsync(int id);
    Task CreateAsync(TagSearchCreateDto dto);
    Task UpdateAsync(TagSearchUpdateDto dto);
    Task SoftDeleteAsync(int id);
    Task RestoreAsync(int id);
}

public class TagSearchesService : ITagSearchesService
{
    private readonly MyContext _context;

    public TagSearchesService(MyContext context)
    {
        _context = context;
    }

    public async Task<List<TagSearchListDto>> GetAllAsync()
    {
        return await _context.TagSearches
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Title)
            .Select(x => new TagSearchListDto
            {
                Id = x.Id,
                Title = x.Title,
                Slug = x.Slug
            })
            .ToListAsync();
    }

    public async Task<List<TagSearchListDto>> GetDeletedAsync()
    {
        return await _context.TagSearches
            .IgnoreQueryFilters()
            .Where(x => x.IsDeleted)
            .OrderByDescending(x => x.UpdatedAt)
            .Select(x => new TagSearchListDto
            {
                Id = x.Id,
                Title = x.Title,
                Slug = x.Slug
            })
            .ToListAsync();
    }

    public async Task<TagSearchUpdateDto?> GetByIdForUpdateAsync(int id)
    {
        var tag = await _context.TagSearches.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (tag == null) return null;

        return new TagSearchUpdateDto
        {
            Id = tag.Id,
            Title = tag.Title,
            Slug = tag.Slug
        };
    }

    public async Task CreateAsync(TagSearchCreateDto dto)
    {
        var tag = new DataBase.Entity.TagSearch
        {
            Title = dto.Title,
            Slug = dto.Slug
        };

        _context.TagSearches.Add(tag);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TagSearchUpdateDto dto)
    {
        var tag = await _context.TagSearches.FindAsync(dto.Id);
        if (tag == null) return;

        tag.Title = dto.Title;
        tag.Slug = dto.Slug;

        await _context.SaveChangesAsync();
    }

    public async Task SoftDeleteAsync(int id)
    {
        var tag = await _context.TagSearches.FindAsync(id);
        if (tag != null)
        {
            _context.TagSearches.Remove(tag);
            await _context.SaveChangesAsync();
        }
    }

    public async Task RestoreAsync(int id)
    {
        var tag = await _context.TagSearches.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Id == id);
        if (tag != null)
        {
            tag.IsDeleted = false;
            tag.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}