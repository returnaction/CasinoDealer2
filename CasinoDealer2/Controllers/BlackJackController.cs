using CasinoDealer2.Models.BlackJackSettings;
using CasinoDealer2.Models.QuestionModels;
using CasinoDealer2.RepositoryFolder.BalckJackRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CasinoDealer2.Controllers
{
    [Authorize]
    public class BlackJackController : Controller
    {
        private readonly IBlackJackService _blackJackService;
        private readonly UserManager<IdentityUser> _userManager;
        private static BlackJackSettings _settings = new BlackJackSettings { MinBet = 5, MaxBet = 100, Increment = 5 };

        public BlackJackController(UserManager<IdentityUser> userManager, IBlackJackService blackJackService)
        {
            _userManager = userManager;
            _blackJackService = blackJackService;
        }

        public IActionResult BlackJackQuestion()
        {
            var question = _blackJackService.GenerateBlackJackQuestion(_settings);
            var model = new BlackJackVM
            {
                Question = question,
                Settings = _settings
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult UpdateSettings(BlackJackVM model)
        {
            _settings = model.Settings;
            return RedirectToAction("BlackJackQuestion");
        }

        [HttpPost]
        public async Task<IActionResult> BlackJackQuestion(BlackJackVM model)
        {
            var userId = _userManager.GetUserId(User)!;

            bool isCorrect =  await _blackJackService.SaveBlackJackQuestionAsync(model.Question, userId);

            // if the answer is correct
            if (isCorrect)
            {
                return RedirectToAction("BlackJackQuestion", "BlackJack");
            }
            else
            {
                // if the answer is wrong
                model.Question.IncorrectStreak++;
                return View(model);
            }

        }

    }
}
