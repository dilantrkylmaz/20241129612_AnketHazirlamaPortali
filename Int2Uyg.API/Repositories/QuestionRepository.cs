using Int2Uyg.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Int2Uyg.API.Repositories
{
    public class QuestionRepository : GenericRepository<Question>
    {
        public QuestionRepository(AppDbContext context) : base(context) { }

        public override async Task<List<Question>> GetAllAsync()
        {
            return await _context.Questions.Include(q => q.QuestionOptions).ToListAsync();
        }

        public override async Task AddAsync(Question entity)
        {
            await _context.Questions.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public override async Task DeleteAsync(int id)
        {
            var entity = await _context.Questions.FindAsync(id);
            if (entity != null)
            {
                var options = await _context.QuestionOptions.Where(o => o.QuestionId == id).ToListAsync();
                if (options.Any())
                {
                    _context.QuestionOptions.RemoveRange(options);
                }

                _context.Questions.Remove(entity);
                await _context.SaveChangesAsync(); 
            }
        }
    }
}