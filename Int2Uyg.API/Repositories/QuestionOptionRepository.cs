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

        // ✅ BUG #6 FIX: Changed from hard delete (Remove) to soft delete (IsDeleted = true)
        // to be consistent with GenericRepository and all other repositories
        public override async Task DeleteAsync(int id)
        {
            var entity = await _context.QuestionOptions.FindAsync(id);
            if (entity != null)
            {
                entity.IsDeleted = true;
                _context.QuestionOptions.Update(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}