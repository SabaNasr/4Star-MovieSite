using DataBase.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Service.Extention.ImageExtention;
namespace Service.Admin.Content.Banner;

public interface IBannerService
{
    Task<List<BannerListDto>> GetAllAsync();
    Task<List<BannerListDto>> GetDeletedAsync();
    Task<BannerUpdateDto?> GetByIdForUpdateAsync(int id);
    Task CreateAsync(BannerCreateDto dto);
    Task UpdateAsync(BannerUpdateDto dto);
    Task SoftDeleteAsync(int id);
    Task RestoreAsync(int id);

    Task<List<DataBase.Entity.TagSearch>>
        GetActiveTagsForDropdownAsync();
}

public class BannerService : IBannerService
{
    private readonly MyContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public BannerService(
        MyContext context,
        IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<List<BannerListDto>> GetAllAsync()
    {
        return await _context.Banners
            .Where(x => !x.IsDeleted)
            .Include(x => x.TagSearch)
            .OrderBy(x => x.Order)
            .Select(x => new BannerListDto
            {
                Id = x.Id,
                Image = x.Image,
                Text = x.Text,
                TagName = x.TagSearch != null
                    ? x.TagSearch.Title
                    : "بدون تگ",
                Order = x.Order,
                IsActive = x.IsActive
            })
            .ToListAsync();
    }

    public async Task<List<BannerListDto>> GetDeletedAsync()
    {
        return await _context.Banners
            .IgnoreQueryFilters()
            .Where(x => x.IsDeleted)
            .OrderByDescending(x => x.UpdatedAt)
            .Select(x => new BannerListDto
            {
                Id = x.Id,
                Image = x.Image,
                Text = x.Text,
                Order = x.Order,
                IsActive = x.IsActive
            })
            .ToListAsync();
    }

    public async Task<BannerUpdateDto?> GetByIdForUpdateAsync(int id)
    {
        var banner = await _context.Banners
            .FirstOrDefaultAsync(x =>
                x.Id == id &&
                !x.IsDeleted);

        if (banner == null)
            return null;

        return new BannerUpdateDto
        {
            Id = banner.Id,
            Alt = banner.Alt,
            Text = banner.Text,
            Link = banner.Link,
            TagSearchId = banner.TagSearchId,
            Order = banner.Order,
            IsActive = banner.IsActive,
            ExistingImage = banner.Image
        };
    }

    public async Task CreateAsync(BannerCreateDto dto)
    {
        var banner = new DataBase.Entity.Banner
        {
            Alt = dto.Alt,
            Text = dto.Text,
            Link = dto.Link,
            TagSearchId = dto.TagSearchId,
            Order = dto.Order,
            IsActive = dto.IsActive
        };

        _context.Banners.Add(banner);

        await _context.SaveChangesAsync();

        banner.Image = await dto.Image.SaveImageAsync(
            _webHostEnvironment.WebRootPath,
            "banners",
            banner.Id.ToString());

        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(BannerUpdateDto dto)
    {
        var banner = await _context.Banners
            .FindAsync(dto.Id);

        if (banner == null)
            return;

        banner.Alt = dto.Alt;
        banner.Text = dto.Text;
        banner.Link = dto.Link;
        banner.TagSearchId = dto.TagSearchId;
        banner.Order = dto.Order;
        banner.IsActive = dto.IsActive;

        if (dto.NewImage != null)
        {
            banner.Image = await dto.NewImage
                .UpdateImageAsync(
                    banner.Image,
                    _webHostEnvironment.WebRootPath,
                    "banners",
                    dto.Id.ToString());
        }

        await _context.SaveChangesAsync();
    }

    public async Task SoftDeleteAsync(int id)
    {
        var banner = await _context.Banners
            .FindAsync(id);

        if (banner != null)
        {
            _context.Banners.Remove(banner);
            await _context.SaveChangesAsync();
        }
    }

    public async Task RestoreAsync(int id)
    {
        var banner = await _context.Banners
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (banner != null)
        {
            banner.IsDeleted = false;
            banner.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<DataBase.Entity.TagSearch>>
        GetActiveTagsForDropdownAsync()
    {
        return await _context.TagSearches
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Title)
            .ToListAsync();
    }
}