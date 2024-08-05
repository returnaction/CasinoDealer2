using CasinoDealer2.Data;
using CasinoDealer2.Models.Enums;
using CasinoDealer2.Models.QuestionModels;
using CasinoDealer2.Models.RouletteModels;
using CasinoDealer2.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace CasinoDealer2.RepositoryFolder.RouletteRepository
{
    public class RouletteService : IRouletteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly Random _random = new Random();
        private readonly ApplicationDbContext _context;
        public RouletteService(IUnitOfWork unitOfWork, ApplicationDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public QuestionAR GenerateRouletteQuestion(RouletteSettings settings)
        {
            int minBet = settings.MinBet;
            int maxBet = settings.MaxBet;
            int increment = settings.Increment;

            bool isStraightUp = settings.IsStraightUp;
            bool isSplit = settings.IsSplit;
            bool isCorner = settings.IsCorner;
            bool isSixline = settings.IsSixline;
            bool isStreet = settings.IsStreet;

            int straightUp = 0;
            int split = 0;
            int corner = 0;
            int street = 0;
            int sixline = 0;


            if (isStraightUp)
            {
                straightUp = GenerateRandomBet(minBet, maxBet, increment);
            }
            if (isSplit)
            {
                split = GenerateRandomBet(minBet, maxBet, increment);
            }
            if (isCorner)
            {
                corner = GenerateRandomBet(minBet, maxBet, increment);
            }
            if (isStreet)
            {
                street = GenerateRandomBet(minBet, maxBet, increment);
            }
            if (isSixline)
            {
                sixline = GenerateRandomBet(minBet, maxBet, increment);
            }



            double correctAnswer = 0;

            if (straightUp > 0)
                correctAnswer += straightUp * 35;
            if (split > 0)
                correctAnswer += split * 17;
            if (corner > 0)
                correctAnswer += corner * 8;
            if (street > 0)
                correctAnswer += street * 11;
            if (sixline > 0)
                correctAnswer += sixline * 5;

            string questionText = $"What is the result of Straight UP - {straightUp}, Split - {split}, Corner - {corner}, Street - {street}, SixLine - {sixline}";

            var question = new QuestionAR
            {
                QuestionText = questionText,
                CorrectAnswer = correctAnswer,
                IsCorrect = false,
                GameType = GameType.Roulette,
                StraitUp = straightUp,
                Split = split,
                Corner = corner,
                Street = street,
                SixLine = sixline
            };

            return question;
        }

        private int GenerateRandomBet(int minBet, int maxBet, int increment)
        {
            int min = (int)Math.Ceiling((double)minBet / increment);
            int max = (int)Math.Floor((double)maxBet / increment);

            int randomIncrement = _random.Next(min, max + 1);
            return randomIncrement * increment;
        }

        public async Task<bool> SaveRouletteQuestionAsync(QuestionAR question, string userId)
        {
            question.IsCorrect = question.Answer == question.CorrectAnswer;

            question.Id = Guid.NewGuid();
            question.UserId = userId;

            await _context.QuestionsAR.AddAsync(question);
            await _unitOfWork.SaveChangesAsync();

            return question.IsCorrect;
        }

        public async Task<int> GetPersonalRecord(string userId)
        {
            int userRecord = await _context.RouletteTournamentRecords
                .Where(r => r.UserId == userId)
                .Select(r => r.LongestStreak)
                .FirstOrDefaultAsync();

            return userRecord;
        }

        public async Task<RouletteTournamentRecord> GetRouletteTournamentRecordByUserId(string userId)
        {
            return await _context.RouletteTournamentRecords.SingleOrDefaultAsync(record => record.UserId == userId);
        }

        public async Task CreateRouletteTournamentRecordAsync(string userId)
        {
            var rouletteTournamentRecord = new RouletteTournamentRecord()
            {
                LongestStreak = 0,
                UserId = userId
            };

            await _context.RouletteTournamentRecords.AddAsync(rouletteTournamentRecord);
            await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateRouletteTournamentRecord(string userId, bool isCorrect, int currentStreak)
        {
            if (isCorrect)
            {
                currentStreak++;
                var rouletteTournamentRecord = await GetRouletteTournamentRecordByUserId(userId);

                if(currentStreak > rouletteTournamentRecord.LongestStreak)
                {
                    rouletteTournamentRecord.LongestStreak = currentStreak;
                    rouletteTournamentRecord.Date = DateTime.Now;

                    _context.RouletteTournamentRecords.Update(rouletteTournamentRecord);
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                currentStreak = 0;
            }

            return currentStreak;
        }

        public QuestionAR GenerateRouletteTournamentQuestion()
        {
            return GenerateRouletteQuestion(new RouletteSettings { Increment = 5, MinBet = 5, MaxBet = 100, IsStraightUp = true, IsSplit = true, IsCorner = true, IsStreet = true, IsSixline = true });

        }

        public async Task<List<RouletteTournamentRecord>> GetTopRouletteTournamentRecordsAsync(int topN)
        {
            return await _context.RouletteTournamentRecords
                .Include(record => record.User)
                .OrderByDescending(record => record.LongestStreak)
                .Take(topN)
                .ToListAsync();
        }
    }
}
