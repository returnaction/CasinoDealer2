using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CasinoDealer2.Models.BlackJackModels
{
    public class BlackJackTournamentRecord
    {
        [Key]
        public Guid Id { get; set; }
        public int LongestStreak { get; set; }
        public TimeSpan? Time { get; set; }
        public DateTime? Date { get; set; }


        // nav props

        public string UserId { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; } = null!;
    }
}
