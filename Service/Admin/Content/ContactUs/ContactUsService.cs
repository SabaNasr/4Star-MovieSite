using DataBase.Context;
using Microsoft.EntityFrameworkCore;
namespace Service.Admin.Content.ContactUs;

public interface IContactUsService
{
    Task<List<ContactUsListDto>> GetAllAsync();

    Task<ContactUsDetailsDto?> GetDetailsAsync(
        int id);

    Task<bool> DeleteAsync(
        int id);
}

public class ContactUsService
    : IContactUsService
{
    private readonly MyContext _context;

    public ContactUsService(MyContext context)
    {
        _context = context;
    }

    public async Task<List<ContactUsListDto>> GetAllAsync()
    {
        return await _context.ContactUs
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new ContactUsListDto
            {
                Id = x.Id,
                FullName = x.FullName,
                Email = x.Email,
                Phone = x.Phone,
                Message = x.Message,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<ContactUsDetailsDto?>
        GetDetailsAsync(int id)
    {
        return await _context.ContactUs
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new ContactUsDetailsDto
            {
                Id = x.Id,
                FullName = x.FullName,
                Email = x.Email,
                Phone = x.Phone,
                Message = x.Message,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var contact = await _context.ContactUs
            .FirstOrDefaultAsync(x => x.Id == id);

        if (contact == null)
        {
            return false;
        }

        _context.ContactUs.Remove(contact);

        await _context.SaveChangesAsync();

        return true;
    }
}