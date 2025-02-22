using VoteHub.Domain.Entities;
using VoteHub.Domain.Enums;

namespace VotingAppApi.Models
{
    public record Vote
    {
        public int Id { get; init; }
        public int UserId { get; init; } // Required foreign key
        public int CandidateId { get; init; } // Required foreign key
        public int VotingEventId { get; init; } // Required foreign key

        public DateTime VotedAt { get; init; } = DateTime.UtcNow;

        // Navigation properties
        public virtual AppUser User { get; set; } = null!;
        public virtual Candidate Candidate { get; set; } = null!;
        public virtual VotingEvent VotingEvent { get; set; } = null!;
    }
}
