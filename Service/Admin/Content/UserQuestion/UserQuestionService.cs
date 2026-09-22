using DataBase.Context;
using Microsoft.EntityFrameworkCore;
namespace Service.Admin.Content.UserQuestion;

public interface IUserQuestionService
{
    Task<List<UserQuestionListDto>> GetAllAsync();
    Task<UserQuestionDetailsDto?> GetDetailsAsync(int id);
    Task<bool> DeleteAsync(int id);
}

public class UserQuestionService : IUserQuestionService
{
    private readonly MyContext _context;

    public UserQuestionService(MyContext context)
    {
        _context = context;
    }

    public async Task<List<UserQuestionListDto>> GetAllAsync()
    {
        return await _context.UserQuestions
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new UserQuestionListDto
            {
                Id = x.Id,
                FullName = x.FullName,
                Email = x.Email,
                Message = x.Message,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<UserQuestionDetailsDto?> GetDetailsAsync(int id)
    {
        return await _context.UserQuestions
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new UserQuestionDetailsDto
            {
                Id = x.Id,
                FullName = x.FullName,
                Email = x.Email,
                Message = x.Message,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var question = await _context.UserQuestions
            .FirstOrDefaultAsync(x => x.Id == id);

        if (question == null)
            return false;

        _context.UserQuestions.Remove(question);

        await _context.SaveChangesAsync();

        return true;
    }
}