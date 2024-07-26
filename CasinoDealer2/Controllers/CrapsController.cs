using CasinoDealer2.Data;
using CasinoDealer2.Models.Enums;
using CasinoDealer2.Models.QuestionModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System.Runtime.InteropServices;

namespace CasinoDealer2.Controllers
{
    [Authorize]
    public class CrapsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly Random _random = new Random();

        public CrapsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult CrapsQuestion()
        {
            var question = GenerateCrQuestion();
            return View(question);
        }

        [HttpPost]
        public async Task<IActionResult> CrapsQuestion(Question request)
        {
            var userId =  _userManager.GetUserId(User);

            bool isCorrect = false;

            if (request.Answer == request.CorrectAnswer)
                isCorrect = true;
           

            request.Id = Guid.NewGuid();
            request.IsCorrect = isCorrect;
            request.UserId = userId!;

            _context.Questions.Add(request);
            await _context.SaveChangesAsync();

            if (isCorrect)
            {
                return RedirectToAction("CrapsQuestion", "Craps");
            }
            else
            {
                // if the answer is wrong
                request.IncorrectStreak++;
                return View(request);
            }
        }

        private Question GenerateCrQuestion()
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
            else
                return (bet * 7) - (bet / 4);
        }
    }
}
