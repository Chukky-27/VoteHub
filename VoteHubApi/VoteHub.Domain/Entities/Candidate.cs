using VoteHub.Domain.Entities;
using VoteHub.Domain.Enums;

namespace VotingAppApi.Models
{
    public record Candidate
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public CandidatePosition Position { get; init; }
        public int VotingEventId { get; init; } // Foreign key
                                                // New: Navigation property for the associated VotingEvent
        public virtual VotingEvent? VotingEvent { get; set; }

        // Optional: Candidate profile information
        public string? CandidateProfile { get; set; }

        // New: Candidate image URL
        public string? ImageUri { get; set; }

        // Navigation property for votes received by the candidate
        public virtual ICollection<Vote> Votes { get; set; } = new HashSet<Vote>();
    }
}
