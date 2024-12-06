﻿using VoteHub.Domain.Enums;

namespace VotingAppApi.Models
{
    public record AppUser 
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public UserRole Role { get; set; }
    }
}
