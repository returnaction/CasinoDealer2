using CasinoDealer2.Models.BlackJackSettings;
using CasinoDealer2.Models.Enums;
using CasinoDealer2.RepositoryFolder.BalckJackRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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

    // Inital Page
    public IActionResult BlackJackIndex()
    {
        return View();
    }

    public IActionResult BlackJackQuestion()
    {
        BlackJackSettings settings = GetSettingsFromTempData();
        var question = _blackJackService.GenerateBlackJackQuestion(settings);
        var model = new BlackJackVM
        {
            Question = question,
            Settings = settings
        };

        SaveSettingsToTempData(settings);
        return View(model);
    }

    [HttpPost]
    public IActionResult UpdateSettings(BlackJackVM model)
    {
        SaveSettingsToTempData(model.Settings);
        return RedirectToAction("BlackJackQuestion");
    }

    [HttpPost]
    public async Task<IActionResult> BlackJackQuestion(BlackJackVM model)
    {
        var userId = _userManager.GetUserId(User);
        bool isCorrect = await _blackJackService.SaveBlackJackQuestionAsync(model.Question, userId!);

        // Keep settings consistent between questions
        BlackJackSettings settings = GetSettingsFromTempData();
        if (!isCorrect)
        {
            // if the answer is wrong
            model.Question.IncorrectStreak++;
            model.Settings = settings;
            SaveSettingsToTempData(settings);
            return View(model);
        }

        SaveSettingsToTempData(settings);                                                                                                                                       
        return RedirectToAction("BlackJackQuestion");
    }

    private void SaveSettingsToTempData(BlackJackSettings settings)
    {
        TempData["MinBet"] = settings.MinBet;
        TempData["MaxBet"] = settings.MaxBet;
        TempData["Increment"] = settings.Increment;
        TempData["PayoutType"] = settings.PayoutType;
        TempData.Keep("MinBet");
        TempData.Keep("MaxBet");
        TempData.Keep("Increment");
        TempData.Keep("PayoutType");
    }

    private BlackJackSettings GetSettingsFromTempData()
    {
        return new BlackJackSettings
        {
            MinBet = TempData.ContainsKey("MinBet") ? (int)TempData["MinBet"]! : 5,
            MaxBet = TempData.ContainsKey("MaxBet") ? (int)TempData["MaxBet"]! : 100,
            Increment = TempData.ContainsKey("Increment") ? (int)TempData["Increment"]! : 5,
            PayoutType = TempData.ContainsKey("PayoutType") ? (BlackJackPayOutType)TempData["PayoutType"]! : BlackJackPayOutType.ThreeToTwo
        };
    }
}
