using CasinoDealer2.Data;
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

        public IActionResult Index(Question? oldRrequest)
        {
            // if the old request is null it means previous question was answered properly
            if (oldRrequest is null)
            {
                var question = GenerateQuestion();
                return View(question);
            }

            return View(oldRrequest);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitAnswer(Question request)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
                return Unauthorized();

            var userId = user.Id;

            bool isCorrect = request.CorrectAnswer.Equals(request.Answer, StringComparison.OrdinalIgnoreCase);

            request.IsCorrect = isCorrect;

            _context.Questions.Add(request);
            await _context.SaveChangesAsync();

            // if the answer is correct
            if (isCorrect)
            {
                return RedirectToAction("Index", "Roulette");
            }
            else
            {
                // if the answer is wrong
                request.IncorrectStreak++;
                return RedirectToAction(nameof(Index), new { oldRequest = request });
            }


        }

        // Generation question
        private Question GenerateQuestion()
        {
            int number = _random.Next(1, 21) * 5;
            string questionText = $"What is the blackjack payout of {number}";
            string correctAnswer = (number * 1.5).ToString();

            var question = new Question
            {
                QuestionText = questionText,
                CorrectAnswer = correctAnswer,
                Answer = string.Empty,
                IsCorrect = false
            };

            return question;
        }
    }
}
