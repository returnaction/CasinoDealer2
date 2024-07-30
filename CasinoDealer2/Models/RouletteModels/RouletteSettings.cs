namespace CasinoDealer2.Models.RouletteModels
{
    public class RouletteSettings
    {
        public int MinBet { get; set; }
        public int MaxBet { get; set; }
        public int Increment { get; set; }

        public bool IsStraightUp { get; set; }
        public bool IsSplit { get; set; }
        public bool IsCorner { get; set; }
        public bool IsSixline { get; set; }
        public bool IsStreet { get; set; }


    }
}
