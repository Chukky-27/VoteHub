using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VoteHub.Domain.Entities;
using VotingAppApi.Models;
namespace VotingAppApi.Data
{
    public class VotingAppDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<VotingEvent> VotingEvents { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Vote> Votes { get; set; }

        public VotingAppDbContext(DbContextOptions<VotingAppDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure VotingEvent -> Candidates
            modelBuilder.Entity<VotingEvent>()
                .HasMany(e => e.Candidates)
                .WithOne()
                .HasForeignKey(c => c.VotingEventId)
                .OnDelete(DeleteBehavior.Cascade); 

            // Configure VotingEvent -> Votes
            modelBuilder.Entity<VotingEvent>()
                .HasMany(e => e.Votes)
                .WithOne()
                .HasForeignKey(v => v.VotingEventId)
                .OnDelete(DeleteBehavior.Cascade); 
        }

    }

}
