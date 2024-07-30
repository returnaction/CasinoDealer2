using CasinoDealer2.Models.QuestionModels;
using CasinoDealer2.Models.RouletteModels;

namespace CasinoDealer2.RepositoryFolder.RouletteRepository
{
    public interface IRouletteService
    {
        QuestionAR GenerateRouletteQuestion(RouletteSettings settings);
        Task<bool> SaveRouletteQuestionAsync(QuestionAR question, string userId);
    }
}
