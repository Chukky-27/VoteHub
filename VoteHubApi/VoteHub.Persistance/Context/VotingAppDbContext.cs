using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VoteHub.Domain.Entities;
using VotingAppApi.Models;
namespace VotingAppApi.Data
{
    public class VotingAppDbContext : IdentityDbContext<AppUser>
    {
        // DbSet properties for all entities
        public DbSet<VotingEvent> VotingEvents { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Vote> Votes { get; set; }

        // Constructor
        public VotingAppDbContext(DbContextOptions<VotingAppDbContext> options)
            : base(options)
        {
        }

        // OnModelCreating method to configure relationships and constraints
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ensure a user can vote only once per voting event
            modelBuilder.Entity<Vote>()
                .HasIndex(v => new { v.UserId, v.VotingEventId })
                .IsUnique();

            // Configure VotingEvent -> Candidates relationship
            modelBuilder.Entity<VotingEvent>()
                .HasMany(e => e.Candidates)
                .WithOne(c => c.VotingEvent) // Navigation property in Candidate
                .HasForeignKey(c => c.VotingEventId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure VotingEvent -> Votes relationship
            modelBuilder.Entity<VotingEvent>()
                .HasMany(e => e.Votes)
                .WithOne(v => v.VotingEvent) // Navigation property in Vote
                .HasForeignKey(v => v.VotingEventId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Candidate -> Votes relationship
            modelBuilder.Entity<Candidate>()
                .HasMany(c => c.Votes)
                .WithOne(v => v.Candidate) // Navigation property in Vote
                .HasForeignKey(v => v.CandidateId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent accidental deletion of candidates with votes
        }


    }
}
