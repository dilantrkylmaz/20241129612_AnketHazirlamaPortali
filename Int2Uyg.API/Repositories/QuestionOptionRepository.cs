using Int2Uyg.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Int2Uyg.API.Repositories
{
    public class QuestionOptionRepository : GenericRepository<QuestionOption>
    {
        public QuestionOptionRepository(AppDbContext context) : base(context) { }

        public override async Task AddAsync(QuestionOption entity)
        {
            await _context.QuestionOptions.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public override async Task DeleteAsync(int id)
        {
            var entity = await _context.QuestionOptions.FindAsync(id);
            if (entity != null)
            {
                _context.QuestionOptions.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}