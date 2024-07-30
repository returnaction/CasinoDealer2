using CasinoDealer2.Models.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CasinoDealer2.Models.RouletteModels
{
    public class QuestionAR
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string QuestionText { get; set; } = null!;

        [Required]
        public double Answer { get; set; }

        public double CorrectAnswer { get; set; }

        public GameType GameType { get; set; }

        public int StraitUp { get; set; }
        public int Split { get; set; }
        public int Corner { get; set; }
        public int Straight { get; set; }
        public int SixLine { get; set; }


        public bool IsCorrect { get; set; }

        public int IncorrectStreak { get; set; } = 0;

        // nav props
        [Required]
        public string UserId { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; } = null!;
    }
}
