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

        public BlackJackController(UserManager<IdentityUser> userManager, IBlackJackService blackJackService)
        {
            _userManager = userManager;
            _blackJackService = blackJackService;
        }

        public IActionResult BlackJackQuestion()
        {
            var question = _blackJackService.GenerateBlackJackQuestion();
            return View(question);
        }

        [HttpPost]
        public async Task<IActionResult> BlackJackQuestion(Question request)
        {
            var userId = _userManager.GetUserId(User)!;

            bool isCorrect =  await _blackJackService.SaveBlackJackQuestionAsync(request, userId);

            // if the answer is correct
            if (isCorrect)
            {
                return RedirectToAction("BlackJackQuestion", "BlackJack");
            }
            else
            {
                // if the answer is wrong
                request.IncorrectStreak++;
                return View(request);
            }

        }

    }
}
