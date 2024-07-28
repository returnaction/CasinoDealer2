using CasinoDealer2.Data;
using CasinoDealer2.Models.Enums;
using CasinoDealer2.Models.QuestionModels;
using CasinoDealer2.RepositoryFolder.CrapsRepository;
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
        private readonly ICrapsService _crapsService;
        private readonly UserManager<IdentityUser> _userManager;


        public CrapsController(UserManager<IdentityUser> userManager, ICrapsService crapsService)
        {
            _userManager = userManager;
            _crapsService = crapsService;
        }

        public IActionResult CrapsQuestion()
        {
            var question = _crapsService.GenerateCrapsQuestion();
            return View(question);
        }

        [HttpPost]
        public async Task<IActionResult> CrapsQuestion(Question request)
        {
            var userId =  _userManager.GetUserId(User)!;

            bool isCorrect = await _crapsService.SaveCrapsQuestionAsync(request, userId);

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

       
    }
}
