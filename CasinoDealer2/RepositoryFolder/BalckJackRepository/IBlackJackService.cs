using CasinoDealer2.Models.QuestionModels;

namespace CasinoDealer2.RepositoryFolder.BalckJackRepository
{
    public interface IBlackJackService
    {
        Question GenerateBlackJackQuestion();
        Task<bool> SaveBlackJackQuestionAsync(Question question, string userId);
    }
}
