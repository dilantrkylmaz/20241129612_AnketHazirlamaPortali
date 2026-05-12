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

        // QuestionRepository.cs
        public override async Task DeleteAsync(int id)
        {
            var entity = await _context.Questions.FindAsync(id);
            if (entity != null)
            {
                entity.IsDeleted = true;
                _context.Questions.Update(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
    }
