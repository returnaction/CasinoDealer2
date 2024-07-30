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
        public RouletteService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

            }
           

            int multiplier = _random.Next(0, 2) == 0 ? 17 : 35;
            int number = _random.Next(1, 21);

            double correctAnswer = multiplier * number;

            string questionText = $"What is the result of {multiplier} x {number}";

            var question = new QuestionAR
            {
                QuestionText = questionText,
                CorrectAnswer = correctAnswer,
                IsCorrect = false,
                GameType = GameType.Roulette
            };

            return question;
        }

        public async Task<bool> SaveRouletteQuestionAsync(QuestionAR question, string userId)
        {
            //question.IsCorrect = question.Answer == question.CorrectAnswer;

            //question.Id = Guid.NewGuid();
            //question.UserId = userId;

            //await _unitOfWork.Questions.AddAsync(question);
            //await _unitOfWork.SaveChangesAsync();

            //return question.IsCorrect;

            return true;
        }
    }
}
