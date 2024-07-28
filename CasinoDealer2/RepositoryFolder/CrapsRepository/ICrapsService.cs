using CasinoDealer2.Models.QuestionModels;

namespace CasinoDealer2.RepositoryFolder.CrapsRepository
{
    public interface ICrapsService
    {
        Question GenerateCrapsQuestion();
        Task<bool> SaveCrapsQuestionAsync(Question question, string userId);
    }
}
