using Microsoft.AspNetCore.Identity;
using VoteHub.Domain.Enums;
namespace VotingAppApi.Models
{
    public class AppUser : IdentityUser
    {        
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FileNumber { get; set; }      
        public string? Password { get; set; }
        public UserRole Role { get; set; }
    }
}
