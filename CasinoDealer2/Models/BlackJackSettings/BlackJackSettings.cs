using CasinoDealer2.Models.Enums;

namespace CasinoDealer2.Models.BlackJackSettings
{
    public class BlackJackSettings
    {
        public int MinBet { get; set; } 
        public int MaxBet { get; set; } 
        public int Increment { get; set; } 

        public BlackJackPayOutType PayoutType { get; set; }
        
    }
}
