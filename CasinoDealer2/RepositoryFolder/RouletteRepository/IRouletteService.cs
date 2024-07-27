using CasinoDealer2.Models.QuestionModels;

namespace CasinoDealer2.RepositoryFolder.RouletteRepository
{
    public interface IRouletteService
    {
        Question GenerateRouletteQuestion();
        Task<bool> SaveRouletteQuestionAsync(Question question, string userId);
    }
}
