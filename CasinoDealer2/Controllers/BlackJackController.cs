using CasinoDealer2.Data;
using CasinoDealer2.Models.BlackJackModels;
using CasinoDealer2.Models.BlackJackSettings;
using CasinoDealer2.Models.Enums;
using CasinoDealer2.Models.QuestionModels;
using CasinoDealer2.RepositoryFolder.BalckJackRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

    // Inital Page hasn't been done yet. Maybe will add functionallity for traiting.... 
    public IActionResult BlackJackIndex()
    {
        return View();
    }

    //___________________Black Jack________________

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

    //_________________Black Jack Tournament__________________

    public IActionResult BlackJackTournamentQuestion()
    {
        TempData["CurrentStreak"] = 0;
        //TempData["PersonalRecord"] = 0;

        Question questionForTournament = _blackJackService.GenerateBlackJackQuestion(new BlackJackSettings { Increment = 5, MinBet = 5, MaxBet = 10000, PayoutType = BlackJackPayOutType.ThreeToTwo });

        ViewBag.CurrentStreak = TempData["CurrentStreak"];
       // ViewBag.PersonalRecord = TempData["PersonalRecord"];

        return View(questionForTournament);
    }

    [HttpPost]
    public async Task<IActionResult> BlackJackTournamentQuestion(Question question)
    {
        var user = await _userManager.GetUserAsync(User);

        int currentStreak = (int)(TempData["CurrentStreak"] ?? 0);

        bool isCorrect = question.Answer == question.CorrectAnswer;
        currentStreak = await _blackJackService.UpdateBlackJackTournamentRecord(user!.Id, isCorrect, currentStreak);

        TempData["CurrentStreak"] = currentStreak;

        Question newQuestionForTournament = _blackJackService.GenerateBlackJackTournamentQuestion();

        ViewBag.CurrentStreak = currentStreak;

        return View(newQuestionForTournament);

    }

    public async Task<IActionResult> CreateBlackJackTournamentRecord()
    {
        IdentityUser? user = await _userManager.GetUserAsync(User);

        BlackJackTournamentRecord? record = await _blackJackService.GetBlackJackTournamentRecordByUserId(user!.Id);
            //await _context.BlackJackTournamentRecords.FirstOrDefaultAsync(u => u.UserId == user!.Id);

        if(record is null)
        {
            await _blackJackService.CreateBlackJackTournamentRecordAsync(user.Id);
            //var blackJackTournamentRecord = new BlackJackTournamentRecord()
            //{
            //    LongestStreak = 0,
            //    UserId = user!.Id,
            //};

            //await _blackJackService.AddAsync(blackJackTournamentRecord);
            
            //await _context.BlackJackTournamentRecords.AddAsync(blackJackTournamentRecord);
            //await _context.SaveChangesAsync();
        }

       return RedirectToAction(nameof(BlackJackTournamentQuestion));
    }
}
