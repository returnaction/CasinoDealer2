using CasinoDealer2.Models.BlackJackSettings;
using CasinoDealer2.Models.Enums;
using CasinoDealer2.Models.QuestionModels;
using CasinoDealer2.Models.RouletteModels;
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
            RouletteSettings rouletteSettings = GetSettingsFromTempData();
            var question = _rouletteService.GenerateRouletteQuestion(rouletteSettings);

            RouletteVM rouletteVM = new()
            {
                QuestionAR = question,
                SettingsAR = rouletteSettings
            };

            SaveSettingsToTempData(rouletteSettings);

            return View(rouletteVM);
        }





        [HttpPost]
        public async Task<IActionResult> RouletteQuestion(RouletteVM request)
        {
            var userId = _userManager.GetUserId(User)!;

            bool isCorrect =  await _rouletteService.SaveRouletteQuestionAsync(request.QuestionAR, userId);

            // if the answer is correct
            if (isCorrect)
            {
                return RedirectToAction("RouletteQuestion", "Roulette");
            }
            else
            {
                // if the answer is wrong
                request.QuestionAR.IncorrectStreak++;
                return View(request);
            }

        }

        [HttpPost]
        public IActionResult UpdateSettings(RouletteVM model)
        {
            SaveSettingsToTempData(model.SettingsAR);
            return RedirectToAction("RouletteQuestion");
        }

        private RouletteSettings GetSettingsFromTempData()
        {
            return new RouletteSettings
            {
                MinBet = TempData.ContainsKey("MinBet") ? (int)TempData["MinBet"]! : 1,
                MaxBet = TempData.ContainsKey("MaxBet") ? (int)TempData["MaxBet"]! : 10,
                Increment = TempData.ContainsKey("Increment") ? (int)TempData["Increment"]! : 1,
                IsStraightUp = TempData.ContainsKey("IsStraightUp") ? (bool)TempData["IsStraightUp"]! : true,
                IsSplit = TempData.ContainsKey("IsSplit") ? (bool)TempData["IsSplit"]! : false,
                IsCorner = TempData.ContainsKey("IsCorner") ? (bool)TempData["IsCorner"]! : false,
                IsStreet = TempData.ContainsKey("IsStreet") ? (bool)TempData["IsStreet"]! : false,
                IsSixline = TempData.ContainsKey("IsSixline") ? (bool)TempData["IsSixline"]! : false,
            };
        }

        private void SaveSettingsToTempData(RouletteSettings settings)
        {
            TempData["MinBet"] = settings.MinBet;
            TempData["MaxBet"] = settings.MaxBet;
            TempData["Increment"] = settings.Increment;
            TempData["IsStraightUp"] = settings.IsStraightUp;
            TempData["IsSplit"] = settings.IsSplit;
            TempData["IsCorner"] = settings.IsCorner;
            TempData["IsStreet"] = settings.IsStreet;
            TempData["IsSixline"] = settings.IsSixline;
            TempData.Keep("MinBet");
            TempData.Keep("MaxBet");
            TempData.Keep("Increment");
            TempData.Keep("IsStraightUp");
            TempData.Keep("IsSplit");
            TempData.Keep("IsCorner");
            TempData.Keep("IsStreet");
            TempData.Keep("IsSixline");
        }

    }
}
