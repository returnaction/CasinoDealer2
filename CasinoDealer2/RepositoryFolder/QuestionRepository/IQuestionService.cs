using CasinoDealer2.Models.QuestionModels;
using CasinoDealer2.RepositoryFolder.BaseRepository;

namespace CasinoDealer2.RepositoryFolder.QuestionRepository
{
    public interface IQuestionService: IRepository<Question>
    {
        Task<IEnumerable<Question>> GetQuestionsByUserIdAsync(string userId);
    }
}
