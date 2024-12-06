using Microsoft.EntityFrameworkCore;
using VotingAppApi.Models;
namespace VotingAppApi.Data
{
    public class VotingAppDbContext : DbContext 
    {
        public DbSet<AppUser> Users { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Vote> Votes { get; set; }

        public VotingAppDbContext(DbContextOptions<VotingAppDbContext> options) 
            : base(options)
        {

        }
    }
      
}
