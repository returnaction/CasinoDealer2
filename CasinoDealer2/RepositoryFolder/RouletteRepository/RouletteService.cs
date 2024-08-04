using CasinoDealer2.Data;
using CasinoDealer2.Models.Enums;
using CasinoDealer2.Models.QuestionModels;
using CasinoDealer2.Models.RouletteModels;
using CasinoDealer2.UnitOfWork;
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
                straightUp = _random.Next(minBet, maxBet + 1);
            }
            if (isSplit)
            {
                split = _random.Next(minBet, maxBet + 1);
            }
            if (isCorner)
            {
                corner = _random.Next(minBet, maxBet + 1);
            }
            if (isStreet)
            {
                street = _random.Next(minBet, maxBet + 1);
            }
            if (isSixline)
            {
                sixline = _random.Next(minBet, maxBet + 1);
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

        public async Task<bool> SaveRouletteQuestionAsync(QuestionAR question, string userId)
        {
            question.IsCorrect = question.Answer == question.CorrectAnswer;

            question.Id = Guid.NewGuid();
            question.UserId = userId;

            await _context.QuestionsAR.AddAsync(question);
            await _unitOfWork.SaveChangesAsync();

            return question.IsCorrect;
        }
    }
}
