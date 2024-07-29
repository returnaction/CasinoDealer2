using CasinoDealer2.Models.BlackJackModels;
using CasinoDealer2.Models.QuestionModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CasinoDealer2.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Question> Questions { get; set; }
        public DbSet<BlackJackTournamentRecord> BlackJackTournamentRecords { get; set; }

    }
}
