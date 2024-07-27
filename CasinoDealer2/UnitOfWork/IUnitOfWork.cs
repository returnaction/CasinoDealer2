
using CasinoDealer2.RepositoryFolder.QuestionRepository;

namespace CasinoDealer2.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IQuestionService Questions { get; }
        Task<int> SaveChangesAsync();
    }
}
