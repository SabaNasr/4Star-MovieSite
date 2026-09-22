using DataBase.Context;
using Microsoft.EntityFrameworkCore;
namespace Service.Admin.Blog.BlogCategory;

// ==================================================
// Interface 
// ==================================================
public interface IBlogCategoryService
{
    Task<List<BlogCategoryListDto>> GetAllAsync();

    Task<BlogCategoryCreateDto> GetCreateDataAsync();

    Task<bool> CreateAsync(BlogCategoryCreateDto dto);

    Task<BlogCategoryEditDto?> GetEditDataAsync(int id);

    Task<bool> UpdateAsync(BlogCategoryEditDto dto);

    Task<BlogCategoryDetailsDto?> GetDetailsAsync(int id);

    Task<bool> ChangeStatusAsync(int id);
}
// ==================================================
// Service 
// ==================================================
public class BlogCategoryService : IBlogCategoryService
{
    private readonly MyContext _context;

    public BlogCategoryService(MyContext context)
    {
        _context = context;
    }


    // ==================================================
    // Get All
    // ==================================================

    public async Task<List<BlogCategoryListDto>> GetAllAsync()
    {
        return await _context.BlogCategories
            .AsNoTracking()
            .OrderBy(x => x.DisplayOrder)
            .ThenBy(x => x.Name)
            .Select(x => new BlogCategoryListDto
            {
                Id = x.Id,
                Name = x.Name,
                Slug = x.Slug,
                Description = x.Description,
                DisplayOrder = x.DisplayOrder,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            })
            .ToListAsync();
    }


    // ==================================================
    // Get Create Data
    // ==================================================

    public Task<BlogCategoryCreateDto> GetCreateDataAsync()
    {
        var dto = new BlogCategoryCreateDto
        {
            IsActive = true
        };

        return Task.FromResult(dto);
    }


    // ==================================================
    // Create
    // ==================================================

    public async Task<bool> CreateAsync(BlogCategoryCreateDto dto)
    {
        var name = dto.Name.Trim();
        var slug = dto.Slug.Trim();

        var nameExists = await _context.BlogCategories
            .AnyAsync(x => x.Name == name);

        if (nameExists)
            return false;

        var slugExists = await _context.BlogCategories
            .AnyAsync(x => x.Slug == slug);

        if (slugExists)
            return false;

        var category = new DataBase.Entity.BlogCategory
        {
            Name = name,
            Slug = slug,
            Description = dto.Description.Trim(),
            DisplayOrder = dto.DisplayOrder,
            IsActive = dto.IsActive
        };

        _context.BlogCategories.Add(category);

        await _context.SaveChangesAsync();

        return true;
    }


    // ==================================================
    // Get Edit Data
    // ==================================================

    public async Task<BlogCategoryEditDto?> GetEditDataAsync(int id)
    {
        return await _context.BlogCategories
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new BlogCategoryEditDto
            {
                Id = x.Id,
                Name = x.Name,
                Slug = x.Slug,
                Description = x.Description,
                DisplayOrder = x.DisplayOrder,
                IsActive = x.IsActive
            })
            .FirstOrDefaultAsync();
    }


    // ==================================================
    // Update
    // ==================================================

    public async Task<bool> UpdateAsync(BlogCategoryEditDto dto)
    {
        var category = await _context.BlogCategories
            .FirstOrDefaultAsync(x => x.Id == dto.Id);

        if (category == null)
            return false;


        var name = dto.Name.Trim();
        var slug = dto.Slug.Trim();


        var nameExists = await _context.BlogCategories
            .AnyAsync(x =>
                x.Id != dto.Id &&
                x.Name == name);

        if (nameExists)
            return false;


        var slugExists = await _context.BlogCategories
            .AnyAsync(x =>
                x.Id != dto.Id &&
                x.Slug == slug);

        if (slugExists)
            return false;


        category.Name = name;
        category.Slug = slug;
        category.Description = dto.Description.Trim();
        category.DisplayOrder = dto.DisplayOrder;
        category.IsActive = dto.IsActive;

        await _context.SaveChangesAsync();

        return true;
    }


    // ==================================================
    // Get Details
    // ==================================================

    public async Task<BlogCategoryDetailsDto?> GetDetailsAsync(int id)
    {
        return await _context.BlogCategories
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new BlogCategoryDetailsDto
            {
                Id = x.Id,
                Name = x.Name,
                Slug = x.Slug,
                Description = x.Description,
                DisplayOrder = x.DisplayOrder,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            })
            .FirstOrDefaultAsync();
    }


    // ==================================================
    // Change Status
    // ==================================================

    public async Task<bool> ChangeStatusAsync(int id)
    {
        var category = await _context.BlogCategories
            .FirstOrDefaultAsync(x => x.Id == id);

        if (category == null)
            return false;

        category.IsActive = !category.IsActive;

        await _context.SaveChangesAsync();

        return true;
    }
}