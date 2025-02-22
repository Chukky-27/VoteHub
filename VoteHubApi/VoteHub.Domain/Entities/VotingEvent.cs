using VoteHub.Domain.Enums;
using VotingAppApi.Models;

namespace VoteHub.Domain.Entities
{
    public class VotingEvent
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public VotingStatus Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Existing properties...
        public bool IsValid()
        {
            return StartDate < EndDate;
        }
        public bool AreCandidatesUnique()
        {
            var candidateNames = Candidates.Select(c => c.Name).Distinct();
            var candidatePositions = Candidates.Select(c => c.Position).Distinct();

            return candidateNames.Count() == Candidates.Count && candidatePositions.Count() == Candidates.Count;
        }
        public bool IsVotingOpen()
        {
            var now = DateTime.UtcNow;
            return now >= StartDate && now <= EndDate;
        }

        // Audit fields
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        // Navigation properties
        public virtual ICollection<Candidate> Candidates { get; set; } = new HashSet<Candidate>();
        public virtual ICollection<Vote> Votes { get; set; } = new HashSet<Vote>();
    }

}
