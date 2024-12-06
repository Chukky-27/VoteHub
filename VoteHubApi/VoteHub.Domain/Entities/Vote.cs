using VoteHub.Domain.Enums;

namespace VotingAppApi.Models
{
    public record Vote
    {
        public int? Id { get; set; }
        public int? UserId { get; set; }
        public int? CandidateId { get; set; }
        public int? VotingEventId { get; set; } // Links vote to a voting event
        public DateTime? VotedAt { get; set; } = DateTime.UtcNow;
    }


}
