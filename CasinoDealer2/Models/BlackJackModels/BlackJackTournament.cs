using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CasinoDealer2.Models.BlackJackModels
{
    public class BlackJackTournament
    {
        public string QuestionText { get; set; } = null!;
        public double CorrectAnswer { get; set; }

        [Required]
        public double UserAnswer { get; set; }
        public bool IsCorrect { get; set; }
        public TimeSpan TimeTaken { get; set; }
        public int CorrectStreak { get; set; }

        // nav props
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; }
    }
}
