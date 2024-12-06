namespace VotingAppApi.Models
{
    public record Candidate
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Position { get; set; }
        public int VoteCount { get; set; }
    }
}
