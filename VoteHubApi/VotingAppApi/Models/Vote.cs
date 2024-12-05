namespace VotingAppApi.Models
{
    public class Vote
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CandidateId { get; set; }
        public DateTime VotedAt { get; set; } = DateTime.UtcNow;
    }
}
