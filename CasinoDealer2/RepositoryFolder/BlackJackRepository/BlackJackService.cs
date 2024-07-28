using CasinoDealer2.Models.BlackJackSettings;
using CasinoDealer2.Models.Enums;
using CasinoDealer2.Models.QuestionModels;
using CasinoDealer2.UnitOfWork;

namespace CasinoDealer2.RepositoryFolder.BalckJackRepository
{
    public class BlackJackService : IBlackJackService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly Random _random = new Random();
        public BlackJackService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

            await _unitOfWork.Questions.AddAsync(question);
            await _unitOfWork.SaveChangesAsync();

            return question.IsCorrect;
        }
    }
}
