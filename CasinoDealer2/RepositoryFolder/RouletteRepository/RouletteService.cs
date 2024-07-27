using CasinoDealer2.Models.Enums;
using CasinoDealer2.Models.QuestionModels;
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

        public Question GenerateRouletteQuestion()
        {
            int multiplier = _random.Next(0, 2) == 0 ? 17 : 35;
            int number = _random.Next(1, 21);

            double correctAnswer = multiplier * number;

            string questionText = $"What is the result of {multiplier} x {number}";

            var question = new Question
            {
                QuestionText = questionText,
                CorrectAnswer = correctAnswer,
                IsCorrect = false,
                GameType = GameType.Roulette
            };

            return question;
        }

        public async Task<bool> SaveRouletteQuestionAsync(Question question, string userId)
        {
            question.IsCorrect = question.Answer == question.CorrectAnswer;

            question.Id = Guid.NewGuid();
            question.UserId = userId;

            await _unitOfWork.Questions.AddAsync(question);
            await _unitOfWork.SaveChangesAsync();

            return question.IsCorrect;
        }
    }
}
