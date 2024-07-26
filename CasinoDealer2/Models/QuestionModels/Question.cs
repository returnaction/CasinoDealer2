using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CasinoDealer2.Models.Enums;

namespace CasinoDealer2.Models.QuestionModels
{
    public class Question
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string QuestionText { get; set; } = null!;

        [Required]
        public double Answer { get; set; }

        public double CorrectAnswer { get; set; }

        public GameType GameType { get; set; }


        public bool IsCorrect { get; set; }

        public int IncorrectStreak { get; set; } = 0;

        // nav props
        [Required]
        public string UserId { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; } = null!;
    }
}
