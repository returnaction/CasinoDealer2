using CasinoDealer2.Models;
using CasinoDealer2.Models.HomeModels;
using CasinoDealer2.RepositoryFolder.BalckJackRepository;
using CasinoDealer2.RepositoryFolder.RouletteRepository;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CasinoDealer2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlackJackService _blackJackService;
        private readonly IRouletteService _rouletteService;

        public HomeController(ILogger<HomeController> logger, IBlackJackService blackJackService, IRouletteService rouletteService)
        {
            _logger = logger;
            _blackJackService = blackJackService;
            _rouletteService = rouletteService;
        }

        public async Task<IActionResult> Index()
        {
            var model = new HomeVM
            {
                TopBlackJackTournamentRecords = await _blackJackService.GetTopBlackJackTournamentRecordsAsync(5),
                TopRouletteTournamentRecords = await _rouletteService.GetTopRouletteTournamentRecordsAsync(5)
            };
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
