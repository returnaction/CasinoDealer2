using CasinoDealer2.Models.BlackJackModels;
using CasinoDealer2.Models.BlackJackSettings;
using CasinoDealer2.Models.QuestionModels;
using CasinoDealer2.RepositoryFolder.BaseRepository;

namespace CasinoDealer2.RepositoryFolder.BalckJackRepository
{
    public interface IBlackJackService : IRepository<BlackJackTournamentRecord>
    {
        Question GenerateBlackJackQuestion(BlackJackSettings settings);
        Task<bool> SaveBlackJackQuestionAsync(Question question, string userId);

        Question GenerateBlackJackTournamentQuestion();

        // find BlackJackTournamentQuestion by userId
        Task<BlackJackTournamentRecord> GetBlackJackTournamentRecordByUserId(string userId);
        Task<int> UpdateBlackJackTournamentRecord(string userId, bool isCorrect, int currentStreak);
        Task CreateBlackJackTournamentRecordAsync(string userId);


        // Get list of Top users
        Task<List<BlackJackTournamentRecord>> GetTopBlackJackTournamentRecordsAsync(int topN);

    }
}
