using CasinoDealer2.Models.BlackJackModels;
using CasinoDealer2.Models.RouletteModels;

namespace CasinoDealer2.Models.HomeModels
{
    public class HomeVM
    {
        public List<BlackJackTournamentRecord> TopBlackJackTournamentRecords { get; set; } = null!;
        public List<RouletteTournamentRecord> TopRouletteTournamentRecords { get; set; } = null!;
    }
}
