using CasinoDealer2.Data;
using CasinoDealer2.Models.QuestionModels;
using CasinoDealer2.RepositoryFolder.QuestionRepository;

namespace CasinoDealer2.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private  IQuestionService _questions;


        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }
        public IQuestionService Questions => _questions ??= new QuestionService(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
