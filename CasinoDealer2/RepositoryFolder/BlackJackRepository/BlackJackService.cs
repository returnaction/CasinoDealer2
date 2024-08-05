using CasinoDealer2.Data;
using CasinoDealer2.Models.BlackJackModels;
using CasinoDealer2.Models.BlackJackSettings;
using CasinoDealer2.Models.Enums;
using CasinoDealer2.Models.QuestionModels;
using CasinoDealer2.RepositoryFolder.BaseRepository;
using CasinoDealer2.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace CasinoDealer2.RepositoryFolder.BalckJackRepository
{
    public class BlackJackService : Repository<BlackJackTournamentRecord>, IBlackJackService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly Random _random = new Random();


        public BlackJackService( ApplicationDbContext context) : base(context)
        {
        }

        public Question GenerateBlackJackQuestion(BlackJackSettings settings)
        {
            int minBet = settings.MinBet;
            int maxBet = settings.MaxBet;
            int increment = settings.Increment;

            int number;
            if (minBet == 0 || maxBet == 0 || increment == 0)
            {
                number = _random.Next(1, 21) * 5;
            }
            else
            {
                number = _random.Next(minBet / increment, maxBet / increment + 1) * increment;
            }

            string questionText = $"BlackJack of {number}";
            double correctAnswer;

            if (settings.PayoutType == BlackJackPayOutType.ThreeToTwo)
                correctAnswer = number * 1.5;
            else
            {
                correctAnswer = number * 1.2;
            }

            var question = new Question
            {
                QuestionText = questionText,
                CorrectAnswer = correctAnswer,
                IsCorrect = false,
                GameType = GameType.BalckJack
            };

            return question;
        }

        public async Task<bool> SaveBlackJackQuestionAsync(Question question, string userId)
        {
            question.IsCorrect = question.Answer == question.CorrectAnswer;

            question.Id = Guid.NewGuid();
            question.UserId = userId!;

            await _context.Questions.AddAsync(question);
            await _context.SaveChangesAsync();

            return question.IsCorrect;
        }

        // BlackJack Tournament Question
        public Question GenerateBlackJackTournamentQuestion()
        {
            return GenerateBlackJackQuestion(new BlackJackSettings { Increment = 5, MinBet = 5, MaxBet = 10000, PayoutType = BlackJackPayOutType.ThreeToTwo });
        }

        public async Task<int> UpdateBlackJackTournamentRecord(string userId, bool isCorrect, int currentStreak)
        {
            if (isCorrect)
            {
                currentStreak++;
                var bjTournamentRecord = await GetBlackJackTournamentRecordByUserId(userId);

                if (currentStreak > bjTournamentRecord.LongestStreak)
                {
                    bjTournamentRecord.LongestStreak = currentStreak;
                    bjTournamentRecord.Date = DateTime.Now;

                    _context.BlackJackTournamentRecords.Update(bjTournamentRecord);
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                currentStreak = 0;
            }

            return currentStreak;
        }

        public async Task<BlackJackTournamentRecord> GetBlackJackTournamentRecordByUserId(string userId)
        {
           return await _context.BlackJackTournamentRecords.SingleOrDefaultAsync(record => record.UserId == userId);
        }

        public async Task CreateBlackJackTournamentRecordAsync(string userId)
        {
            var blackJackTournamentRecord = new BlackJackTournamentRecord()
            {
                LongestStreak = 0,
                UserId = userId
            };

            await _context.BlackJackTournamentRecords.AddAsync(blackJackTournamentRecord);
            await _context.SaveChangesAsync();
        }

        public async Task<List<BlackJackTournamentRecord>> GetTopBlackJackTournamentRecordsAsync(int topN)
        {
            return await _context.BlackJackTournamentRecords
                 .Include(record => record.User)
                 .OrderByDescending(record => record.LongestStreak)
                 .Take(topN)
                 .ToListAsync();
        }

        public async Task<int> GetPersonalRecord(string userId)
        {
            int userRecord = await _context.BlackJackTournamentRecords
                .Where(r => r.UserId == userId)
                .Select(r => r.LongestStreak)
                .FirstOrDefaultAsync();

            return userRecord;
        }
    }

}



