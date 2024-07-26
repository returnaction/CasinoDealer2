using CasinoDealer2.Models.QuestionModels;
using CasinoDealer2.RepositoryFolder.BaseRepository;

namespace CasinoDealer2.RepositoryFolder.QuestionRepository
{
    public interface IQuestionRepsitory : IRepository<Question>
    {
        Task<IEnumerable<Question>> GetQuestionsByUserIdAsync(string userId);
    }
}
