using Azure.Core;
using CasinoDealer2.Models.Enums;
using CasinoDealer2.Models.QuestionModels;
using CasinoDealer2.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

        public Question GenerateBlackJackQuestion()
        {
            int number = _random.Next(1, 21) * 5;
            string questionText = $"What is the blackjack payout of {number}";
            double correctAnswer = number * 1.5;

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
