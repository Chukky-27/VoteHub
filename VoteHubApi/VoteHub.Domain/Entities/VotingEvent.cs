using VoteHub.Domain.Enums;
using VotingAppApi.Models;

namespace VoteHub.Domain.Entities
{
    public record VotingEvent
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public VotingStatus? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ICollection<Candidate>? Candidates { get; set; }
        public ICollection<Vote>? Votes { get; set; }
    }

}
