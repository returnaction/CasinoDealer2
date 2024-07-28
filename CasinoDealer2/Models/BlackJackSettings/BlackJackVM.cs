using CasinoDealer2.Models.QuestionModels;

namespace CasinoDealer2.Models.BlackJackSettings
{
    public class BlackJackVM
    {
        public Question Question { get; set; } = new Question();
        public BlackJackSettings Settings { get; set; } = new BlackJackSettings();
    }
}
