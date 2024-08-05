using CasinoDealer2.Models.QuestionModels;
using CasinoDealer2.Models.RouletteModels;

namespace CasinoDealer2.RepositoryFolder.RouletteRepository
{
    public interface IRouletteService
    {
        QuestionAR GenerateRouletteQuestion(RouletteSettings settings);
        Task<bool> SaveRouletteQuestionAsync(QuestionAR question, string userId);

        Task<int> GetPersonalRecord(string userId);

        Task<RouletteTournamentRecord> GetRouletteTournamentRecordByUserId(string userId);
        Task<int> UpdateRouletteTournamentRecord(string userId, bool isCorrect, int currentStreak);
        Task CreateRouletteTournamentRecordAsync(string userId);
        QuestionAR GenerateRouletteTournamentQuestion();

        //Get list of Top RouletteTournament Players
        Task<List<RouletteTournamentRecord>> GetTopRouletteTournamentRecordsAsync(int topN);

    }
}
