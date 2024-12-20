﻿using VoteHub.Domain.Enums;

namespace VotingAppApi.Models
{
    public record Candidate
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public CandidatePosition Position { get; set; }
        public int VotingEventId { get; set; } // Foreign key
        public int VoteCount { get; set; }
    }
}
