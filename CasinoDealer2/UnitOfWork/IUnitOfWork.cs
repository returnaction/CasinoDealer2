
using CasinoDealer2.RepositoryFolder.BalckJackRepository;
using CasinoDealer2.RepositoryFolder.QuestionRepository;

namespace CasinoDealer2.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IQuestionService Questions { get; }
        IBlackJackService BlackJack { get; }
        Task<int> SaveChangesAsync();
    }
}
