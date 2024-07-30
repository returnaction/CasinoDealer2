using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CasinoDealer2.Models.RouletteModels
{
    public class RouletteTournamentRecord
    {
        [Key]
        public Guid Id { get; set; }
        public int LongestStreak { get; set; }
        public int CurrentStreak { get; set; }
        public TimeSpan? Time { get; set; }
        public DateTime? Date { get; set; }


        // nav props

        public string UserId { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; } = null!;
    }
}
