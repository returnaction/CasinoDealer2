using Azure.Core;
using CasinoDealer2.Models.Enums;
using CasinoDealer2.Models.QuestionModels;
using CasinoDealer2.UnitOfWork;

namespace CasinoDealer2.RepositoryFolder.CrapsRepository
{
    public class CrapsService : ICrapsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly Random _random = new Random();

        public CrapsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> SaveCrapsQuestionAsync(Question question, string userId)
        {
            question.IsCorrect = question.Answer == question.CorrectAnswer;

            question.Id = Guid.NewGuid();
            question.UserId = userId!;

            await _unitOfWork.Questions.AddAsync(question);
            await _unitOfWork.SaveChangesAsync();

            return question.IsCorrect;
        }

        public Question GenerateCrapsQuestion()
        {
            int diceRolled = RollDice();
            int bet = GenerateRandomHornBet();
            string questionText = $"Horn. Rolls: {diceRolled} | bet: {bet}";
            double correctAnswer = CalculateCorrectHornBetAnswer(bet, diceRolled);

            var question = new Question
            {
                QuestionText = questionText,
                CorrectAnswer = correctAnswer,
                GameType = GameType.Craps,
                DiceRolled = diceRolled
            };

            return question;
        }

        private int RollDice()
        {
            int[] outcome = { 2, 3, 11, 12 };
            return outcome[_random.Next(0, outcome.Length)];
        }

        private int GenerateRandomHornBet()
        {
            return _random.Next(1, 26) * 4;
        }

        private double CalculateCorrectHornBetAnswer(int bet, int rolledNumber)
        {
            if (rolledNumber == 3 || rolledNumber == 11)
                return bet * 3;

            return (bet * 7) - (bet / 4);
        }
    }
}
