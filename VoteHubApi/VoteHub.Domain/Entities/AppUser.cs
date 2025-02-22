using Microsoft.AspNetCore.Identity;
using VoteHub.Domain.Enums;
namespace VotingAppApi.Models
{
    public class AppUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FileNumber { get; set; }

        // Use this if you want to store roles explicitly (optional)
        public UserRole Role { get; set; } = UserRole.Voter;

        // Navigation property for votes cast by the user
        public virtual ICollection<Vote> Votes { get; set; } = new HashSet<Vote>();
    }
}
