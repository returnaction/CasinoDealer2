using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CasinoDealer2.Models.QuestionModels
{
    public class Question
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string QuestionText { get; set; } = null!;

        [Required]
        public string Answer { get; set; } = null!;


        public string CorrectAnswer { get; set; } = null!;


        public bool IsCorrect { get; set; }

        public int IncorrectStreak { get; set; } = 0;

        // nav props
        [Required]
        public string UserId { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; } = null!;
    }
}
