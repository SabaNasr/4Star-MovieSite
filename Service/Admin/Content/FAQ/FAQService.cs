using DataBase.Context;
using Microsoft.EntityFrameworkCore;
namespace Service.Admin.Content.FAQ;

public interface IFAQService
{
    Task<List<FAQListDto>> GetAllAsync();
    Task<FAQCreateDto> GetCreateDataAsync();
    Task<bool> CreateAsync(FAQCreateDto dto);
    Task<FAQEditDto?> GetEditDataAsync(int id);
    Task<bool> UpdateAsync(FAQEditDto dto);
    Task<FAQDetailsDto?> GetDetailsAsync(int id);
    Task<bool> ChangeStatusAsync(int id);
}

public class FAQService : IFAQService
{
    private readonly MyContext _context;

    public FAQService(MyContext context)
    {
        _context = context;
    }

    public async Task<List<FAQListDto>> GetAllAsync()
    {
        return await _context.FAQs
            .AsNoTracking()
            .OrderBy(x => x.Order)
            .ThenByDescending(x => x.CreatedAt)
            .Select(x => new FAQListDto
            {
                Id = x.Id,
                Question = x.Question,
                Answer = x.Answer,
                Order = x.Order,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            })
            .ToListAsync();
    }

    public Task<FAQCreateDto> GetCreateDataAsync()
    {
        var dto = new FAQCreateDto
        {
            IsActive = true,
            Order = 0
        };

        return Task.FromResult(dto);
    }

    public async Task<bool> CreateAsync(FAQCreateDto dto)
    {
        var faq = new DataBase.Entity.FAQ
        {
            Question = dto.Question.Trim(),
            Answer = dto.Answer.Trim(),
            Order = dto.Order,
            IsActive = dto.IsActive
        };

        _context.FAQs.Add(faq);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<FAQEditDto?> GetEditDataAsync(int id)
    {
        return await _context.FAQs
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new FAQEditDto
            {
                Id = x.Id,
                Question = x.Question,
                Answer = x.Answer,
                Order = x.Order,
                IsActive = x.IsActive
            })
            .FirstOrDefaultAsync();
    }

    public async Task<bool> UpdateAsync(FAQEditDto dto)
    {
        var faq = await _context.FAQs
            .FirstOrDefaultAsync(x => x.Id == dto.Id);

        if (faq == null)
            return false;

        faq.Question = dto.Question.Trim();
        faq.Answer = dto.Answer.Trim();
        faq.Order = dto.Order;
        faq.IsActive = dto.IsActive;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<FAQDetailsDto?> GetDetailsAsync(int id)
    {
        return await _context.FAQs
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new FAQDetailsDto
            {
                Id = x.Id,
                Question = x.Question,
                Answer = x.Answer,
                Order = x.Order,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<bool> ChangeStatusAsync(int id)
    {
        var faq = await _context.FAQs
            .FirstOrDefaultAsync(x => x.Id == id);

        if (faq == null)
            return false;

        faq.IsActive = !faq.IsActive;

        await _context.SaveChangesAsync();

        return true;
    }
}