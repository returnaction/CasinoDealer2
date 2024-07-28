using CasinoDealer2.Models.QuestionModels;
using CasinoDealer2.RepositoryFolder.RouletteRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CasinoDealer2.Controllers
{
    [Authorize]
    public class RouletteController : Controller
    {
        private readonly IRouletteService _rouletteService;
        private readonly UserManager<IdentityUser> _userManager;

        public RouletteController(UserManager<IdentityUser> userManager, IRouletteService rouletteService)
        {
            _userManager = userManager;
            _rouletteService = rouletteService;
        }

        public IActionResult RouletteQuestion()
        {
            var question = _rouletteService.GenerateRouletteQuestion();
            return View(question);
        }

        [HttpPost]
        public async Task<IActionResult> RouletteQuestion(Question request)
        {
            var userId = _userManager.GetUserId(User)!;

            bool isCorrect = await _rouletteService.SaveRouletteQuestionAsync(request, userId);

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

    }
}
