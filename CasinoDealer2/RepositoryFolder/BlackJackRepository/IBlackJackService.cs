using CasinoDealer2.Models.BlackJackModels;
using CasinoDealer2.Models.BlackJackSettings;
using CasinoDealer2.Models.QuestionModels;

namespace CasinoDealer2.RepositoryFolder.BalckJackRepository
{
    public interface IBlackJackService
    {
        Question GenerateBlackJackQuestion(BlackJackSettings settings);
        Task<bool> SaveBlackJackQuestionAsync(Question question, string userId);

        Task<Question> GenerateBlackJackTournamentQuestion();
        Task<int> UpdateBlackJackTournamentRecord(string userId, bool isCorrect, int currentStreak);

    }
}
