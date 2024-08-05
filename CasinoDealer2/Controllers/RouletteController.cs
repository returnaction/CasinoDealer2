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


        //_________________Roulette Tournament__________________

        public async Task<IActionResult> RouletteTournamentQuestion()
        {
            var userId = _userManager.GetUserId(User);
            TempData["CurrentStreak"] = 0;
            TempData["PersonalRecord"] = await _rouletteService.GetPersonalRecord(userId!);

            QuestionAR questionForTournament = _rouletteService.GenerateRouletteQuestion(new RouletteSettings { Increment = 5, MinBet = 5, MaxBet = 500, IsStraightUp = true });

            ViewBag.CurrentStreak = TempData["CurrentStreak"];
            ViewBag.PersonalRecord = TempData["PersonalRecord"];

            TempData.Keep("PersonalRecord");

            return View(questionForTournament);
        }

        [HttpPost]
        public async Task<IActionResult> RouletteTournamentQuestion(QuestionAR question)
        {
            var user = await _userManager.GetUserAsync(User);

            int currentStreak = (int)(TempData["CurrentStreak"] ?? 0);
            int personalRecordTemp = (int)(TempData["PersonalRecord"] ?? 0);

            bool isCorrect = question.Answer == question.CorrectAnswer;
            currentStreak = await _rouletteService.UpdateRouletteTournamentRecord(user!.Id, isCorrect, currentStreak);

            TempData["CurrentStreak"] = currentStreak;
            if (personalRecordTemp < currentStreak)
            {
                TempData["PersonalRecord"] = currentStreak;
            }

            QuestionAR newQuestionForTournament = _rouletteService.GenerateRouletteTournamentQuestion();

            ViewBag.CurrentStreak = currentStreak;
            ViewBag.PersonalRecord = TempData["PersonalRecord"];

            TempData.Keep("PersonalRecord");
            return View(newQuestionForTournament);
        }

        public async Task<IActionResult> CreateBlackJackTournamentRecord()
        {
            IdentityUser? user = await _userManager.GetUserAsync(User);

            RouletteTournamentRecord? record = await _rouletteService.GetRouletteTournamentRecordByUserId(user!.Id);

            if(record is null)
            {
                await _rouletteService.CreateRouletteTournamentRecordAsync(user.Id);
            }

            return RedirectToAction(nameof(RouletteTournamentQuestion));
        }
    }
}
