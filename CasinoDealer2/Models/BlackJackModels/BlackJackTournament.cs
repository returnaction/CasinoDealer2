using Microsoft.AspNetCore.Identity;
using NuGet.Protocol.Core.Types;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CasinoDealer2.Models.BlackJackModels
{
    public class BlackJackTournament
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string QuestionText { get; set; }
        public double CorrectAnswer { get; set; }

        [Required]
        public double UserAnswer { get; set; }
        public bool IsCorrect { get; set; }
        public TimeSpan TimeTaken { get; set; }
        public string CorrectStreak { get; set; }

        // nav props
        [Required]
        public  string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; }

    }
}
