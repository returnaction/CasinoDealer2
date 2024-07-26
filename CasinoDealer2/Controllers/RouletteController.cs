using CasinoDealer2.Data;
using CasinoDealer2.Models.Enums;
using CasinoDealer2.Models.QuestionModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CasinoDealer2.Controllers
{
    [Authorize]
    public class RouletteController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly Random _random = new Random();

        public RouletteController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult RouletteQuestion()
        {
            var question = GenerateRQuestion();
            return View(question);
        }

        [HttpPost]
        public async Task<IActionResult> RouletteQuestion(Question request)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
                return Unauthorized();

            var userId = user.Id;

            bool isCorrect;
            if (double.TryParse(request.Answer.ToString(), out double userAnswer))
            {
                isCorrect = Math.Abs(userAnswer - request.CorrectAnswer) < 0.01;
            }
            else
            {
                isCorrect = false;
            }

            // create a new Id. The second time if my answer is wrong it will not going to save with the same id into db;
            // TODO: maybe I should delete in model Guid.NewGuid(); there is no point of that.
            request.Id = Guid.NewGuid();
            request.IsCorrect = isCorrect;
            request.UserId = userId;

            _context.Questions.Add(request);
            await _context.SaveChangesAsync();

            // if the answer is correct
            if (isCorrect)
            {
                return RedirectToAction("RouletteQuestion", "Roulette");
            }
            else
            {
                // if the answer is wrong
                request.IncorrectStreak++;
                return View(request);
            }

        }


        private Question GenerateRQuestion()
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

    }
}
