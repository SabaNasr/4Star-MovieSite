using DataBase.Context;
using Microsoft.EntityFrameworkCore;
namespace Service.Admin.Content.ContactUsCard;

public interface IContactUsCardService
{
    Task<List<ContactUsCardListDto>> GetAllAsync();

    Task<ContactUsCardCreateDto>
        GetCreateDataAsync();

    Task<bool> CreateAsync(
        ContactUsCardCreateDto dto);

    Task<ContactUsCardEditDto?>
        GetEditDataAsync(int id);

    Task<bool> UpdateAsync(
        ContactUsCardEditDto dto);

    Task<ContactUsCardDetailsDto?>
        GetDetailsAsync(int id);

    Task<bool> ChangeStatusAsync(
        int id);

    Task<bool> DeleteAsync(
        int id);
}

public class ContactUsCardService
    : IContactUsCardService
{
    private readonly MyContext _context;

    public ContactUsCardService(MyContext context)
    {
        _context = context;
    }

    public async Task<List<ContactUsCardListDto>>
        GetAllAsync()
    {
        return await _context.ContactUsCards
            .AsNoTracking()
            .OrderBy(x => x.DisplayOrder)
            .ThenBy(x => x.Title)
            .Select(x => new ContactUsCardListDto
            {
                Id = x.Id,
                Icon = x.Icon,
                Title = x.Title,
                Content = x.Content,
                DisplayOrder = x.DisplayOrder,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            })
            .ToListAsync();
    }

    public Task<ContactUsCardCreateDto>
        GetCreateDataAsync()
    {
        var dto = new ContactUsCardCreateDto
        {
            IsActive = true
        };

        return Task.FromResult(dto);
    }

    public async Task<bool> CreateAsync(
        ContactUsCardCreateDto dto)
    {
        var card = new DataBase.Entity.ContactUsCard
        {
            Icon = dto.Icon.Trim(),
            Title = dto.Title.Trim(),
            Content = dto.Content.Trim(),
            DisplayOrder = dto.DisplayOrder,
            IsActive = dto.IsActive
        };

        var contactUs = await _context.ContactUs
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync();

        if (contactUs == null)
        {
            contactUs = new DataBase.Entity.ContactUs();

            _context.ContactUs.Add(contactUs);

            await _context.SaveChangesAsync();
        }

        card.ContactUsId = contactUs.Id;

        _context.ContactUsCards.Add(card);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<ContactUsCardEditDto?>
        GetEditDataAsync(int id)
    {
        return await _context.ContactUsCards
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new ContactUsCardEditDto
            {
                Id = x.Id,
                Icon = x.Icon,
                Title = x.Title,
                Content = x.Content,
                DisplayOrder = x.DisplayOrder,
                IsActive = x.IsActive
            })
            .FirstOrDefaultAsync();
    }

    public async Task<bool> UpdateAsync(
        ContactUsCardEditDto dto)
    {
        var card = await _context.ContactUsCards
            .FirstOrDefaultAsync(x => x.Id == dto.Id);

        if (card == null)
        {
            return false;
        }

        card.Icon = dto.Icon.Trim();
        card.Title = dto.Title.Trim();
        card.Content = dto.Content.Trim();
        card.DisplayOrder = dto.DisplayOrder;
        card.IsActive = dto.IsActive;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<ContactUsCardDetailsDto?>
        GetDetailsAsync(int id)
    {
        return await _context.ContactUsCards
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new ContactUsCardDetailsDto
            {
                Id = x.Id,
                Icon = x.Icon,
                Title = x.Title,
                Content = x.Content,
                DisplayOrder = x.DisplayOrder,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<bool> ChangeStatusAsync(
        int id)
    {
        var card = await _context.ContactUsCards
            .FirstOrDefaultAsync(x => x.Id == id);

        if (card == null)
        {
            return false;
        }

        card.IsActive = !card.IsActive;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var card = await _context.ContactUsCards
            .FirstOrDefaultAsync(x => x.Id == id);

        if (card == null)
        {
            return false;
        }

        _context.ContactUsCards.Remove(card);

        await _context.SaveChangesAsync();

        return true;
    }
}