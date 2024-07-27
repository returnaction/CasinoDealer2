using CasinoDealer2.Data;
using CasinoDealer2.Models.QuestionModels;
using CasinoDealer2.RepositoryFolder.BaseRepository;
using Microsoft.EntityFrameworkCore;

namespace CasinoDealer2.RepositoryFolder.QuestionRepository
{
    public class QuestionService : Repository<Question>, IQuestionService
    {
        public QuestionService(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Question>> GetQuestionsByUserIdAsync(string userId)
        {
            return await _context.Questions.Where(q => q.UserId == userId).ToListAsync();
        }
    }
}
